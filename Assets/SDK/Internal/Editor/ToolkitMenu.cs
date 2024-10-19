using UnityEditor;
using UnityEngine;

public class ToolkitMenu : MonoBehaviour
{
    [MenuItem("Creature Creator SDK/Map/Create New", priority = 10)]
    public static void CreateNewMap()
    {
    }

    [MenuItem("Creature Creator SDK/Map/Validate _F3", priority = 11)]
    public static void ValidateMap()
    {
    }

    [MenuItem("Creature Creator SDK/Map/Build and Test _F5", priority = 12)]
    public static void BuildAndTestMap()
    {
    }

    [MenuItem("Creature Creator SDK/Map/Upload", priority = 100)]
    public static void UploadMap()
    {
    }


    [MenuItem("Creature Creator SDK/Body Part/Create New", priority = 20)]
    public static void CreateNewBodyPart()
    {
    }

    [MenuItem("Creature Creator SDK/Body Part/Upload", priority = 100)]
    public static void UploadBodyPart()
    {
    }


    [MenuItem("Creature Creator SDK/Pattern/Create New", priority = 30)]
    public static void CreateNewPattern()
    {
    }

    [MenuItem("Creature Creator SDK/Pattern/Upload", priority = 100)]
    public static void UploadPattern()
    {
    }


    [MenuItem("Creature Creator SDK/Settings/Connect to Steam", priority = 100)]
    public static void ConnectToSteam()
    {
        EditorSteamManager.ConnectToSteam();
    }
}
