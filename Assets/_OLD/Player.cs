// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        #region Fields
        [SerializeField] private CreatureSourcePlayer creature;
        [SerializeField] private new CameraOrbit camera;
        #endregion

        #region Properties
        public CreatureSourcePlayer Creature => creature;
        public CameraOrbit Camera => camera;
        #endregion
    }
}