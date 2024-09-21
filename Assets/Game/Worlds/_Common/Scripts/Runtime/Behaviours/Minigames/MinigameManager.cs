using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameManager : MonoBehaviourSingleton<MinigameManager>
    {
        #region Fields
        [SerializeField] private ScoreboardUI scoreboard;
        [Space]
        [SerializeField] private RectTransform infoRT;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [Space]
        [SerializeField] private TextMeshProUGUI playText;
        [SerializeField] private Toggle playToggle;
        [SerializeField] private CanvasGroup paginationCG;
        #endregion

        #region Properties
        public ScoreboardUI Scoreboard => scoreboard;

        public float InfoOffset
        {
            get => infoRT.offsetMax.y;
            set => infoRT.offsetMax = new Vector2(0, value);
        }

        public MinigamePad CurrentPad { get; set; }
        public Minigame CurrentMinigame { get; set; }
        #endregion

        #region Methods
        public void SetTitle(string title)
        {
            titleText.text = title;
            titleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(title));
        }
        public void SetSubtitle(string subtitle, Color colour)
        {
            subtitleText.text = subtitle;
            subtitleText.color = colour;
            subtitleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(subtitle));
        }
        public void SetPlay(string text, bool isInteractable)
        {
            playText.text = text;
            playToggle.interactable = isInteractable;
        }
        public void SetSwap(bool canSwap)
        {
            paginationCG.interactable = canSwap;
            paginationCG.alpha = canSwap ? 1f : 0.5f;
        }
        #endregion
    }
}