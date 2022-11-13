using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] public CanvasGroup canvasGroup;

        public void Setup(Achievement achievement)
        {
            name = achievement.id;
            icon.sprite = achievement.unlockedIcon;
        }
    }
}