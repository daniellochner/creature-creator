using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ScoreboardUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private RectTransform scoresRT;
        [SerializeField] private ScoreUI scorePrefab;

        private RectTransform rectTransform;
        #endregion

        #region Methods
        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        public void Setup(Minigame minigame)
        {
            Clear();

            
        }

        public void Clear()
        {
            scoresRT.DestroyChildren();
        }
        #endregion
    }
}