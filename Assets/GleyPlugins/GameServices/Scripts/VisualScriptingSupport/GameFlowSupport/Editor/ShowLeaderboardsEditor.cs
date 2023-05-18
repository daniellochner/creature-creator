#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEditor;
    [CustomEditor(typeof(ShowLeaderboards), true)]
    public class ShowLeaderboardsEditor : ActionEditor
    {
        public override bool IsFoldable()
        {
            return false;
        }
    }
}
#endif
