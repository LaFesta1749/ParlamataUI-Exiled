using System;
using Exiled.Events.Handlers;

namespace ParlamataUI
{
    public static class RoundTimer
    {
        private static DateTime? _roundStartTime;

        public static void Enable()
        {
            Server.RoundStarted += OnRoundStarted;
            Server.RoundEnded += OnRoundEnded;
        }

        public static void Disable()
        {
            Server.RoundStarted -= OnRoundStarted;
            Server.RoundEnded -= OnRoundEnded;
            _roundStartTime = null;
        }

        private static void OnRoundStarted()
        {
            _roundStartTime = DateTime.UtcNow;
        }

        private static void OnRoundEnded(Exiled.Events.EventArgs.Server.RoundEndedEventArgs _)
        {
            _roundStartTime = null;
        }

        public static TimeSpan GetElapsedTime()
        {
            return _roundStartTime.HasValue
                ? DateTime.UtcNow - _roundStartTime.Value
                : TimeSpan.Zero;
        }

        public static string GetFormattedElapsedTime()
        {
            var elapsed = GetElapsedTime();
            return $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        }
    }
}
