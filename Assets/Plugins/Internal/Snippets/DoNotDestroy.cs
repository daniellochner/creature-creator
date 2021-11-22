using UnityEngine;

namespace DanielLochner.Assets
{
    public class DoNotDestroy : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.SetParent(null); // DontDestroyOnLoad only works for root game objects (i.e., parent == null).
            DontDestroyOnLoad(gameObject);
        }
    }
}