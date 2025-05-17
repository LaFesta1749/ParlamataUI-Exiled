using CommandSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPSetCommand : ICommand
    {
        public string Command => "xpset";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Set a player's XP directly by UserID.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 2 || !int.TryParse(arguments.At(1), out int newXp))
            {
                response = "Usage: .xpset <UserID> <NewXP>";
                return false;
            }

            string userId = arguments.At(0);
            if (!XPManager.AllData.TryGetValue(userId, out var data))
            {
                response = $"No XP data for: {userId}";
                return false;
            }

            data.XP = newXp;
            data.Level = XPManager.CalculateLevel(newXp);

            XPManager.Save(userId, data);
            response = $"Set XP of {userId} to {newXp}. New level: {data.Level}";
            return true;
        }
    }
}
