// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/NightVision")]
    public class NightVision : Ability
    {
        [Header("Night Vision")]
        [SerializeField] private Ability see;
        [SerializeField] private PostProcessProfile defaultProfile;
        [SerializeField] private PostProcessProfile blindedProfile;
        [SerializeField] private PostProcessProfile nightVisionProfile;
        [SerializeField] private Light spotlightPrefab;

        private Light spotlight;

        public override void OnAdd()
        {
            PostProcessManager.Instance.BlendToProfile(nightVisionProfile, 0.25f);
            MinimapManager.Instance.SetVisibility(SettingsManager.Data.Map);

            spotlight = Instantiate(spotlightPrefab, Player.Instance.Camera.MainCamera.transform);
            float target = spotlight.intensity;
            PostProcessManager.Instance.InvokeOverTime(delegate (float p)
            {
                spotlight.intensity = p * target;
            }, 
            0.25f);
        }
        public override void OnRemove()
        {
            PostProcessManager.Instance.BlendToProfile(Player.Instance.Abilities.Abilities.Contains(see) ? defaultProfile : blindedProfile, 0.25f);
            MinimapManager.Instance.SetVisibility(false);

            Destroy(spotlight.gameObject);
        }
    }
}