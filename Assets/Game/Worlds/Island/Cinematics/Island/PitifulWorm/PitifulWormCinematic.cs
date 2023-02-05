using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class PitifulWormCinematic : CCCinematic
    {
        public override void Begin()
        {
            base.Begin();

            SetVisibility(true);

            BlackBars.Instance.SetVisibility(true, 0f);
            EditorManager.Instance.SetVisibility(false, 0f);

            TutorialManager.Instance.SetVisibility(false, 0f);
        }

        public override void End()
        {
            base.End();

            SetVisibility(false);

            BlackBars.Instance.SetVisibility(false, 0.25f);

            TutorialManager.Instance.SetVisibility(true, 0.25f);
            TutorialManager.Instance.Begin();

            SetMusic(true, 0.25f);
        }
    }
}