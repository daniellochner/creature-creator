using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BuildBattleCinematic : MinigameCinematic
    {
        public override void Show()
        {
            base.Show();
            MinigameManager.Instance.InfoOffset -= 100;
        }
        public override void Hide()
        {
            base.Hide();
            MinigameManager.Instance.InfoOffset += 100;
        }
    }
}