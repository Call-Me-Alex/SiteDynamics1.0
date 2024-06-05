namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using PlayerRoles;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Team : ICommand
    {
        public string Command { get; } = "team";

        public string[] Aliases { get; } = { "tm" };

        public string Description { get; } = "Controls custom teams | Supports: Filters";

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
                    case "info" or "i":
                        response = "Info: \n";
                        response += "- Ignore sender: " + API.Features.Teams.ignoreSender.ToString() + '\n';
                        response += "- Ignore staff: " + API.Features.Teams.ignoreStaff.ToString() + '\n';
                        return true;
                    case "show" or "s":
                        sbyte counter = 1;
                        response = "\n Are created: " + API.Features.Teams.areCreated.ToString() + '\n';
                        response += "<color=yellow><b>Teams: </b></color>" + '\n';
                        List<RoleTypeId> teamRoles = API.Features.Teams.teamRoles;
                        teamRoles.Reverse();
                        foreach (RoleTypeId role in teamRoles)
                        {
                            response += $"({counter}) " + role.ToString() + '\n';
                            counter++;
                        }
                        return true;
                    case "filters" or "f":
                        API.Features.Teams.ignoreSender = bool.Parse(arguments.At(1));
                        API.Features.Teams.ignoreStaff = bool.Parse(arguments.At(1));
                        response = "Changed player filters!";
                        return true;
                    case "create" or "cr":
                        API.Features.Teams.Commands.Create(Exiled.API.Features.Player.Get(sender), arguments.Skip(1).ToArray(), out string creationResponse);
                        response = creationResponse;
                        return API.Features.Teams.areCreated;
                    /*
                    case "fill" or "fl":
                        API.Features.Teams.Commands.Fill();
                        response = API.Features.Teams.areCreated ? "Filled teams!" : "You must first create teams!";
                        return API.Features.Teams.areCreated;
                    */
                    /*
                    case "balance" or "b":
                        API.Features.Teams.Commands.Balance();
                        response = API.Features.Teams.areCreated ? "Balanced teams!" : "You must first create teams!";
                        return API.Features.Teams.areCreated;
                    */
                    case "add" or "a":
                        Exiled.API.Features.Player playerToAdd = Exiled.API.Features.Player.Get(int.Parse(arguments.At(1)));
                        int teamToAdd = int.Parse(arguments.At(2));
                        API.Features.Teams.Commands.SetTeam(playerToAdd, teamToAdd);
                        response = $"Added {playerToAdd.Nickname } to team {teamToAdd}!";
                        return true;
                    case "remove" or "r":
                        Exiled.API.Features.Player playerToRemove = Exiled.API.Features.Player.Get(int.Parse(arguments.At(1)));
                        API.Features.Teams.Commands.RemoveTeam(playerToRemove);
                        response = $"Removed {playerToRemove.Nickname }!";
                        return true;
                    case "clear" or "cl":
                        API.Features.Teams.Commands.Clear();
                        response = API.Features.Teams.areCreated ? "You must first create teams!" : "Cleared teams!";
                        return !API.Features.Teams.areCreated;
                    case "respawn" or "rs":
                        if (arguments.Count == 1)
                            API.Features.Teams.Commands.Respawn();
                        else if (arguments.Count == 2)
                            API.Features.Teams.Commands.Respawn(int.Parse(arguments.At(1)));

                        response = API.Features.Teams.areCreated ? "Respawned teams!" : "You must first create teams!";
                        return API.Features.Teams.areCreated;
                    case "forcerespawn" or "frs":
                        if (arguments.Count == 1)
                            API.Features.Teams.Commands.ForceRespawn();
                        else if (arguments.Count == 2)
                            API.Features.Teams.Commands.ForceRespawn(int.Parse(arguments.At(1)));

                        response = API.Features.Teams.areCreated ? "Forced teams respawns!" : "You must first create teams!";
                        return API.Features.Teams.areCreated;

                }
            }

            response = "Usage: sitedynamics (sd) team (tm) [subcommand] "      + '\n';

            response += "<color=yellow><b> special subcommands: </b></color> " + '\n';
            response += "- filters (f) [ignoresender] [ignorestaffers]"        + '\n';
                                                                               
            response += "<color=yellow><b> subcommands: </b></color> "         + '\n';
            response += "- info (i) "                                          + '\n';
            response += "- show (s) "                                          + '\n';
            response += "- create (cr) [class] [class] [class] [class] "       + '\n';
            //response += "- fill (fl) "                                         + '\n';
            //response += "- balance (b) "                                       + '\n';
            response += "- add (a) [playerID] [teamID]"                        + '\n';
            response += "- remove (r) [playerID] "                             + '\n';
            response += "- clear (cl) "                                        + '\n';
            response += "- respawn (rs) [team (optional)] "                    + '\n';
            response += "- forcerespawn (frs) [team (optional)] "              + '\n';

            return false;
        }
    }
}