#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEditor;
    [CustomEditor(typeof(ShowAchievements), true)]
    public class ShowAchievementsEditor : ActionEditor
    {
        public override bool IsFoldable()
        {
            return false;
        }
    }
}
#endif
