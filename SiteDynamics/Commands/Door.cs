namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using PluginAPI.Core;

    using System;

    public class Door : ICommand
    {
        public string Command { get; } = "door";

        public string[] Aliases { get; } = { "d" };

        public string Description { get; } = "Controls doors | Supports: Selection";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"sd.{Command}"))
            {
                response = "You don't have permission to execute this command!";
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
                        response += "- Selection: " + API.Features.Doors.selection.ToString() + '\n';
                        return true;
                    case "select" or "s":
                        if (arguments.Count == 2)
                            API.Features.Doors.Select(arguments.At(1));
                        else if (arguments.Count == 3)
                            API.Features.Doors.Select(arguments.At(1), float.Parse(arguments.At(2)));
                        response = "Changed door selection!";
                        return true;
                    case "open" or "o":
                        API.Features.Doors.Selected(plr, API.Features.Doors.Commands.Open);
                        response = "Opened door!";
                        return true;
                    case "close" or "c":
                        API.Features.Doors.Selected(plr, API.Features.Doors.Commands.Close);
                        response = "Closed door!";
                        return true;
                    case "lock" or "l":
                        API.Features.Doors.Selected(plr, API.Features.Doors.Commands.Lock);
                        response = "Locked door!";
                        return true;
                    case "unlock" or "ul":
                        API.Features.Doors.Selected(plr, API.Features.Doors.Commands.Unlock);
                        response = "Unlocked door!";
                        return true;
                    case "destroy" or "d":
                        API.Features.Doors.Selected(plr, API.Features.Doors.Commands.Destroy);
                        response = "Destroyed! (TODO)";
                        return true;
                    case "lockfor" or "lf":
                        API.Features.Doors.Selected(plr, float.Parse(arguments.At(1)), API.Features.Doors.Commands.LockFor);
                        response = $"Locked for {float.Parse(arguments.At(1))}s !";
                        return true;
                    case "unlockafter" or "uf":
                        API.Features.Doors.Selected(plr, float.Parse(arguments.At(1)), API.Features.Doors.Commands.UnlockAfter);
                        response = $"Will unlock in {float.Parse(arguments.At(1))}s !";
                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) door (d) [subcommand] "          + '\n';

            response += "<color=yellow><b> special subcommands: </b></color> "    + '\n';
            response += "- select (s) [selection] [amount / distance (optional)]" + '\n';

            response += "<color=yellow><b> subcommands: </b></color> "            + '\n';
            response += "- info (i) "                                             + '\n';
            response += "- open (o) "                                             + '\n';
            response += "- close (c) "                                            + '\n';
            response += "- lock (l) "                                             + '\n';
            response += "- unlock (ul) "                                          + '\n';
            response += "- destroy (d) "                                          + '\n';
            response += "- lockfor (lf) [duration] "                              + '\n';
            response += "- unlockafter (uf) [duration] "                          + '\n';

            return false;
        }
    }
}