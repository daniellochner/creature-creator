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
        #endregion

        #region Properties
        public MinimapIconUI MinimapIconUI { get; private set; }
        #endregion

        #region Methods
        private void OnEnable()
        {
            if (MinimapIconUI == null)
            {
                MinimapIconUI = MinimapManager.Instance.Add(icon, color, () => onClick.Invoke(), isButton, isTarget, isImportant);
                MinimapManager.Instance.Track(this, MinimapIconUI, lockPos, lockRot);
                enabled = isVisible;
            }
            else
            {
                MinimapIconUI.gameObject.SetActive(true);
            }
        }
        private void OnDisable()
        {
            if (MinimapIconUI != null)
            {
                MinimapIconUI.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            if (MinimapIconUI != null)
            {
                Destroy(MinimapIconUI.gameObject);
            }
        }
        private void Update()
        {
            if (MinimapIconUI != null && track)
            {
                MinimapManager.Instance.Track(this, MinimapIconUI, lockPos, lockRot);
            }
        }
        #endregion
    }
}