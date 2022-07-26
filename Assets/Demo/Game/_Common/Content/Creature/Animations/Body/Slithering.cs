// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Slithering : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float slitherAngle;
        [SerializeField] private float slitherWidth;
        [SerializeField] private float slitherRate;
        private float x;
        #endregion

        #region Properties
        private Transform Head
        {
            get => Creature.Constructor.Bones[Creature.Constructor.Bones.Count - 1];
        }
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            x = 0f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float t = animator.GetFloat("%LSpeed");
            float f = x * Mathf.PI * slitherRate;

            float rot = -t * slitherAngle * Mathf.Sin(f);
            float pos =  t * slitherWidth * Mathf.Cos(f);
            
            Head.localRotation = Quaternion.Euler(0, rot, 0);
            Head.localPosition = Vector3.right * pos;

            x += Time.deltaTime;
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Head.localPosition = Vector3.zero;
            Head.localRotation = Quaternion.identity;
        }
        #endregion
    }
}