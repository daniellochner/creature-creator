using UnityEngine;

namespace DanielLochner.Assets
{
    public class TieredFramerate : MonoBehaviourSingleton<TieredFramerate>
    {
        #region Fields
        [SerializeField] private int[] framerateTiers;
        [SerializeField] private float buffer;
        [SerializeField] private int maxWarnings;
        [SerializeField] private bool showLogs;

        private int currentTier;
        private float avg;
        private int warnings;
        #endregion

        #region Methods
        private void Update()
        {
            avg += (Time.smoothDeltaTime - avg) * 0.03f;

            float currentFramerate = 1f / avg;
            if (showLogs)
            {
                Debug.Log(currentFramerate);
            }

            if (currentTier < framerateTiers.Length)
            {
                int tierFramerate = framerateTiers[currentTier];

                if ((currentFramerate + buffer) <= tierFramerate)
                {
                    warnings++;

                    if (showLogs)
                    {
                        Debug.Log($"Warn: {currentFramerate}");
                    }

                    if (warnings >= maxWarnings)
                    {
                        currentTier++;

                        if (currentTier < framerateTiers.Length)
                        {
                            Application.targetFrameRate = framerateTiers[currentTier];

                            if (showLogs)
                            {
                                Debug.Log($"Tier {currentTier}: {framerateTiers[currentTier]}");
                            }
                        }

                        warnings = 0;
                    }
                }
            }
        }
        public void Reset()
        {
            currentTier = 0;
            warnings = 0;
            avg = 0;
        }
        #endregion
    }
}