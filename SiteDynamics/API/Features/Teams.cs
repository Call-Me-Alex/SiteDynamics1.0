namespace SiteDynamics.API.Features
{
    using Exiled.API.Features;
    using PlayerRoles;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class Teams
    {
        public static bool areCreated { get; set; } = false;

        public static List<List<Player>> teams { get; set; } = new List<List<Player>>();
        public static List<RoleTypeId> teamRoles { get; set; } = new List<RoleTypeId>();

        public static bool ignoreSender { get; set; } = Plugin.Instance.Config.IgnoreSender;
        public static bool ignoreStaff { get; set; } = Plugin.Instance.Config.IgnoreStaff;

        public static class Commands
        {
            public static void SetTeam(Player player, int team)
            {
                teams[team - 1].Add(player);
            }

            public static void RemoveTeam(Player player)
            {
                foreach (List<Player> team in teams)
                    if (team.Contains(player))
                        team.Remove(player);
            }

            public static void Create(Player sender, string[] teamsRoles, out string response) 
            {
                int teamsCount = teamsRoles.Length;
                Mathf.Clamp(teamsCount, 1, 4);

                for (int i = 0; i < teamsCount; i++)
                    teams.Add(new List<Player>());
                
                if (areCreated) 
                {
                    response = "Teams have already been created!";
                    return;
                }
                
                List<Player> players = Player.List.Where(player => 
                    !(ignoreStaff && player.RemoteAdminAccess) && 
                    !(ignoreSender && player == sender)).ToList();
               
                if (players.Count == 0 || players.Count < teamsCount)
                {
                    response = "Not enough players!";
                    return;
                }

                int playersPerTeam = players.Count / teamsCount;
                int playersLeftover = players.Count % teamsCount;
                int playerIndex = 0;

                for (int teamIndex = 1; teamIndex <= teamsCount; teamIndex++)
                {
                    int currentPlayerCount = playersPerTeam + (playersLeftover > 0 ? 1 : 0);
                    playersLeftover--;

                    for (int i = 0; i < currentPlayerCount; i++)
                    {
                        SetTeam(players[playerIndex], teamIndex);
                        playerIndex++;
                    }

                    string teamRole = teamsRoles[teamIndex - 1];
                    switch (teamRole.ToLower())
                    {
                        default:
                            response = "Not a valid team class!";
                            return;
                        case "classd" or "cd":
                            teamRoles.Add(RoleTypeId.ClassD);
                            break;
                        case "scientist" or "sc":
                            teamRoles.Add(RoleTypeId.Scientist);
                            break;
                        case "chaos":
                            teamRoles.Add(RoleTypeId.ChaosRifleman);
                            break;
                        case "ntf":
                            teamRoles.Add(RoleTypeId.NtfPrivate);
                            break;
                    }
                }

                for (int teamIndex = 0; teamIndex < teamsCount; teamIndex++)
                    AllInTeam(teamIndex + 1, teamRoles[teamIndex], Players.Commands.SetRole);

                areCreated = true;

                response = $"Succesfully created {teamsCount} teams!";
            }

            public static void Fill()
            {
                if (!areCreated) {  return; }

                List<Player> playersToAdd = new List<Player>();

                foreach (Player player in Player.List)
                    foreach (List<Player> team in teams)
                        if (!team.Contains(player))
                        {
                            playersToAdd.Add(player);
                            break;
                        }

                int totalTeamPlayers = 0;
                foreach (List<Player> team in teams)
                    totalTeamPlayers += team.Count;

                int average = totalTeamPlayers / teams.Count;

                for (int teamIndex = 0; teamIndex < teams.Count; teamIndex++)
                {
                    int playersNeeded = average - teams[teamIndex].Count;
                    for (int i = 0; i < playersNeeded; i++)
                    {
                        if (playersToAdd.Count > 0)
                        {
                            SetTeam(playersToAdd[0], teamIndex);
                            playersToAdd.RemoveAt(0);
                        }
                    }
                }
            }

            public static void Balance()
            {
                if (!areCreated) { return; }

                int totalPlayers = 0;
                foreach (List<Player> team in teams)
                {
                    totalPlayers += team.Count;
                }

                int average = totalPlayers / teams.Count;

                for (int teamIndex = 0; teamIndex < teams.Count; teamIndex++)
                {
                    int playersNeeded = average - teams[teamIndex].Count;

                    if (playersNeeded > 0)
                    {
                        for (int otherTeamIndex = 0; otherTeamIndex < teams.Count; otherTeamIndex++)
                        {
                            if (otherTeamIndex != teamIndex && teams[otherTeamIndex].Count > average)
                            {
                                while (teams[teamIndex].Count < average && teams[otherTeamIndex].Count > average)
                                {
                                    
                                    Player playerToMove = teams[otherTeamIndex][0];
                                    SetTeam(playerToMove, teamIndex);
                                    teams[otherTeamIndex].Remove(playerToMove);
                                }
                            }
                        }
                    }
                }
            }

            public static void Clear()
            {
                if (areCreated)
                {
                    teams.Clear();
                    teamRoles.Clear();
                    areCreated = false;
                }
            }

            public static void ForceRespawn(int team)
            {
                foreach (Player player in teams[team])
                    API.Features.Players.Commands.SetRole(player, teamRoles[team]);
            }

            public static void ForceRespawn()
            {
                foreach (List<Player> team in teams)
                    foreach (Player player in team)
                        API.Features.Players.Commands.SetRole(player, teamRoles[teams.IndexOf(team)]);
            }

            public static void Respawn(int team)
            {
                foreach (Player player in teams[team])
                    if (player.Role == RoleTypeId.Spectator)
                        Players.Commands.SetRole(player, teamRoles[team]);
            }

            public static void Respawn()
            {
                foreach (List<Player> team in teams)
                    foreach (Player player in team)
                        if (player.Role == RoleTypeId.Spectator)
                            Players.Commands.SetRole(player, teamRoles[teams.IndexOf(team)]);
            }

        }

        public static void Closest()
        {
            
        }

        public static void Near()
        {

        }

        public static void Random()
        {

        }

        public static void AllInTeam(int team, Action<Player> command)
        {
            foreach (Player player in teams[team - 1])
                command(player);
        }

        public static void AllInTeam<T>(int team, T value, Action<Player, T> command)
        {
            foreach (Player player in teams[team - 1])
                command(player, value);
        }

        public static void All<T>(T value, Action<Player, T> command)
        {
            foreach (List<Player> team in teams)
                AllInTeam(teams.IndexOf(team), value, command);
        }
    }
}