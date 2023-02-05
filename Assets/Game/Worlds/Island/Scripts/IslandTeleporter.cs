using DanielLochner.Assets.CreatureCreator.Cinematics.Island;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IslandTeleporter : TeleportManager
    {
        [SerializeField] private ArriveOnRaftCinematic cinematic;

        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (!GameSetup.Instance.DoTutorial)
            {
                if (prevScene == "Sandbox" && nextScene == "Island")
                {
                    cinematic.Begin(false);
                }
            }
        }
    }
}