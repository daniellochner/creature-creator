// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureNonPlayerRemote : CreatureNonPlayer
    {
        [SerializeField] private TextAsset cachedData;

        public override void Setup()
        {
            base.Setup();

            if (!Hider.IsHidden)
            {
                if (cachedData != null)
                {
                    Constructor.Construct(JsonUtility.FromJson<CreatureData>(cachedData.text));
                    Hider.OnShow();
                }
                else
                {
                    Hider.RequestShow();
                }
            }
            else
            {
                Hider.OnHide();
            }
        }
    }
}