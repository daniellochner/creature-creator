using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FarmTeleporter : TeleportManager
    {
        public override void OnEnter(string prevScene, string nextScene, CreatureData data)
        {
            base.OnEnter(prevScene, nextScene, data);
        }
    }
}