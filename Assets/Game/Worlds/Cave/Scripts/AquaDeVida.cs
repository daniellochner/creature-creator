using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AquaDeVida : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject healPrefab;
        [SerializeField] private Ability swimAbility;
        #endregion

        #region Methods
        public void OnTriggerEnter(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && player.Abilities.Abilities.Contains(swimAbility))
            {
                Instantiate(healPrefab, player.transform.position, Quaternion.identity, Dynamic.Transform);
                player.Health.HealthPercentage = 1f;
            }
        }
        #endregion
    }
}