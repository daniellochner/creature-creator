using UnityEngine;

namespace DanielLochner.Assets
{
    public class CaveAmbientLighting : MonoBehaviour
    {
        public void SetAmbientLighting(float intensity)
        {
            RenderSettings.ambientIntensity = intensity;
        }
    }
}