// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class AnimateMouth : Animate<MouthAnimator>
    {
        public override List<MouthAnimator> BodyParts => m_MonoBehaviour.Mouths;
    }
}