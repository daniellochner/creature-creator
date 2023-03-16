using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class NetworkDespawner : NetworkBehaviour
    {
        #region Fields
        private bool despawn;
        #endregion

        #region Methods
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (despawn)
            {
                Despawn();
            }
        }

        public void Despawn()
        {
            if (IsServer)
            {
                if (NetworkObject.IsSpawned)
                {
                    NetworkObject.Despawn(true);
                }
                else
                {
                    despawn = true;
                }
            }
            else
            {
                DespawnServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc()
        {
            Despawn();
        }
        #endregion
    }
}