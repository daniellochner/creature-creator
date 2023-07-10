// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    public class Emit : Ability
    {
        #region Fields
        [Header("Emit")]
        [SerializeField] private int maxEmitters;
        [SerializeField] private float duration;
        #endregion

        #region Properties
        public CreatureEmitter CreatureEmitter { get; private set; }

        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing;

        public List<BodyPartEmitter> Emitters
        {
            get
            {
                List<BodyPartEmitter> emitters = new List<BodyPartEmitter>();
                foreach (BodyPartConstructor constructor in CreatureEmitter.Constructor.BodyParts)
                {
                    BodyPartEmitter emitter = constructor.GetComponent<BodyPartEmitter>();
                    if (emitter != null)
                    {
                        if (emitter.Constructor.BodyPart.Abilities.Contains(this))
                        {
                            emitters.Add(emitter);
                            emitters.Add(emitter.Flipped);
                        }
                        if (emitters.Count >= maxEmitters)
                        {
                            break;
                        }
                    }
                }
                return emitters;
            }
        }
        #endregion

        #region Methods
        public override void Setup(CreatureAbilities creatureAbilities)
        {
            base.Setup(creatureAbilities);
            CreatureEmitter = creatureAbilities.GetComponent<CreatureEmitter>();
        }
        public override void Shutdown()
        {
            base.Shutdown();
            foreach (BodyPartEmitter emitter in Emitters)
            {
                CreatureEmitter.StopEmittingFrom(emitter);
            }
        }

        public override void OnPerform()
        {
            foreach (BodyPartEmitter emitter in Emitters)
            {
                CreatureEmitter.EmitFrom(emitter, duration);
            }
        }
        #endregion
    }
}