using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Goal : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Team team;
        [SerializeField] private Transform start;
        [SerializeField] private GameObject confettiPrefab;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && !Scoreboard.Instance.HasWon)
            {
                Instantiate(confettiPrefab, other.transform.position, Quaternion.identity);

                other.GetComponent<Unity.Netcode.Components.NetworkTransform>().Teleport(start.position, start.rotation, other.transform.localScale);
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (team == Team.Red)
                {
                    Scoreboard.Instance.BlueScore.Value++;
                }
                else
                if (team == Team.Blue)
                {
                    Scoreboard.Instance.RedScore.Value++;
                }
            }
        }
        #endregion

        #region Enums
        private enum Team
        {
            Red,
            Blue
        }
        #endregion
    }
}