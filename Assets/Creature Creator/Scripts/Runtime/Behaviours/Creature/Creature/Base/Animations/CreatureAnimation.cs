// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class CreatureAnimation : SceneLinkedSMB<CreatureAnimator>
    {
        public CreatureAnimator Creature => m_MonoBehaviour;

        public override void OnStart(Animator animator)
        {
            if (Creature.IsAnimated)
            {
                Setup();
            }
        }

        public virtual void Setup() { }
    }
}