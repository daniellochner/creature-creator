using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Teleport : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string targetSceneId;
        [SerializeField] private Keybind keybind;
        [Space]
        [SerializeField] private TextMeshPro teleportText;
        [SerializeField] private LookAtConstraint teleportConstraint;

        private TrackRegion region;
        #endregion

        #region Properties
        private bool CanChange
        {
            get => (WorldManager.Instance.World is WorldSP) || (NetworkManager.Singleton.IsServer && (region.tracked.Count == NetworkPlayersMenu.Instance.NumPlayers));
        }
        private bool ShowCount
        {
            get => (WorldManager.Instance.World is WorldMP) && (NetworkPlayersMenu.Instance.NumPlayers > 1);
        }

        public bool IsChanging { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => SetupUtility.IsSetup(GameSetup.Instance, Player.Instance));

            teleportConstraint.AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
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

                if (!IsChanging && CanChange && InputUtility.GetKeyDown(keybind))
                {
                    ConfirmationDialog.Confirm(LocalizationUtility.Localize("teleport_title", LocalizationUtility.Localize(targetSceneId)), LocalizationUtility.Localize("teleport_message", targetSceneId), onYes: delegate
                    {
                        ChangeScene();
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

        private async void ChangeScene()
        {
            IsChanging = true;

            // Lobby
            if (WorldManager.Instance.World is WorldMP)
            {
                UpdateLobbyOptions options = new UpdateLobbyOptions()
                {
                    Data = new System.Collections.Generic.Dictionary<string, DataObject>()
                    {
                        { "mapName", new DataObject(DataObject.VisibilityOptions.Public, targetSceneId) }
                    }
                };
                options.HostId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.UpdateLobbyAsync(LobbyHelper.Instance.JoinedLobby.Id, options);
            }

            // Scene
            NetworkManager.Singleton.SceneManager.LoadScene(targetSceneId, LoadSceneMode.Single);
        }

        private void UpdateInfo()
        {
            string text = $"{LocalizationUtility.Localize(targetSceneId)}<br>";
            if (ShowCount)
            {
                text += $"{region.tracked.Count}/{NetworkPlayersMenu.Instance.NumPlayers}<br>";
            }
            if (CanChange)
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