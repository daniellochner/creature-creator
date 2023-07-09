using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Factory : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject[] effects;
        #endregion

        #region Methods
        public void Smoke()
        {
            SmokeClientRpc();
        }
        [ClientRpc]
        private void SmokeClientRpc()
        {
            foreach (GameObject effect in effects)
            {
                effect.SetActive(true);
            }
        }
        #endregion
    }
}