using CommandSystem;
using System;
using ParlamataUI.XPSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPResetCommand : ICommand
    {
        public string Command => "xpr";
        public string[] Aliases => new[] { "xpra" };
        public string Description => "Reset XP for a user or everyone.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Usage:\n.xpr <UserID> - Reset single\n.xpra - Reset ALL";
                return false;
            }

            string target = arguments.At(0);
            if (target.Equals("xpra", StringComparison.OrdinalIgnoreCase))
            {
                XPManager.AllData.Clear();
                XPManager.SaveAll();
                response = "Reset XP for ALL users.";
                return true;
            }

            if (!XPManager.AllData.ContainsKey(target))
            {
                response = $"No XP data for: {target}";
                return false;
            }

            XPManager.AllData[target] = new PlayerXP { UserId = target };
            XPManager.Save(target, XPManager.AllData[target]);
            response = $"XP data reset for: {target}";
            return true;
        }
    }
}
