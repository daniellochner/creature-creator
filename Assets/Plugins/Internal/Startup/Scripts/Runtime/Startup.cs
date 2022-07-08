// Startup
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        private void Start()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}