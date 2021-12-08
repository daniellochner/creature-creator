// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class RecolourRed : TransitionEffectBase<Graphic>
    {
        public override void OnTransition(Graphic graphic, float red)
        {
            graphic.color = new Color(red, graphic.color.g, graphic.color.b, graphic.color.a);
        }
    }
}