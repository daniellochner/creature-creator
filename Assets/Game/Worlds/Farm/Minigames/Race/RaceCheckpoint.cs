using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RaceCheckpoint : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Race race;
        [SerializeField] private RaceCheckpoint prevCheckpoint;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && (race.CurrentCheckpoint == prevCheckpoint))
            {
                race.CurrentCheckpoint = this;

                if (race.StartCheckpoint == this)
                {
                    race.LapServerRpc(NetworkManager.Singleton.LocalClientId);
                }
            }
        }
        #endregion
    }
}