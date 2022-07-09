using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class LoadingManager : MonoBehaviourSingleton<LoadingManager>
    {
        #region Fields
        [SerializeField] private Image logo;

        private CanvasGroup fadeCanvasGroup;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            fadeCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void LoadScene(string scene, UnityAction onLoad = null, UnityAction onPreLoad = null)
        {
            StartCoroutine(LoadSceneRoutine(scene, onLoad, onPreLoad));
        }
        private IEnumerator LoadSceneRoutine(string scene, UnityAction onLoad, UnityAction onPreLoad)
        {
            logo.fillAmount = 0f;
            yield return StartCoroutine(fadeCanvasGroup.Fade(true, 1f));

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            onPreLoad?.Invoke();
            while (!operation.isDone)
            {
                logo.fillAmount = operation.progress;
                yield return null;
            }
            onLoad?.Invoke();

            yield return StartCoroutine(fadeCanvasGroup.Fade(false, 1f));
        }
        #endregion
    }
}