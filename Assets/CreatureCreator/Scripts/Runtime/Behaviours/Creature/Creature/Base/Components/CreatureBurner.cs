// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureBurner : NetworkBehaviour
    {
        public NetworkVariable<bool> IsBurning { get; set; } = new NetworkVariable<bool>();
    }
}