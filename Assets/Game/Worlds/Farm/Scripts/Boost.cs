using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Boost : NetworkBehaviour
    {
        [SerializeField] private float boostSpeed;
        [SerializeField] private float boostTime;

        private void OnTriggerEnter(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null)
            {
                player.Speedup.SpeedUp(boostSpeed, boostTime);
            }
        }
    }
}