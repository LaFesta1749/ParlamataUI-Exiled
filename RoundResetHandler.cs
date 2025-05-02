using Exiled.Events.EventArgs.Server;

namespace ParlamataUI
{
    public static class RoundResetHandler
    {
        public static void OnRoundEnded(RoundEndedEventArgs _)
        {
            HintRenderer.ClearAllHints();
        }
    }
}
