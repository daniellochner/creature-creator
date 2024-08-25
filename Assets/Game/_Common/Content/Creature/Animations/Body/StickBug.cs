// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class StickBug : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float ver;
        [SerializeField] private float hor;
        [SerializeField] private float t;
        private float x = 0f;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            x = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // https://www.desmos.com/calculator/y8r6zd0cgn
            float h = hor * (Mathf.Sin(x * Mathf.PI));
            float v = ver * (-Mathf.Cos((x - 0.5f) * Mathf.PI * 2f) / 2f - 0.5f);

            Creature.Constructor.Root.localPosition = new Vector3(h, v, 0f);

            x += Time.deltaTime * t;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.Constructor.Root.localPosition = Vector3.zero;
            Creature.Effector.StopSoundsSelf();
        }
        #endregion
    }
}