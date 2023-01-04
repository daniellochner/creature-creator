using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class BlinkingGraphic : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax range = new MinMax(0, 1);
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isGlobal = false;

        private Graphic graphic;
        private float timeElapsed;
        #endregion

        #region Properties
        public bool IsBlinking { get; set; } = true;
        #endregion

        #region Methods
        private void Awake()
        {
            graphic = GetComponent<Graphic>();
        }
        private void Update()
        {
            if (IsBlinking)
            {
                //float a = range.min + Mathf.PingPong(timeElapsed, range.max - range.min);

                float x = isGlobal ? Time.time : timeElapsed;
                float a = (range.max - range.min) * 0.5f * (Mathf.Cos(Mathf.PI * x) + 1f) + range.min;

                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, a);
                timeElapsed += Time.deltaTime * speed;
            }
        }
        #endregion
    }
}
