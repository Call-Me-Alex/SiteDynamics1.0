namespace SiteDynamics.API.Features
{
    using API.Enums;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using PlayerRoles;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class Players
    {
        public static Selection selection { get; set; } = Selection.Closest;
        public static float selectionValue { get; set; } = 0.0f;

        public static bool ignoreSender { get; set; } = Plugin.Instance.Config.IgnoreSender;
        public static bool ignoreStaff { get; set; } = Plugin.Instance.Config.IgnoreStaff;

        public static class Commands
        {
            public static void RandomTeleport(Player player)
            {
                player.RandomTeleport(typeof(Room));
            }

            public static void SafeRandomTeleport(Player player)
            {
                Room room;
                do
                    room = Room.Random();
                while (Plugin.Instance.Config.SafeRandomTPBlacklist.Contains(room.Type) || (Plugin.Instance.Config.SafeRandomTPExcludeDecontaminated && room.Zone == ZoneType.LightContainment && Exiled.API.Features.Map.IsLczDecontaminated));

                player.Teleport(room);
            }

            public static void Teleport(Player player, Vector3 position)
            {
                player.Teleport(position);
            }

            public static void DeleteInventory(Player player)
            {
                player.ClearInventory();
            }

            public static void SetRole(Player player, RoleTypeId role)
            {
                player.Role.Set(role);
            }

            public static void Kill(Player player) 
            {
                player.Kill(DamageType.Unknown);
            }

            public static void GiveEffect(Player player, EffectCategory effectCategory) 
            {
                player.ApplyRandomEffect(effectCategory);
            }
        }

        public static void Closest(Player sender, Action<Player> command)
        {
            foreach (Player player in Player.List)
                if (player != sender && (player.Position - sender.Position).magnitude < 10)
                {
                    if (ignoreStaff && player.RemoteAdminAccess)
                        break;

                    command(player);
                    break;
                }
        }

        public static void Closest<T>(Player sender, T value, Action<Player, T> command)
        {
            foreach (Player player in Player.List)
                if (player != sender && (player.Position - sender.Position).magnitude < 10)
                {
                    if (ignoreStaff && player.RemoteAdminAccess)
                        break;

                    command(player, value);
                    break;
                }
        }

        public static void Near(Player sender, float distance, Action<Player> command)
        {
            foreach (Player player in Player.List)
            {
                if (player == sender || (player.Position - sender.Position).magnitude >= distance)
                    continue;

                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                command(player);
            }
        }

        public static void Near<T>(Player sender, float distance, T value, Action<Player, T> command)
        {
            foreach (Player player in Player.List)
            {
                if (player == sender || (player.Position - sender.Position).magnitude >= distance)
                    continue;

                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                command(player, value);
            }
        }

        public static void Random(Player sender, int amount, Action<Player> command)
        {
            List<Player> players = new List<Player>();
            List<Player> pickedPlayers = new List<Player>();

            foreach (Player player in Player.List)
            {
                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                if (ignoreSender && player == sender)
                    continue;

                players.Add(player);
            }

            Mathf.Clamp(amount, 0, players.Count);

            for (int i = 0; i < amount; i++)
            {
                Player player = players.ElementAt(Plugin.randomGen.Next(0, players.Count));
                pickedPlayers.Add(player);
                players.Remove(player);
            }

            foreach (Player player in pickedPlayers)
                command(player);
        }

        public static void Random<T>(Player sender, int amount, T value, Action<Player, T> command)
        {
            List<Player> players = new List<Player>();
            List<Player> pickedPlayers = new List<Player>();

            foreach (Player player in Player.List)
            {
                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                if (ignoreSender && player == sender)
                    continue;

                players.Add(player);
            }

            Mathf.Clamp(amount, 0, players.Count);

            for (int i = 0; i < amount; i++)
            {
                Player player = players.ElementAt(Plugin.randomGen.Next(0, players.Count));
                if (player == null)
                    break;
                pickedPlayers.Add(player);
                players.Remove(player);
            }

            foreach (Player player in pickedPlayers)
                command(player, value);
        }

        public static void All(Player sender, Action<Player> command)
        {
            foreach (Player player in Player.List)
            {
                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                if (ignoreSender && player == sender)
                    continue;

                command(player);
            }
                
        }

        public static void All<T>(Player sender, T value, Action<Player, T> command)
        {
            foreach (Player player in Player.List)
            {
                if (ignoreStaff && player.RemoteAdminAccess)
                    continue;

                if (ignoreSender && player == sender)
                    continue;

                command(player, value);
            }
        }

        public static void Selected(Player sender, Action<Player> command)
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
                    All(sender, command);
                    break;
                case Selection.Random:
                    Random(sender, (int)selectionValue, command);
                    break;
            }
        }

        public static void Selected<T>(Player sender, T value, Action<Player, T> command)
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
                    All(sender, value, command);
                    break;
                case Selection.Random:
                    Random(sender, (int)selectionValue, value, command);
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