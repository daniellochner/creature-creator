using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Soccer : TeamMinigame
    {
        #region Fields
        [Header("Soccer")]
        [SerializeField] private float spawnDelay;
        [SerializeField] private Ball[] balls;
        [Space]
        [SerializeField] private TextMeshPro scoreText;
        [Space]
        [SerializeField] private int maxGoals;
        [Space]
        [SerializeField] private float blinkTime;
        [SerializeField] private int blinkCount;
        #endregion

        #region Properties
        public NetworkVariable<int> RedScore { get; private set; } = new NetworkVariable<int>(0);
        public NetworkVariable<int> BlueScore { get; private set; } = new NetworkVariable<int>(0);
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();

            if (IsClient)
            {
                RedScore.OnValueChanged += OnRedScoreChanged;
                BlueScore.OnValueChanged += OnBlueScoreChanged;
                UpdateInGameScoreboard();
            }
        }

        protected override void Setup()
        {
            base.Setup();

            completing.onEnter += OnCompletingEnter;
        }

        #region Introducing
        protected override void OnCinematic()
        {
            base.OnCinematic();

            this.Invoke(SpawnBalls, 0.5f);
        }

        public void SpawnBalls()
        {
            for (int i = 0; i < balls.Length; i++)
            {
                Ball ball = balls[i];
                this.Invoke(delegate
                {
                    ball.Teleport(ball.transform.parent.position);
                },
                spawnDelay * i);
            }
        }
        #endregion

        #region Building
        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            EditorManager.Instance.SetRestrictedBones(5);

            List<string> bodyParts = new List<string>();
            foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                BodyPart bodyPart = obj.Value as BodyPart;
                if (bodyPart.Abilities.Find(x => x is Abilities.Flap))
                {
                    bodyParts.Add(obj.Key);
                }
            }
            EditorManager.Instance.SetRestrictedBodyParts(bodyParts);
        }
        #endregion

        #region Playing
        protected override IEnumerator GameplayLogicRoutine()
        {
            return new WaitUntil(() => RedScore.Value >= maxGoals || BlueScore.Value >= maxGoals);
        }

        private void OnRedScoreChanged(int oldScore, int newScore)
        {
            if (IsServer && State.Value == MinigameStateType.Playing)
            {
                SetTeamScore((int)Team.Red, newScore);
            }

            UpdateInGameScoreboard();
        }
        private void OnBlueScoreChanged(int oldScore, int newScore)
        {
            if (IsServer && State.Value == MinigameStateType.Playing)
            {
                SetTeamScore((int)Team.Blue, newScore);
            }

            UpdateInGameScoreboard();
        }
        private void UpdateInGameScoreboard()
        {
            scoreText.text = $"{BlueScore.Value:00}-{RedScore.Value:00}";
        }
        #endregion

        #region Completing
        private void OnCompletingEnter()
        {
            CompleteSoccerClientRpc();
        }

        [ClientRpc]
        private void CompleteSoccerClientRpc()
        {
            StartCoroutine(WinningScoreboardRoutine());
        }

        private IEnumerator WinningScoreboardRoutine()
        {
            if (RedScore.Value > BlueScore.Value)
            {
                scoreText.color = teams[(int)Team.Red].colour;
            }
            else
            if (BlueScore.Value > RedScore.Value)
            {
                scoreText.color = teams[(int)Team.Blue].colour;
            }
            else
            {
                scoreText.color = Color.white;
            }

            for (int i = 0; i < blinkCount; ++i)
            {
                scoreText.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                scoreText.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }
            scoreText.color = Color.white;

            if (IsServer)
            {
                RedScore.Value = BlueScore.Value = 0;
            }
        }
        #endregion
        #endregion

        #region Enums
        public enum Team
        {
            Red,
            Blue
        }
        #endregion
    }
}