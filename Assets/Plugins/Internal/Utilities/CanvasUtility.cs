using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public static class CanvasUtility
    {
        public static bool IsPointerOverUI
        {
            get
            {
                for (int i = -1; i < Input.touchCount; ++i)
                {
                    if (EventSystem.current.IsPointerOverGameObject(i))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}