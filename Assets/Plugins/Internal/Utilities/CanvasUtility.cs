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
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; ++i)
                    {
                        Touch touch = Input.touches[i];
                        if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            return true;
                        }
                    }
                }
                else if (EventSystem.current.IsPointerOverGameObject())
                {
                    return true;
                }
                return false;
            }
        }
    }
}