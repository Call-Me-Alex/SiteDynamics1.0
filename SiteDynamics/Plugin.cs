namespace SiteDynamics
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using PlayerRoles;
    using SiteDynamics.API.Enums;
    using SiteDynamics.EventHandlers;
    using System;
    using System.Collections.Generic;

    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "CallMeAlex";
        public override string Name { get; } = "Site Dynamics";
        public override string Prefix { get; } = "site_dynamics";
        public override Version Version { get; } = new(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new(8, 8, 1);

        private static readonly Plugin Singleton = new();

        public static Random randomGen = new Random();

        private ServerHandler serverHandler;
        private PlayerHandler playerHandler;
        private SCP914Handler scp914Handler;

        private Plugin()
        {
        }  
        
        public static Plugin Instance => Singleton;

        public override PluginPriority Priority { get; } = PluginPriority.Last;

        public override void OnEnabled()
        {
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            base.OnDisabled();
        }

        public override void OnReloaded()
        {
            base.OnReloaded();
        }

        private void RegisterEvents()
        {
            serverHandler = new ServerHandler();
            playerHandler = new PlayerHandler();
            scp914Handler = new SCP914Handler();

            // Server
            Exiled.Events.Handlers.Server.RestartingRound += serverHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.RespawningTeam += serverHandler.OnRespawningTeam;
            
            // Player
            Exiled.Events.Handlers.Player.Died += playerHandler.OnPlayerDeath;
            Exiled.Events.Handlers.Player.TriggeringTesla += playerHandler.OnTriggetingTesla;
            Exiled.Events.Handlers.Player.Left += playerHandler.OnLeft;

            // SCP 914
            Exiled.Events.Handlers.Scp914.Activating += scp914Handler.OnActivating;
        }

        private void UnregisterEvents()
        {
            // Server
            Exiled.Events.Handlers.Server.RestartingRound -= serverHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.RespawningTeam -= serverHandler.OnRespawningTeam;

            // Player
            Exiled.Events.Handlers.Player.Died -= playerHandler.OnPlayerDeath;
            Exiled.Events.Handlers.Player.TriggeringTesla -= playerHandler.OnTriggetingTesla;
            Exiled.Events.Handlers.Player.Left -= playerHandler.OnLeft;

            // SCP 914
            Exiled.Events.Handlers.Scp914.Activating -= scp914Handler.OnActivating;

            scp914Handler = null;
            playerHandler = null;
            serverHandler = null;
        }
    }
}