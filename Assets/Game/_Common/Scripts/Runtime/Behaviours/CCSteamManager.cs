using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [DefaultExecutionOrder(1)]
    public class CCSteamManager : SteamManager
    {
#if UNITY_STANDALONE
        protected override void Awake()
        {
            if (EducationManager.Instance.IsEducational)
            {
                enabled = false;
            }
            else
            {
                base.Awake();
            }
        }
#endif
    }
}