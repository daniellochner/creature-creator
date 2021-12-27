// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class ScaleX : TransitionEffectBase<RectTransform>
    {
        public override void OnTransition(RectTransform rectTransform, float scale)
        {
            rectTransform.localScale = new Vector3(scale, rectTransform.localScale.y, rectTransform.localScale.z);
        }
    }
}