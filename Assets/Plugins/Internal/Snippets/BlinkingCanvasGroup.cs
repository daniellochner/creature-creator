using UnityEngine;

namespace DanielLochner.Assets
{
    public class BlinkingCanvasGroup : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax range = new MinMax(0, 1);
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isGlobal = false;

        private float timeElapsed;
        #endregion

        #region Properties
        public bool IsBlinking { get; set; } = true;

        public CanvasGroup CanvasGroup { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
        private void Update()
        {
            if (IsBlinking)
            {
                float x = isGlobal ? Time.time : timeElapsed;
                float a = (range.max - range.min) * 0.5f * (Mathf.Cos(Mathf.PI * x) + 1f) + range.min;

                CanvasGroup.alpha = a;
                timeElapsed += Time.deltaTime * speed;
            }
        }
        #endregion
    }
}
