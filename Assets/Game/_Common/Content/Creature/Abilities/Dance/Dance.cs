using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    public class Dance : Ability
    {
        #region Fields
        [Header("Dance")]
        [SerializeField] private string soundId;
        [SerializeField] private string animationTrigger;
        [Space]
        [SerializeField] private string music;
        [SerializeField] private string composer;
        [SerializeField] private string url;
        #endregion

        #region Properties
        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing;
        #endregion

        #region Methods
        public override void OnPerform()
        {
            // Animation
            if (!string.IsNullOrEmpty(animationTrigger))
            {
                Player.Instance.Animator.Params.SetTrigger(animationTrigger);
                Player.Instance.Mover.StopMoving();
            }

            // Music
            if (!string.IsNullOrEmpty(soundId))
            {
                Player.Instance.Effects.PlaySound(soundId, 1f);
                NotificationsManager.Notify($"{music} ~ {composer}", () => Application.OpenURL(url));
            }
        }
        #endregion
    }
}