using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Bees : NetworkBehaviour
    {
        [SerializeField] private MinMax stingDamage;
        [SerializeField] private float stingCooldown;
        private float stingTimeLeft;

        private void OnTriggerStay(Collider other)
        {
            if (IsServer)
            {
                CreatureBase creature = other.GetComponent<CreatureBase>();
                if (creature != null)
                {
                    TimerUtility.OnTimer(ref stingTimeLeft, stingCooldown, Time.deltaTime, delegate
                    {
                        creature.Health.TakeDamage(stingDamage.Random);
                    });
                }
            }
        }
    }
}