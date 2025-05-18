using Exiled.API.Features;
using Exiled.Events.Commands.Reload;

namespace ParlamataUI.XPSystem
{
    public static class XPManager
    {
        public static PlayerXP GetData(Player player) => XPDatabase.Get(player.UserId);

        public static void AddXP(Player player, int amount)
        {
            var data = XPDatabase.Get(player.UserId);

            data.XP += amount;

            int newLevel = CalculateLevel(data.XP);
            if (newLevel > data.Level)
            {
                data.Level = newLevel;
                player.Broadcast(5, $"<color=#FFD700>LEVEL UP! → {newLevel}</color>");
            }

            XPDatabase.Save(data);

            HintRenderer.RenderUI(player);
        }

        public static Dictionary<string, PlayerXP> AllData { get; } = new();

        public static void Save(string _, PlayerXP data)
        {
            XPDatabase.Save(data); // userId се съдържа вече вътре в `data.UserId`
        }

        public static void SaveAll()
        {
            foreach (var pair in AllData)
                XPDatabase.Save(pair.Value);
        }

        public static int CalculateLevel(int totalXp)
        {
            // Проста формула: първо ниво на 0 XP, после +250 на всяко ниво
            int level = 1;
            int neededXp = 250;

            while (totalXp >= neededXp)
            {
                totalXp -= neededXp;
                level++;
                neededXp = 250 + (level - 1) * 50;
            }

            return level;
        }

        public static void Reward(Player player, string reason)
        {
            if (!Plugin.Instance.Config.IsEnabled || !Plugin.Instance.Config.ShowXP)
                return;

            // XP събития, които може да се повтарят
            string[] repeatableEvents = { "kill", "death", "door", "spawn", "generator", "upgrade", "throw", "ressurect", "resurrect" };

            if (!repeatableEvents.Contains(reason.ToLower()) && XPEventCache.HasDone(player.UserId, reason))
                return; // Играчът вече е получил XP за това събитие този рунд

            var rewards = Plugin.Instance.Config.XPRewards;
            int amount = reason.ToLower() switch
            {
                "kill" => rewards.OnKill,
                "death" => rewards.OnDeath,
                "escape" => rewards.OnEscape,
                "win" => rewards.OnWin,
                "door" => rewards.OnDoorOpen,
                "pickup" => rewards.OnPickupItem,
                "drop" => rewards.OnDropItem,
                "throw" => rewards.OnThrowGrenade,
                "generator" => rewards.OnGeneratorActivate,
                "upgrade" => rewards.OnUpgradeItem,
                "use" => rewards.OnUseMedical,
                "spawn" => rewards.OnSpawn,
                "ressurect" or "resurrect" => rewards.OnResurrect,
                _ => 0
            };

            if (amount <= 0)
                return;

            AddXP(player, amount);

            string readable = reason.ToLower() switch
            {
                "kill" => "killing a player",
                "death" => "dying",
                "escape" => "escaping",
                "win" => "winning the round",
                "door" => "opening a door",
                "pickup" => "picking up an item",
                "drop" => "dropping an item",
                "throw" => "throwing a grenade",
                "generator" => "activating a generator",
                "upgrade" => "upgrading an item in SCP-914",
                "use" => "using a medical item",
                "spawn" => "spawning",
                "ressurect" or "resurrect" => "resurrecting a player",
                _ => reason
            };

            HintRenderer.ShowXPMessage(player, $"You have received {amount} XP for {readable}.");
        }

        public static int GetXPRequired(int level)
        {
            // Обратна на горната: сума от XP до дадено ниво
            int xp = 0;
            for (int i = 1; i < level; i++)
                xp += 250 + (i - 1) * 50;
            return xp;
        }
    }
}
