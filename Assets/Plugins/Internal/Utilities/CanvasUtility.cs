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
                return EventSystem.current.IsPointerOverGameObject();

                //if (!EventSystem.current.IsPointerOverGameObject())
                //{
                //    return false;
                //}

                //PointerEventData pointer = new PointerEventData(EventSystem.current);
                //pointer.position = Input.mousePosition;

                //List<RaycastResult> raycastResults = new List<RaycastResult>();
                //EventSystem.current.RaycastAll(pointer, raycastResults);

                //bool isPointerOverUI = false;
                //if (raycastResults.Count > 0)
                //{
                //    foreach (var go in raycastResults)
                //    {
                //        string currentLayer = LayerMask.LayerToName(go.gameObject.layer);

                //        if (currentLayer.Equals("UI"))
                //        {
                //            isPointerOverUI = true;
                //            break;
                //        }
                //    }
                //}
                //return isPointerOverUI;
            }
        }
    }
}