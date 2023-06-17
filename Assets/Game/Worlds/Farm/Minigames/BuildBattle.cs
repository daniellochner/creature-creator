using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BuildBattle : IndividualMinigame
    {
        #region Fields
        [Header("Build Battle")]
        [SerializeField] private TextMeshProUGUI topicText;
        [SerializeField] private CreatureConstructor creatureToRate;
        [SerializeField] private Cinematic ratingCinematic;
        [SerializeField] private CanvasGroup ratingCG;
        [SerializeField] private int rateTime;
        [SerializeField] private int stepSize;
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

            starting.onEnter += OnStartingEnter;

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
                if (t >= top)
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

        #region Starting
        private void OnStartingEnter()
        {
            SetScoreboardClientRpc(false);
        }

        [ClientRpc]
        private void SetScoreboardClientRpc(bool isVisible)
        {
            if (InMinigame)
            {
                MinigameManager.Instance.Scoreboard.gameObject.SetActive(isVisible);
            }
        }
        #endregion

        #region Playing
        protected override IEnumerator GameplayLogicRoutine()
        {
            StartRatingClientRpc();

            yield return new WaitForSeconds(1f);

            foreach (ulong clientId in players)
            {
                playerRatings[clientId] = new Dictionary<ulong, int>()
                {
                    { clientId, 0 }
                };

                currentClientId = clientId;


                SetRatingPadVisibleClientRpc(true);

                CreaturePlayer player = NetworkManager.SpawnManager.GetPlayerNetworkObject(currentClientId).GetComponent<CreaturePlayer>();
                DisplayCreatureClientRpc(currentClientId, player.Constructor.Data);

                RateTimeLeft.Value = rateTime;
                while (RateTimeLeft.Value > 0)
                {
                    yield return new WaitForSeconds(1f);
                    RateTimeLeft.Value--;
                }

                SetRatingPadVisibleClientRpc(false);
                ClearCreatureClientRpc();


                yield return new WaitForSeconds(2f);
            }

            // Determine winner (from the players that are currently in the game)
            foreach (ulong ratedClientId in players)
            {
                int rating = 0;
                foreach (ulong ratingClientId in players)
                {
                    rating += playerRatings[ratedClientId][ratingClientId];
                }
                SetScore(ratedClientId, rating);
            }

            StopRatingClientRpc();
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

        public void SetMyRating(int rating)
        {
            SetRatingServerRpc(rating, NetworkManager.Singleton.LocalClientId);
            SetRatingPadInteractable(false);
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
                SetRatingPadInteractable(!NetworkUtils.IsPlayer(currentClientId));
            }

            creatureToRate.transform.parent.localRotation = Quaternion.identity;
            creatureToRate.Demolish();
            creatureToRate.Body.gameObject.SetActive(true);
            creatureToRate.Construct(data);
        }

        [ClientRpc]
        private void SetRatingPadVisibleClientRpc(bool isVisible)
        {
            if (InMinigame)
            {
                ratingCG.gameObject.SetActive(isVisible);
            }
        }

        private void SetRatingPadInteractable(bool isInteractable)
        {
            ratingCG.interactable = isInteractable;
            ratingCG.alpha = isInteractable ? 1f : 0.25f;
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
            SetScoreboardClientRpc(true);

            SetRatingPadVisibleClientRpc(false);
            ClearCreatureClientRpc();
        }

        private void OnCompletingExit()
        {
            playerRatings.Clear();

            TopicId.Value = -1;
        }

        [ClientRpc]
        private void ClearCreatureClientRpc()
        {
            creatureToRate.Body.gameObject.SetActive(false);
        }
        #endregion
        #endregion
    }
}