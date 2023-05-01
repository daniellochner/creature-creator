using System.Collections;
using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class FpsCounter : MonoBehaviourSingleton<FpsCounter>
    {
        #region Fields
        [SerializeField] private float updateRate;
        private TextMeshProUGUI fpsText;
        private int fps;
        #endregion

        #region Properties
        public int FPS
        {
            get => fps;
            private set
            {
                fpsText.text = (fps = value).ToString();

                if (fps >= 60)
                {
                    fpsText.color = Color.green;
                }
                else
                if (fps >= 30)
                {
                    fpsText.color = new Color(1f, 165f / 255f, 0f);
                }
                else
                if (fps >= 15)
                {
                    fpsText.color = Color.yellow;
                }
                else
                {
                    fpsText.color = Color.red;
                }
            }
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            fpsText = GetComponent<TextMeshProUGUI>();
        }
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / updateRate);
                FPS = (int)(1f / Time.smoothDeltaTime);
            }
        }
        #endregion
    }
}