using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        public virtual void Begin()
        {
            CinematicManager.Instance.IsInCinematic = true;
        }
        public virtual void End()
        {
            CinematicManager.Instance.IsInCinematic = false;
        }
    }
}