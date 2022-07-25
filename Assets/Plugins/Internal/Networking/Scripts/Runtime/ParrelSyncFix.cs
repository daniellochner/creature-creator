#if UNITY_EDITOR
using ParrelSync;
#endif

using UnityEngine;

namespace DanielLochner.Assets
{
    public class ParrelSyncFix : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Start()
        {
            if (ClonesManager.IsClone())
            {
                UnityEditor.PlayerSettings.productName = ClonesManager.GetArgument();
            }
        }
#endif
    }
}