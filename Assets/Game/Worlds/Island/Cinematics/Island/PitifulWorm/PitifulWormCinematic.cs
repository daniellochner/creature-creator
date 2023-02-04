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

            BlackBars.Instance.SetVisibility(false, 0.25f);
            TutorialManager.Instance.SetVisibility(true, 0.25f);

            gameObject.SetActive(false);
            Player.Instance.Constructor.Body.gameObject.SetActive(true);

            TutorialManager.Instance.Begin();

            string music = SettingsManager.Data.InGameMusic.ToString();
            if (SettingsManager.Data.InGameMusic == Settings.InGameMusicType.None)
            {
                music = null;
            }
            MusicManager.Instance.FadeTo(music, 0f, 1f);
        }
    }
}