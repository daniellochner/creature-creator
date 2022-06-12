// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class AnimateEye : Animate<EyeAnimator>
    {
        public override List<EyeAnimator> BodyParts => m_MonoBehaviour.Eyes;
    }
}