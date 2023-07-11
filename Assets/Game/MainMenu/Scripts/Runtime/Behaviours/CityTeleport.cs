// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class CityTeleport : Teleport
    {
        protected override void Awake()
        {
            base.Awake();

            if (!CityReleaseManager.IsCityReleased)
            {
                gameObject.SetActive(false);
            }
        }
    }
}