using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Commandable : CreatureInteractable
    {
        protected override void OnInteract(Interactor interactor)
        {
            CommandServerRpc(interactor.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CommandServerRpc(NetworkObjectReference commander)
        {
            if (commander.TryGet(out NetworkObject obj))
            {
                AnimalAI animalAI = GetComponent<AnimalAI>();
                if (animalAI.CanFollow)
                {
                    if (animalAI.GetState<AnimalAI.Following>("FOL").Target == null)
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
    }
}