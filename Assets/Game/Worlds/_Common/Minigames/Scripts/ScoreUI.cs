using UnityEngine;
using TMPro;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ScoreUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private int score;
        #endregion

        #region Properties
        public string Id
        {
            get; set;
        }
        public string DisplayName
        {
            get => displayNameText.text;
            set => displayNameText.text = value;
        }
        public int Score
        {
            get => score;
            set
            {
                score = value;
                scoreText.text = score.ToString();
            }
        }
        #endregion

        #region Methods
        public void Setup(string id, int score, string displayName = null)
        {
            Id = id;
            Score = score;

            if (displayName != null)
            {
                DisplayName = displayName;
            }
            else
            {
                DisplayName = id;
            }
        }
        #endregion
    }
}