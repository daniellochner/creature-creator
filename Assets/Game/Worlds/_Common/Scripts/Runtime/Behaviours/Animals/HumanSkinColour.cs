using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HumanSkinColour : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Color[] skinColours;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureLoader Loader { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Loader = GetComponent<CreatureLoader>();

            Loader.OnShow += OnShow;
        }
        
        private void OnShow()
        {
            Color skinColour = skinColours[NetworkObjectId % (ulong)skinColours.Length];
            Constructor.SetPrimaryColour(skinColour);
        }
        #endregion
    }
}