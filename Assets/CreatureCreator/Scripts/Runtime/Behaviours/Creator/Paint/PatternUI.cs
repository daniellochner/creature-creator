// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(ExpandOnHover))]
    public class PatternUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private ClickUI clickUI;
        [SerializeField] private HoverUI hoverUI;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject newGO;
        #endregion

        #region Properties
        public Toggle SelectToggle => selectToggle;
        public ClickUI ClickUI => clickUI;
        public HoverUI HoverUI => hoverUI;
        public CanvasGroup CanvasGroup => canvasGroup;

        public Pattern Pattern { get; private set; }
        #endregion

        #region Methods
        public void Setup(Pattern pattern, Material material, bool isNew = false)
        {
            Pattern = pattern;

            Image graphic = selectToggle.graphic as Image;
            Image targetGraphic = transform.GetChild(0).GetComponent<Image>();

            graphic.sprite = targetGraphic.sprite = pattern.Icon;
            graphic.material = targetGraphic.material = material;

            clickUI.OnLeftClick.AddListener(HideNew);
            hoverUI.OnEnter.AddListener(HideNew);

            if (isNew)
            {
                newGO.SetActive(true);
            }
        }
        public void SetUsable(bool isUsable)
        {
            canvasGroup.alpha = isUsable ? 1f : 0.2f;
        }

        private void HideNew()
        {
            newGO.SetActive(false);
        }
        #endregion
    }
}