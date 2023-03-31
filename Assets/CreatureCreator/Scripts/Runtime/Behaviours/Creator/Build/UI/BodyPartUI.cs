// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
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

        public BodyPart BodyPart => bodyPart;
        #endregion

        #region Methods
        public void Setup(BodyPart bodyPart, bool isNew = false)
        {
            this.bodyPart = bodyPart;

            icon.sprite = bodyPart.Icon;

            hoverUI.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    StatisticsMenu.Instance.Setup(bodyPart, Input.mousePosition);
                    HideNew();
                }
            });
            hoverUI.OnExit.AddListener(delegate
            {
                StatisticsMenu.Instance.Clear();
            });

            dragUI.OnPress.AddListener(delegate
            {
                Select();

                animator.SetBool("Expanded", false);

                if (SystemUtility.IsDevice(DeviceType.Desktop))
                {
                    StatisticsMenu.Instance.Close();
                }
            });
            dragUI.OnRelease.AddListener(delegate
            {
                Deselect();
            });

            clickUI.OnLeftClick.AddListener(delegate
            {
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    StartCoroutine(ClickToOpenRoutine(bodyPart));
                }
            });

            if (isNew)
            {
                newGO.SetActive(true);
            }
        }

        private void HideNew()
        {
            newGO.SetActive(false);
        }

        public void Select()
        {             
            tmp = new GameObject("TMP", typeof(RectTransform));
            tmp.transform.SetParent(transform.parent);
            tmp.transform.SetSiblingIndex(transform.GetSiblingIndex());

            layoutElement.ignoreLayout = true;
            transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();

            HideNew();
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

        private IEnumerator ClickToOpenRoutine(BodyPart bodyPart)
        {
            StatisticsMenu.Instance.Setup(bodyPart, transform.position);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            StatisticsMenu.Instance.Close();
        }
        #endregion
    }
}