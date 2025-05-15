using CommandSystem;
using System;
using System.Linq;
using ParlamataUI.XPSystem;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class XPListCommand : ICommand
    {
        public string Command => "xpl";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Show top 10 players by level and XP.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var allPlayers = XPManager.AllData.OrderByDescending(p => p.Value.Level)
                .ThenByDescending(p => p.Value.XP)
                .Take(10);

            response = string.Join("\n", allPlayers.Select((pair, index) =>
            {
                string user = pair.Value.LastKnownName ?? pair.Key;
                int lvl = pair.Value.Level;
                int xp = pair.Value.XP;
                int next = XPManager.GetXPRequired(lvl + 1);
                return $"#{index + 1} | {user} — Level {lvl} ({xp} / {next} XP)";
            }));

            return true;
        }
    }
}
