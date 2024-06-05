namespace SiteDynamics.EventHandlers
{
    using Exiled.Events.EventArgs.Scp914;

    internal sealed class SCP914Handler
    {
        public void OnActivating(ActivatingEventArgs ev)
        {
            ev.IsAllowed = !API.Features.Round.prevent914;
        }
    }
}