using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class CinematicManager : MonoBehaviourSingleton<CinematicManager>
    {
        public bool IsInCinematic { get; set; }

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