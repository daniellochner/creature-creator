using UnityEngine;
using LeTai.Asset.TranslucentImage;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(TranslucentImageSource))]
    public class TranslucentImageAssigner : MonoBehaviour
    {
        private void OnEnable()
        {
            TranslucentImageSource source = GetComponent<TranslucentImageSource>();
            foreach (TranslucentImage image in FindObjectsOfType<TranslucentImage>())
            {
                if (image.source == null)
                {
                    image.source = source;
                }
            }
        }
    }
}