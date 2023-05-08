#if UNITY_IOS

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace IronSource.Editor
{
	public class IronSourceBuildPostprocessor
	{
		[PostProcessBuild]
		public static void OnPostprocessBuild (BuildTarget buildTarget, string buildPath)
		{
			if (buildTarget == BuildTarget.iOS) {
				string projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
				string dirpath = Application.dataPath + "/IronSource/Editor/";
				string currentNamespace = MethodBase.GetCurrentMethod().DeclaringType.Namespace;

				updateProject (buildTarget, projectPath);

				if (Directory.Exists (dirpath)) {
					//Match the classes that has "Settings" in their name, and don't start with "I"
					var files = Directory.GetFiles (dirpath, "*.cs", SearchOption.TopDirectoryOnly).Where (file => Regex.IsMatch (Path.GetFileName (file), "^(?!(IAdapter|IronSource)).+Settings.*$"));

					//Go over all the adapter settings classes, and call their updateProject method
					foreach (string file in files) {
						string classname = Path.GetFileNameWithoutExtension (file);

						if (!String.IsNullOrEmpty (classname)) {
							IAdapterSettings adapter = (IAdapterSettings)Activator.CreateInstance (Type.GetType (currentNamespace + "." + classname));
							adapter.updateProject (buildTarget, projectPath);
						}
					}
				}
			}

			Debug.Log ("IronSource build postprocessor finished");
		}

		private static void updateProject (BuildTarget buildTarget, string projectPath)
		{
			Debug.Log ("IronSource - Update project for IronSource");

			PBXProject project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));

 			string targetId; 
#if UNITY_2019_3_OR_NEWER
            targetId = project.GetUnityMainTargetGuid();
#else
            targetId = project.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
            
			// Required System Frameworks
			project.AddFrameworkToProject (targetId, "AdSupport.framework", false);
			project.AddFrameworkToProject (targetId, "AudioToolbox.framework", false);
			project.AddFrameworkToProject (targetId, "AVFoundation.framework", false);
			project.AddFrameworkToProject (targetId, "CoreGraphics.framework", false);
			project.AddFrameworkToProject (targetId, "CoreMedia.framework", false);
			project.AddFrameworkToProject (targetId, "CoreTelephony.framework", false);
			project.AddFrameworkToProject (targetId, "CoreVideo.framework", false);
			project.AddFrameworkToProject (targetId, "CFNetwork.framework", false);		
			project.AddFrameworkToProject (targetId, "Foundation.framework", false);
			project.AddFrameworkToProject (targetId, "MobileCoreServices.framework", false);
			project.AddFrameworkToProject (targetId, "QuartzCore.framework", false);
			project.AddFrameworkToProject (targetId, "Security.framework", false);
			project.AddFrameworkToProject (targetId, "StoreKit.framework", false);
			project.AddFrameworkToProject (targetId, "SystemConfiguration.framework", false);
			project.AddFrameworkToProject (targetId, "WebKit.framework", false);
		
			
			project.AddFileToBuild (targetId, project.AddFile ("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));

			// Custom Link Flag
			project.AddBuildProperty (targetId, "OTHER_LDFLAGS", "-ObjC");

			File.WriteAllText (projectPath, project.WriteToString ());
		}
	}
}
#endif

