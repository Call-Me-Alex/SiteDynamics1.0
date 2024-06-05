namespace SiteDynamics.API.Features
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using MEC;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Tasks
    {
        public static List<MEC.CoroutineHandle> coroutines { get; set; } = new List<CoroutineHandle>();

        public static class Commands
        {
            public static bool CallPeriodically(float time, string commandName, ArraySegment<string> commandArguments, ICommandSender sender, out string response)
            {
                ICommand commandToExecute = null;

                foreach (var commandHandlerPair in Plugin.Instance.Commands)
                {
                    if (commandHandlerPair.Key == typeof(RemoteAdminCommandHandler))
                    {
                        foreach (var commandPair in commandHandlerPair.Value)
                        {
                            if (commandPair.Value.Command == commandName.ToLower())
                            {
                                commandToExecute = commandPair.Value;
                                break;
                            }
                        }
                    }
                }

                if (commandToExecute == null)
                {
                    response = "Command inserted not found!";
                    return false;
                }

                if (!sender.CheckPermission($"sd.{commandArguments.At(0)}"))
                {
                    response = "You dont have permission to execute this command!";
                    return false;
                }

                coroutines.Add(MEC.Timing.RunCoroutine(RepeatCommand(time, commandToExecute, commandArguments, sender)));

                response = "Successfully started command execution!";
                return true;
            }

            public static void Stop(int id)
            {
                MEC.Timing.KillCoroutines(coroutines.ElementAt(id));
                coroutines.Remove(coroutines.ElementAt(id));
            }

            public static void StopAll()
            {
                foreach (CoroutineHandle handle in coroutines)
                {
                    MEC.Timing.KillCoroutines(handle);
                    //coroutines.Remove(handle);
                }
            }

            public static void Pause(int id)
            {
                MEC.Timing.PauseCoroutines(coroutines.ElementAt(id));
            }

            public static void Resume(int id)
            {
                MEC.Timing.ResumeCoroutines(coroutines.ElementAt(id));
            }
        }

        private static IEnumerator<float> RepeatCommand(float time, ICommand command, ArraySegment<string> arguments, ICommandSender sender)
        {
            while (true)
            {
                command.Execute(arguments, sender, out string response);
                yield return MEC.Timing.WaitForSeconds(time);
            }
        }


    }
}