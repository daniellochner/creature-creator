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
        [SerializeField] private PlayerFriend friend;
        [SerializeField] private CreatureNamer namer;
        #endregion

        #region Properties
        public CreatureSelectable Selectable => selectable;
        public PlayerFriend Friend => friend;
        public CreatureNamer Namer => namer;
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            selectable = GetComponent<CreatureSelectable>();
            friend = GetComponent<PlayerFriend>();
            namer = GetComponent<CreatureNamer>();
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
            Friend.Setup();

            Loader.OnShow += OnFirstTimeShown;
            Loader.ShowToMe();
        }

        public override void OnDie(DamageReason reason, string inflicter)
        {
            base.OnDie(reason, inflicter);
            Selectable.SetSelected(false, true);
        }

        public override void OnShow()
        {
            base.OnShow();

            Constructor.Body.gameObject.SetActive(true);

            Informer.Capture();

            Collider.UpdateCollider();
            Optimizer.Optimize();

            Namer.SetVisible(true);

            Collider.enabled = true;
            Animator.enabled = !Rider.IsRiding;
            Underwater.enabled = true;
        }
        public override void OnHide()
        {
            base.OnHide();

            Constructor.Body.gameObject.SetActive(false);

            Selectable.SetSelected(false);

            Namer.SetVisible(false);

            Collider.enabled = false;
            Animator.enabled = false;
            Underwater.enabled = false;
        }

        private void OnFirstTimeShown()
        {
            if (Loader.IsHidden.Value)
            {
                Loader.OnHide();
            }
            Loader.OnShow -= OnFirstTimeShown;
        }
        #endregion
    }
}