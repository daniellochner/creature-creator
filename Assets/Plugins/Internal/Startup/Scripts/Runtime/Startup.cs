// Startup
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] public string sceneToLoad;
        private void Start()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}