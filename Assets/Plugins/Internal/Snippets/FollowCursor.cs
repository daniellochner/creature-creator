using UnityEngine;

namespace DanielLochner.Assets
{
    public class FollowCursor : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.position = Input.mousePosition;
        }
    }
}