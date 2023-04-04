#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEngine;

    [AddComponentMenu("")]

    public class ShowAchievements : Action
    {
        // Code implementing the effect of the action
        protected override void OnExecute()
        {
            // Do something with the declared property
            GameServices.Instance.ShowAchievementsUI();
        }
    }
}
#endif
