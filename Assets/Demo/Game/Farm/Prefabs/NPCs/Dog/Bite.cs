// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Bite : SceneLinkedSMB<CreatureAnimator>
    {
        private Transform Head
        {
            get => m_MonoBehaviour.Constructor.Bones[m_MonoBehaviour.Constructor.Bones.Count - 1];
        }


    }
}