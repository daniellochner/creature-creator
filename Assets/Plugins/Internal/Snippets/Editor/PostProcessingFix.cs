using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
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
                    pp.AddSettings(ao);
                    EditorUtility.SetDirty(pp);

                    Debug.Log($"Fixed {ppPath}", pp);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}