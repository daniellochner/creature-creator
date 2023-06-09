using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ScoreboardUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ScoreUI scorePrefab;

        private RectTransform rectTransform;

        private Dictionary<string, ScoreUI> scores = new Dictionary<string, ScoreUI>();
        #endregion

        #region Properties
        public float OffsetX
        {
            get => rectTransform.offsetMin.x;
            set => rectTransform.offsetMin = new Vector2(value, 0);
        }
        public float OffsetY
        {
            get => rectTransform.offsetMax.y;
            set => rectTransform.offsetMax = new Vector2(0, value);
        }
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

        public void Add(string id, string displayName, int score)
        {
            ScoreUI scoreUI = Instantiate(scorePrefab, rectTransform);
            scoreUI.Setup(id, score, displayName);

            scores.Add(id, scoreUI);
        }
        public void Remove(string id)
        {
            ScoreUI scoreUI = scores[id];
            scores.Remove(id);
            Destroy(scoreUI.gameObject);
        }
        public void Set(string id, int score)
        {
            scores[id].Score = score;
        }

        public void Sort(bool isAscendingOrder)
        {
            List<ScoreUI> s = new List<ScoreUI>(scores.Values);

            s.Sort((x, y) => x.Score.CompareTo(y.Score) * (isAscendingOrder ? 1 : -1));

            for (int i = 0; i < scores.Count; i++)
            {
                s[i].transform.SetSiblingIndex(i);
            }
        }

        public void Clear()
        {
            scores.Clear();
            rectTransform.DestroyChildren();
        }
        #endregion
    }
}