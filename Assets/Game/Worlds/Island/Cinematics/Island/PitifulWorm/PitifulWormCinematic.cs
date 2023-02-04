using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class PitifulWormCinematic : Cinematic
    {
        public override void Begin()
        {
            base.Begin();

            EditorManager.Instance.SetVisibility(false, 0f);
            TutorialManager.Instance.SetVisibility(false, 0f);
            BlackBars.Instance.SetVisibility(true);

            gameObject.SetActive(true);
            Player.Instance.Constructor.Body.gameObject.SetActive(false);
        }
        public override void End()
        {
            base.End();

            BlackBars.Instance.SetVisibility(false, 1f);
            TutorialManager.Instance.SetVisibility(true, 1f);

            gameObject.SetActive(false);
            Player.Instance.Constructor.Body.gameObject.SetActive(true);

            TutorialManager.Instance.Begin();
        }
    }
}