using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1000)]
    public class LocalRemoteDiff : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Object[] destroyOnLocal;
        [SerializeField] private Object[] destroyOnRemote;
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
            }
            else
            {
                foreach (Object obj in destroyOnRemote)
                {
                    Destroy(obj);
                }
            }
        }
        #endregion
    }
}