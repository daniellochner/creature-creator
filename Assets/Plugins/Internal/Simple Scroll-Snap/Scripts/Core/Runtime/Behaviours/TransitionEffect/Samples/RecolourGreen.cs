// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class RecolourGreen : TransitionEffectBase<Graphic>
    {
        public override void OnTransition(Graphic graphic, float green)
        {
            graphic.color = new Color(graphic.color.r, green, graphic.color.b, graphic.color.a);
        }
    }
}