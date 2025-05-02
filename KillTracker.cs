using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace ParlamataUI
{
    public static class KillTracker
    {
        private static readonly Dictionary<Player, int> _killCounts = new();

        public static void Enable()
        {
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public static void Disable()
        {
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            _killCounts.Clear();
        }

        private static void OnRoundStarted()
        {
            _killCounts.Clear();
        }

        private static void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs _)
        {
            _killCounts.Clear();
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
