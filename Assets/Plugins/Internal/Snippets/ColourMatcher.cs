using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class ColourMatcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sourceText;
        [SerializeField] private TextMeshProUGUI targetText;
        private void Start()
        {
            TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Add(delegate (Object obj)
            {
                if (obj == sourceText)
                {
                    targetText.color = sourceText.color;
                }
            });
        }
    }
}