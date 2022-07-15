// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCamera))]
    public class CreatureInteractor : Interactor
    {
        #region Properties
        public CreatureCamera CreatureCamera { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureCamera = GetComponent<CreatureCamera>();
        }

        public void Setup()
        {
            interactionCamera = CreatureCamera.Camera;

            InteractionsManager.Instance.OnTarget += delegate (GameObject targeted)
            {
                CreatureCamera.CameraOrbit.SetFrozen(targeted != null);
            };
        }
        #endregion
    }
}