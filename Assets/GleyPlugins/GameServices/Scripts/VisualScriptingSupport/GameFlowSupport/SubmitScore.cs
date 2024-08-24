#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEngine;
    [AddComponentMenu("")]

    public class SubmitScore : Action
    {
        // Declare a Variable-friendly property for the action
        [SerializeField]
        private long scoreValue;
        [SerializeField]
        private Variable scoreVariable;
        [SerializeField]
        private LeaderboardNames leaderboard;

        // Define a convenience property getter
        private long score
        {
            // Link string value and Variable reference through an extension method
            get { return (long)scoreVariable.GetValue(scoreValue); }
        }

        // Code implementing the effect of the action
        protected override void OnExecute()
        {
            GameServices.Instance.SubmitScore(score, leaderboard);
        }
    }
}
#endif
