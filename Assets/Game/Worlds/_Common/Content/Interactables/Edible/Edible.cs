using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Edible : CreatureInteractable
    {
        #region Fields
        [SerializeField] private Diet diet;
        [SerializeField] private MinMax minMaxHunger;
        [SerializeField] private AudioClip eatSound;
        [SerializeField] private AudioMixerGroup soundEffectsMixer;

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
                AudioSourceUtility.PlayClipAtPoint(eatSound, transform.position, 1f, soundEffectsMixer);
                hasEaten = true;

                DisposeServerRpc();
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