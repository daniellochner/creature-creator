using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class Dialog<M> : MenuSingleton<M> where M : Menu
    {
        #region Fields
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected Button ignoreButton;
        [SerializeField] protected Button closeButton;
        #endregion

        #region Methods
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
        #endregion
    }
}