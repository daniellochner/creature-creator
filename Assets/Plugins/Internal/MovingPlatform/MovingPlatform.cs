using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MovingPlatform : NetworkBehaviour
    {
        [SerializeField] private string playerTag;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                other.transform.SetParent(transform);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                other.transform.SetParent(null);
            }
        }
    }
}