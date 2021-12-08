// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class RotateZ : TransitionEffectBase<RectTransform>
    {
        public override void OnTransition(RectTransform rectTransform, float angle)
        {
            rectTransform.localRotation = Quaternion.Euler(new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, angle));
        }
    }
}