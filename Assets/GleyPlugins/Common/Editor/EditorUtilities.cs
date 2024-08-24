using UnityEditor;

namespace Gley.Common
{
    public class EditorUtilities
    {
        /// <summary>
        /// Create a folder at path recursively
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string[] folders = path.Split('/');
                string tempPath = "";
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    tempPath += folders[i];
                    if (!AssetDatabase.IsValidFolder(tempPath + "/" + folders[i + 1]))
                    {
                        AssetDatabase.CreateFolder(tempPath, folders[i + 1]);
                        AssetDatabase.Refresh();
                    }
                    tempPath += "/";
                }
            }
        }


        public static string FindFolder(string folderName, string parent)
        {
            string result = null;
            var folders = AssetDatabase.GetSubFolders("Assets");
            foreach (var folder in folders)
            {
                result = Recursive(folder, folderName, parent);
                if (result != null)
                {
                    return result;
                }
            }
            return result;
        }

        static string Recursive(string currentFolder, string folderToSearch, string parent)
        {
            if (currentFolder.EndsWith($"{parent}/{folderToSearch}"))
            {
                return currentFolder;
            }
            var folders = AssetDatabase.GetSubFolders(currentFolder);
            foreach (var fld in folders)
            {
                string result = Recursive(fld, folderToSearch, parent);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
