using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Island
{
    public class LeaveOnRaftCinematic : CCCinematic
    {
        #region Fields
        [SerializeField] private Transform prevRaft;
        [SerializeField] private Transform raft;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speed;
        #endregion

        #region Methods
        public override void Begin()
        {
            base.Begin();

            Fader.FadeInOut(1f, delegate
            {
                SetVisibility(true);

                BlackBars.Instance.SetVisibility(true, 0f);
                EditorManager.Instance.SetVisibility(false, 0f);

                SpawnCreature(spawnPoint);
                StartCoroutine(MoveRoutine());
            });
            SetMusic(false, 1f);
        }

        protected override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            prevRaft.gameObject.SetActive(!isVisible);
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