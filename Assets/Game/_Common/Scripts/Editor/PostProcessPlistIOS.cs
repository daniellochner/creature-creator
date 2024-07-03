#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PostProcessPlistIOS
    {
        [PostProcessBuild]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                // Read plist
                string plistPath = pathToBuiltProject + "/Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get plist's root
                PlistElementDict rootDict = plist.root;

                // Set "Privacy - Tracking Usage Description" string
                rootDict.SetString("NSUserTrackingUsageDescription", "This data will ONLY be used to deliver personalized ads.");

                // Write plist
                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}
#endif