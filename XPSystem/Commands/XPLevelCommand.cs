using CommandSystem;
using System;
using ParlamataUI.XPSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPLevelCommand : ICommand
    {
        public string Command => "xplvl";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Add levels to a player by UserID.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2 || !int.TryParse(arguments.At(1), out int amount))
            {
                response = "Usage: .xplvl <UserID> <LevelAmount>";
                return false;
            }

            string userId = arguments.At(0);
            if (!XPManager.AllData.TryGetValue(userId, out var data))
            {
                response = $"No XP data for: {userId}";
                return false;
            }

            data.Level += amount;
            XPManager.Save(userId, data);
            response = $"Added {amount} levels to {data.LastKnownName ?? userId}. New level: {data.Level}";
            return true;
        }
    }
}
