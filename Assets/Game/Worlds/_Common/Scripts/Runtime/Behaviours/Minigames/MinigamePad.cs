using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigamePad : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Minigame minigame;
        [Space]
        [SerializeField] private TextMeshProUGUI minigameText;
        [SerializeField] private LookAtConstraint minigameLookAtConstraint;

        private TrackRegion region;
        private bool isVisible;
        #endregion

        #region Properties
        private bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                minigameText.gameObject.SetActive(isVisible);
            }
        }

        protected bool HasRequestedMinigame
        {
            get => PlayerPrefs.GetInt("REQUESTED_MINIGAME", 0) == 1;
            set => PlayerPrefs.SetInt("REQUESTED_MINIGAME", value ? 1 : 0);
        }
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
        }
        private void OnDisable()
        {
            IsVisible = false;
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
                UpdateInfo();
            }
        }

        private void Setup()
        {
            minigameLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });

            region.OnTrack += OnTrack;
            region.OnLoseTrackOf += OnLoseTrackOf;
        }
        
        private void OnTrack(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                UpdateInfo();
                IsVisible = true;
            }
        }
        private void OnLoseTrackOf(Collider col)
        {
            if (col.CompareTag("Player/Local"))
            {
                IsVisible = false;
                SignOut();
            }
        }

        private void UpdateInfo()
        {
            string text = $"{minigame.Name}<br>";

            // Waiting Players
            int waitingPlayers = minigame.WaitingPlayers.Value;
            if (waitingPlayers < minigame.MinPlayers)
            {
                text += $"{TextUtility.FormatError(waitingPlayers, true)}/{minigame.MinPlayers}<br>";
            }
            else
            {
                text += $"{TextUtility.FormatError(waitingPlayers, waitingPlayers > minigame.MaxPlayers)}/{minigame.MaxPlayers}<br>";
            }

            // Wait Time
            int waitTimeLeft = minigame.WaitTimeLeft.Value;
            if (waitingPlayers >= minigame.MinPlayers && waitingPlayers <= minigame.MaxPlayers && waitTimeLeft <= minigame.WaitTime)
            {
                text += $"{waitTimeLeft}";
            }

            minigameText.text = text;
        }

        public void SignUp()
        {
            MinigameManager.Instance.CurrentPad = this;
            minigame.SignMeUp(true);

            Player.Instance.Rider.Dismount();

            if (!WorldManager.Instance.IsMultiplayer && !HasRequestedMinigame)
            {
                this.Invoke(delegate
                {
                    InformationDialog.Inform(LocalizationUtility.Localize("minigame_singleplayer_title"), LocalizationUtility.Localize("minigame_singleplayer_message"));
                },
                0.5f);
            }

            HasRequestedMinigame = true;
        }
        public void SignOut()
        {
            MinigameManager.Instance.CurrentPad = null;
            minigame.SignMeUp(false);
        }
        #endregion
    }
}