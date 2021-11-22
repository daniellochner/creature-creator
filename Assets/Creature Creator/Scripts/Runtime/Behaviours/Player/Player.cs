// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        #region Fields
        [SerializeField] private CreaturePlayer playerCreature;
        [SerializeField] private CameraOrbit cameraOrbit;
        #endregion

        #region Properties
        public CreaturePlayer Creature => playerCreature;
        public CameraOrbit Camera => cameraOrbit;
        #endregion
    }
}