namespace SiteDynamics.API.Features
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Doors;
    using SiteDynamics.API.Enums;
    using System;

    public static class Zones
    {
        public static Selection selection { get; set; } = Selection.Closest;
        public static float selectionValue { get; set; } = 0.0f;

        public static class Commands
        {
            public static void Kill(Player sender, ZoneType zone)
            {
                foreach (Player player in Player.List)
                {
                    if (player.Zone == zone && player != sender)
                        Players.Commands.Kill(player);
                }
            }

            public static void Isolate(ZoneType zone) 
            {
                switch (zone) 
                {
                    case ZoneType.Surface:
                        Doors.Commands.Lock(DoorType.ElevatorGateA);
                        Doors.Commands.Lock(DoorType.ElevatorGateB);
                        break;
                    case ZoneType.Entrance:
                        Doors.Commands.Lock(DoorType.GateA);
                        Doors.Commands.Lock(DoorType.GateB);
                        Doors.Commands.Lock(DoorType.CheckpointEzHczA);
                        Doors.Commands.Lock(DoorType.CheckpointEzHczB);
                        break;
                    case ZoneType.HeavyContainment:
                        Doors.Commands.Lock(DoorType.CheckpointEzHczA);
                        Doors.Commands.Lock(DoorType.CheckpointEzHczB);
                        Doors.Commands.Lock(DoorType.ElevatorLczA);
                        Doors.Commands.Lock(DoorType.ElevatorLczB);
                        break;
                    case ZoneType.LightContainment:
                        Doors.Commands.Lock(DoorType.CheckpointLczA);
                        Doors.Commands.Lock(DoorType.CheckpointLczB);
                        break;
                }
            }

            
        }

        public static void Closest(Player player, Action<ZoneType> command)
        {
            command(player.Zone);
        }

        public static void Closest(Player player, Action<Player, ZoneType> command)
        {
            command(player, player.Zone);
        }
        
        public static void Near(Player sender, float distance, Action<ZoneType> command)
        {
            // TODO / TONOTDO :(
        }

        public static void Random()
        {
            
        }

        public static void All(Action<ZoneType> command)
        {
            command(ZoneType.LightContainment);
            command(ZoneType.HeavyContainment);
            command(ZoneType.Entrance);
            command(ZoneType.Surface);
        }

        public static void All(Player player, Action<Player, ZoneType> command)
        {
            command(player, ZoneType.LightContainment);
            command(player, ZoneType.HeavyContainment);
            command(player, ZoneType.Entrance);
            command(player, ZoneType.Surface);
        }

        public static void Selected(Player sender, Action<ZoneType> command)
        {
            switch (selection)
            {
                case Selection.Closest:
                    Closest(sender, command);
                    break;
                case Selection.Near:
                    //Near(sender, selectionValue, command);
                    break;
                case Selection.All:
                    All(command);
                    break;
                case Selection.Random:
                    //Random((int)selectionValue, command);
                    break;
            }
        }

        public static void Selected(Player sender, Action<Player, ZoneType> command)
        {
            switch (selection)
            {
                case Selection.Closest:
                    Closest(sender, command);
                    break;
                case Selection.Near:
                    //Near(sender, selectionValue, command);
                    break;
                case Selection.All:
                    All(sender, command);
                    break;
                case Selection.Random:
                    //Random((int)selectionValue, command);
                    break;
            }
        }

        public static void Select(string newSelection, float newValue = 0.0f)
        {
            switch (newSelection)
            {
                case "closest" or "c":
                    selection = Selection.Closest;
                    selectionValue = newValue;
                    break;
                case "near" or "n":
                    selection = Selection.Near;
                    selectionValue = newValue;
                    break;
                case "all" or "a":
                    selection = Selection.All;
                    selectionValue = newValue;
                    break;
                case "random" or "r":
                    selection = Selection.Random;
                    selectionValue = newValue;
                    break;
            }
        }
    }
}