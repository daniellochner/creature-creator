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
            }
        }
    }
}