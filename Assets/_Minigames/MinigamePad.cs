using System.Collections;
using TMPro;
using Unity.Netcode;
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
        public int NumPlayers
        {
            get => region.tracked.Count;
        }

        private bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                minigameText.gameObject.SetActive(isVisible);
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
                UpdateInfo();
            }
        }

        private void Setup()
        {
            if (GameSetup.Instance.IsMultiplayer)
            {
                minigameLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });

                region.OnTrack += OnTrack;
                region.OnLoseTrackOf += OnLoseTrackOf;
            }
            else
            {
                gameObject.SetActive(false);
            }
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
            }
        }

        private void UpdateInfo()
        {
            string text = $"{minigame.Name}<br>";

            if (NumPlayers < minigame.MinPlayers)
            {
                text += $"{TextUtility.FormatError(NumPlayers, true)}/{minigame.MinPlayers}<br>";
            }
            else
            {
                text += $"{TextUtility.FormatError(NumPlayers, NumPlayers > minigame.MaxPlayers)}/{minigame.MaxPlayers}<br>";
            }
            
            if (minigame.WaitTimeLeft.Value <= minigame.WaitTime)
            {
                text += $"{minigame.WaitTimeLeft.Value}";
            }

            minigameText.text = text;
        }
        #endregion
    }
}