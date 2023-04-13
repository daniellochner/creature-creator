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
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
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
                    //else if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    //{
                    //    return true;
                    //}
                }
                return false;
#endif
            }
        }
    }
}