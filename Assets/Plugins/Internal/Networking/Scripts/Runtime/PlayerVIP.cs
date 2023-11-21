using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerVIP : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color vipColour;
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
            if (DataContainer.Data.isVIP)
            {
                SetAsVIP();
            }
        }

        public void SetAsVIP()
        {
            Recolour.SetColour(vipColour);
        }
        #endregion
    }
}