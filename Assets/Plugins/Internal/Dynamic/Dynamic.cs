using UnityEngine;

namespace DanielLochner.Assets
{
    public class Dynamic : MonoBehaviourSingleton<Dynamic>
    {
        #region Fields
        [SerializeField] private new Transform transform;
        [SerializeField] private Transform worldCanvas;
        [SerializeField] private Transform overlayCanvas;
        #endregion

        #region Properties
        public static Transform Transform { get { return Instance.transform; } }
        public static Transform WorldCanvas { get { return Instance.worldCanvas; } }
        public static Transform OverlayCanvas { get { return Instance.overlayCanvas; } }
        #endregion
    }
}