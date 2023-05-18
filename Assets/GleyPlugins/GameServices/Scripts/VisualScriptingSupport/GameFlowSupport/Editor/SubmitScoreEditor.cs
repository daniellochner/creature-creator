#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEditor;

    [CustomEditor(typeof(SubmitScore))]
    public class SubmitScoreEditor : ActionEditor
    {
        // Declare properties exactly as defined in the Action subclass
        protected SerializedProperty scoreValue;
        protected SerializedProperty scoreVariable;
        protected SerializedProperty leaderboard;

        // Action user interface
        protected override void OnActionGUI()
        {
            // Draws a Variable-friendly text field for the property in the Inspector
            PropertyField("Score Variable", scoreValue, scoreVariable);
            PropertyField("Leaderboard", leaderboard);
        }
    }
}
#endif
