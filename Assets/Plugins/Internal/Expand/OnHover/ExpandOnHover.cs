using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(HoverUI))]
    public class ExpandOnHover : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float expandScale = 1.075f;
        [SerializeField] private float expandTime = 0.25f;
        [SerializeField] private HoverUI hoverUI;

        private Coroutine expandCoroutine;
        #endregion

        #region Methods
        private void Start()
        {
            hoverUI.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    SetExpanded(true);
                }
            });
            hoverUI.OnExit.AddListener(delegate
            {
                SetExpanded(false);
            });
        }
        private void OnDisable()
        {
            SetScale(1f);
        }

        public void SetExpanded(bool isExpanded)
        {
            this.StopStartCoroutine(ExpandRoutine(isExpanded), ref expandCoroutine);
        }

        private IEnumerator ExpandRoutine(bool isExpanded)
        {
            float initialScale = transform.localScale.x;
            if (isExpanded)
            {
                for (float i = initialScale; i < expandScale; i += (Time.unscaledDeltaTime / expandTime) * (expandScale - 1f))
                {
                    SetScale(i);
                    yield return null;
                }
                SetScale(expandScale);
            }
            else
            {
                for (float i = initialScale; i > 1f; i -= (Time.unscaledDeltaTime / expandTime) * (expandScale - 1f))
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
        #endregion
    }
}