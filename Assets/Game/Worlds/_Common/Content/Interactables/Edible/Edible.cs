using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Edible : CreatureInteractable
    {
        #region Fields
        [SerializeField] private Diet diet;
        [SerializeField] private MinMax minMaxHunger;
        [SerializeField] private AudioClip eatSound;
        [SerializeField] private AudioMixerGroup soundEffectsMixer;
        [SerializeField] private UnityEvent onEat;

        private bool hasEaten;
        #endregion

        #region Properties
        public UnityEvent OnEat => onEat;
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
            if (!hasEaten)
            {
                hunger.Hunger += minMaxHunger.Random;
                AudioSourceUtility.PlayClipAtPoint(eatSound, transform.position, 1f, soundEffectsMixer);

                if (TryGetComponent(out NetworkDespawner despawner))
                {
                    despawner.Despawn();
                }
                else
                {
                    Destroy(gameObject);
                }

                onEat.Invoke();

                hasEaten = true;
            }
        }
        #endregion
    }
}