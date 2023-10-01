using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class PlayerDataContainer : NetworkBehaviour
    {
        #region Properties
        public NetworkVariable<PlayerData> PlayerData { get; set; } = new NetworkVariable<PlayerData>();
        #endregion

        #region Methods
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                PlayerData.Value = NetworkHostManager.Instance.Players[OwnerClientId];
            }
        }
        #endregion
    }
}