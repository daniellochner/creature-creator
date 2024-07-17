using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class PlayerLevel : NetworkBehaviour
    {
        public NetworkVariable<int> Level { get; } = new NetworkVariable<int>(0, writePerm: NetworkVariableWritePermission.Owner);

        public PlayerDataContainer DataContainer { get; private set; }

        public PlayerNameUI NameUI => NetworkPlayersMenu.Instance?.GetPlayerNameUI(OwnerClientId);
        public PlayerNamer Namer { get; private set; }

        private void Awake()
        {
            Namer = GetComponent<PlayerNamer>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }

        private void Start()
        {
            Level.OnValueChanged += OnLevelChanged;
            OnLevelChanged(default, Level.Value);
        }

        private void OnLevelChanged(int prevLevel, int nextLevel)
        {
            string username = DataContainer.Data.username;
            Namer?.SetName(username, nextLevel);
            NameUI.SetName(username, nextLevel);
        }
    }
}