using UnityEngine;

namespace DanielLochner.Assets
{
    public class UniformScaler : MonoBehaviour
    {
        public void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}