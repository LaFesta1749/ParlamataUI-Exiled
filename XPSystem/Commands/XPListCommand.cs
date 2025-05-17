using CommandSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class XPLeaderboardCommand : ICommand
    {
        public string Command => "xpleaderboard";
        public string[] Aliases => new[] { "xpl" };
        public string Description => "Show top 10 players by level and XP, including your own position.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not CommandSender cmdSender)
            {
                response = "Only players can use this command.";
                return false;
            }

            var sorted = XPManager.AllData.OrderByDescending(p => p.Value.Level)
                                          .ThenByDescending(p => p.Value.XP)
                                          .ToList();

            var top10 = sorted.Take(10);
            string leaderboard = string.Join("\n", top10.Select((pair, index) =>
            {
                string name = pair.Value.LastKnownName ?? pair.Key;
                int level = pair.Value.Level;
                int xp = pair.Value.XP;
                int next = XPManager.GetXPRequired(level + 1);
                return $"#{index + 1} | {name} — Level {level} ({xp} / {next} XP)";
            }));

            // Найди текущия потребител
            string userId = cmdSender.SenderId;
            int playerIndex = sorted.FindIndex(p => p.Key == userId);

            if (playerIndex == -1)
            {
                response = leaderboard + $"\n\n⚠ You are not in the leaderboard.";
                return true;
            }

            var myData = sorted[playerIndex].Value;
            string myName = myData.LastKnownName ?? userId;
            int myLevel = myData.Level;
            int myXp = myData.XP;
            int myNext = XPManager.GetXPRequired(myLevel + 1);

            response = leaderboard +
                       $"\n\n📍 Your Position: #{playerIndex + 1} | {myName} — Level {myLevel} ({myXp} / {myNext} XP)";
            return true;
        }
    }
}
