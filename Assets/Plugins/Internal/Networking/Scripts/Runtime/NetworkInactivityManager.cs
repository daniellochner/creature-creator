using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NetworkInactivityManager : NetworkSingleton<NetworkInactivityManager>
    {
        #region Fields
        [SerializeField] private int inactiveTime = 300;
        [SerializeField] private int warnTime = 60;
        [SerializeField] private bool kickHost;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float timeLeft;
        private bool hasWarned;

        private Vector3 prevMousePosition;
        #endregion

        #region Properties
        public Action OnInactivityKick { get; set; }
        public Action<int> OnInactivityWarn { get; set; }
        #endregion

        #region Methods
        private void Start()
        {
            timeLeft = inactiveTime;
        }
        private void Update()
        {
            if (!kickHost && IsHost) return;

            if (Input.anyKey || Input.mousePosition != prevMousePosition || Input.mouseScrollDelta != Vector2.zero)
            {
                timeLeft = inactiveTime;
                hasWarned = false;

                prevMousePosition = Input.mousePosition;

                return;
            }

            if (timeLeft < 0)
            {
                if (hasWarned)
                {
                    NetworkConnectionManager.Instance.ForceDisconnect("You were kicked for inactivity.");

                    OnInactivityKick?.Invoke();
                }
                else
                {
                    timeLeft = warnTime;
                    hasWarned = true;

                    OnInactivityWarn?.Invoke(warnTime);
                }
            }

            timeLeft -= Time.deltaTime;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            timeLeft = inactiveTime;
        }
#endif
        #endregion
    }
}