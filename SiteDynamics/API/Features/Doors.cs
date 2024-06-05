namespace SiteDynamics.API.Features
{
    using API.Enums;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Doors;
    using Exiled.API.Interfaces;
    using System;

    public static class Doors
    {
        public static Selection selection { get; set; } = Selection.Closest;
        public static float selectionValue { get; set; } = 0.0f;

        public static class Commands
        {
            public static void Open(Door door)
            {
                door.IsOpen = true;
            }

            public static void Open(DoorType doorType)
            {
                Door.Get(doorType).IsOpen = true;
            }

            public static void Close(Door door)
            {
                door.IsOpen = false;
            }

            public static void Close(DoorType doorType)
            {
                Door.Get(doorType).IsOpen = false;
            }

            public static void ToggleOpen(Door door)
            {
                if (door.IsOpen)
                    door.IsOpen = false;
                else
                    door.IsOpen = true;
            }

            public static void Lock(Door door)
            {
                door.ChangeLock(Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void Lock(DoorType doorType)
            {
                Door.Get(doorType).ChangeLock(DoorLockType.AdminCommand);
            }

            public static void Unlock(Door door)
            {
                door.Unlock();
            }

            public static void Unlock(DoorType doorType)
            {
                Door.Get(doorType).Unlock();
            }

            public static void ToggleLocked(Door door)
            {
                if (door.IsLocked)
                    door.Unlock();
                else
                    door.ChangeLock(Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void LockFor(Door door, float time)
            {
                door.Lock(time, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void UnlockAfter(Door door, float time)
            {
                door.Unlock(time, Exiled.API.Enums.DoorLockType.AdminCommand);
            }

            public static void Destroy(Door door)
            {
                if (door is IDamageableDoor damageableDoor)
                    damageableDoor.IsDestroyed = true;
            }
        }

        public static void Closest(Player sender, Action<Door> command)
        {
            command(Door.GetClosest(sender.Position, out float distance));
        }

        public static void Closest<T>(Player sender, T value, Action<Door, T> command)
        {
            command(Door.GetClosest(sender.Position, out float distance), value);
        }
      
        public static void Near(Player sender, float distance, Action<Door> command)
        {
            foreach (Door door in Door.List)
                if ((door.Position - sender.Position).magnitude < distance)
                    command(door);
        }

        public static void Near<T>(Player sender, float distance, T value, Action<Door, T> command)
        {
            foreach (Door door in Door.List)
                if ((door.Position - sender.Position).magnitude < distance)
                    command(door, value);
        }

        public static void Random(int amount, Action<Door> action)
        {
            for (int i = amount; i > 0; i--)
                action(Door.Random());
        }

        public static void Random<T>(int amount, T value, Action<Door, T> action)
        {
            for (int i = amount; i > 0; i--)
                action(Door.Random(), value);
        }

        public static void All(Action<Door> command)
        {
            foreach (Door door in Door.List)
                command(door);
        }

        public static void All<T>(T value, Action<Door, T> command)
        {
            foreach (Door door in Door.List)
                command(door, value);
        }

        public static void Selected(Player sender, Action<Door> command)
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

        public static void Selected<T>(Player sender, T value, Action<Door, T> command)
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