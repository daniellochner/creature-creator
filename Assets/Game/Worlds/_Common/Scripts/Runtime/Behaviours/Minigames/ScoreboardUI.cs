using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ScoreboardUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ScoreUI scorePrefab;

        private Dictionary<string, ScoreUI> scores = new Dictionary<string, ScoreUI>();
        #endregion

        #region Properties
        private RectTransform RectTransform => transform as RectTransform;

        public float OffsetX
        {
            get => RectTransform.offsetMin.x;
            set => RectTransform.offsetMin = new Vector2(value, 0);
        }
        public float OffsetY
        {
            get => RectTransform.offsetMax.y;
            set => RectTransform.offsetMax = new Vector2(0, value);
        }
        #endregion

        #region Methods
        public void Add(string id, string displayName, int score)
        {
            ScoreUI scoreUI = Instantiate(scorePrefab, RectTransform);
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
            RectTransform.DestroyChildren();
        }
        #endregion
    }
}