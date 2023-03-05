using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Scoreboard : NetworkSingleton<Scoreboard>
    {
        #region Fields
        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private int blinkCount;
        [SerializeField] private float blinkTime;

        private Coroutine winCoroutine;
        #endregion

        #region Properties
        public NetworkVariable<int> RedScore { get; private set; } = new NetworkVariable<int>(0);
        public NetworkVariable<int> BlueScore { get; private set; } = new NetworkVariable<int>(0);
        #endregion

        #region Methods
        private void Start()
        {
            RedScore.OnValueChanged += delegate
            {
                UpdateScoreboard();
            };
            BlueScore.OnValueChanged += delegate
            {
                UpdateScoreboard();
            };

            UpdateScoreboard();
        }
        
        private void UpdateScoreboard()
        {
            if (winCoroutine != null)
            {
                Reset();
            }

            scoreText.text = $"{BlueScore.Value:00}-{RedScore.Value:00}";
            if (RedScore.Value >= 10 || BlueScore.Value >= 10)
            {
                winCoroutine = StartCoroutine(WinRoutine());
#if USE_STATS
                StatsManager.Instance.UnlockAchievement("ACH_FOOTBALL_FRENZY");
#endif
            }
        }

        private IEnumerator WinRoutine()
        {
            for (int i = 0; i < blinkCount; ++i)
            {
                scoreText.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                scoreText.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }

            Reset();
        }
        private void Reset()
        {
            StopCoroutine(winCoroutine);
            winCoroutine = null;
            scoreText.enabled = true;

            if (IsServer)
            {
                RedScore.Value = BlueScore.Value = 0;
            }
        }
        #endregion
    }
}