using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

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
        private bool isTeleporting, isVisible;
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

        private bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                teleportText.gameObject.SetActive(isVisible);
            }
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
            Setup();
        }
        private void Update()
        {
            if (IsVisible)
            {
                if (!isTeleporting && InputUtility.GetKeyDown(keybind) && CanTeleport)
                {
                    RequestTeleport();
                }
                UpdateInfo();
            }
        }

        private void Setup()
        {
            teleportLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });

            region.OnTrack += OnTrack;
            region.OnLoseTrackOf += OnLoseTrackOf;
        }

        private void OnTrack(Collider col1, Collider col2)
        {
            if (col1.CompareTag("Player/Local"))
            {
                UpdateInfo();
                IsVisible = true;

                if (SystemUtility.IsDevice(DeviceType.Handheld) && CanTeleport)
                {
                    this.Invoke(RequestTeleport, 1f);
                }
            }
        }
        private void OnLoseTrackOf(Collider col1, Collider col2)
        {
            if (col1.CompareTag("Player/Local"))
            {
                IsVisible = false;
            }
        }

        private void RequestTeleport()
        {
            ConfirmationDialog.Confirm(LocalizationUtility.Localize("teleport_title", LocalizationUtility.Localize(targetMapId)), LocalizationUtility.Localize("teleport_message"), onYes: delegate
            {
                UnlockMapClientRpc();

                if (cinematic != null)
                {
                    TeleportCinematicClientRpc();
                }
                else
                {
                    InitializeTeleport();
                }

                isTeleporting = true;
            });
        }

        private async void InitializeTeleport()
        {
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

            Player.Instance.Collider.enabled = false;
        }
        [ClientRpc]
        private void UnlockMapClientRpc()
        {
            if (!WorldManager.Instance.World.CreativeMode && ProgressManager.Instance.UnlockMap(Enum.Parse<Map>(targetMapName)))
            {
                NotificationsManager.Notify(LocalizationUtility.Localize("map_unlocked", LocalizationUtility.Localize(targetMapId)));
            }
        }

        private void UpdateInfo()
        {
            string text = $"{LocalizationUtility.Localize(targetMapId)}<br>";

            if (ShowCount)
            {
                text += $"{region.tracked.Count}/{NetworkPlayersMenu.Instance.NumPlayers}<br>";
            }
            if (SystemUtility.IsDevice(DeviceType.Desktop) && CanTeleport)
            {
                text += $"<size=1>[{keybind.ToString()}]</size>";
            }

            teleportText.text = text;
        }
        #endregion
    }
}