using System.IO;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SaveUtility
    {
        public static void Save<T>(string filePath, T data, string encryptionKey = null)
        {
            File.WriteAllText(filePath, StringCipher.Encrypt(JsonUtility.ToJson(data), encryptionKey));
        }
        public static T Load<T>(string filePath, string encryptionKey = null) where T : class, new()
        {
            T tmp = null;
            if (File.Exists(filePath))
            {
                try
                {
                    tmp = JsonUtility.FromJson<T>(StringCipher.Decrypt(File.ReadAllText(filePath), encryptionKey));
                }
                catch { return null; }
            }
            return tmp ?? new T();
        }
    }
}