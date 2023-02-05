using UnityEngine;

namespace DanielLochner.Assets
{
    public static class CameraUtility
    {
        public static Camera MainCamera
        {
            get
            {
                Camera cam = Camera.main;
                if (cam == null)
                {
                    foreach (Camera c in Object.FindObjectsOfType<Camera>(true))
                    {
                        if (c.CompareTag("MainCamera"))
                        {
                            return c;
                        }
                    }
                }
                return cam;
            }
        }
    }
}