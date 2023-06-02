using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ScrollRectExtension : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        [SerializeField] private ScrollRect[] scrollRects;

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            foreach (ScrollRect scrollRect in scrollRects)
            {
                scrollRect.OnInitializePotentialDrag(eventData);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            foreach (ScrollRect scrollRect in scrollRects)
            {
                scrollRect.OnBeginDrag(eventData);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            foreach (ScrollRect scrollRect in scrollRects)
            {
                scrollRect.OnDrag(eventData);
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (ScrollRect scrollRect in scrollRects)
            {
                scrollRect.OnEndDrag(eventData);
            }
        }
        public void OnScroll(PointerEventData eventData)
        {
            foreach (ScrollRect scrollRect in scrollRects)
            {
                scrollRect.OnScroll(eventData);
            }
        }
    }
}