using UnityEngine;

namespace DanielLochner.Assets
{
    public class FollowCursor : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                transform.position = Input.mousePosition;
            }
        }
    }
}