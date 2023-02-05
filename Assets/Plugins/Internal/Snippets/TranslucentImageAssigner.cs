using UnityEngine;
using LeTai.Asset.TranslucentImage;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TranslucentImageSource))]
    public class TranslucentImageAssigner : MonoBehaviour
    {
        private TranslucentImageSource source;

        private void Awake()
        {
            source = GetComponent<TranslucentImageSource>();
        }
        private void OnEnable()
        {
            foreach (TranslucentImage image in FindObjectsOfType<TranslucentImage>(true))
            {
                if (image.source == null)
                {
                    image.source = source;
                }
            }
            source.enabled = true;
        }
        private void OnDisable()
        {
            source.enabled = false;
        }
    }
}