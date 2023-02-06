using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Teleport : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string targetMapName;
        [SerializeField] private string targetMapId;
        [SerializeField] private Keybind keybind;
        [SerializeField] private TeleportCinematic cinematic;
        [Space]
        [SerializeField] private TextMeshPro teleportText;
        [SerializeField] private LookAtConstraint teleportLookAtConstraint;

        private TrackRegion region;
        private bool isTeleporting;
        #endregion

        #region Properties
        private bool CanTeleport
        {
            get => (WorldManager.Instance.World is WorldSP) || (NetworkManager.Singleton.IsServer && (region.tracked.Count == NetworkPlayersMenu.Instance.NumPlayers));
        }
        private bool ShowCount
        {
            get => (WorldManager.Instance.World is WorldMP) && (NetworkPlayersMenu.Instance.NumPlayers > 1);
        }
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
        }
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);

            teleportLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                UpdateInfo();
                SetVisibility(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                UpdateInfo();

                if (!isTeleporting && CanTeleport && InputUtility.GetKeyDown(keybind))
                {
                    ConfirmationDialog.Confirm(LocalizationUtility.Localize("teleport_title", LocalizationUtility.Localize(targetMapId)), LocalizationUtility.Localize("teleport_message"), onYes: delegate
                    {
                        if (cinematic != null)
                        {
                            TeleportCinematicClientRpc();
                        }
                        else
                        {
                            InitializeTeleport();
                        }
                        HideClientRpc();
                    });
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                SetVisibility(false);
            }
        }

        private async void InitializeTeleport()
        {
            isTeleporting = true;

            if (WorldManager.Instance.World is WorldMP)
            {
                UpdateLobbyOptions options = new UpdateLobbyOptions()
                {
                    Data = new System.Collections.Generic.Dictionary<string, DataObject>()
                    {
                        { "mapName", new DataObject(DataObject.VisibilityOptions.Public, targetMapName) },
                        { "mapId", new DataObject(DataObject.VisibilityOptions.Public, targetMapId) }
                    }
                };
                options.HostId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.UpdateLobbyAsync(LobbyHelper.Instance.JoinedLobby.Id, options);
            }

            TeleportClientRpc();
        }
        [ClientRpc]
        private void TeleportClientRpc()
        {
            TeleportManager.Instance.TeleportTo(targetMapName, JsonUtility.FromJson<CreatureData>(JsonUtility.ToJson(Player.Instance.Constructor.Data)));
        }
        [ClientRpc]
        private void TeleportCinematicClientRpc()
        {
            if (IsHost)
            {
                cinematic.OnTeleport = InitializeTeleport;
            }
            cinematic.Begin();
        }
        [ClientRpc]
        private void HideClientRpc()
        {
            Player.Instance.Loader.HideFromOthers();
        }

        private void UpdateInfo()
        {
            string text = $"{LocalizationUtility.Localize(targetMapId)}<br>";
            if (ShowCount)
            {
                text += $"{region.tracked.Count}/{NetworkPlayersMenu.Instance.NumPlayers}<br>";
            }
            if (CanTeleport)
            {
                text += $"<size=1>[{keybind.ToString()}]</size>";
            }
            teleportText.text = text;
        }
        private void SetVisibility(bool isVisible)
        {
            teleportText.gameObject.SetActive(isVisible);
        }
        #endregion
    }
}