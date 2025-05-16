using CommandSystem;
using Exiled.API.Features;
using System;
using ParlamataUI.XPSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPGiveAllCommand : ICommand
    {
        public string Command => "xpgiveall";
        public string[] Aliases => new[] { "xpga" };
        public string Description => "Gives XP to all currently online players.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1 || !int.TryParse(arguments.At(0), out int xpAmount))
            {
                response = "Usage: .xpgiveall <Amount>";
                return false;
            }

            int count = 0;

            foreach (var player in Player.List)
            {
                if (!player.IsConnected)
                    continue;

                XPManager.AddXP(player, xpAmount);
                count++;
            }

            response = $"✅ {xpAmount} XP given to {count} online players.";
            return true;
        }
    }
}
