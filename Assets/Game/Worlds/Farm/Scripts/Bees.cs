using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Bees : MonoBehaviour
    {
        [SerializeField] private MinMax stingDamage;
        [SerializeField] private float stingCooldown;
        private float stingTimeLeft;

        private void OnTriggerStay(Collider other)
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