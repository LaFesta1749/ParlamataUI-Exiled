using Exiled.API.Features;
using PlayerRoles;
using System.Linq;

namespace ParlamataUI
{
    public static class SpectatorTracker
    {
        public static int GetSpectatorCount(Player target)
        {
            if (target == null || !target.IsAlive)
                return 0;

            return target.CurrentSpectatingPlayers
                         .Count(p => p.Role.Type != RoleTypeId.Overwatch);
        }
    }
}
