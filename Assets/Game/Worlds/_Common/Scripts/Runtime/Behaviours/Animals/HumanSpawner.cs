using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HumanSpawner : AnimalSpawner
    {
        public override void Setup(NetworkObject npc)
        {
            base.Setup(npc);

            HumanAI human = npc.GetComponent<HumanAI>();
        }
    }
}