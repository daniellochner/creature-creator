using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float pressScale = 0.95f;
        [SerializeField] private float pressTime = 0.25f;
        private bool isPressed, isScaling, isScaledIn;

        public void OnPointerDown(PointerEventData eventData)
        {
            Press();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Release();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Release();
        }

        private void Press()
        {
            if (!isPressed)
            {
                isPressed = true;

                if (!isScaling && !isScaledIn)
                {
                    StartCoroutine(ScalingRoutine(ScaleInOutRoutine()));
                }
            }
        }
        private void Release()
        {
            if (isPressed)
            {
                isPressed = false;

                if (!isScaling && isScaledIn)
                {
                    StartCoroutine(ScalingRoutine(ScaleOutRoutine()));
                }
            }
        }

        private IEnumerator ScalingRoutine(IEnumerator routine)
        {
            isScaling = true;
            yield return routine;
            isScaling = false;
        }
        private IEnumerator ScaleInOutRoutine()
        {
            yield return ScaleInRoutine();
            if (isPressed)
            {
                isScaling = false;
                yield break;
            }
            yield return ScaleOutRoutine();
        }
        private IEnumerator ScaleInRoutine()
        {
            yield return ScaleRoutine(1f, pressScale);
            isScaledIn = true;
        }
        private IEnumerator ScaleOutRoutine()
        {
            yield return ScaleRoutine(pressScale, 1f);
            isScaledIn = false;
        }
        private IEnumerator ScaleRoutine(float initialScale, float targetScale)
        {
            yield return this.InvokeOverTime(delegate (float t)
            {
                SetScale(Mathf.Lerp(initialScale, targetScale, t));
            },
            pressTime);
        }

        private void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}