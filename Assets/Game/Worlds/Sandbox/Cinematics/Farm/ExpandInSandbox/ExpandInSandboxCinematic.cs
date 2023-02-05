using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Cinematics.Sandbox
{
    public class ExpandInSandboxCinematic : CCCinematic
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform target;

        public override void Begin()
        {
            base.Begin();

            Fader.FadeInOut(1f, delegate
            {
                SetVisibility(true);

                BlackBars.Instance.SetVisibility(true, 0f);
                EditorManager.Instance.SetVisibility(false, 0f);

                target.position = SpawnCreature(spawnPoint).Body.position;
            });
            SetMusic(false, 1f);
        }
    }
}