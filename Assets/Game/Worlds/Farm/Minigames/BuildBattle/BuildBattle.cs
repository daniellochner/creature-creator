using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BuildBattle : IndividualMinigame
    {
        #region Fields
        [Header("Build Battle")]
        [SerializeField] private TextMeshProUGUI topicText;
        [SerializeField] private CreatureConstructor creatureToRate;
        [SerializeField] private Cinematic ratingCinematic;
        [SerializeField] private GameObject ratingPad;
        [SerializeField] private int rateTime;
        [SerializeField] private int stepSize;
        [SerializeField] private Toggle[] ratingToggles;
        [SerializeField] private string[] topicIds;

        private Dictionary<ulong, Dictionary<ulong, int>> playerRatings = new Dictionary<ulong, Dictionary<ulong, int>>();
        private ulong currentClientId;
        #endregion

        #region Properties
        public NetworkVariable<int> TopicId { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> RateTimeLeft { get; set; } = new NetworkVariable<int>();
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();

            if (IsClient)
            {
                TopicId.OnValueChanged += OnTopicIdChanged;
                RateTimeLeft.OnValueChanged += OnRateTimeLeftChanged;

                OnTopicIdChanged(default, TopicId.Value);
            }
        }

        protected override void Setup()
        {
            base.Setup();

            building.onEnter += OnBuildingEnter;
            building.onExit += OnBuildingExit;

            starting.onEnter += OnStartingEnter;

            playing.onEnter += OnPlayingEnter;
            playing.onExit += OnPlayingExit;

            completing.onEnter += OnCompletingEnter;
            completing.onExit += OnCompletingExit;
        }

        #region Introducing
        protected override void OnCinematic()
        {
            base.OnCinematic();
            StartCoroutine(ChooseRandomTopicRoutine());
        }

        private IEnumerator ChooseRandomTopicRoutine()
        {
            yield return new WaitForSeconds(0.5f);

            int top = 0, current = Random.Range(0, topicIds.Length);
            yield return this.InvokeOverTime(delegate (float p)
            {
                float t = EasingFunction.EaseOutQuad(0f, 1000f, p);
                if (t > top)
                {
                    TopicId.Value = (current++) % topicIds.Length;
                    top += stepSize;
                }
            }, 4);
        }

        private void OnTopicIdChanged(int oldTopicId, int newTopicId)
        {
            topicText.gameObject.SetActive(newTopicId != -1);
            if (newTopicId != -1)
            {
                topicText.text = LocalizationUtility.Localize(topicIds[newTopicId]);
            }
        }
        #endregion

        #region Building
        private void OnBuildingEnter()
        {
            SetTopicClientRpc(true);
        }
        private void OnBuildingExit()
        {
            SetTopicClientRpc(false);
        }

        [ClientRpc]
        private void SetTopicClientRpc(bool isVisible)
        {
            if (InMinigame)
            {
                MinigameManager.Instance.SetTitle(isVisible ? LocalizationUtility.Localize(topicIds[TopicId.Value]) : null);
            }
        }
        #endregion

        #region Starting
        private void OnStartingEnter()
        {
            SetScoreboardActiveClientRpc(false);
            SetPlayerNameActiveClientRpc(false);
        }

        [ClientRpc]
        private void SetScoreboardActiveClientRpc(bool isActive)
        {
            if (InMinigame)
            {
                MinigameManager.Instance.Scoreboard.gameObject.SetActive(isActive);
            }
        }

        [ClientRpc]
        private void SetPlayerNameActiveClientRpc(bool isActive)
        {
            if (InMinigame)
            {
                foreach (CreaturePlayer player in CreaturePlayer.Players)
                {
                    if (player is CreaturePlayerRemote)
                    {
                        player.Namer.enabled = isActive;
                    }
                }
            }
        }
        #endregion

        #region Playing
        private void OnPlayingEnter()
        {
            StartRatingClientRpc();
        }
        private void OnPlayingExit()
        {
            StopRatingClientRpc();
            SetPlayerNameActiveClientRpc(true);
        }

        protected override IEnumerator GameplayLogicRoutine()
        {
            yield return new WaitForSeconds(1f);

            // Rate
            foreach (ulong clientId in players)
            {
                playerRatings[clientId] = new Dictionary<ulong, int>();
                currentClientId = clientId;


                SetRatingPadActiveClientRpc(true);

                DisplayCreatureClientRpc(currentClientId, NetworkManager.SpawnManager.GetPlayerNetworkObject(currentClientId).GetComponent<CreaturePlayer>().Constructor.Data);

                RateTimeLeft.Value = rateTime;
                while (RateTimeLeft.Value > 0)
                {
                    yield return new WaitForSeconds(1f);
                    RateTimeLeft.Value--;
                }

                SetRatingPadActiveClientRpc(false);
                ClearCreatureClientRpc();


                yield return new WaitForSeconds(1f);
            }

            // Determine Winner
            foreach (ulong ratedClientId in players)
            {
                int rating = 0;
                foreach (ulong ratingClientId in players)
                {
                    rating += playerRatings[ratedClientId][ratingClientId];
                }
                SetScore(ratedClientId, rating);
            }
        }

        [ClientRpc]
        private void StartRatingClientRpc()
        {
            ratingCinematic.Begin();
        }

        [ClientRpc]
        private void StopRatingClientRpc()
        {
            ratingCinematic.Unpause();
        }

        private void SetMyRating(int rating)
        {
            SetRatingServerRpc(rating, NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetRatingServerRpc(int rating, ulong ratingClientId)
        {
            playerRatings[currentClientId][ratingClientId] = rating;
        }

        [ClientRpc]
        private void DisplayCreatureClientRpc(ulong currentClientId, CreatureData data)
        {
            if (InMinigame)
            {
                SetRatingPadActiveClientRpc(!NetworkUtils.IsPlayer(currentClientId));
            }

            creatureToRate.transform.parent.localRotation = Quaternion.identity;
            creatureToRate.Demolish();
            creatureToRate.Body.gameObject.SetActive(true);
            creatureToRate.Construct(data);
        }

        [ClientRpc]
        private void ClearCreatureClientRpc()
        {
            if (InMinigame)
            {
                for (int i = 0; i < ratingToggles.Length; i++)
                {
                    if (ratingToggles[i].isOn)
                    {
                        SetMyRating(i+1);
                        break;
                    }
                }
            }

            creatureToRate.Body.gameObject.SetActive(false);
        }

        [ClientRpc]
        private void SetRatingPadActiveClientRpc(bool isActive)
        {
            if (InMinigame)
            {
                ratingPad.SetActive(isActive);
                if (isActive)
                {
                    ratingToggles[0].SetIsOnWithoutNotify(true);
                }
            }
        }

        private void OnRateTimeLeftChanged(int oldTime, int newTime)
        {
            if (InMinigame)
            {
                if (newTime > 0)
                {
                    Color color = (newTime <= 3) ? Color.red : Color.white;

                    MinigameManager.Instance.SetSubtitle(newTime.ToString(), color);
                }
                else
                {
                    MinigameManager.Instance.SetSubtitle(null, Color.white);
                }
            }
        }
        #endregion

        #region Completing
        private void OnCompletingEnter()
        {
            SetScoreboardActiveClientRpc(true);

            SetRatingPadActiveClientRpc(false);
            ClearCreatureClientRpc();
        }

        private void OnCompletingExit()
        {
            playerRatings.Clear();

            TopicId.Value = -1;
        }
        #endregion
        #endregion
    }
}