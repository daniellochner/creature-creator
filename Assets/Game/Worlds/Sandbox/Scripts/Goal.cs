using MoreMountains.NiceVibrations;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Goal : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Soccer soccer;

        [SerializeField] private Soccer.Team team;
        [SerializeField] private GameObject confettiPrefab;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;

            if (other.CompareTag("Ball"))
            {
                SpawnConfettiClientRpc(other.transform.position);
                other.GetComponent<Ball>().Teleport(other.transform.parent.position);

                if (soccer.State.Value == Minigame.MinigameStateType.Playing)
                {
                    if (team == Soccer.Team.Red)
                    {
                        soccer.BlueScore.Value++;
                    }
                    else
                    if (team == Soccer.Team.Blue)
                    {
                        soccer.RedScore.Value++;
                    }
                }
            }
        }

        [ClientRpc]
        private void SpawnConfettiClientRpc(Vector3 position)
        {
            Instantiate(confettiPrefab, position, Quaternion.identity);

            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        #endregion
    }
}