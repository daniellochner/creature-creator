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

        public bool HasWon { get; private set; }
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
        }
        
        private void UpdateScoreboard()
        {
            if (HasWon) return;

            scoreText.text = $"{BlueScore.Value:00}:{RedScore.Value:00}";
            if (RedScore.Value >= 10 || BlueScore.Value >= 10)
            {
                if (winCoroutine != null)
                {
                    StopCoroutine(winCoroutine);
                }
                winCoroutine = StartCoroutine(WinRoutine());
            }
        }

        private IEnumerator WinRoutine()
        {
            HasWon = true;

            for (int i = 0; i < blinkCount; ++i)
            {
                scoreText.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                scoreText.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }

            HasWon = false;
            RedScore.Value = BlueScore.Value = 0;
        }
        #endregion
    }
}