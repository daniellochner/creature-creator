namespace GleyGameServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class GameServicesSettingsWindow : EditorWindow
    {
        private const string FOLDER_NAME = "GameServices";
        private const string PARENT_FOLDER = "GleyPlugins";
        private static string rootFolder;
        private static string rootWithoutAssets;

        private GUIStyle labelStyle;
        private GameServicesSettings gameServicesSettings;
        private List<Achievement> localAchievements;
        private List<Leaderboard> localLeaderboards;
        private Vector2 scrollPosition = Vector2.zero;
        private string googleAppId;
        private string errorText = "";
        private bool useForAndroid;
        private bool useForIos;
        private bool usePlaymaker;
        private bool useBolt;
        private bool useGameFlow;

        [MenuItem("Window/Gley/Game Services", false, 40)]
        private static void Init()
        {
            if (!LoadRootFolder())
                return;

            string path = $"{rootFolder}/Scripts/Version.txt";

            StreamReader reader = new StreamReader(path);
            string longVersion = JsonUtility.FromJson<Gley.Common.AssetVersion>(reader.ReadToEnd()).longVersion;

            // Get existing open window or if none, make a new one:
            GameServicesSettingsWindow window = (GameServicesSettingsWindow)GetWindow(typeof(GameServicesSettingsWindow));
            window.titleContent = new GUIContent("Game Services - v." + longVersion);
            window.minSize = new Vector2(520, 520);
            window.Show();
        }

        static bool LoadRootFolder()
        {
            rootFolder = Gley.Common.EditorUtilities.FindFolder(FOLDER_NAME, PARENT_FOLDER);
            if (rootFolder == null)
            {
                Debug.LogError($"Folder Not Found:'{PARENT_FOLDER}/{FOLDER_NAME}'");
                return false;
            }
            rootWithoutAssets = rootFolder.Substring(7, rootFolder.Length - 7);
            return true;
        }

        private void OnEnable()
        {
            if (!LoadRootFolder())
                return;

            try
            {
                labelStyle = new GUIStyle(EditorStyles.label);
            }
            catch { }
            //load Game Serviced data
            gameServicesSettings = Resources.Load<GameServicesSettings>("GameServicesData");
            if (gameServicesSettings == null)
            {
                CreateAdSettings();
                gameServicesSettings = Resources.Load<GameServicesSettings>("GameServicesData");
            }

            useForAndroid = gameServicesSettings.useForAndroid;
            useForIos = gameServicesSettings.useForIos;
            googleAppId = gameServicesSettings.googleAppId;
            usePlaymaker = gameServicesSettings.usePlaymaker;
            useGameFlow = gameServicesSettings.useGameFlow;
            useBolt = gameServicesSettings.useBolt;

            localAchievements = new List<Achievement>();
            for (int i = 0; i < gameServicesSettings.allGameAchievements.Count; i++)
            {
                localAchievements.Add(gameServicesSettings.allGameAchievements[i]);
            }

            localLeaderboards = new List<Leaderboard>();
            for (int i = 0; i < gameServicesSettings.allGameLeaderboards.Count; i++)
            {
                localLeaderboards.Add(gameServicesSettings.allGameLeaderboards[i]);
            }
        }

        /// <summary>
        /// Saves the Settings Window data
        /// </summary>
        private void SaveSettings()
        {
            //setup preprocessor directives based on settings
            if (useForAndroid)
            {
                AddPreprocessorDirective("UseGooglePlayGamesPlugin", false, BuildTargetGroup.Android);
            }
            else
            {
                AddPreprocessorDirective("UseGooglePlayGamesPlugin", true, BuildTargetGroup.Android);
            }
            if (useForIos)
            {
                AddPreprocessorDirective("UseGameCenterPlugin", false, BuildTargetGroup.iOS);
            }
            else
            {
                AddPreprocessorDirective("UseGameCenterPlugin", true, BuildTargetGroup.iOS);
            }

            if (usePlaymaker)
            {
                AddPreprocessorDirective("USE_PLAYMAKER_SUPPORT", false, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_PLAYMAKER_SUPPORT", false, BuildTargetGroup.iOS);
            }
            else
            {
                AddPreprocessorDirective("USE_PLAYMAKER_SUPPORT", true, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_PLAYMAKER_SUPPORT", true, BuildTargetGroup.iOS);
            }

            if (useBolt)
            {
                AddPreprocessorDirective("USE_BOLT_SUPPORT", false, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_BOLT_SUPPORT", false, BuildTargetGroup.iOS);
            }
            else
            {
                AddPreprocessorDirective("USE_BOLT_SUPPORT", true, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_BOLT_SUPPORT", true, BuildTargetGroup.iOS);
            }

            if (useGameFlow)
            {
                AddPreprocessorDirective("USE_GAMEFLOW_SUPPORT", false, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_GAMEFLOW_SUPPORT", false, BuildTargetGroup.iOS);
            }
            else
            {
                AddPreprocessorDirective("USE_GAMEFLOW_SUPPORT", true, BuildTargetGroup.Android);
                AddPreprocessorDirective("USE_GAMEFLOW_SUPPORT", true, BuildTargetGroup.iOS);
            }

            //save id`s
            gameServicesSettings.googleAppId = googleAppId;
            gameServicesSettings.useForAndroid = useForAndroid;
            gameServicesSettings.useForIos = useForIos;
            gameServicesSettings.usePlaymaker = usePlaymaker;
            gameServicesSettings.useGameFlow = useGameFlow;
            gameServicesSettings.useBolt = useBolt;

            gameServicesSettings.allGameAchievements = new List<Achievement>();
            for (int i = 0; i < localAchievements.Count; i++)
            {
                gameServicesSettings.allGameAchievements.Add(localAchievements[i]);
            }

            gameServicesSettings.allGameLeaderboards = new List<Leaderboard>();
            for (int i = 0; i < localLeaderboards.Count; i++)
            {
                gameServicesSettings.allGameLeaderboards.Add(localLeaderboards[i]);
            }

            CreateManifestFile();
            CreateEnumFiles();
            EditorUtility.SetDirty(gameServicesSettings);
        }

        /// <summary>
        /// Display Settings Window
        /// </summary>
        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));
            GUILayout.Label("Select your platforms:", EditorStyles.boldLabel);
            useForAndroid = EditorGUILayout.Toggle("Android", useForAndroid);
            useForIos = EditorGUILayout.Toggle("iOS", useForIos);
            EditorGUILayout.Space();

            GUILayout.Label("Enable visual scripting tool support:", EditorStyles.boldLabel);
            usePlaymaker = EditorGUILayout.Toggle("Playmaker Support", usePlaymaker);
            useGameFlow = EditorGUILayout.Toggle("Game Flow Support", useGameFlow);
            useBolt = EditorGUILayout.Toggle("Bolt Support", useBolt);
            EditorGUILayout.Space();
            //Google play setup
            if (useForAndroid)
            {
                GUILayout.Label("Google Play Services Settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                if (GUILayout.Button("Download Google Play Games SDK"))
                {
                    Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
                }
                GUILayout.Label("You just need to import the SDK, no additional setup is required");
                EditorGUILayout.Space();

                googleAppId = EditorGUILayout.TextField("Google Play App ID", googleAppId);
                EditorGUILayout.Space();
            }


            if (useForAndroid || useForIos)
            {
                //achievement setup
                GUILayout.Label("Achievements Settings", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Achievement Name");
                if (useForAndroid)
                {
                    GUILayout.Label("Google Play ID");
                }
                if (useForIos)
                {
                    GUILayout.Label("Game Center ID");
                }
                GUILayout.Label("");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                for (int i = 0; i < localAchievements.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    localAchievements[i].name = EditorGUILayout.TextField(localAchievements[i].name);
                    localAchievements[i].name = Regex.Replace(localAchievements[i].name, @"^[\d-]*\s*", "");
                    localAchievements[i].name = localAchievements[i].name.Replace(" ", "");
                    localAchievements[i].name = localAchievements[i].name.Trim();
                    if (useForAndroid)
                    {
                        localAchievements[i].idGoogle = EditorGUILayout.TextField(localAchievements[i].idGoogle);
                    }
                    if (useForIos)
                    {
                        localAchievements[i].idIos = EditorGUILayout.TextField(localAchievements[i].idIos);
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        localAchievements.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                EditorGUILayout.Space();

                if (GUILayout.Button("Add new achievement"))
                {
                    localAchievements.Add(new Achievement());
                }
                EditorGUILayout.Space();

                //leaderboard setup
                GUILayout.Label("Leaderboards Settings", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Leaderboard Name");
                if (useForAndroid)
                {
                    GUILayout.Label("Google Play ID");
                }
                if (useForIos)
                {
                    GUILayout.Label("Game Center ID");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                for (int i = 0; i < localLeaderboards.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    localLeaderboards[i].name = EditorGUILayout.TextField(localLeaderboards[i].name);
                    localLeaderboards[i].name = Regex.Replace(localLeaderboards[i].name, @"^[\d-]*\s*", "");
                    localLeaderboards[i].name = localLeaderboards[i].name.Replace(" ", "");
                    localLeaderboards[i].name = localLeaderboards[i].name.Trim();
                    if (useForAndroid)
                    {
                        localLeaderboards[i].idGoogle = EditorGUILayout.TextField(localLeaderboards[i].idGoogle);
                    }
                    if (useForIos)
                    {
                        localLeaderboards[i].idIos = EditorGUILayout.TextField(localLeaderboards[i].idIos);
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        localLeaderboards.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                EditorGUILayout.Space();

                if (GUILayout.Button("Add new leaderboard"))
                {
                    localLeaderboards.Add(new Leaderboard());
                }
            }
            EditorGUILayout.Space();

            //save
            GUILayout.Label(errorText, labelStyle);
            if (GUILayout.Button("Save"))
            {
                if (CheckForNull() == false)
                {
                    SaveSettings();
                    labelStyle.normal.textColor = Color.black;
                    errorText = "Save Success";
                }
            }
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// Checks if any ID is null or duplicate
        /// </summary>
        /// <returns></returns>
        private bool CheckForNull()
        {
            for (int i = 0; i < localAchievements.Count - 1; i++)
            {
                for (int j = i + 1; j < localAchievements.Count; j++)
                {
                    if (localAchievements[i].name == localAchievements[j].name)
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = localAchievements[i].name + " Already exists. No duplicates allowed";
                        return true;
                    }
                }
            }

            for (int i = 0; i < localLeaderboards.Count - 1; i++)
            {
                for (int j = i + 1; j < localLeaderboards.Count; j++)
                {
                    if (localLeaderboards[i].name == localLeaderboards[j].name)
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = localLeaderboards[i].name + " Already exists. No duplicates allowed";
                        return true;
                    }
                }
            }

            for (int i = 0; i < localAchievements.Count; i++)
            {
                if (String.IsNullOrEmpty(localAchievements[i].name))
                {
                    labelStyle.normal.textColor = Color.red;
                    errorText = "Achievement name cannot be empty! Please fill all of them";
                    return true;
                }
                if (useForAndroid)
                {
                    if (String.IsNullOrEmpty(localAchievements[i].idGoogle))
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = "Google Play ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }
                if (useForIos)
                {
                    if (String.IsNullOrEmpty(localAchievements[i].idIos))
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = "Game Center ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }
            }

            for (int i = 0; i < localLeaderboards.Count; i++)
            {
                if (String.IsNullOrEmpty(localLeaderboards[i].name))
                {
                    labelStyle.normal.textColor = Color.red;
                    errorText = "Leaderboard name cannot be empty! Please fill all of them";
                    return true;
                }
                if (useForAndroid)
                {
                    if (String.IsNullOrEmpty(localLeaderboards[i].idGoogle))
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = "Leaderboard`s Google Play ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }
                if (useForIos)
                {
                    if (String.IsNullOrEmpty(localLeaderboards[i].idIos))
                    {
                        labelStyle.normal.textColor = Color.red;
                        errorText = "Leaderboard`s Game Center ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Create Settings asset to store user Settings Window setup
        /// </summary>
        public static void CreateAdSettings()
        {
            GameServicesSettings asset = ScriptableObject.CreateInstance<GameServicesSettings>();
            Gley.Common.EditorUtilities.CreateFolder($"{rootFolder}/Resources");

            AssetDatabase.CreateAsset(asset, $"{rootFolder}/Resources/GameServicesData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Add scripting define symbols
        /// </summary>
        /// <param name="directive">symbol name</param>
        /// <param name="remove">if true -> remove directive</param>
        /// <param name="target">target platform</param>
        private void AddPreprocessorDirective(string directive, bool remove, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            if (remove)
            {
                if (textToWrite.Contains(directive))
                {
                    Debug.Log(textToWrite);
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


        /// <summary>
        /// Auto-generate Google Play manifest to replace the one generated by Google
        /// </summary>
        private void CreateManifestFile()
        {
            Gley.Common.EditorUtilities.CreateFolder($"{rootFolder}/Plugins/Android/GameServicesManifest.plugin");


            string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                "<manifest xmlns:android = \"http://schemas.android.com/apk/res/android\"\n" +
                "package=\"com.google.example.games.mainlibproj\">\n" +
                "<application>\n" +
                "<meta-data android:name=\"com.google.android.gms.games.APP_ID\" android:value = \"\\" + googleAppId + "\" />\n" +
                "<activity android:name=\"com.google.games.bridge.NativeBridgeActivity\" android:theme = \"@android:style/Theme.Translucent.NoTitleBar.Fullscreen\" />\n" +
                "</application>\n" +
                "</manifest>";

            File.WriteAllText($"{Application.dataPath} /{rootWithoutAssets}/Plugins/Android/GameServicesManifest.plugin/AndroidManifest.xml", text);

            text = "target=android-16\nandroid.library = true";
            File.WriteAllText($"{Application.dataPath}/{rootWithoutAssets}/Plugins/Android/GameServicesManifest.plugin/project.properties", text);
            AssetDatabase.Refresh();

            // disable Google plugin
            if (useForAndroid)
            {
                ((PluginImporter)PluginImporter.GetAtPath($"{rootFolder}/Plugins/Android/GameServicesManifest.plugin")).SetCompatibleWithAnyPlatform(false);
                ((PluginImporter)PluginImporter.GetAtPath($"{rootFolder}/Plugins/Android/GameServicesManifest.plugin")).SetCompatibleWithPlatform(BuildTarget.Android, true);
                //((PluginImporter)PluginImporter.GetAtPath("Assets/GooglePlayGames/Plugins/Android/GooglePlayGamesManifest.androidlib")).SetCompatibleWithAnyPlatform(false);
                //((PluginImporter)PluginImporter.GetAtPath("Assets/GooglePlayGames/Plugins/Android/GooglePlayGamesManifest.androidlib")).SetCompatibleWithPlatform(BuildTarget.Android, false);
            }
            else
            {
                ((PluginImporter)PluginImporter.GetAtPath($"{rootFolder}/Plugins/Android/GameServicesManifest.plugin")).SetCompatibleWithAnyPlatform(false);
                ((PluginImporter)PluginImporter.GetAtPath($"{rootFolder}/Plugins/Android/GameServicesManifest.plugin")).SetCompatibleWithPlatform(BuildTarget.Android, false);
            }
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// Automatically generates enums based on names added in Settings Window
        /// </summary>
        private void CreateEnumFiles()
        {
            string text =
            "public enum AchievementNames\n" +
            "{\n";
            for (int i = 0; i < localAchievements.Count; i++)
            {
                text += localAchievements[i].name + ",\n";
            }
            text += "}";
            File.WriteAllText($"{Application.dataPath}/{rootWithoutAssets}/Scripts/AchievementNames.cs", text);

            text =
            "public enum LeaderboardNames\n" +
            "{\n";
            for (int i = 0; i < localLeaderboards.Count; i++)
            {
                text += localLeaderboards[i].name + ",\n";
            }
            text += "}";
            File.WriteAllText($"{Application.dataPath}/{rootWithoutAssets}/Scripts/LeaderboardNames.cs", text);

            AssetDatabase.Refresh();
        }
    }
}
