using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LockedChest : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private AudioSource unlockAS;
        [SerializeField] private Transform capT;
        private bool isOpen;
        #endregion

        #region Methods
        public void TryOpen()
        {
            OpenServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void OpenServerRpc()
        {
            if (!isOpen)
            {
                StartCoroutine(OpenRoutine());
                isOpen = true;
            }
        }

        private IEnumerator OpenRoutine()
        {
            OpenClientRpc();
            yield return new WaitForSeconds(1.25f); // Wait for key unlock sound.

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
            {
                capT.localRotation = Quaternion.Slerp(Quaternion.Euler(90f, 0f, 0f), Quaternion.identity, p);
            },
            1f);
        }

        [ClientRpc]
        private void OpenClientRpc()
        {
            unlockAS.Play();
        }
        #endregion
    }
}