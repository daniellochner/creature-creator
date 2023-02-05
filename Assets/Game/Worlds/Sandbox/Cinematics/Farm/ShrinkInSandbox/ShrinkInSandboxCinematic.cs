using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Sandbox
{
    public class ShrinkInSandboxCinematic : CCCinematic
    {
        #region Fields
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform target;
        #endregion

        #region Methods
        public override void Begin()
        {
            base.Begin();

            SetVisibility(true);

            BlackBars.Instance.SetVisibility(true, 0f);
            EditorManager.Instance.SetVisibility(false, 0f);

            target.position = SpawnCreature(spawnPoint, TeleportManager.dataBuffer).Body.position;
        }
        public override void End()
        {
            base.End();

            Fader.FadeInOut(1f, delegate
            {
                SetVisibility(false);

                BlackBars.Instance.SetVisibility(false, 0f);
                EditorManager.Instance.SetVisibility(true, 0f);
            });
            SetMusic(true, 1f);
        }
        #endregion
    }
}