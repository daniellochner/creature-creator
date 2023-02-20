using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class ColourMatcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sourceText;
        [SerializeField] private TextMeshProUGUI targetText;
        private void Update()
        {
            targetText.color = sourceText.color;
        }
    }
}