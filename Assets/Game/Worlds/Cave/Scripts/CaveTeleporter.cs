using DanielLochner.Assets.CreatureCreator.Cinematics.Cave;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CaveTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private ExitCrackCinematic exitCrackCinematic;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Farm")
            {
                exitCrackCinematic.Begin();
            }
        }
        #endregion
    }
}