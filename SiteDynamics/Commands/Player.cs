namespace SiteDynamics.Commands
{
    using CommandSystem;
    using System;
    using API.Features;
    using PlayerRoles;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    public class Player : ICommand
    {
        public string Command { get; } = "player";

        public string[] Aliases { get; } = { "p" };

        public string Description { get; } = "Controls players | Supports: Selection, Filters";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"sd.{Command}"))
            {
                response = "You dont have permission to execute this command!";
                return false;
            }

            if (arguments.Count >= 1)
            {
                Exiled.API.Features.Player plr = Exiled.API.Features.Player.Get(sender);

                switch (arguments.At(0))
                {
                    default:
                        response = "Subcommand not recognized!";
                        return false;
                    case "info" or "i":
                        response = "Info: \n";
                        response += "- Selection: " + API.Features.Players.selection.ToString()        + '\n';
                        response += "- Ignore sender: " + API.Features.Players.ignoreSender.ToString() + '\n';
                        response += "- Ignore staff: " + API.Features.Players.ignoreStaff.ToString()   + '\n';
                        return true;
                    case "select" or "s":
                        if (arguments.Count == 2)
                            API.Features.Players.Select(arguments.At(1));
                        else if (arguments.Count == 3)
                            API.Features.Players.Select(arguments.At(1), float.Parse(arguments.At(2)));
                        response = "Changed player selection!";
                        return true;
                    case "filters" or "f":
                        API.Features.Players.ignoreSender = bool.Parse(arguments.At(1));
                        API.Features.Players.ignoreStaff = bool.Parse(arguments.At(1));
                        response = "Changed player filters!";
                        return true;
                    case "saferandomteleport" or "srtp":
                        API.Features.Players.Selected(plr, API.Features.Players.Commands.SafeRandomTeleport);
                        response = "Safe random teleport performed!";
                        return true;
                    case "deleteinventory" or "di":
                        API.Features.Players.Selected(plr, API.Features.Players.Commands.DeleteInventory);
                        response = "Deleted inventories!";
                        return true;
                    case "setrole" or "sr":
                        API.Features.Players.Selected(plr, (RoleTypeId)Enum.Parse(typeof(RoleTypeId), arguments.At(1)), API.Features.Players.Commands.SetRole);
                        response = "Setted roles!";
                        return true;
                    case "kill" or "k":
                        API.Features.Players.Selected(plr, API.Features.Players.Commands.Kill);
                        response = "Killed players!";
                        return true;
                    case "seteffect" or "se":
                        API.Features.Players.Selected(plr, (EffectCategory)Enum.Parse(typeof(EffectCategory), arguments.At(1)), API.Features.Players.Commands.GiveEffect);
                        response = "Setted effect!";
                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) player (p) [subcommand] "        + '\n';

            response += "<color=yellow><b> special subcommands: </b></color> "    + '\n';
            response += "- select (s) [selection] [amount / distance (optional)]" + '\n';
            response += "- filters (f) [ignoresender] [ignorestaffers]"           + '\n';

            response += "<color=yellow><b> subcommands: </b></color> "            + '\n';
            response += "- info (i) "                                             + '\n';
            response += "- saferandomteleport (srtp) "                            + '\n';
            response += "- deleteinventory (di) "                                 + '\n';
            response += "- setrole (sr) [roleID] "                                + '\n';
            response += "- kill (k) "                                             + '\n';
            response += "- seteffect (se) [effectcategoryID] "                    + '\n';

            return false;
        }
    }
}