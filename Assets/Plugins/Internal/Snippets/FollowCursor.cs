using UnityEngine;

namespace DanielLochner.Assets
{
    public class FollowCursor : MonoBehaviour
    {
        private void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}