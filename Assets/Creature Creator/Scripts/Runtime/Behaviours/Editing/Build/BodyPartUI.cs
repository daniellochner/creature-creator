// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(ExpandOnHover))]
    public class BodyPartUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image icon;
        [SerializeField] private Animator animator;
        [SerializeField] private DragUI dragUI;
        [SerializeField] private HoverUI hoverUI;
        [SerializeField] private ClickUI clickUI;

        private BodyPart bodyPart;
        #endregion

        #region Properties
        public DragUI DragUI => dragUI;
        public HoverUI HoverUI => hoverUI;
        public ClickUI ClickUI => clickUI;
        #endregion

        #region Methods
        public void Setup(BodyPart bodyPart)
        {
            this.bodyPart = bodyPart;

            icon.sprite = bodyPart.Icon;

            dragUI.OnPress.AddListener(delegate
            {
                animator.SetBool("Expanded", false);
            });
        }
        #endregion
    }
}