using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        #region Properties
        public UnityAction OnBegin { get; set; }
        public UnityAction OnEnd { get; set; }
        #endregion

        #region Methods
        private void OnDestroy()
        {
            if (CinematicManager.Instance)
            {
                End();
            }
        }

        public virtual void Begin()
        {
            CinematicManager.Instance.IsInCinematic = true;
            gameObject.SetActive(true);
            OnBegin?.Invoke();
        }
        public virtual void End()
        {
            CinematicManager.Instance.IsInCinematic = false;
            gameObject.SetActive(false);
            OnEnd?.Invoke();
        }

        public virtual void Show()
        {
        }
        public virtual void Hide()
        {
        }
        #endregion
    }
}