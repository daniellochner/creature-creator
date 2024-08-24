#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEngine;

    [AddComponentMenu("")]

    public class SubmitAchievement : Action
    {
        [SerializeField]
        private AchievementNames achievementToSubmit;

        // Code implementing the effect of the action
        protected override void OnExecute()
        {
            // Do something with the declared property
            Debug.Log("SUBMIT ACHIEVEMENT " + achievementToSubmit);
            GameServices.Instance.SubmitAchievement(achievementToSubmit);
        }
    }
}
#endif
