// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Flame : Emission
    {
        #region Fields
        [Header("Flame")]
        [SerializeField] private Fire firePrefab;
        [SerializeField] private MinMax minMaxDamage;

        private BoxCollider boxCollider;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            boxCollider = GetComponent<BoxCollider>();
        }

        public override void OnTick()
        {
            Collider[] colliders = Physics.OverlapBox(boxCollider.transform.TransformPoint(boxCollider.center), boxCollider.size, boxCollider.transform.rotation);
            foreach (Collider collider in colliders)
            {
                CreatureBase creature = collider.GetComponent<CreatureBase>();
                if (creature != null)
                {
                    bool ignore = (creature is CreaturePlayer) && (creature.NetworkObjectId == Emitter.CreatureEmitter.NetworkObjectId || !WorldManager.Instance.EnablePVP);
                    if (!ignore)
                    {
                        if (!creature.Burner.IsBurning.Value)
                        {
                            Fire fire = Instantiate(firePrefab, Dynamic.Transform);
                            fire.Burn(creature);

                            fire.NetworkObject.SpawnWithOwnership(OwnerClientId, true);
                        }

                        creature.Health.TakeDamage(minMaxDamage.Random, DamageReason.Fire);
                    }
                }
            }
        }
        #endregion
    }
}