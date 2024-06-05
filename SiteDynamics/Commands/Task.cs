namespace SiteDynamics.Commands
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using PluginAPI.Core;

    using System;

    public class Task : ICommand
    {
        public string Command { get; } = "task";

        public string[] Aliases { get; } = { "t" };

        public string Description { get; } = "Executes commands automatically";

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
                    case "show" or "s":
                        response = "Tasks: ";
                        foreach (MEC.CoroutineHandle coroutine in API.Features.Tasks.coroutines)
                            response += "[" + (coroutine.Key - 1).ToString() + "] " + coroutine.ToString() + '\n';
                        return true;
                    case "begin" or "b":
                        bool success = API.Features.Tasks.Commands.CallPeriodically(float.Parse(arguments.At(1)), "sitedynamics", arguments.Segment(2), sender, out string result);
                        response = result;
                        return success;
                    case "end" or "e":
                        API.Features.Tasks.Commands.Stop(int.Parse(arguments.At(1)));
                        response = "Stopped task!";
                        return true;
                    case "pause" or "p":
                        API.Features.Tasks.Commands.Pause(int.Parse(arguments.At(1)));
                        response = "Paused task!";
                        return true;
                    case "resume" or "r":
                        API.Features.Tasks.Commands.Resume(int.Parse(arguments.At(1)));
                        response = "Resumed task!";
                        return true;

                }
            }

            response = "Usage: sitedynamics (sd) task (t) [subcommand] " + '\n';
            response += "<color=yellow><b> subcommands: </b></color> "   + '\n';
            response += "- show (s) "                                    + '\n';
            response += "- begin (b) [duration] [command] "              + '\n';
            response += "- end (e) [taskID] "                            + '\n';
            response += "- pause (p) [taskID] "                          + '\n';
            response += "- resume (r) [taskID] "                         + '\n';

            return false;
        }
    }
}