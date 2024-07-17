using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerNamer : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private PlayerName namePrefab;
        [SerializeField] protected float height;

        protected PlayerName playerName;
        #endregion

        #region Properties
        public PlayerDataContainer DataContainer { get; set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            DataContainer = GetComponent<PlayerDataContainer>();
        }

        public virtual void Setup()
        {
            playerName = Instantiate(namePrefab, transform.position + transform.up * height, transform.rotation, transform);
            playerName.Setup(DataContainer.Data);
            SetVisible(false);
        }

        public void SetName(string name, int experienceLevel)
        {
            playerName.SetName(name, experienceLevel);
        }
        public virtual void SetColour(Color colour)
        {
            playerName.SetColour(colour);
        }
        public virtual void SetVisible(bool isActive)
        {
            if (playerName != null)
            {
                playerName.gameObject.SetActive(isActive);
            }
        }
        #endregion
    }
}