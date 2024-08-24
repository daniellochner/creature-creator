#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEditor;

    [CustomEditor(typeof(Login), true)]
    public class LoginEditor : FunctionEditor
    {
        protected override void OnActionGUI()
        {
            OutputField(DataType.Boolean);
        }
    }
}
#endif
