using LeTai.Asset.TranslucentImage;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TranslucentImage))]
    public class TranslucentImageAcquirer : MonoBehaviour
    {
        private TranslucentImage img;
        private void Awake()
        {
            img = GetComponent<TranslucentImage>();
        }
        private void Update()
        {
            if (img.source == null || !img.source.gameObject.activeInHierarchy)
            {
                img.source = FindObjectOfType<TranslucentImageSource>();
            }
        }
    }
}