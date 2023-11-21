using Unity.Services.RemoteConfig;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerVerified : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color verifiedColour;
        [SerializeField] private SecretKey verificationKey;
        [Header("Setup")]
        [SerializeField] private string playersToVerify;
        [SerializeField, Button("Encrypt")] private bool encrypt;
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
            string verifiedPlayers = StringCipher.Decrypt(RemoteConfigService.Instance.appConfig.GetString("verified_players"), verificationKey.Value); // "player1,player2,player3,[...]"
            if (!string.IsNullOrEmpty(verifiedPlayers))
            {
                if (verifiedPlayers.Contains(DataContainer.Data.playerId.ToString()))
                {
                    SetAsVerified();
                }
            }
        }

        public void SetAsVerified()
        {
            Recolour.SetColour(verifiedColour);
        }

        [ContextMenu("Encrypt")]
        public void Encrypt()
        {
            Debug.Log(StringCipher.Encrypt(playersToVerify, verificationKey.Value));
        }
        #endregion
    }
}