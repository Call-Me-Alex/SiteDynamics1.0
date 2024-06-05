namespace SiteDynamics.API.Features
{
    using PlayerRoles;
    using Exiled.API.Features;
    using System.Collections.Generic;

    public static class Round
    {
        public static bool preventRespawns { get; set; } = false;

        public static bool prevent914 { get; set; } = false;

        public static bool respawnOnDeath { get; set; } = false;
        public static RoleTypeId respawnRole { get; set; } = 0;

        public static List<Player> recentWavePlayers { get; set; } = new List<Player>();

        public static class Commands
        {
            public static void ToggleRespawnLock()
            {
                preventRespawns = !preventRespawns;
            }

            public static void Toggle914Lock()
            {
                prevent914 = !prevent914;
            }

            public static void ToggleRespawOnDeath(RoleTypeId newRole = RoleTypeId.ClassD)
            {
                respawnOnDeath = !respawnOnDeath;
                respawnRole = newRole;
            }

            public static void DespawnWave()
            {
                foreach (Player player in recentWavePlayers) 
                    API.Features.Players.Commands.SetRole(player, RoleTypeId.Spectator);
            }
        }
    }
}