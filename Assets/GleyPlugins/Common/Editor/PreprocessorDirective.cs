using UnityEditor;

namespace Gley.Common
{
    public class PreprocessorDirective
    {
        public static void AddToPlatform(string directive, bool remove, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            if (remove)
            {
                if (textToWrite.Contains(directive))
                {
                    textToWrite = textToWrite.Replace(directive, "");
                }
            }
            else
            {
                if (!textToWrite.Contains(directive))
                {
                    if (textToWrite == "")
                    {
                        textToWrite += directive;
                    }
                    else
                    {
                        textToWrite += "," + directive;
                    }
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
        }

        public static void AddToCurrent(string directive, bool remove)
        {
            AddToPlatform(directive, remove, EditorUserBuildSettings.selectedBuildTargetGroup);
        }
    }
}
