using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1000)]
    public class LocalRemoteDiff : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onLocal;
        [SerializeField] private UnityEvent onRemote;

        [SerializeField] public Object[] destroyOnLocal;
        [SerializeField] public Object[] destroyOnRemote;
        #endregion

        #region Properties
        private bool IsLocal => IsOwner;
        #endregion

        #region Methods
        private void Start()
        {
            if (IsLocal)
            {
                foreach (Object obj in destroyOnLocal)
                {
                    Destroy(obj);
                }
                onLocal.Invoke();
            }
            else
            {
                foreach (Object obj in destroyOnRemote)
                {
                    Destroy(obj);
                }
                onRemote.Invoke();
            }
        }
        #endregion
    }
}