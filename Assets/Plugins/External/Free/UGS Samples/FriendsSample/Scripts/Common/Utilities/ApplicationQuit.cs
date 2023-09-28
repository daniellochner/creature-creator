using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.Samples.Utilities
{
    public class ApplicationQuit : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
        }

        [UsedImplicitly]
        public void Quit()
        {
#if(UNITY_EDITOR)
            EditorApplication.ExitPlaymode();
#else   
            Application.Quit();
#endif
        }
    }
}