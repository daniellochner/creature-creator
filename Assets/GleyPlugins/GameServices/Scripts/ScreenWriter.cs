namespace GleyGameServices
{
    using UnityEngine;
    // writes on the screen

    public class ScreenWriter : MonoBehaviour
    {
        private static string logMessage;
        private static ScreenWriter instance;

        public static void Write(object message)
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "DebugMessagesHolder";
                instance = go.AddComponent<ScreenWriter>();
                logMessage += ("\nDebugMessages instance created on DebugMessagesHolder");
            }
            logMessage += "\n" + message.ToString();
        }

        //void OnGUI()
        //{
        //    if (logMessage != null)
        //    {
        //        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), logMessage);
        //        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), "Clear"))
        //        {
        //            logMessage = null;
        //        }
        //    }
        //}
    }
}
