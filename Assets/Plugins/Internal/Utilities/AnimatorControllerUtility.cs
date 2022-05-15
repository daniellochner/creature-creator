using UnityEngine;

namespace DanielLochner.Assets
{
    public static class AnimatorControllerUtility
    {
#if UNITY_EDITOR
        public static bool HasParameter(this UnityEditor.Animations.AnimatorController controller, string paramName)
        {
            foreach (AnimatorControllerParameter param in controller.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }
#endif
    }
}