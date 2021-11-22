using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    /// <summary>
    /// Filter mouse click raycasts which fall outside of (or within - when invert flag is toggled) a specified rectangle on the screen.
    /// </summary>
    public class UIRaycastClickFilter : MonoBehaviour, ICanvasRaycastFilter, IPointerClickHandler
    {
        #region Fields
        [SerializeField] private RectTransform rect;
        [SerializeField] private bool invertFilter;
        [Space]
        [SerializeField] private UnityEvent onClicked;
        #endregion

        #region Methods
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (!isActiveAndEnabled || !rect)
            {
                return true;
            }

            bool valid;
            if (eventCamera)
            {
                valid = !RectTransformUtility.RectangleContainsScreenPoint(rect, sp, eventCamera);
            }
            else
            {
                valid = !RectTransformUtility.RectangleContainsScreenPoint(rect, sp);
            }
            return valid;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if ((eventData.pointerPressRaycast.isValid && invertFilter) || (!eventData.pointerPressRaycast.isValid && !invertFilter))
            {
                onClicked.Invoke();
            }
        }
        #endregion
    }
}