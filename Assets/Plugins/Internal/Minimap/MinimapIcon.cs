using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class MinimapIcon : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool isVisible = true;
        [SerializeField] private bool track = true;
        [SerializeField] private bool lockPos = false;
        [SerializeField] private bool lockRot = false;
        [SerializeField] private bool isButton = true;
        [SerializeField] private bool isTarget = false;
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
                minimapIconUI = MinimapManager.Instance.Add(icon, color, () => onClick.Invoke(), isButton, isTarget);
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