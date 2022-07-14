// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInteractor : Interactor
    {
        #region Fields
        [SerializeField] private CameraOrbit cameraOrbit;
        #endregion

        #region Methods
        private void Start()
        {
            InteractionsManager.Instance.OnTarget += delegate (GameObject targeted)
            {
                cameraOrbit.SetFrozen(targeted != null);
            };
        }
        #endregion
    }
}