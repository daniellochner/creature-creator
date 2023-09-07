using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class LoadingManager : MonoBehaviourSingleton<LoadingManager>
    {
        #region Fields
        [SerializeField] private Image logo;

        private CanvasGroup canvasGroup;
        private float progress;
        #endregion

        #region Properties
        public float Progress
        {
            get => progress;
            set
            {
                progress = value;
                logo.fillAmount = progress;
            }
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Load(string sceneName, Action onLoad = null)
        {
            StartCoroutine(LoadRoutine(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single), onLoad));
        }

        public virtual IEnumerator LoadRoutine(AsyncOperation operation, Action onLoad)
        {
            Coroutine fadeIn = StartCoroutine(canvasGroup.FadeRoutine(true, 1f));

            Progress = 0f;
            while (!operation.isDone)
            {
                Progress = operation.progress;
                yield return null;
            }
            Progress = 1f;

            StopCoroutine(fadeIn);
            canvasGroup.alpha = 1f;

            yield return WaitUntilRoutine();

            onLoad?.Invoke();

            yield return canvasGroup.FadeRoutine(false, 1f);
            canvasGroup.alpha = 0f;
        }
        public virtual IEnumerator WaitUntilRoutine()
        {
            yield return null;
        }
        #endregion
    }
}