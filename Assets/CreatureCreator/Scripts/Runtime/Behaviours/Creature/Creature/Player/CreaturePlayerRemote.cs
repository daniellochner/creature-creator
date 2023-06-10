// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelectable))]
    public class CreaturePlayerRemote : CreaturePlayer
    {
        #region Fields
        [SerializeField] private CreatureSelectable selectable;
        #endregion

        #region Properties
        public CreatureSelectable Selectable => selectable;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selectable = GetComponent<CreatureSelectable>();
        }
#endif
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);
            Setup();
        }

        public override void Setup()
        {
            base.Setup();

            Selectable.Setup();
            Namer.Setup();

            if (!Loader.IsHidden)
            {
                Loader.ShowToMe();
            }
            else
            {
                Loader.OnHide();
            }
        }

        public override void OnShow()
        {
            base.OnShow();

            Constructor.Body.gameObject.SetActive(true);

            Informer.Capture();

            Collider.UpdateCollider();
            Optimizer.Optimize();

            Collider.enabled = true;
            Animator.enabled = true;

            Namer.enabled = true;
        }
        public override void OnHide()
        {
            base.OnHide();

            Constructor.Body.gameObject.SetActive(false);

            Selectable.SetSelected(false);

            Collider.enabled = false;
            Animator.enabled = false;

            Namer.enabled = false;
        }
        #endregion
    }
}