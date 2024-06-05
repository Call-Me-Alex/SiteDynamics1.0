namespace SiteDynamics.Commands
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions; 

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SiteDynamicsParent : ParentCommand
    {
        public SiteDynamicsParent() => LoadGeneratedCommands();

        public override string Command { get; } = "sitedynamics";
        public override string[] Aliases { get; } = { "sd" };
        public override string Description { get; } = "Lets you control the facility!";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Help());

            RegisterCommand(new Task());
            RegisterCommand(new Team());

            RegisterCommand(new Round());
            RegisterCommand(new Map());

            RegisterCommand(new Player());
            RegisterCommand(new Door());
            RegisterCommand(new Room());
            RegisterCommand(new Zone());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "\nPlease enter a valid subcommand:";

            foreach (ICommand command in AllCommands)
            {
                if (sender.CheckPermission($"sd.{command.Command}"))
                {
                    response += $"\n\n<color=yellow><b>- {command.Command} ({string.Join(", ", command.Aliases)})</b></color>\n<color=white>{command.Description}</color>";
                }
            }

            return false;
        }
    }
}