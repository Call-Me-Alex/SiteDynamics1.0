namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using PlayerRoles;
    using System;

    public class Round : ICommand
    {
        public string Command { get; } = "round";

        public string[] Aliases { get; } = { "rn" };

        public string Description { get; } = "Controls the round";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"sd.{Command}"))
            {
                response = "You dont have permission to execute this command!";
                return false;
            }

            if (arguments.Count >= 1)
            {
                switch (arguments.At(0))
                {
                    default:
                        response = "Subcommand not recognized!";
                        return false;
                    case "respawnlock" or "rl":
                        API.Features.Round.Commands.ToggleRespawnLock();
                        response = API.Features.Round.preventRespawns ? "Respawn prevention enabled!" : "Respawn prevention disabled!";
                        return true;
                    case "autorespawn" or "ar":
                        API.Features.Round.Commands.ToggleRespawOnDeath(arguments.Count == 2 ? (RoleTypeId)int.Parse(arguments.At(1)) : default);
                        response = API.Features.Round.respawnOnDeath ? "Auto respawn enabled!" : "Auto respawn disabled!";
                        return true;
                    case "despawnwave" or "dw":
                        API.Features.Round.Commands.DespawnWave();
                        response = "Despawned most recent wave!";
                        return true;
                    case "914lock" or "914l":
                        API.Features.Round.Commands.Toggle914Lock();
                        response = "Toggled 914 lock!";
                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) round (rn) [subcommand] " + '\n';
            response += "<color=yellow><b> subcommands: </b></color> "     + '\n';
            response += "- respawnlock (rl) "                              + '\n';
            response += "- autorespawn (ar) [roleID] "                     + '\n';
            response += "- despawnwave (dw) "                              + '\n';
            response += "- 914lock (914l) "                                + '\n';

            return false;
        }
    }
}