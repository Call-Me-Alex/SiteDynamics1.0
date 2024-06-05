namespace SiteDynamics.EventHandlers
{
    using Exiled.Events.EventArgs.Player;
    using PluginAPI.Events;

    internal sealed class PlayerHandler
    {
        public void OnPlayerDeath(DiedEventArgs ev) 
        {
            if (API.Features.Round.respawnOnDeath)
                API.Features.Players.Commands.SetRole(ev.Player, API.Features.Round.respawnRole);
        }

        public void OnTriggetingTesla(TriggeringTeslaEventArgs ev)
        {
            if (API.Features.Map.disabledTeslas.Contains(ev.Tesla))
                ev.DisableTesla = true;
        }

        public void OnLeft(LeftEventArgs ev)
        {
            API.Features.Teams.Commands.RemoveTeam(ev.Player);
        }
    }
}