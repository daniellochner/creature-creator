using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float scaleTime = 0.1f;

        private Coroutine scaleCoroutine;

        public void OnPointerDown(PointerEventData eventData)
        {
            SetPressed(true);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            SetPressed(false);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            SetPressed(false);
        }

        private void SetPressed(bool isPressed)
        {

            this.StopStartCoroutine(ScaleRoutine(isPressed), ref scaleCoroutine);
        }

        private IEnumerator ScaleRoutine(bool isScaled)
        {
            float initialScale = transform.localScale.x;
            if (isScaled)
            {
                for (float i = initialScale; i > 0.95f; i -= (Time.unscaledDeltaTime / scaleTime) * 0.05f)
                {
                    SetScale(i);
                    yield return null;
                }
                SetScale(0.95f);
            }
            else
            {
                for (float i = initialScale; i < 1f; i += (Time.unscaledDeltaTime / scaleTime) * 0.05f)
                {
                    SetScale(i);
                    yield return null;
                }
                SetScale(1f);
            }
        }
        private void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}