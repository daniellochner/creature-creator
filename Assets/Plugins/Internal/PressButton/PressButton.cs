using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float pressedScale = 0.95f;
        [SerializeField] private float pressTime = 0.25f;

        private Coroutine pressCoroutine;

        void Start()
        {
            if (GetComponent<Animator>() != null)
            {
                Debug.Log(gameObject, gameObject);
            }
        }

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
            this.StopStartCoroutine(ScaleRoutine(isPressed ? pressedScale : 1f), ref pressCoroutine);
        }

        private IEnumerator ScaleRoutine(float targetScale)
        {
            float initialScale = transform.localScale.x;
            yield return this.InvokeOverTime(delegate (float t)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(initialScale, targetScale, t);
            },
            pressTime);
        }
    }
}