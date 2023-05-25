
namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class PitifulWormCinematic : CCCinematic
    {
        #region Methods
        public override void Show()
        {
            base.Show();

            EditorManager.Instance.SetVisibility(false, 0f);
        }
        public override void End()
        {
            base.End();

            TutorialManager.Instance.SetVisibility(true, 0.25f);
            TutorialManager.Instance.Begin();
        }
        #endregion
    }
}