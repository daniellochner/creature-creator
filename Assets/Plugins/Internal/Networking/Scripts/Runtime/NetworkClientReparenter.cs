using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NetworkClientReparenter : NetworkBehaviour
    {
        #region Fields
        private Transform prevParent;

        private Vector3 currentScale;
        private Vector3 currentPosition;
        private Quaternion currentRotation;

        private ClientNetworkTransform clientNetworkTransform;
        #endregion

        #region Methods
        private void Awake()
        {
            clientNetworkTransform = GetComponent<ClientNetworkTransform>();
        }
        private void Update()
        {
            if (!IsServer)
            {
                if (prevParent != transform.parent)
                {
                    clientNetworkTransform.Teleport(currentPosition, currentRotation, currentScale);
                    prevParent = transform.parent;
                }

                currentPosition = transform.position;
                currentRotation = transform.rotation;
                currentScale = transform.localScale;
            }
        }
        #endregion
    }
}