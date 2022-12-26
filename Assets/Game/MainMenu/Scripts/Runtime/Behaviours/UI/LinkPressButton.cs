using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(PressButton))]
    public class LinkPressButton : MonoBehaviour
    {
        #region Fields
        [SerializeField] private PressButton linked;
        private PressButton source;
        #endregion

        #region Methods
        private void Awake()
        {
            source = GetComponent<PressButton>();
        }
        private void Update()
        {
            linked.IsPressed = source.IsPressed;
        }
        #endregion
    }
}