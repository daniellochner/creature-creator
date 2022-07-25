using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class NetworkDespawner : NetworkBehaviour
    {
        private bool despawn;

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
            if (NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn(true);
            }
            else
            {
                despawn = true;
            }
        }
    }
}