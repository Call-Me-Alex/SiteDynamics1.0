namespace SiteDynamics.EventHandlers
{
    using Exiled.Events.EventArgs.Server;
    using MEC;
    using PlayerRoles;
    using SiteDynamics.API.Enums;
    using System.Collections.Generic;
    using Exiled.API.Features;

    internal sealed class ServerHandler
    {
        public void OnRestartingRound()
        {
            if (!Plugin.Instance.Config.KeepSettingsAfterRoundRestart)
            {
                // Task
                if (API.Features.Tasks.coroutines != null)
                    API.Features.Tasks.Commands.StopAll();
                API.Features.Tasks.coroutines = new List<CoroutineHandle>();

                // Teams
                API.Features.Teams.areCreated = false;
                API.Features.Teams.teams = new List<List<Player>>();
                API.Features.Teams.teamRoles = new List<RoleTypeId>();
                API.Features.Teams.ignoreSender = Plugin.Instance.Config.IgnoreSender;
                API.Features.Teams.ignoreStaff = Plugin.Instance.Config.IgnoreStaff;

                // Round
                API.Features.Round.preventRespawns = false;
                API.Features.Round.prevent914 = false;
                API.Features.Round.respawnOnDeath = false;
                API.Features.Round.respawnRole = 0;
                API.Features.Round.recentWavePlayers = new List<Player>();

                // Map
                API.Features.Map.disabledTeslas = new List<TeslaGate>();

                // Players
                API.Features.Players.selection = Selection.Closest;
                API.Features.Players.selectionValue = 0.0f;
                API.Features.Players.ignoreSender = Plugin.Instance.Config.IgnoreSender;
                API.Features.Players.ignoreStaff = Plugin.Instance.Config.IgnoreStaff;

                // Doors
                API.Features.Doors.selection = Selection.Closest;
                API.Features.Doors.selectionValue = 0.0f;

                // Rooms
                API.Features.Rooms.selection = Selection.Closest;
                API.Features.Rooms.selectionValue = 0.0f;

                // Zones
                API.Features.Zones.selection = Selection.Closest;
                API.Features.Zones.selectionValue = 0.0f;
            }
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            API.Features.Round.recentWavePlayers = ev.Players;
            ev.IsAllowed = !API.Features.Round.preventRespawns;
        }

        public void OnPlayerDeath(Exiled.Events.EventArgs.Player.DiedEventArgs ev) 
        {
            if (API.Features.Round.respawnOnDeath)
                API.Features.Players.Commands.SetRole(ev.Player, API.Features.Round.respawnRole);
        }
    }
}