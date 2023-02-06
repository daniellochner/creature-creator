using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class LeaveOnRaftCinematic : TeleportCinematic
    {
        #region Fields
        [SerializeField] private Transform raft;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;
        #endregion

        #region Methods
        public override void Show()
        {
            base.Show();
            SpawnCreature(spawnPoint);
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