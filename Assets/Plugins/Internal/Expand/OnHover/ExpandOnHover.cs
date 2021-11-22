using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator), typeof(HoverUI))]
    public class ExpandOnHover : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Animator animator;
        [SerializeField] private HoverUI hoverUI;
        #endregion

        #region Methods
        private void Start()
        {
            hoverUI.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    animator.SetBool("Expanded", true);
                }
            });
            hoverUI.OnExit.AddListener(delegate
            {
                animator.SetBool("Expanded", false);
            });
        }
        #endregion
    }
}