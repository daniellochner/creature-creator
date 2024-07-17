using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureLevel : PlayerLevel
    {
        private void Update()
        {
            if (IsLocalPlayer)
            {
                Level.Value = ProgressManager.Data.Level;
            }
        }
    }
}