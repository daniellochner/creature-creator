using DanielLochner.Assets.CreatureCreator.Cinematics.Island;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class IslandTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private ArriveOnRaftCinematic arriveOnRaftCinematic;

        [SerializeField] private Unity.Netcode.Components.NetworkTransform raft;
        [SerializeField] private Transform shore;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (!SettingsManager.Instance.ShowTutorial)
            {
                if (prevScene == "Farm")
                {
                    if (NetworkManager.Singleton.IsHost)
                    {
                        raft.Teleport(shore.position, shore.rotation, Vector3.one);
                    }
                    arriveOnRaftCinematic.Begin();
                }
            }
        }
        #endregion
    }
}