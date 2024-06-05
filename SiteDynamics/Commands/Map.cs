namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using PlayerRoles;
    using System;
    using UnityEngine;

    public class Map : ICommand
    {
        public string Command { get; } = "map";

        public string[] Aliases { get; } = { "m" };

        public string Description { get; } = "Controls the map";

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
                    case "playsound" or "ps":
                        if (arguments.Count == 2)
                            API.Features.Map.Commands.PlayAmbientSound();
                        else if (arguments.Count == 3)
                            API.Features.Map.Commands.PlayAmbientSound(int.Parse(arguments.At(2)));
                        response = "Played a sound!";
                        return true;
                    case "placeblood" or "pb":
                        API.Features.Map.Commands.PlaceBlood(plr.Position, plr.CameraTransform.forward);
                        response = "Placed blood!";
                        return true;
                    case "placetantrum" or "pt":
                        API.Features.Map.Commands.PlaceTantrum(plr.Position);
                        response = "Placed tantrum!";
                        return true;
                    case "toggletesla" or "tt":
                        API.Features.Map.Commands.ToggleTesla(plr);
                        response = "Toggled tesla!";
                        return true;

                }
            }

            response = "Usage: sitedynamics (sd) map (m) [subcommand] " + '\n';
            response += "<color=yellow><b> subcommands: </b></color> "  + '\n';
            response += "- playsound (ps) [soundID (optional)] "        + '\n';
            response += "- placeblood (pb) "                            + '\n';
            response += "- placetantrum (pt) "                          + '\n';
            response += "- toggletesla (tt) "                           + '\n';

            return false;
        }
    }
}