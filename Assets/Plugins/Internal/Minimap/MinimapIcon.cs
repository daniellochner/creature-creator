using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class MinimapIcon : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isVisible = true;
        [SerializeField] private bool track;
        [SerializeField] private bool lockPos;
        [SerializeField] private bool lockRot;
        [SerializeField] private bool isButton;
        [SerializeField] private bool isTarget;
        [SerializeField] private bool isImportant;
        [Space]
        [SerializeField] private Sprite icon;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private UnityEvent onClick;

        private MinimapIconUI minimapIconUI;
        #endregion

        #region Methods
        private void OnEnable()
        {
            if (minimapIconUI == null)
            {
                minimapIconUI = MinimapManager.Instance.Add(icon, color, () => onClick.Invoke(), isButton, isTarget, isImportant);
                MinimapManager.Instance.Track(this, minimapIconUI, lockPos, lockRot);
                enabled = isVisible;
            }
            else
            {
                minimapIconUI.gameObject.SetActive(true);
            }
        }
        private void OnDisable()
        {
            if (minimapIconUI != null)
            {
                minimapIconUI.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            if (minimapIconUI != null)
            {
                Destroy(minimapIconUI.gameObject);
            }
        }
        private void Update()
        {
            if (minimapIconUI != null && track)
            {
                MinimapManager.Instance.Track(this, minimapIconUI, lockPos, lockRot);
            }
        }
        #endregion
    }
}