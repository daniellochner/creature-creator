using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class FontSizeStep : MonoBehaviour
    {
        #region Fields
        [SerializeField] private AnimationCurve fontSize;
        private TextMeshProUGUI text;
        #endregion

        #region Methods
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        private void Start()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(delegate (Object obj)
            {
                if (obj == text)
                {
                    text.fontSize = fontSize.Evaluate(Mathf.InverseLerp(text.fontSizeMin, text.fontSizeMax, text.fontSize));
                }
            });
        }
        #endregion
    }
}