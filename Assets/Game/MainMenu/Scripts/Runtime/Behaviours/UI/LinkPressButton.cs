using UnityEngine;

namespace DanielLochner.Assets
{
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
            linked.transform.localScale = source.transform.localScale;
        }
        #endregion
    }
}