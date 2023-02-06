using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class PitifulWormCinematic : CCCinematic
    {
        public override void Show()
        {
            base.Show();

            TutorialManager.Instance.SetVisibility(false, 0f);
        }

        public override void Hide()
        {
            base.Hide();

            TutorialManager.Instance.SetVisibility(true, 0.25f);
            TutorialManager.Instance.Begin();
        }
    }
}