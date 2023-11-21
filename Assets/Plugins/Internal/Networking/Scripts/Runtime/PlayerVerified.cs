using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerVerified : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color verifiedColour;
        [SerializeField] private SecretKey verifiedPlayerIds;
        #endregion

        #region Properties
        private PlayerRecolour Recolour { get; set; }
        private PlayerDataContainer DataContainer { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Recolour = GetComponent<PlayerRecolour>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }

        public void Setup()
        {
            if (verifiedPlayerIds.Value.Contains(DataContainer.Data.playerId.ToString()))
            {
                SetAsVerified();
            }
        }

        public void SetAsVerified()
        {
            Recolour.SetColour(verifiedColour);
        }
        #endregion
    }
}