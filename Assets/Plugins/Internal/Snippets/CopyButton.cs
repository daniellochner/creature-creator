using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Button))]
    public class CopyButton : MonoBehaviour
    {
        [SerializeField] private Button copiedButton;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(delegate
            {
                copiedButton.onClick.Invoke();
            });
        }
    }
}