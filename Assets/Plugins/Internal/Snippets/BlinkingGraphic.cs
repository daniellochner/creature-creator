using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class BlinkingGraphic : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax range = new MinMax(0, 1);
        [SerializeField] private float speed = 1;

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
                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, range.min + Mathf.PingPong(timeElapsed, range.max - range.min));
                timeElapsed += Time.deltaTime * speed;
            }
        }
        #endregion
    }
}
