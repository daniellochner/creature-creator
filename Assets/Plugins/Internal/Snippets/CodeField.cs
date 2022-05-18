using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TMP_InputField))]
    public class CodeField : MonoBehaviour
    {
        private void Awake()
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            inputField.asteriskChar = '•';
            inputField.onSelect.AddListener(delegate
            {
                inputField.contentType = TMP_InputField.ContentType.Standard;
                inputField.ForceLabelUpdate();
            });
            inputField.onDeselect.AddListener(delegate
            {
                inputField.contentType = TMP_InputField.ContentType.Password;
                inputField.ForceLabelUpdate();
            });
        }
    }
}