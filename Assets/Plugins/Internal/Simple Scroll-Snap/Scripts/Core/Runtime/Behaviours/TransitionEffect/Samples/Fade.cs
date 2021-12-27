// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class Fade : TransitionEffectBase<CanvasGroup>
    {
        public override void OnTransition(CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }
}