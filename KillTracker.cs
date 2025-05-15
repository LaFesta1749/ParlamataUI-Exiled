using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Linq;

namespace ParlamataUI
{
    public static class KillTracker
    {
        private static readonly Dictionary<Player, int> _killCounts = new();
        private static readonly Dictionary<Player, int> _scp106Traps = new();

        public static void Enable()
        {
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocket;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += OnEscapingPocket;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public static void Disable()
        {
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocket;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= OnEscapingPocket;
            _scp106Traps.Clear();
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            _killCounts.Clear();
        }

        private static void OnRoundStarted()
        {
            _killCounts.Clear();
            _scp106Traps.Clear();
        }

        private static void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs _)
        {
            _killCounts.Clear();
            _scp106Traps.Clear();
        }

        public static int GetTrappedVictims(Player player)
        {
            return _scp106Traps.TryGetValue(player, out int count) ? count : 0;
        }

        private static void OnEnteringPocket(EnteringPocketDimensionEventArgs ev)
        {
            Player scp106 = Player.List.FirstOrDefault(p => p.Role.Type == PlayerRoles.RoleTypeId.Scp106);
            if (scp106 == null)
                return;

            if (_scp106Traps.ContainsKey(scp106))
                _scp106Traps[scp106]++;
            else
                _scp106Traps[scp106] = 1;
        }

        private static void OnEscapingPocket(EscapingPocketDimensionEventArgs ev)
        {
            Player scp106 = Player.List.FirstOrDefault(p => p.Role.Type == PlayerRoles.RoleTypeId.Scp106);
            if (scp106 == null)
                return;

            string message = $"<b><color=#ff4444>{ev.Player.Nickname}</color></b> escaped your pocket dimension!";
            var hint = new HintServiceMeow.Core.Models.Hints.Hint
            {
                FontSize = 24,
                XCoordinate = 0,
                YCoordinate = 900,
                Alignment = HintServiceMeow.Core.Enum.HintAlignment.Center,
                Text = message
            };

            HintServiceMeow.Core.Utilities.PlayerDisplay.Get(scp106).AddHint(hint);
        }
        private static void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker == null || ev.Attacker == ev.Player || !ev.Attacker.IsAlive)
                return;

            if (_killCounts.ContainsKey(ev.Attacker))
                _killCounts[ev.Attacker]++;
            else
                _killCounts[ev.Attacker] = 1;
        }

        public static int GetKills(Player player)
        {
            return _killCounts.TryGetValue(player, out int count) ? count : 0;
        }
    }
}
