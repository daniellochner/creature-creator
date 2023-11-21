using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class PlayerDataContainer : NetworkBehaviour
    {
        #region Properties
        private NetworkVariable<PlayerData> PlayerData { get; set; } = new NetworkVariable<PlayerData>();

        public PlayerData Data => PlayerData.Value;
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