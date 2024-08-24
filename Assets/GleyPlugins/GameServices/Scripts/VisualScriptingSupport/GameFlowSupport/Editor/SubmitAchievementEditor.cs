#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEditor;

    [CustomEditor(typeof(SubmitAchievement))]
    public class SubmitAchievementEditor : ActionEditor
    {
        // Declare properties exactly as defined in the Action subclass
        protected SerializedProperty achievementToSubmit;

        // Action user interface
        protected override void OnActionGUI()
        {
            // Draws a Variable-friendly text field for the property in the Inspector
            PropertyField("Achievement to submit", achievementToSubmit);
        }
    }
}
#endif
