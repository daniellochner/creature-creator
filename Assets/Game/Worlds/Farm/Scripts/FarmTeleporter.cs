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
        [SerializeField] private BusCinematic arriveOnBusCinematic;
        [Space]
        [SerializeField] private Platform beachPlatform;
        [SerializeField] private Water ocean;
        [SerializeField] private Zone beach;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Island")
            {
                beachPlatform.TeleportTo(true, false);

                ZoneManager.Instance.EnterZone(beach, false);
                ocean.SetVisibility(true);

                arriveOnRaftCinematic.Begin();

                if (!HasRequestedReview)
                {
                    arriveOnRaftCinematic.OnEnd += RatingManager.Instance.Rate;
                }
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
            else
            if (prevScene == "City")
            {
                arriveOnBusCinematic.Begin();
            }
        }
        #endregion
    }
}