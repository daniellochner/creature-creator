// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        #region Fields
        [SerializeField] private CreaturePlayerLocal creature;
        [SerializeField] private new CameraOrbit camera;
        #endregion

        #region Properties
        public CreaturePlayerLocal Creature
        {
            get => creature;
            set => creature = value;
        }
        public CameraOrbit Camera
        {
            get => camera;
            set => camera = value;
        }
        #endregion
    }
}