namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.Permissions.Extensions;
    using PluginAPI.Core;
    using System;

    public class Zone : ICommand
    {
        public string Command { get; } = "zone";

        public string[] Aliases { get; } = { "z" };

        public string Description { get; } = "Controls zones | Supports: Selection";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"sd.{Command}"))
            {
                response = "You dont have permission to execute this command!";
                return false;
            }

            PluginAPI.Core.Player plr = PluginAPI.Core.Player.Get(sender);

            if (arguments.Count >= 1)
            {
                switch (arguments.At(0))
                {
                    default:
                        response = "Subcommand not recognized!";
                        return false;
                    case "info" or "i":
                        response = "Info: \n";
                        response += "- Selection: " + API.Features.Zones.selection.ToString() + '\n';
                        return true;
                    case "select" or "s":
                        if (arguments.Count == 2)
                            API.Features.Zones.Select(arguments.At(1));
                        else if (arguments.Count == 3)
                            API.Features.Zones.Select(arguments.At(1), float.Parse(arguments.At(2)));
                        response = "Changed door selection!";
                        return true;
                    case "kill" or "k":
                        API.Features.Zones.Selected(plr, API.Features.Zones.Commands.Kill);
                        response = "Killed every player in zone!";
                        return true;
                    case "isolate" or "iso":
                        API.Features.Zones.Selected(plr, API.Features.Zones.Commands.Isolate);
                        response = "Isolated the closest zone!";
                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) zone (z) [subcommand] "          + '\n';

            response += "<color=yellow><b> special subcommands: </b></color> "    + '\n';
            response += "- select (s) [selection] [amount / distance (optional)]" + '\n';

            response += "<color=yellow><b> subcommands: </b></color> "            + '\n';
            response += "- info (i) "                                             + '\n';
            response += "- kill (k) "                                             + '\n';
            response += "- isolate (iso) "                                        + '\n';

            return false;
        }
    }
}