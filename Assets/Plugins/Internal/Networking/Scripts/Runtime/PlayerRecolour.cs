using UnityEngine;
using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class PlayerRecolour : NetworkBehaviour
    {
        #region Properties
        private MinimapIcon MinimapIcon { get; set; }
        private PlayerNamer Namer { get; set; }
        private PlayerDeathMessenger DeathMessenger { get; set; }

        private PlayerNameUI NameUI => NetworkPlayersMenu.Instance?.GetPlayerNameUI(OwnerClientId);
        #endregion

        #region Methods
        private void Awake()
        {
            MinimapIcon = GetComponent<MinimapIcon>();
            Namer = GetComponent<PlayerNamer>();
            DeathMessenger = GetComponent<PlayerDeathMessenger>();
        }

        public void SetColour(Color colour)
        {
            MinimapIcon.MinimapIconUI.SetColour(colour);

            Namer?.SetColour(colour);
            DeathMessenger?.SetColour(colour);

            NameUI?.SetTextColour(colour);
        }
        #endregion
    }
}