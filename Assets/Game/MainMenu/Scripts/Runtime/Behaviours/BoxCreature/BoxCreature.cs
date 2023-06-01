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

        private CreatureConstructor creature;
        #endregion

        #region Methods
        public void Spawn(CreatureData creatureData)
        {
            creature = Instantiate(displayPrefab, transform.position, transform.rotation, transform);
            creature.Construct(creatureData);

            creature.Realign();

            creature.GetComponent<CreatureOptimizer>().Optimize();

            this.InvokeOverTime(delegate (float progress)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(0, 1, progress);
            }, 0.5f);
        }
        public void ReplaceWithRagdoll()
        {
            if (creature.gameObject.activeSelf == false)
            {
                return;
            }
            creature.gameObject.SetActive(false);
            click.enabled = false;

            CreatureConstructor ragdoll = creature.GetComponent<CreatureRagdoll>().Generate();
            gameObject.layer = LayerMask.NameToLayer("Creature");
        }
        #endregion
    }
}