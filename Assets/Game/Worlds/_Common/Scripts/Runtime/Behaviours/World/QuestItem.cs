using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class QuestItem : NetworkBehaviour
    {
        #region Methods
        private void Start()
        {
            if (WorldManager.Instance.World.CreativeMode)
            {
                if (IsServer)
                {
                    NetworkObject.Despawn();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        public override void OnNetworkDespawn()
        {
            gameObject.SetActive(false);
            base.OnNetworkDespawn();
        }
        #endregion
    }
}