using DanielLochner.Assets.CreatureCreator.Cinematics.Sandbox;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SandboxTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private ShrinkInSandboxCinematic shrinkInSandboxCinematic;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Farm")
            {
                shrinkInSandboxCinematic.Begin();
            }
        }
        #endregion
    }
}