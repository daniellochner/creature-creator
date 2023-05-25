// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BoxCreature : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor displayPrefab;
        [SerializeField] private Click click;

        private CreatureConstructor creatureConstructor;
        #endregion

        #region Methods
        public void Spawn(CreatureData creatureData)
        {
            creatureConstructor = Instantiate(displayPrefab, transform.position, transform.rotation, transform);
            creatureConstructor.Construct(creatureData);

            creatureConstructor.Realign();

            this.InvokeOverTime(delegate (float progress)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(0, 1, progress);
            }, 0.5f);
        }
        public void ReplaceWithRagdoll()
        {
            if (creatureConstructor.gameObject.activeSelf == false)
            {
                return;
            }
            creatureConstructor.gameObject.SetActive(false);
            click.enabled = false;

            CreatureConstructor ragdoll = creatureConstructor.GetComponent<CreatureRagdoll>().Generate();
            gameObject.layer = LayerMask.NameToLayer("Creature");
        }
        #endregion
    }
}