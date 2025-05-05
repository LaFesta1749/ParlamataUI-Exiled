using Exiled.Events.EventArgs.Server;
using ParlamataUI.XPSystem;
using Exiled.API.Features;

namespace ParlamataUI
{
    public static class RoundResetHandler
    {
        public static void OnRoundEnded(RoundEndedEventArgs _)
        {
            HintRenderer.ClearAllHints();
            XPEventCache.Clear(); // ← Това добави

            if (Plugin.Instance.Config.Debug)
                Log.Debug("[XPSystem] Cleared XP cache on round end.");
        }
    }
}
