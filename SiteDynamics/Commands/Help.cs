namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using PlayerRoles;
    using System;

    public class Help : ICommand
    {
        public string Command { get; } = "help";

        public string[] Aliases { get; } = { "h" };

        public string Description { get; } = "Helps using the plugin";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count > 0) 
            {
                sbyte counter = 0;

                switch (arguments.At(0))
                {
                    default:
                        response = "Plugin info: \n";

                        response += "<color=yellow><b><u>Creator: </color></b></u> " + Plugin.Instance.Author + "\n";
                        response += "<color=yellow><b><u>Version: </color></b></u> " + Plugin.Instance.Version + "\n";
                        response += "<color=yellow><b><u>Required Exiled version: </color></b></u> " + Plugin.Instance.RequiredExiledVersion + "\n\n";
                        return false;
                    case "selection":
                        response = "<color=yellow><b>Selections:</b></color> \n";
                        response += "closest (c)" + '\n';
                        response += "near (n)" + '\n';
                        response += "all (a)" + '\n';
                        response += "random (r)" + '\n';

                        return true;
                    case "amount":
                        response = "amount = integer number (Example: 10) \n";

                        return true;
                    case "distance":
                        response = "distance = float number (Example: 20.0) \n";

                        return true;
                    case "ignorestaff":
                        response = "ignorestaff = boolean value (Example: true/false) \n";

                        return true;
                    case "ignoresender":
                        response = "ignoresender = boolean value (Example: true/false) \n";

                        return true;
                    case "class":
                        response = "<color=yellow><b>Classes:</b></color> \n";
                        response += "classd (cd)" + '\n';
                        response += "scientist (sc)" + '\n';
                        response += "ntf (ntf)" + '\n';
                        response += "chaos (chaos)" + '\n';

                        return true;
                    case "rgb":
                        response = "rgb = 3 integer numbers (Example: 10 15 20) \n";
                        response += "r = red amount" + '\n';
                        response += "g = green amount" + '\n';
                        response += "b = blue amount" + '\n';

                        return true;
                    case "duration":
                        response = "duration = float number (Example: 10.0) \n";

                        return true;
                    case "command":
                        response = "command = any command visible by writing sitedynamics (sd) in the remote admin console \n";

                        return true;
                    case "soundID":
                        response = "soundID = (unknown) integer number (Example: 17) \n";

                        return true;
                    case "effectcategoryID":
                        response = "<color=yellow><b>Effect categories IDs:</b></color> \n";

                        foreach (string enumValue in Enum.GetNames(typeof(EffectCategory)))
                        {
                            response += $"({counter}) " + enumValue + '\n';
                            counter++;
                        }

                        return true;
                    case "roleID":
                        response = "<color=yellow><b>Roles IDs:</b></color> \n";

                        foreach (string enumValue in Enum.GetNames(typeof(RoleTypeId)))
                        {
                            response += $"({counter}) " + enumValue + '\n';
                            counter++;
                        }

                        return true;
                    case "taskID":
                        response = "execute this command to see: sitedynamics (sd) task (t) show (s)";

                        return true;
                    case "playerID":
                        response = "<color=yellow><b>Player IDs:</b></color> \n";

                        foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
                        {
                            response += $"({player.Id}) " + player.Nickname + '\n';
                        }

                        return true;
                    case "teamID":
                        response = "<color=yellow><b>Teams IDs:</b></color> \n";

                        response += "(1) Team 1" + '\n';
                        response += "(2) Team 2" + '\n';
                        response += "(3) Team 3" + '\n';
                        response += "(4) Team 4" + '\n';

                        return true;
                }
            }

            response = "Usage: sitedynamics (sd) help (h) [parameter name] " + '\n';
            response += "<color=yellow><b> parameter names: </b></color> " + '\n';

            response += "- selection " + '\n';
            response += "- amount " + '\n';
            response += "- distance " + '\n';

            response += "- ignorestaff " + '\n';
            response += "- ignoresender " + '\n';
            
            response += "- class " + '\n';
            response += "- rgb " + '\n';
            response += "- duration " + '\n';
            response += "- command " + '\n';

            response += "- soundID " + '\n';
            response += "- effectcategoryID " + '\n';
            response += "- roleID " + '\n';
            response += "- taskID " + '\n';
            response += "- playerID " + '\n';
            response += "- teamID " + '\n';

            return false;
        }
    }
}