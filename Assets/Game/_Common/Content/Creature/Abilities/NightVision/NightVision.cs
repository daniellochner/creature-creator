// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/NightVision")]
    public class NightVision : Ability
    {
        [Header("NightVision")]
        [SerializeField] private PostProcessProfile defaultProfile;
        [SerializeField] private PostProcessProfile nightVisionProfile;

        public override void OnAdd()
        {
            PostProcessManager.Instance.BlendToProfile(defaultProfile, 0.25f);
            MinimapManager.Instance.SetVisibility(SettingsManager.Data.Map);
        }
        public override void OnRemove()
        {
            PostProcessManager.Instance.BlendToProfile(nightVisionProfile, 0.25f);
            MinimapManager.Instance.SetVisibility(false);
        }
    }
}