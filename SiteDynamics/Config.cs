namespace SiteDynamics
{
    using Exiled.API.Interfaces;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using Exiled.API.Enums;

    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        public bool IgnoreStaff { get; set; } = false;

        public bool IgnoreSender { get; set; } = false;

        public bool KeepSettingsAfterRoundRestart { get; set; } = false;

        public List<RoomType> SafeRandomTPBlacklist { get; set; } = new List<RoomType> { RoomType.HczTestRoom, RoomType.HczTesla, RoomType.EzCollapsedTunnel, RoomType.Lcz173, RoomType.Pocket, RoomType.EzShelter, RoomType.Hcz079 };

        public bool SafeRandomTPExcludeDecontaminated { get; set; } = true;
    }
}