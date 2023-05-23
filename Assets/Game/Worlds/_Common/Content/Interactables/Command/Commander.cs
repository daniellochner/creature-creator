using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Commander : NetworkBehaviour
    {
        #region Methods
        public void TryCommand(Interactor interactor)
        {
            CommandServerRpc(interactor.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CommandServerRpc(NetworkObjectReference commander)
        {
            if (commander.TryGet(out NetworkObject obj) && TryGetComponent(out AnimalAI animalAI))
            {
                if (animalAI.CanFollow)
                {
                    if (animalAI.Target == null)
                    {
                        animalAI.Follow(obj.transform);
                    }
                    else
                    {
                        animalAI.StopFollowing();
                    }
                }
            }
        }
        #endregion
    }
}