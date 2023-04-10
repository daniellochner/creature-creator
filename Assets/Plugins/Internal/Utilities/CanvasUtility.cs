using System.Collections.Generic;
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
#if UNITY_STANDALONE
                return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
                for (int i = 0; i < Input.touchCount; ++i)
                {
                    Touch touch = Input.touches[i];
                    if (touch.phase == TouchPhase.Began)
                    {
                        PointerEventData data = new PointerEventData(EventSystem.current)
                        {
                            position = touch.position
                        };
                        List<RaycastResult> results = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(data, results);

                        if (results.Count > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
#endif
            }
        }
    }
}