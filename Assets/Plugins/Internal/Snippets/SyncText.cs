using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class SyncText : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI sourceText;
        [SerializeField] private TextMeshProUGUI targetText;
        #endregion

        #region Methods
        private void Awake()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        private void OnTextChanged(Object obj)
        {
            if (obj == sourceText)
            {
                targetText.text = sourceText.text;
                targetText.ForceMeshUpdate();
            }
        }
        #endregion
    }
}