namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    public class Room : ICommand
    {
        public string Command { get; } = "room";

        public string[] Aliases { get; } = { "r" };

        public string Description { get; } = "Controls rooms | Supports: Selection";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"sd.{Command}"))
            {
                response = "You dont have permission to execute this command!";
                return false;
            }

            Exiled.API.Features.Player plr = Exiled.API.Features.Player.Get(sender);

            if (arguments.Count >= 1)
            {
                switch (arguments.At(0))
                {
                    default:
                        response = "Subcommand not recognized!";
                        return false;
                    case "info" or "i":
                        response = "Info: \n";
                        response += "- Selection: " + API.Features.Rooms.selection.ToString() + '\n';
                        return true;
                    case "select" or "s":
                        if (arguments.Count == 2)
                            API.Features.Rooms.Select(arguments.At(1));
                        else if (arguments.Count == 3)
                            API.Features.Rooms.Select(arguments.At(1), float.Parse(arguments.At(2)));
                        response = "Changed room selection!";
                        return true;
                    case "open" or "o":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Open);
                        response = "Opened room!";
                        return true;
                    case "close" or "c":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Close);
                        response = "Closed room!";
                        return true;
                    case "lock" or "l":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Lock);
                        response = "Enabled lock!";
                        return true;
                    case "unlock" or "ul":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Unlock);
                        response = "Disabled lock!";
                        return true;
                    case "destroy" or "d":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Destroy);
                        response = "Destroyed! (TODO)";
                        return true;
                    case "lockfor" or "lf":
                        API.Features.Rooms.Selected(plr, float.Parse(arguments.At(1)), API.Features.Rooms.Commands.LockFor);
                        response = $"Locked room for {float.Parse(arguments.At(1))}s !";
                        return true;
                    case "unlockafter" or "uf":
                        API.Features.Rooms.Selected(plr, float.Parse(arguments.At(1)), API.Features.Rooms.Commands.UnlockAfter);
                        response = $"Unlocked room for {float.Parse(arguments.At(1))}s !";
                        return true;
                    case "setcolor" or "sc":
                        API.Features.Rooms.Selected(plr, float.Parse(arguments.At(1)), float.Parse(arguments.At(2)), float.Parse(arguments.At(3)), API.Features.Rooms.Commands.SetColor);
                        response = "Setted new room color!";
                        return true;
                    /*
                    case "setshiftspeed" or "sss":
                        response = "WIP Setted new room color shift speed!";
                        return true;
                    */
                    case "blackout" or "b":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.Blackout);
                        response = "Powered off the room!";
                        return true;
                    case "blackoutfor" or "bf":
                        API.Features.Rooms.Selected(plr, float.Parse(arguments.At(1)), API.Features.Rooms.Commands.Blackout);
                        response = "Powered off the room!";
                        return true;
                    case "poweron" or "po":
                        API.Features.Rooms.Selected(plr, API.Features.Rooms.Commands.PowerOn);
                        response = "Powerer on the room!";
                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) room (r) [subcommand] "          + '\n';

            response += "<color=yellow><b> special subcommands: </b></color> "    + '\n';
            response += "- select (s) [selection] [amount / distance (optional)]" + '\n';

            response += "<color=yellow><b> subcommands: </b></color> "            + '\n';
            response += "- info (i) "                                             + '\n';
            response += "- open (o) "                                             + '\n';
            response += "- close (c) "                                            + '\n';
            response += "- lock (l) "                                             + '\n';
            response += "- unlock (ul) "                                          + '\n';
            response += "- lockfor (lf) [duration] "                              + '\n';
            response += "- unlockafter (uf) [duration] "                          + '\n';
            response += "- setcolor (sc) [r] [g] [b] "                            + '\n';
            //response += "- setshiftspeed (sss) "                                  + '\n';
            response += "- blackout (b) "                                         + '\n';
            response += "- blackoutfor (bf) [duration] "                          + '\n';
            response += "- poweron (po) "                                         + '\n';

            return false;
        }
    }
}