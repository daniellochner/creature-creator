using UnityEngine;

namespace DanielLochner.Assets
{
    public class MobileLight : MonoBehaviour
    {
        private void Awake()
        {
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;
            }
        }
    }
}