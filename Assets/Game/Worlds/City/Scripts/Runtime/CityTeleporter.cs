using DanielLochner.Assets.CreatureCreator.Cinematics.Farm;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CityTeleporter : TeleportManager
    {
        [SerializeField] private BusCinematic arriveOnBusCinematic;

        public override void OnEnter(string prevScene, string nextScene)
        {
            base.OnEnter(prevScene, nextScene);
            if (prevScene == "Farm")
            {
                arriveOnBusCinematic.Begin();
            }
        }
    }
}