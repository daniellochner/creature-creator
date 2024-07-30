using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryContentMenu : MenuSingleton<FactoryContentMenu>
    {
        public TextMeshProUGUI titleText;
        public RectTransform contentRoot;
        private FactoryContentUI targetContent;
        private Transform prevParent;

        public void View(FactoryContentUI content)
        {
            titleText.text = content.Title;

            targetContent = content;
            prevParent = targetContent.transform.parent;
            SetParent(content, contentRoot);

            Open();
        }

        public override void Close(bool instant = false)
        {
            base.Close(instant);

            SetParent(targetContent, prevParent);
        }

        private void SetParent(FactoryContentUI content, Transform parent)
        {
            content.RectTransform.SetParent(parent);
            content.RectTransform.offsetMin = content.RectTransform.offsetMax = Vector2.zero;
            content.RectTransform.localScale = Vector3.one;
            content.csCalculator.Calculate();
        }
    }
}