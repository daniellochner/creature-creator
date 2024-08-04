using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class QuestLinker : MonoBehaviour
    {
        [SerializeField] private Quest quest;
        private NPCSpawner spawner;

        private void Awake()
        {
            spawner = GetComponent<NPCSpawner>();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);

            if (!WorldManager.Instance.IsCreative)
            {
                SpawnedNPC npc = null;
                yield return new WaitUntil(() =>
                {
                    npc = SpawnedNPC.spawned.Find(x => x.spawnerId.Value == spawner.NetworkObjectId);
                    return (npc != null);
                });
                quest.transform.SetZeroParent(npc.transform);
            }
        }
    }
}