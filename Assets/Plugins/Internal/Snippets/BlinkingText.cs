using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BlinkingText : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Vector2 range = new Vector2(0, 1);
        [SerializeField] private float speed = 1;

        private TextMeshProUGUI text;
        private float timeElapsed;
        #endregion

        #region Properties
        public bool IsBlinking { get; set; } = false;
        #endregion

        #region Methods
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        private void Update()
        {
            if (IsBlinking)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, range.x + Mathf.PingPong(timeElapsed, range.y - range.x));
                timeElapsed += Time.deltaTime * speed;
            }
        }

        public void Restart()
        {
            IsBlinking = true;
            timeElapsed = 0;
        }
        #endregion
    }
}