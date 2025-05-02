using Exiled.Events.EventArgs.Player;

namespace ParlamataUI
{
    public static class PlayerCleanupHandler
    {
        public static void OnPlayerDestroying(DestroyingEventArgs ev)
        {
            string userId = ev.Player.UserId;
            HintRenderer.RemoveHintsFor(userId);
        }
    }
}
