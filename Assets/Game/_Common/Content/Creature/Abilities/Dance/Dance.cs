using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    public class Dance : Ability
    {
        [Header("Dance")]
        [SerializeField] private string soundId;
        [SerializeField] private string animationTrigger;
        [Space]
        [SerializeField] private string music;
        [SerializeField] private string composer;
        [SerializeField] private string url;

        public override bool CanPerform => base.CanPerform && !EditorManager.Instance.IsEditing;

        public override void OnPerform()
        {
            Player.Instance.Animator.Params.SetTrigger(animationTrigger);
            Player.Instance.Effects.PlaySound(soundId);

            Player.Instance.Mover.StopMoving();

            NotificationsManager.Notify($"{music} ~ {composer}", () => Application.OpenURL(url));
        }
    }
}