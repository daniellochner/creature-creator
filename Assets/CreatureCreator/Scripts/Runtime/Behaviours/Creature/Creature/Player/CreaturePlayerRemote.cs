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
            yield return new WaitUntilSetup(Player.Instance);
            Setup();
        }

        public override void Setup()
        {
            base.Setup();

            Selectable.Setup();
            Namer.Setup();

            if (!Hider.IsHidden)
            {
                Hider.RequestShow();
            }
            else
            {
                Hider.OnHide();
            }
        }








        //public override void OnDie()
        //{
        //    base.OnDie();
        //    OnHide();
        //}
        //public override void OnShow()
        //{
        //    base.OnShow();
        //    gameObject.SetActive(true);
        //}
        //public override void OnHide()
        //{
        //    base.OnHide();
        //    gameObject.SetActive(false);
        //}
        #endregion
    }
}