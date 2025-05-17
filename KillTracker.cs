using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using HSMHint = HintServiceMeow.Core.Models.Hints.Hint;

namespace ParlamataUI
{
    public static class KillTracker
    {
        private static readonly Dictionary<Player, int> _killCounts = new();
        private static readonly Dictionary<Player, int> _scp106Traps = new();
        private static readonly Dictionary<string, HSMHint> PocketEscapeHints = new();
        private static readonly Dictionary<string, CoroutineHandle> PocketHintCoroutines = new();

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

            string userId = scp106.UserId;
            var display = PlayerDisplay.Get(scp106);

            // Премахни стария Hint (ако съществува)
            if (PocketEscapeHints.TryGetValue(userId, out var oldHint))
            {
                display.RemoveHint(oldHint);
                PocketEscapeHints.Remove(userId);
            }

            // Спри старата coroutine (ако съществува)
            if (PocketHintCoroutines.TryGetValue(userId, out var oldCoroutine))
            {
                Timing.KillCoroutines(oldCoroutine);
                PocketHintCoroutines.Remove(userId);
            }

            // Създай нов Hint
            string message = $"<b><color=#ff4444>{ev.Player.Nickname}</color></b> escaped your pocket dimension!";
            var newHint = new HSMHint
            {
                FontSize = 24,
                XCoordinate = 0,
                YCoordinate = 900,
                Alignment = HintAlignment.Center,
                Text = message
            };

            display.AddHint(newHint);
            PocketEscapeHints[userId] = newHint;

            // Започни coroutine за премахване след 3 сек
            CoroutineHandle handle = Timing.RunCoroutine(RemovePocketHintAfterDelay(scp106, newHint, 3f));
            PocketHintCoroutines[userId] = handle;
        }

        private static IEnumerator<float> RemovePocketHintAfterDelay(Player scp106, HSMHint hint, float delay)
        {
            yield return Timing.WaitForSeconds(delay);

            string userId = scp106.UserId;
            if (PocketEscapeHints.TryGetValue(userId, out var current) && current == hint)
            {
                PlayerDisplay.Get(scp106).RemoveHint(hint);
                PocketEscapeHints.Remove(userId);
                PocketHintCoroutines.Remove(userId);
            }
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
