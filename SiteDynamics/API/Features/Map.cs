namespace SiteDynamics.API.Features
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Map
    {
        public static List<TeslaGate> disabledTeslas {  get; set; } = new List<TeslaGate>();

        public static class Commands
        {
            public static void PlayAmbientSound()
            {
                Exiled.API.Features.Map.PlayAmbientSound();
            }

            public static void PlayAmbientSound(int id)
            {
                Exiled.API.Features.Map.PlayAmbientSound(id);
            }

            public static void PlaceBlood(Vector3 position, Vector3 direction)
            {
                Exiled.API.Features.Map.PlaceBlood(position, direction);
            }

            public static void PlaceTantrum(Vector3 position)
            {
                Exiled.API.Features.Map.PlaceTantrum(position, true);
            }

            public static void Explode(Vector3 position, ProjectileType projectileType)
            {
                Exiled.API.Features.Map.Explode(position, projectileType);
            }

            public static void ToggleTesla(Player player) 
            {
                foreach(TeslaGate teslaGate in TeslaGate.List)
                    if (teslaGate.IsPlayerInIdleRange(player) || teslaGate.IsPlayerInTriggerRange(player))
                        if (disabledTeslas.Contains(teslaGate))
                            disabledTeslas.Remove(teslaGate);
                        else
                            disabledTeslas.Add(teslaGate);
            }
        }

        
    }
}