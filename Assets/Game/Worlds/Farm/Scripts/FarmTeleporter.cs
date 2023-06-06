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
        [SerializeField] private Platform beachPlatform;
        [SerializeField] private Water ocean;
        [SerializeField] private Zone beach;
        #endregion

        #region Properties
        private bool HasRequestedReview
        {
            get => PlayerPrefs.GetInt("REQUESTED_REVIEW", 0) == 1;
            set => PlayerPrefs.SetInt("REQUESTED_REVIEW", value ? 1 : 0);
        }
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
        }
        #endregion
    }
}