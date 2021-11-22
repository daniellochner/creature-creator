using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Version : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = GetComponent<TextMeshProUGUI>().text.Replace("<version>", Application.version);
        }
    }
}