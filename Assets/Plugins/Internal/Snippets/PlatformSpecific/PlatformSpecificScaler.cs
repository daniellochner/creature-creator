using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecificScaler : MonoBehaviour
    {
        #region Fields
        [SerializeField] private DeviceType targetPlatform;
        [SerializeField] private float scale;

        private Vector3 initialScale;
        #endregion

        #region Properties
        public float Scale => scale;
        #endregion

        #region Methods
        private void Awake()
        {
            initialScale = transform.localScale;
            if (SystemUtility.IsDevice(targetPlatform))
            {
                SetScale(scale);
            }
        }

        public void SetScale(float scale)
        {
            transform.localScale = initialScale * scale;
        }
        #endregion
    }
}