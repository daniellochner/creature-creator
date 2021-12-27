// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class RecolourBlue : TransitionEffectBase<Graphic>
    {
        public override void OnTransition(Graphic graphic, float blue)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, blue, graphic.color.a);
        }
    }
}