using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCCinematic : Cinematic
    {
        #region Methods
        private void SetVisibility(bool isVisible)
        {
            if (Player.Instance)
            {
                Player.Instance.Camera.enabled = !isVisible;
            }
        }
        public override void Show()
        {
            base.Show();
            SetVisibility(true);
        }
        public override void Hide()
        {
            base.Hide();
            SetVisibility(false);
        }
        #endregion
    }
}