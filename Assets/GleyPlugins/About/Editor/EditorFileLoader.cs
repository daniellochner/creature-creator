namespace Gley.About
{
    using UnityEngine;
#if UNITY_2018_3_OR_NEWER
    using UnityEngine.Networking;
#endif

    public class EditorFileLoaded
    {
#if UNITY_2018_3_OR_NEWER
        UnityWebRequest www;
#else
        WWW www;
#endif
        public void LoadFile(string url)
        {
#if UNITY_2018_3_OR_NEWER
            www = UnityWebRequest.Get(url);
            www.SendWebRequest();
#else
            www = new WWW(url);
#endif
        }

        public bool IsDone()
        {
            return www.isDone;
        }

        public string GetResult()
        {
            if (string.IsNullOrEmpty(www.error))
            {
#if UNITY_2018_3_OR_NEWER
                return www.downloadHandler.text;
#else
                return www.text;
#endif
            }
            return null;
        }
    }
}