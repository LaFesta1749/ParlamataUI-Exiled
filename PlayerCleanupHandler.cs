using Exiled.Events.EventArgs.Player;
using ParlamataUI.XPSystem;
using Exiled.API.Features;

namespace ParlamataUI
{
    public static class PlayerCleanupHandler
    {
        public static void OnPlayerDestroying(DestroyingEventArgs ev)
        {
            string userId = ev.Player.UserId;

            HintRenderer.RemoveHintsFor(userId);
            XPEventCache.OnPlayerLeft(userId); // ← Това добави

            if (Plugin.Instance.Config.Debug)
                Log.Debug($"[XPSystem] Removed XP cache for {userId}.");
        }
    }
}
