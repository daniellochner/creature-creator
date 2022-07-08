// Startup
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private bool waitForKeyPress;

        private void Start()
        {
            if (!waitForKeyPress) SceneManager.LoadScene(sceneToLoad);
        }

        private void Update()
        {
            if (waitForKeyPress && Input.anyKeyDown)
            {
                Fader.Fade(true, 1f, delegate
                {
                    SceneManager.LoadScene(sceneToLoad);
                    Fader.Fade(false, 1f);
                });
                enabled = false;
            }
        }
    }
}