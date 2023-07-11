// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CityTeleport : NetworkBehaviour
    {
        public NetworkVariable<bool> IsVisible { get; set; } = new NetworkVariable<bool>(false);

        private void Start()
        {
            if (IsServer)
            {
                IsVisible.Value = CityReleaseManager.IsCityReleased;
            }
            gameObject.SetActive(IsVisible.Value);
        }
    }
}