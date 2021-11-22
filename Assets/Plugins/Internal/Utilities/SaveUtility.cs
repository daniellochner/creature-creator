using System.IO;

namespace DanielLochner.Assets
{
    public static class SaveUtility
    {
        public static void Save(string text, string filePath)
        {
            File.WriteAllText(filePath, text);
        }
        public static string Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return null;
            }
        }
    }
}