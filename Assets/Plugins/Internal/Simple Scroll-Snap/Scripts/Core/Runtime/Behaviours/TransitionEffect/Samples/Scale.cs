// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class Scale : TransitionEffectBase<RectTransform>
    {
        public override void OnTransition(RectTransform rectTransform, float scale)
        {
            rectTransform.localScale = Vector3.one * scale;
        }
    }
}