using CommandSystem;
using System;
using ParlamataUI.XPSystem;
using Exiled.API.Features;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPAddCommand : ICommand
    {
        public string Command => "xpadd";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Adds XP to a player by UserID.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2 || !int.TryParse(arguments.At(1), out int xpAmount))
            {
                response = "Usage: .xpadd <UserID> <Amount>";
                return false;
            }

            string userId = arguments.At(0);
            if (!XPManager.AllData.TryGetValue(userId, out var data))
            {
                response = $"No XP data found for: {userId}";
                return false;
            }

            data.XP += xpAmount;
            data.Level = XPManager.CalculateLevel(data.XP);
            XPManager.Save(userId, data);

            response = $"Added {xpAmount} XP to {data.UserId}. New level: {data.Level}, XP: {data.XP}";
            return true;
        }
    }
}
