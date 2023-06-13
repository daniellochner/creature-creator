using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TickDamage : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private DamageReason damageReason;
        [SerializeField] private MinMax damage;
        [SerializeField] private float cooldown;
        private float timeLeft;
        #endregion

        #region Methods
        private void OnTriggerStay(Collider other)
        {
            if (IsServer)
            {
                CreatureBase creature = other.GetComponent<CreatureBase>();
                if (creature != null)
                {
                    TimerUtility.OnTimer(ref timeLeft, cooldown, Time.deltaTime, delegate
                    {
                        creature.Health.TakeDamage(damage.Random, damageReason);
                    });
                }
            }
        }
        #endregion
    }
}