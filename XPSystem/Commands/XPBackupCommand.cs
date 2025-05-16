using CommandSystem;
using Exiled.API.Features;
using System;
using System.IO;

namespace ParlamataUI.XPSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class XPBackupCommand : ICommand
    {
        public string Command => "xpbackup";
        public string[] Aliases => new[] { "xpb" };
        public string Description => "Create a backup of the XPSystem.db database.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                string baseDir = Path.Combine(Paths.Configs); // /.config/EXILED/Configs
                string dbFile = Path.Combine(baseDir, "XPSystem.db");
                string backupFile = Path.Combine(baseDir, "XPSystem.bak");

                if (!File.Exists(dbFile))
                {
                    response = "XPSystem.db not found!";
                    return false;
                }

                if (File.Exists(backupFile))
                    File.Delete(backupFile);

                File.Copy(dbFile, backupFile);
                response = "XPSystem.bak created successfully in .config/EXILED/Configs.";
                return true;
            }
            catch (Exception ex)
            {
                response = $"Backup failed: {ex.Message}";
                return false;
            }
        }
    }
}
