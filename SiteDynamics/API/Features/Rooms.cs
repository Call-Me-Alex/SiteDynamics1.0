namespace SiteDynamics.API.Features
{
    using Exiled.API.Features;
    using Exiled.API.Features.Doors;
    using SiteDynamics.API.Enums;
    using System;
    using UnityEngine;
    using YamlDotNet.Core.Tokens;

    public static class Rooms
    {
        public static Selection selection { get; set; } = Selection.Closest;
        public static float selectionValue { get; set; } = 0.0f;

        public static class Commands
        {
            public static void Open(Room room)
            {
                foreach (Door door in room.Doors)
                    API.Features.Doors.Commands.Open(door);
            }

            public static void Close(Room room)
            {
                foreach (Door door in room.Doors)
                    API.Features.Doors.Commands.Close(door);
            }

            public static void Lock(Room room)
            {
                room.LockDown(float.MaxValue, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void LockFor(Room room, float time)
            {
                room.LockDown(time, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void Unlock(Room room)
            {
                room.UnlockAll();
            }

            public static void Destroy(Room room)
            {
                foreach (Door door in room.Doors)
                    API.Features.Doors.Commands.Destroy(door);
            }

            public static void UnlockAfter(Room room, float time)
            {
                foreach (Door door in room.Doors)
                    API.Features.Doors.Commands.UnlockAfter(door, time);
            }

            public static void Blackout(Room room)
            {
                room.Blackout(-1.0f, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void Blackout(Room room, float time)
            {
                room.Blackout(time, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void PowerOn(Room room)
            {
                room.Blackout(float.MinValue, Exiled.API.Enums.DoorLockType.AdminCommand);
                room.UnlockAll();
            }

            public static void SetColor(Room room, float r, float g, float b)
            {
                room.Color = new Color(r, g, b);
            }

            public static void Updatecolor(Room room, float shiftspeed)
            {
                
            }
  
        }

        public static void Closest(Player player, Action<Room> command)
        {
            command(player.CurrentRoom);
        }

        public static void Closest<T>(Player player, T value, Action<Room, T> command)
        {
            command(player.CurrentRoom, value);
        }

        public static void Closest<T>(Player player, T valueX, T valueY, T valueZ, Action<Room, T, T, T> command)
        {
            command(player.CurrentRoom, valueX, valueY, valueZ);
        }
        

        public static void Near(Player sender, float distance, Action<Room> command)
        {
            foreach (Room room in Room.List)
                if ((room.Position - sender.Position).magnitude < distance)
                    command(room);
        }

        public static void Near<T>(Player sender, float distance, T value, Action<Room, T> command)
        {
            foreach (Room room in Room.List)
                if ((room.Position - sender.Position).magnitude < distance)
                    command(room, value);
        }

        public static void Near<T>(Player sender, float distance, T valueX, T valueY, T valueZ, Action<Room, T, T, T> command)
        {
            foreach (Room room in Room.List)
                if ((room.Position - sender.Position).magnitude < distance)
                    command(room, valueX, valueY, valueZ);
        }

        public static void Random(int amount, Action<Room> action)
        {
            for (int i = amount; i > 0; i--)
                action(Room.Random());
        }

        public static void Random<T>(int amount, T value, Action<Room, T> action)
        {
            for (int i = amount; i > 0; i--)
                action(Room.Random(), value);
        }

        public static void Random<T>(int amount, T valueX, T valueY, T valueZ, Action<Room, T, T, T> action)
        {
            for (int i = amount; i > 0; i--)
                action(Room.Random(), valueX, valueY, valueZ);
        }
        

        public static void All(Action<Room> command)
        {
            foreach (Room room in Room.List)
                command(room);
        }

        public static void All<T>(T value, Action<Room, T> command)
        {
            foreach (Room room in Room.List)
                command(room, value);
        }

        public static void All<T>(T valueX, T valueY, T valueZ, Action<Room, T, T, T> command)
        {
            foreach (Room room in Room.List)
                command(room, valueX, valueY, valueZ);
        }

        public static void Selected(Player sender, Action<Room> command)
        {
            switch (selection)
            {
                case Selection.Closest:
                    Closest(sender, command);
                    break;
                case Selection.Near:
                    Near(sender, selectionValue, command);
                    break;
                case Selection.All:
                    All(command);
                    break;
                case Selection.Random:
                    Random((int)selectionValue, command);
                    break;
            }
        }

        public static void Selected<T>(Player sender, T value, Action<Room, T> command)
        {
            switch (selection)
            {
                case Selection.Closest:
                    Closest(sender, value, command);
                    break;
                case Selection.Near:
                    Near(sender, selectionValue, value, command);
                    break;
                case Selection.All:
                    All(value, command);
                    break;
                case Selection.Random:
                    Random((int)selectionValue, value, command);
                    break;
            }
        }

        public static void Selected<T>(Player sender, T valueX, T valueY, T valueZ, Action<Room, T, T, T> command)
        {
            switch (selection)
            {
                case Selection.Closest:
                    Closest(sender, valueX, valueY, valueZ, command);
                    break;
                case Selection.Near:
                    Near(sender, selectionValue, valueX, valueY, valueZ, command);
                    break;
                case Selection.All:
                    All(valueX, valueY, valueZ, command);
                    break;
                case Selection.Random:
                    Random((int)selectionValue, valueX, valueY, valueZ, command);
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