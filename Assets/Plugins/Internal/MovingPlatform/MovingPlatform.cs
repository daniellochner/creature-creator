using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MovingPlatform : NetworkBehaviour
    {
        #region Fields
        private TrackRegion region;
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
        }
        private void Start()
        {
            if (IsServer)
            {
                region.OnTrack += OnTrack;
                region.OnLoseTrackOf += OnLoseTrackOf;
            }
        }

        private void OnTrack(Collider other)
        {
            SetParent(new NetworkObjectReference(other.gameObject), true);
        }
        private void OnLoseTrackOf(Collider other)
        {
            SetParent(new NetworkObjectReference(other.gameObject), false);
        }

        private void SetParent(NetworkObjectReference networkObjectRef, bool isParented)
        {
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                networkObject.transform.parent = isParented ? transform : null;
            }
        }
        #endregion
    }
}