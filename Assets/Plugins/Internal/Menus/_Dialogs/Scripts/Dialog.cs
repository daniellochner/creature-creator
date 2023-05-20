using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(1)]
    public class Dialog<M> : MenuSingleton<M> where M : Menu
    {
        #region Fields
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected Button ignoreButton;
        [SerializeField] protected Button closeButton;
        [SerializeField] protected GameObject dialog;

        private CanvasGroup canvasGroup;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        protected virtual void Start()
        {
            if (!IsOpen)
            {
                dialog.SetActive(false);
                animator.enabled = false;
            }
        }
        protected virtual void LateUpdate()
        {
            if (IsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ignoreButton.onClick.Invoke();
                }
            }
        }
        public override void Open(bool instant = false)
        {
            animator.enabled = true;
            dialog.SetActive(true);
            if (IsOpen)
            {
                ignoreButton.onClick.Invoke();
            }
            base.Open(instant);
        }
        public override void OnEndClose()
        {
            base.OnEndClose();
            dialog.SetActive(false);
            animator.enabled = false;

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        }
        #endregion
    }
}