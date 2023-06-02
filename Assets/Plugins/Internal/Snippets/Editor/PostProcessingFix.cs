using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets
{
    public class PostProcessingFix : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            foreach (string ppGUID in AssetDatabase.FindAssets("t:PostProcessProfile"))
            {
                string ppPath = AssetDatabase.GUIDToAssetPath(ppGUID);
                PostProcessProfile pp = AssetDatabase.LoadAssetAtPath<PostProcessProfile>(ppPath);
                if (pp.TryGetSettings(out AmbientOcclusion ao))
                {
                    pp.RemoveSettings<AmbientOcclusion>();
                    var setting = pp.AddSettings(ao);
                    setting.active = true;

                    EditorUtility.SetDirty(pp);

                    Debug.Log($"Fixed {ppPath}", pp);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}