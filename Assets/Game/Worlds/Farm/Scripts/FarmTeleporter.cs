using DanielLochner.Assets.CreatureCreator.Cinematics.Island;
using DanielLochner.Assets.CreatureCreator.Cinematics.Farm;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FarmTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private ArriveOnRaftCinematic arriveOnRaftCinematic;
        [SerializeField] private ExpandInSandboxCinematic expandInSandboxCinematic;
        [SerializeField] private Platform raftPlatform;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Island")
            {
                arriveOnRaftCinematic.Begin();
                raftPlatform.TeleportTo(false);
            }
            else
            if (prevScene == "Sandbox")
            {
                expandInSandboxCinematic.Begin();
            }
        }
        #endregion
    }
}