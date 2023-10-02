using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class CopyText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void Copy()
        {
            GUIUtility.systemCopyBuffer = text.text;
        }
    }
}