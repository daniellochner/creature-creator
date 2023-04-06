// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/NightVision")]
    public class NightVision : Ability
    {
        #region Fields
        [Header("Night Vision")]
        [SerializeField] private Ability see;
        [SerializeField] private PostProcessProfile defaultProfile;
        [SerializeField] private PostProcessProfile blindedProfile;
        [SerializeField] private PostProcessProfile nightVisionProfile;
        [SerializeField] private AudioClip enableSound;
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField] private Light spotlightPrefab;

        private Light spotlight;
        private bool isEnabled;
        #endregion

        #region Methods
        public override void OnPerform()
        {
            SetNightVision(isEnabled = !isEnabled);
        }
        public override void OnRemove()
        {
            if (isEnabled)
            {
                SetNightVision(isEnabled = false);
            }
        }

        private void SetNightVision(bool isEnabled)
        {
            if (isEnabled)
            {
                PostProcessManager.Instance.BlendToProfile(nightVisionProfile, 0.25f);

                spotlight = Instantiate(spotlightPrefab, Player.Instance.Camera.MainCamera.transform);
                float target = spotlight.intensity;
                PostProcessManager.Instance.InvokeOverTime(delegate (float p)
                {
                    spotlight.intensity = p * target;
                },
                0.25f);

                AudioSourceUtility.PlayClipAtPoint(enableSound, null, 1, mixer);
            }
            else
            {
                PostProcessManager.Instance.BlendToProfile(Player.Instance.Abilities.Abilities.Contains(see) ? defaultProfile : blindedProfile, 0.25f);
                Destroy(spotlight.gameObject);
            }
        }
        #endregion
    }
}