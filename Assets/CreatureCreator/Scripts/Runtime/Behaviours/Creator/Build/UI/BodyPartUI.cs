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
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private GameObject newGO;

        private BodyPart bodyPart;
        private GameObject tmp;
        #endregion

        #region Properties
        public DragUI DragUI => dragUI;
        public HoverUI HoverUI => hoverUI;
        public ClickUI ClickUI => clickUI;
        public CanvasGroup CanvasGroup => canvasGroup;
        #endregion

        #region Methods
        public void Setup(BodyPart bodyPart, bool isNew = false)
        {
            this.bodyPart = bodyPart;

            icon.sprite = bodyPart.Icon;

            dragUI.OnPress.AddListener(delegate
            {
                animator.SetBool("Expanded", false);
            });

            if (isNew)
            {
                newGO.SetActive(true);
                hoverUI.OnEnter.AddListener(delegate
                {
                    newGO.SetActive(false);
                });
            }
        }

        public void Select()
        {             
            tmp = new GameObject("TMP", typeof(RectTransform));
            tmp.transform.SetParent(transform.parent);
            tmp.transform.SetSiblingIndex(transform.GetSiblingIndex());

            layoutElement.ignoreLayout = true;
            transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();
        }
        public void Deselect()
        {
            if (tmp != null)
            {
                layoutElement.ignoreLayout = false;
                transform.SetParent(tmp.transform.parent);
                transform.SetSiblingIndex(tmp.transform.GetSiblingIndex());
                
                Destroy(tmp);
            }
        }
        #endregion
    }
}