using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Edible : CreatureInteractable
    {
        #region Fields
        [SerializeField] private Diet diet;
        [SerializeField] private MinMax minMaxHunger;
        [SerializeField] private AudioClip eatSound;

        private bool hasEaten;
        #endregion

        #region Methods
        public override bool CanInteract(Interactor interactor)
        {
            CreatureStatistics statistics = interactor.GetComponent<CreatureConstructor>().Statistics;
            return base.CanInteract(interactor) && !hasEaten && (statistics.Diet == diet || statistics.Diet == Diet.Omnivore);
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);

            CreatureHunger hunger = interactor.GetComponent<CreatureHunger>();
            if (!hasEaten && hunger.Hunger < 1f)
            {
                hunger.Hunger += minMaxHunger.Random;
                AudioSource.PlayClipAtPoint(eatSound, transform.position);
                DisposeServerRpc();
                hasEaten = true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisposeServerRpc()
        {
            NetworkObject.Despawn();
        }
        #endregion
    }
}