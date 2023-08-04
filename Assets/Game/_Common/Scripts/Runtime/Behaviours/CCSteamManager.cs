using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCSteamManager : SteamManager
    {
        protected override void Awake()
        {
            if (Application.version.EndsWith("-edu"))
            {
                enabled = false;
            }
            else
            {
                base.Awake();
            }
        }
    }
}