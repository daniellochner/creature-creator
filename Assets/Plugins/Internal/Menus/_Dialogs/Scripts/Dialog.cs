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
        #endregion
    }
}