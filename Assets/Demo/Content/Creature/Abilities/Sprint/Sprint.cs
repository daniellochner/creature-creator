// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Sprint")]
    public class Sprint : Ability
    {
        [Header("Sprint")]
        [SerializeField] private PostProcessProfile sprintProfile;
    }
}