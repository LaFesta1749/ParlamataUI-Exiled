using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Server;

namespace ParlamataUI.XPSystem
{
    public static class XPEvents
    {
        public static void Register()
        {
            Exiled.Events.Handlers.Player.Dying += OnDeath;
            Exiled.Events.Handlers.Player.Escaping += OnEscape;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickup;
            Exiled.Events.Handlers.Player.DroppingItem += OnDrop;
            Exiled.Events.Handlers.Player.Spawning += OnSpawn;
            Exiled.Events.Handlers.Player.UsingItem += OnUseItem;
            Exiled.Events.Handlers.Player.ThrowingRequest += OnThrow;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnUpgrade;
            Exiled.Events.Handlers.Player.InteractingDoor += OnDoor;
            Exiled.Events.Handlers.Player.ActivatingGenerator += OnGenerator;
            Exiled.Events.Handlers.Scp049.FinishingRecall += OnRecall;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public static void Unregister()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDeath;
            Exiled.Events.Handlers.Player.Escaping -= OnEscape;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickup;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDrop;
            Exiled.Events.Handlers.Player.Spawning -= OnSpawn;
            Exiled.Events.Handlers.Player.UsingItem -= OnUseItem;
            Exiled.Events.Handlers.Player.ThrowingRequest -= OnThrow;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnUpgrade;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnDoor;
            Exiled.Events.Handlers.Player.ActivatingGenerator -= OnGenerator;
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnRecall;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private static void OnDeath(DyingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Attacker != ev.Player)
                XPManager.Reward(ev.Attacker, "kill");

            XPManager.Reward(ev.Player, "death");
        }

        private static void OnEscape(EscapingEventArgs ev) => XPManager.Reward(ev.Player, "escape");

        private static void OnPickup(PickingUpItemEventArgs ev) => XPManager.Reward(ev.Player, "pickup");

        private static void OnDrop(DroppingItemEventArgs ev) => XPManager.Reward(ev.Player, "drop");

        private static void OnUseItem(UsingItemEventArgs ev) => XPManager.Reward(ev.Player, "use");

        private static void OnSpawn(SpawningEventArgs ev) => XPManager.Reward(ev.Player, "spawn");

        private static void OnRecall(FinishingRecallEventArgs ev)
        {
            var scp049Player = Player.Get(ev.Scp049.Owner);
            if (scp049Player != null)
                XPManager.Reward(scp049Player, "ressurect");
        }

        private static void OnThrow(ThrowingRequestEventArgs ev) => XPManager.Reward(ev.Player, "throw");

        private static void OnUpgrade(UpgradingInventoryItemEventArgs ev)
        {
            if (ev.Player != null)
                XPManager.Reward(ev.Player, "upgrade");
        }

        private static void OnDoor(InteractingDoorEventArgs ev)
        {
            if (ev.IsAllowed && ev.Door.IsOpen)
                XPManager.Reward(ev.Player, "door");
        }

        private static void OnGenerator(ActivatingGeneratorEventArgs ev)
        {
            if (ev.IsAllowed)
                XPManager.Reward(ev.Player, "generator");
        }

        private static void OnRoundEnded(RoundEndedEventArgs _)
        {
            foreach (var player in Player.List)
            {
                if (player.IsAlive)
                    XPManager.Reward(player, "win");
            }
        }
    }
}
