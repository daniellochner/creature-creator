// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSelectable))]
    public class CreatureNonPlayer : CreatureBase
    {
        #region Fields
        [SerializeField] private CreatureSelectable selectable;
        [SerializeField] private Commandable commandable;
        #endregion

        #region Properties
        public CreatureSelectable Selectable => selectable;
        public Commandable Commandable => commandable;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selectable = GetComponent<CreatureSelectable>();
            commandable = GetComponent<Commandable>();
        }
#endif
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance, Player.Instance);
            Setup();
        }

        public override void Setup()
        {
            base.Setup();
            Selectable.Setup();

            if (!Loader.IsHidden)
            {
                Loader.ShowToMe();
            }
            else
            {
                Loader.OnHide();
            }
        }

        public override void OnDie()
        {
            base.OnDie();
            Selectable.SetSelected(false, true);
        }

        public override void OnShow()
        {
            base.OnShow();

            Constructor.Body.gameObject.SetActive(true);

            Informer.Capture();

            Collider.enabled = true;
            Animator.enabled = true;
        }
        public override void OnHide()
        {
            base.OnHide();

            Constructor.Body.gameObject.SetActive(false);

            Selectable.SetSelected(false);

            Collider.enabled = false;
            Animator.enabled = false;
        }
        #endregion
    }
}