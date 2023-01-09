using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FastTravel : MapIcon
    {
        public override void OnClick()
        {
            Player.Instance.Mover.Teleport(transform.position);
        }
    }
}