using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class ListElementSlider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Fields
        [SerializeField] private UnityEvent onSlideBegin;
        [SerializeField] private UnityEvent onSlideEnd;
        [SerializeField] private UnityEvent<float> onSlide;
        [Space]
        [SerializeField] private float threshold;

        private Vector3 pointerStartPos, sliderStartPos;
        private float displacement;
        private bool isSliding;
        #endregion

        #region Properties
        private RectTransform RectTransform => transform as RectTransform;
        #endregion

        #region Methods
        public void OnBeginDrag(PointerEventData eventData)
        {
            pointerStartPos = Input.mousePosition;
            sliderStartPos = RectTransform.localPosition;

            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                isSliding = true;
            }

            onSlideBegin?.Invoke();

            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isSliding)
            {
                displacement = Mathf.Max(0, Mathf.Min(Input.mousePosition.x - pointerStartPos.x, threshold * RectTransform.rect.width));
                RectTransform.localPosition = Vector3.right * (sliderStartPos.x + displacement);

                onSlide?.Invoke(displacement / (threshold * RectTransform.rect.width));
            }
            else
            {
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isSliding)
            {
                if (displacement / RectTransform.rect.width >= threshold)
                {
                    onSlideEnd?.Invoke();
                }
            }
            else
            {
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
            }

            RectTransform.localPosition = sliderStartPos;
            displacement = 0f;
            isSliding = false;
        }
        #endregion
    }
}