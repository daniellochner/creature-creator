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
        [SerializeField] private Image logo;

        private CanvasGroup fadeCanvasGroup;


        protected override void Awake()
        {
            base.Awake();
            fadeCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void LoadScene(string scene, UnityAction onLoad = null)
        {
            StartCoroutine(LoadSceneRoutine(scene, onLoad));



            //NetworkManager.Singleton.SceneManager.LoadScene(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            //NetworkManager.Singleton.SceneManager.OnSceneEvent += OnNetworkLoad;
            
            //NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnNetworkLoad;
        }

        private IEnumerator LoadSceneRoutine(string scene, UnityAction onLoad = null)
        {
            logo.fillAmount = 0f;
            yield return StartCoroutine(fadeCanvasGroup.Fade(true, 1f));

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while (!operation.isDone)
            {
                logo.fillAmount = operation.progress;
                yield return null;
            }
            onLoad?.Invoke();

            yield return StartCoroutine(fadeCanvasGroup.Fade(false, 1f));
        }
    }
}