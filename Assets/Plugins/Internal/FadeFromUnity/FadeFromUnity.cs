using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeFromUnity : MonoBehaviourSingleton<FadeFromUnity>
    {
        #region Fields
        private CanvasGroup canvasGroup;
        #endregion

        #region Methods
        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }
        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            yield return StartCoroutine(canvasGroup.FadeRoutine(false, 1f, true, () => gameObject.SetActive(false)));
        }
        #endregion
    }
}