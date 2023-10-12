using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class DelayHideText : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI sourceText;
        [SerializeField] private float delay = 5f;

        private Coroutine updateStatusCoroutine;
        #endregion

        #region Methods
        private void Awake()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        public void OnTextChanged(Object obj)
        {
            if (obj == sourceText)
            {
                if (updateStatusCoroutine != null)
                {
                    StopCoroutine(updateStatusCoroutine);
                }
                updateStatusCoroutine = this.Invoke(Hide, delay);
            }
        }
        private void Hide()
        {
            sourceText.text = "";
        }
        #endregion
    }
}