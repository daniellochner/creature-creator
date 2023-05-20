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
    public class MinigamePad : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Minigame minigame;
        [SerializeField] private float countdownTime;
        [Space]
        [SerializeField] private TextMeshProUGUI minigameText;
        [SerializeField] private LookAtConstraint minigameLookAtConstraint;

        private TrackRegion region;
        private bool isVisible;
        private float timeLeft;
        #endregion

        #region Properties
        private bool CanBegin
        {
            get => (minigame.State == Minigame.GameState.WaitingForPlayers) && (NumPlayers > minigame.MinPlayers) && (NumPlayers < minigame.MaxPlayers);
        }
        private int NumPlayers
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

                if (CanBegin)
                {
                    if (timeLeft > 0)
                    {
                        timeLeft -= Time.deltaTime;
                    }
                    else
                    {
                        minigame.Begin();
                    }
                }
                else
                {
                    timeLeft = countdownTime;
                }
            }
        }

        private void Setup()
        {
            minigameLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });

            region.OnTrack += OnTrack;
            region.OnLoseTrackOf += OnLoseTrackOf;
        }

        private void OnTrack(Collider col1, Collider col2)
        {
            if (col1.CompareTag("Player/Local"))
            {
                UpdateInfo();
                IsVisible = true;
            }
        }
        private void OnLoseTrackOf(Collider col1, Collider col2)
        {
            if (col1.CompareTag("Player/Local"))
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
            
            if (timeLeft <= countdownTime)
            {
                text += $"<size=1>{Mathf.RoundToInt(timeLeft)}</size>";
            }

            minigameText.text = text;
        }
        #endregion
    }
}