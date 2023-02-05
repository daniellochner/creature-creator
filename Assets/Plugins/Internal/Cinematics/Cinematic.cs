using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Cinematic : MonoBehaviour
    {
        #region Properties
        public UnityAction OnBegin { get; set; }
        public UnityAction OnBeginFade { get; set; }
        public UnityAction OnEnd { get; set; }
        public UnityAction OnEndFade { get; set; }
        #endregion

        #region Methods
        private void OnDestroy()
        {
            if (CinematicManager.Instance)
            {
                CinematicManager.Instance.IsInCinematic = false;
            }
        }

        public virtual void Begin(bool fade)
        {
            CinematicManager.Instance.IsInCinematic = true;
            OnBegin?.Invoke();

            if (fade)
            {
                Fader.Fade(true, 1f, delegate
                {
                    BlackBars.Instance.SetVisibility(true);
                    BeginFade();
                    Fader.Fade(false, 1f);
                });
            }
            else
            {
                BlackBars.Instance.SetVisibility(true);
                BeginFade();
            }
        }
        public virtual void BeginFade()
        {
            OnBeginFade?.Invoke();
        }
        public virtual void End(bool fade)
        {
            CinematicManager.Instance.IsInCinematic = false;
            OnEnd?.Invoke();

            if (fade)
            {
                Fader.Fade(true, 1f, delegate
                {
                    BlackBars.Instance.SetVisibility(false);
                    EndFade();
                    Fader.Fade(false, 1f);
                });
            }
            else
            {
                BlackBars.Instance.SetVisibility(false);
                EndFade();
            }
        }
        public virtual void EndFade()
        {
            OnEndFade?.Invoke();
        }
        #endregion
    }
}