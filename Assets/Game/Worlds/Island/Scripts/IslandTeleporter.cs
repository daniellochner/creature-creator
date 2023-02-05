using DanielLochner.Assets.CreatureCreator.Cinematics.Island;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IslandTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private ArriveOnRaftCinematic arriveOnRaftCinematic;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (!GameSetup.Instance.DoTutorial)
            {
                if (prevScene == "Farm")
                {
                    arriveOnRaftCinematic.Begin();
                }
            }
        }
        #endregion
    }
}