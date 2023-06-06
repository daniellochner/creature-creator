using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        #region Fields
        private PlayableDirector director;
        #endregion

        #region Properties
        public UnityAction OnBegin { get; set; }
        public UnityAction OnEnd { get; set; }
        public UnityAction OnShow { get; set; }
        public UnityAction OnHide { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }
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
        public void Pause()
        {
            director.Pause();
        }
        public void Unpause()
        {
            director.Play();
        }

        public virtual void Show()
        {
            OnShow?.Invoke();
        }
        public virtual void Hide()
        {
            OnHide?.Invoke();
        }
        #endregion
    }
}