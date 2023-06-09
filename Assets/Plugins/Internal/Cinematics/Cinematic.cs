using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Camera cinematicCam;
        [SerializeField] private PlayableDirector director;
        #endregion

        #region Properties
        public Camera CinematicCamera => cinematicCam;
        public PlayableDirector Director => director;

        public UnityAction OnBegin { get; set; }
        public UnityAction OnEnd { get; set; }
        public UnityAction OnShow { get; set; }
        public UnityAction OnHide { get; set; }
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
            CinematicManager.Instance.CurrentCinematic = this;
            gameObject.SetActive(true);
            OnBegin?.Invoke();
        }
        public virtual void End()
        {
            CinematicManager.Instance.CurrentCinematic = null;
            gameObject.SetActive(false);
            OnEnd?.Invoke();
        }
        public virtual void Show()
        {
            OnShow?.Invoke();
        }
        public virtual void Hide()
        {
            OnHide?.Invoke();
        }

        public void Pause()
        {
            Director.Pause();
        }
        public void Unpause()
        {
            Director.Play();
        }
        #endregion
    }
}