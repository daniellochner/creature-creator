using System;
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
        [SerializeField] private Map targetMap;
        [SerializeField] private int requiredExperience;
        [SerializeField] private TeleportCinematic cinematic;
        [SerializeField] private Keybind keybind;
        [Space]
        [SerializeField] private TextMeshProUGUI teleportText;
        [SerializeField] private LookAtConstraint teleportLookAtConstraint;

        private TrackRegion region;
        private bool isTeleporting, isVisible;
        #endregion

        #region Properties
        private string TargetMapName => targetMap.ToString();
        private string TargetMapId => $"option_map_{TargetMapName.ToLower()}";

        private bool CanTeleport
        {
            get => (WorldManager.Instance.IsCreative || (ProgressManager.Data.Experience >= requiredExperience)) && (!WorldManager.Instance.IsMultiplayer || (IsServer && (NumPlayers == MaxPlayers)));
        }
        private bool ShowRequiredExperience
        {
            get => !WorldManager.Instance.IsCreative && (!WorldManager.Instance.IsMultiplayer || IsServer);
        }
        private bool ShowCount
        {
            get => WorldManager.Instance.IsMultiplayer && (MaxPlayers > 1);
        }
        private int NumPlayers
        {
            get => region.tracked.Count;
        }
        private int MaxPlayers
        {
            get => NetworkPlayersMenu.Instance.NumPlayers;
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

        private void OnTrack(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                UpdateInfo();
                IsVisible = true;

                if (SystemUtility.IsDevice(DeviceType.Handheld) && CanTeleport)
                {
                    this.Invoke(RequestTeleport, 1f);
                }
            }
        }
        private void OnLoseTrackOf(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                IsVisible = false;
            }
        }

        private void RequestTeleport()
        {
            ConfirmationDialog.Confirm(LocalizationUtility.Localize("teleport_title"), LocalizationUtility.Localize("teleport_message", LocalizationUtility.Localize(TargetMapId)), onYes: delegate
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
                        { "mapName", new DataObject(DataObject.VisibilityOptions.Public, TargetMapName) },
                        { "mapId", new DataObject(DataObject.VisibilityOptions.Public, TargetMapId) }
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
            TeleportManager.Instance.TeleportTo(TargetMapName, JsonUtility.FromJson<CreatureData>(JsonUtility.ToJson(Player.Instance.Constructor.Data)));
        }
        [ClientRpc]
        private void TeleportCinematicClientRpc()
        {
            if (IsHost)
            {
                cinematic.OnTeleport = InitializeTeleport;
            }
            cinematic.Begin();

            Player.Instance.Holder.DropAll();

            Player.Instance.Rigidbody.isKinematic = true;
            Player.Instance.Collider.enabled = false;
        }
        [ClientRpc]
        private void UnlockMapClientRpc()
        {
            if (!WorldManager.Instance.World.CreativeMode && !ProgressManager.Instance.IsMapUnlocked(targetMap))
            {
                ProgressManager.Instance.UnlockMap(targetMap);
                NotificationsManager.Notify(LocalizationUtility.Localize("map_unlocked", LocalizationUtility.Localize(TargetMapId)));
            }
        }

        private void UpdateInfo()
        {
            string text = $"{LocalizationUtility.Localize(TargetMapId)}<br>";

            if (ShowCount)
            {
                text += $"{TextUtility.FormatError(NumPlayers, NumPlayers != MaxPlayers)}/{MaxPlayers}<br>";
            }
            if (ShowRequiredExperience)
            {
                text += $"XP: {TextUtility.FormatError(ProgressManager.Data.Experience, ProgressManager.Data.Experience < requiredExperience)}/{requiredExperience}<br>";
            }
            if (SystemUtility.IsDevice(DeviceType.Desktop) && CanTeleport)
            {
                text += $"[{keybind.ToString()}]";
            }

            teleportText.text = text;
        }
        #endregion
    }
}