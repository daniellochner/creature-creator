using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class ArriveOnRaftCinematic : CCCinematic
    {
        #region Fields
        [SerializeField] private Transform raft;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;
        #endregion

        #region Methods
        public override void BeginFade()
        {
            base.BeginFade();
            SpawnCreature(spawnPoint, TeleportManager.dataBuffer);
            StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            while (true)
            {
                raft.position += direction * speed * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        #endregion
    }
}