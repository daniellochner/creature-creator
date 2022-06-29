// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class CreatureAnimation : SceneLinkedSMB<CreatureAnimator>
    {
        public CreatureAnimator Creature => m_MonoBehaviour;
    }
}