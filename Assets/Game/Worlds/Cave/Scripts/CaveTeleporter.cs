using DanielLochner.Assets.CreatureCreator.Cinematics.Cave;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CaveTeleporter : TeleportManager
    {
        #region Fields
        [SerializeField] private Cinematic exitCrackCinematic;
        [SerializeField] private Cinematic enterMineshaftCinematic;
        #endregion

        #region Methods
        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);

            if (prevScene == "Farm")
            {
                exitCrackCinematic.Begin();
            }
            else
            if (prevScene == "City")
            {
                enterMineshaftCinematic.Begin();
            }
        }
        #endregion
    }
}