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
        public TrackRegion Region => region;

        private bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                minigameText.gameObject.SetActive(isVisible);
            }
        }

        public int NumTracked
        {
            get => Region.tracked.Count;
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
            if (WorldManager.Instance.IsMultiplayer)
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
            if (col.gameObject.IsPlayer())
            {
                UpdateInfo();
                IsVisible = true;
            }
        }
        private void OnLoseTrackOf(Collider col)
        {
            if (col.gameObject.IsPlayer())
            {
                IsVisible = false;
            }
        }

        private void UpdateInfo()
        {
            string text = $"{minigame.Name}<br>";

            if (NumTracked < minigame.MinPlayers)
            {
                text += $"{TextUtility.FormatError(NumTracked, true)}/{minigame.MinPlayers}<br>";
            }
            else
            {
                text += $"{TextUtility.FormatError(NumTracked, NumTracked > minigame.MaxPlayers)}/{minigame.MaxPlayers}<br>";
            }
            
            if (NumTracked >= minigame.MinPlayers && NumTracked <= minigame.MaxPlayers && minigame.WaitTimeLeft.Value <= minigame.WaitTime)
            {
                text += $"{minigame.WaitTimeLeft.Value}";
            }

            minigameText.text = text;
        }
        #endregion
    }
}