using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class RedmatchSDKCopier
{
	[MenuItem("Creature Creator/Copy SDK Files")]
	public static void CopyFiles()
	{
		string sourcePath = Path.Combine(Application.dataPath, "Creature Creator SDK");
		string destinationPath = EditorUtility.OpenFolderPanel("Find creature-creator-sdk\\Assets\\Creature Creator SDK", Application.dataPath, "Creature Creator SDK");

		if(string.IsNullOrEmpty(destinationPath))
			return;

		string[] files = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);

		IEnumerable<string> AllFiles()
		{
			foreach(var file in files)
			{
				yield return file;
				yield return file + ".meta";
			}
		}

		foreach(var file in AllFiles())
		{
			string normalizedPath = file.Substring(sourcePath.Length);
			string targetPath = destinationPath + normalizedPath;

			Debug.Log($"Copying {file} to {targetPath}");

			string dir = Directory.GetParent(targetPath).FullName;

			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			File.WriteAllText(targetPath, ProcessFile(File.ReadAllText(file)));
		}

		Debug.Log("Done copying SDK files");
	}

	static string ProcessFile(string source)
	{
		string excludeStartStr = "/*<SDK EXCLUDE>*/";
		string excludeEndStr = "/*<SDK EXCLUDE END>*/";

		int excludeStart = source.IndexOf(excludeStartStr);
		int excludeEnd = source.IndexOf(excludeEndStr);

		if(excludeStart == -1)
		{
			return source;
		}

		string result = source.Substring(0, excludeStart) + source.Substring(excludeEnd + excludeEndStr.Length);

		return result;
	}
}
