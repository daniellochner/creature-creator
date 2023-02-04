using UnityEngine;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        private void OnDestroy()
        {
            if (CinematicManager.Instance)
            {
                CinematicManager.Instance.IsInCinematic = false;
            }
        }

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