using UnityEngine;

namespace DanielLochner.Assets
{
    public class LinkedGameObject : MonoBehaviour
    {
        public GameObject target;

        private void Update()
        {
            if (target != null)
            {
                gameObject.SetActive(target.activeSelf);
            }
        }
    }
}