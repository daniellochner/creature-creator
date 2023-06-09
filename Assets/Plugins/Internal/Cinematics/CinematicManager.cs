using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class CinematicManager : MonoBehaviourSingleton<CinematicManager>
    {
        public Cinematic CurrentCinematic { get; set; }
        public bool IsInCinematic => CurrentCinematic != null;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (BlackBars.Instance)
            {
                BlackBars.Instance.SetVisibility(false);
            }
        }
    }
}