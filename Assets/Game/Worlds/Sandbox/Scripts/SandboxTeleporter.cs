using DanielLochner.Assets.CreatureCreator.Cinematics.Sandbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SandboxTeleporter : TeleportManager
    {
        [SerializeField] private ShrinkInSandboxCinematic shrinkInSandboxCinematic;

        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Island" && nextScene == "Sandbox")
            {
                shrinkInSandboxCinematic.Begin();
            }
        }
    }
}