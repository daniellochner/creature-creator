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
        [SerializeField] private ExitCrackCinematic exitCrackCinematic;
        [Space]
        [SerializeField] private Platform raftPlatform;
        [SerializeField] private TriggerRegion water;
        [SerializeField] private Zone beach;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Island")
            {
                arriveOnRaftCinematic.Begin();
                raftPlatform.TeleportTo(false);

                water.OnTriggerEnter(Player.Instance.Collider.Hitbox);
                ZoneManager.Instance.EnterZone(beach, false);
            }
            else
            if (prevScene == "Sandbox")
            {
                expandInSandboxCinematic.Begin();
            }
            else
            if (prevScene == "Cave")
            {
                exitCrackCinematic.Begin();
            }
        }
        #endregion
    }
}