using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class PitifulWormCinematic : CCCinematic
    {
        public override void Begin(bool fade)
        {
            base.Begin(fade);

            TutorialManager.Instance.SetVisibility(false, 0f);
        }

        public override void End(bool fade)
        {
            CinematicManager.Instance.IsInCinematic = false;

            SetVisibility(false);

            BlackBars.Instance.SetVisibility(false, 0.25f);

            TutorialManager.Instance.SetVisibility(true, 0.25f);
            TutorialManager.Instance.Begin();

            FadeMusic(true, 0.25f);
        }
    }
}