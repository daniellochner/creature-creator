// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SetupGame : MonoBehaviourSingleton<SetupGame>
    {
        #region Fields
        [SerializeField] private NetworkCreaturePlayer player;
        [SerializeField] private Platform platform;

        [SerializeField] private NetworkCreaturesMenu networkMenu;
        [SerializeField] private CreatureInformationMenu informationMenu;
        #endregion

        #region Properties
        public static bool IsNetworkedGame => NetworkManager.Singleton.IsListening;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        private void Start()
        {
            Setup();
        }

        public void Initialize()
        {
            if (IsNetworkedGame)
            {
                InitializeMP();
            }
            else
            {
                InitializeSP();
            }
        }
        private void InitializeMP()
        {
            player.gameObject.SetActive(false);
            networkMenu.gameObject.SetActive(true);
        }
        private void InitializeSP()
        {
            player.gameObject.SetActive(true);
            networkMenu.gameObject.SetActive(false);
        }

        public void Setup()
        {
            EditorManager.Instance.UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            EditorManager.Instance.UnlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            EditorManager.Instance.BaseCash = ProgressManager.Data.Cash;
            EditorManager.Instance.HiddenBodyParts = SettingsManager.Data.HiddenBodyParts;
            EditorManager.Instance.HiddenPatterns = SettingsManager.Data.HiddenPatterns;

            if (IsNetworkedGame)
            {
                SetupMP();
            }
            else
            {
                SetupSP();
            }

            player.SourcePlayerCreature.Informer.Setup(informationMenu);
            player.SourcePlayerCreature.Mover.Platform = platform;
            player.transform.SetPositionAndRotation(platform.transform.position, platform.transform.rotation);

            EditorManager.Instance.Setup();
        }
        private void SetupMP()
        {
            player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<NetworkCreaturePlayer>();
            EditorManager.Instance.Player = player.Player;
            
            player.Player.gameObject.SetActive(true);

            NetworkCreaturesManager.Instance.Setup();
            NetworkCreaturesMenu.Instance.Setup();

            Lobby lobby = LobbyHelper.Instance.JoinedLobby;
            World world = new World(lobby);
            if (NetworkManager.Singleton.IsHost)
            {
                if (world.IsPrivate)
                {
                    InformationDialog.Inform("Private World", $"The code to your private world is:\n<u><b>{lobby.Id}</b></u>\n\nPress the button below to copy it to your clipboard. Press {KeybindingsManager.Data.ViewPlayers} to view it again.", "Copy", true, delegate
                    {
                        GUIUtility.systemCopyBuffer = lobby.Id;
                    });
                }

                if (!world.SpawnNPC)
                {
                    foreach (NetworkCreatureNonPlayer creature in FindObjectsOfType<NetworkCreatureNonPlayer>())
                    {
                        creature.Despawn();
                    }
                }
            }
        }
        private void SetupSP()
        {
            player.Player.gameObject.SetActive(true);

            foreach (NetworkCreatureNonPlayer npc in FindObjectsOfType<NetworkCreatureNonPlayer>())
            {
                npc.SourceCreature.gameObject.SetActive(true);
                npc.SourceCreature.Setup();
            }
        }
        #endregion
    }
}