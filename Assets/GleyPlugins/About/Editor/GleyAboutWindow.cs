namespace Gley.About
{
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public class GleyAboutWindow : EditorWindow
    {
        struct ContactButton
        {
            public GUIContent guiContent;
            public string url;

            public ContactButton(GUIContent guiContent, string url)
            {
                this.guiContent = guiContent;
                this.url = url;
            }
        }

        private const string FOLDER_NAME = "About";
        private static string PARENT_FOLDER = "GleyPlugins";
        private static string rootFolder;

        static GleyAboutWindow window;
        static IconReferences iconReferences;

        private ContactButton[] contactButtons;
        private EditorFileLoaded fileLoader;
        private AssetVersions allAssetsVersion;

        private Vector2 scrollPosition;
        private string updateResult;
        private int nrOfUpdates;
        private bool updateCheck;
        private bool mobileToolsAvailable;

        static AssetStorePackage[] assetStorePackages;

        string[] packagesInsideMobileTools = {
            "Ads",
            "EasyIAP",
            "Notifications",
            "GameServices",
            "RateGame",
            "CrossPromo",
            "AllPlatformsSave",
            "Localization",
            "DailyRewards"
        };

        [MenuItem("Window/Gley/About Gley", false, 0)]
        private static void Init()
        {
            if (!LoadRootFolder())
                return;

            LoadIcons();

            LoadAssetStorePackages();

            window = (GleyAboutWindow)GetWindow(typeof(GleyAboutWindow));
            window.minSize = new Vector2(600, 520);
            window.titleContent = new GUIContent(" About v2.0", iconReferences.gleyLogo);
            window.Show();
        }

        static void LoadAssetStorePackages()
        {
            assetStorePackages = new AssetStorePackage[]
            {
                new AssetStorePackage("TrafficSystem", "Traffic System", iconReferences.trafficSystemIcon, "Highly performant and easy to use traffic system that can make any driving game more fun to play in just a few clicks.", "https://assetstore.unity.com/packages/slug/194888?aid=1011l8QY4"),
                new AssetStorePackage("JumpyCompleteGame", "Mobile Tools", iconReferences.mobileToolsIcon, "All you need to publish your finished game on the store and BONUS a free game with all of them already integrated", "https://assetstore.unity.com/packages/tools/integration/mobile-tools-complete-game-132284?aid=1011l8QY4"),
                new AssetStorePackage("Ads", "Mobile Ads", iconReferences.mobileAdsIcon, "Show ads inside your game with this easy to use, multiple advertisers support tool.", "https://assetstore.unity.com/packages/tools/integration/mobile-ads-gdpr-compliant-102892?aid=1011l8QY4"),
                new AssetStorePackage("EasyIAP", "Easy IAP", iconReferences.easyIAPIcon, "Sell In App products inside your game with minimal setup and very little programming knowledge.", "https://assetstore.unity.com/packages/tools/integration/easy-iap-in-app-purchase-128902?aid=1011l8QY4"),
                new AssetStorePackage("Localization", "Localization (Multi-Language)", iconReferences.localizationIcon, "Make your app international and reach a greater audience by translating your app in multiple languages.", "https://assetstore.unity.com/packages/tools/integration/localization-multi-language-161885?aid=1011l8QY4"),
                new AssetStorePackage("DailyRewards", "Daily (Time Based) Rewards", iconReferences.dailyRewardsIcon, "Increase the retention of your game by using Daily Rewards and Time Based rewards.", "https://assetstore.unity.com/packages/tools/integration/daily-time-based-rewards-161112?aid=1011l8QY4"),
                new AssetStorePackage("Notifications", "Mobile Push Notifications",iconReferences.notificationsIcon, "Send scheduled offline notifications to your users and keep them engaged.", "https://assetstore.unity.com/packages/tools/integration/mobile-push-notifications-156905?aid=1011l8QY4"),
                new AssetStorePackage("GameServices", "Easy Achievements and Leaderboards", iconReferences.achievementsIcon, "Submit achievements and scores with minimal setup and increase competition between your users.", "https://assetstore.unity.com/packages/tools/integration/easy-achievements-and-leaderboards-118119?aid=1011l8QY4"),
                new AssetStorePackage("RateGame", "Rate Game Popup", iconReferences.rateGameIcon, "Increase the number of game ratings by encouraging users to rate your game.", "https://assetstore.unity.com/packages/tools/integration/rate-game-popup-android-ios-139131?aid=1011l8QY4"),
                new AssetStorePackage("CrossPromo", "Mobile Cross Promo", iconReferences.crossPromoIcon, "Easily cross promote and increase popularity for all of your published games.", "https://assetstore.unity.com/packages/tools/integration/mobile-cross-promo-148024?aid=1011l8QY4"),
                new AssetStorePackage("AllPlatformsSave", "All Platforms Save", iconReferences.saveIcon, "Easy to use: same line of code to save or load game data on all supported Unity platforms.", "https://assetstore.unity.com/packages/tools/integration/all-platforms-save-115960?aid=1011l8QY4"),
                new AssetStorePackage("DeliveryVehiclesPack", "Delivery Vehicles Pack", iconReferences.vehiclesIcon, "Delivery Vehicles Pack contains 3 low poly, textured vehicles: Scooter, Three Wheeler, Minivan", "https://assetstore.unity.com/packages/3d/vehicles/land/delivery-vehicles-pack-55528?aid=1011l8QY4")
            };
        }

        static void LoadIcons()
        {
            Object assetToLoad = AssetDatabase.LoadAssetAtPath($"{rootFolder}/Editor/IconReferences.asset", typeof(IconReferences));
            iconReferences = (IconReferences)assetToLoad;
        }


        static bool LoadRootFolder()
        {
            rootFolder = Common.EditorUtilities.FindFolder(FOLDER_NAME, PARENT_FOLDER);
            if (rootFolder == null)
            {
                Debug.LogError($"Folder Not Found: '{PARENT_FOLDER}/{FOLDER_NAME}'");
                PARENT_FOLDER = "Assets";

                rootFolder = Common.EditorUtilities.FindFolder(FOLDER_NAME, PARENT_FOLDER);
                if (rootFolder == null)
                {
                    Debug.LogError($"Folder Not Found: '{PARENT_FOLDER}/{FOLDER_NAME}'");
                    return false;
                }
            }
            return true;
        }

        void OnEnable()
        {
            if (!LoadRootFolder())
                return;

            if (iconReferences == null)
            {
                LoadIcons();
            }

            if (assetStorePackages == null)
            {
                LoadAssetStorePackages();
            }
            contactButtons = new ContactButton[]
            {
                new ContactButton(new GUIContent(" Website", iconReferences.websiteIcon),"https://gleygames.com"),
                new ContactButton(new GUIContent(" Youtube", iconReferences.youtubeIcon),"https://www.youtube.com/c/gleygames"),
                new ContactButton(new GUIContent(" Discord", iconReferences.discordIcon),"https://discord.gg/7eSvKKW"),
                new ContactButton(new GUIContent(" Twitter", iconReferences.twitterIcon),"https://twitter.com/GleyGames"),
                new ContactButton(new GUIContent(" Facebook", iconReferences.facebookIcon),"https://www.youtube.com/c/gleygames"),
                new ContactButton(new GUIContent(" Instagram", iconReferences.instagramIcon),"https://www.instagram.com/gleygames/")
            };

            RefreshState();

            mobileToolsAvailable = (assetStorePackages[1].assetState != AssetState.NotDownloaded);
        }

        void RefreshState()
        {
            nrOfUpdates = 0;
            for (int i = 0; i < assetStorePackages.Length; i++)
            {
                assetStorePackages[i].assetState = GetAssetState(assetStorePackages[i].folderName);
            }

            if (nrOfUpdates != 0)
            {
                updateResult = nrOfUpdates + " updates available";
            }
            else
            {
                updateResult = "No updates available";
            }
        }

        void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.UpperCenter;
            EditorGUILayout.Space();
            GUILayout.Label(iconReferences.gleyCover, labelStyle);
            labelStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Professional assets made easy to use for everyone", labelStyle);
            EditorGUILayout.Space();

            GUILayout.Label("Connect with us:", labelStyle);
            EditorGUILayout.SelectableLabel("gley.assets@gmail.com", labelStyle);

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < contactButtons.Length; i++)
            {
                if (GUILayout.Button(contactButtons[i].guiContent))
                {
                    Application.OpenURL(contactButtons[i].url);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Open Asset Store Publisher Page"))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/19336");
            }
            EditorGUILayout.Space();

            if (updateCheck == false)
            {
                if (GUILayout.Button("Check for Updates"))
                {
                    updateCheck = true;
                    LoadFile();
                }
            }
            else
            {
                GUILayout.Label(updateResult, labelStyle);
            }
            EditorGUILayout.Space();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height - 250));

            DrawPackages();

            GUILayout.EndScrollView();
        }

        void DrawPackages()
        {
            for (int i = 0; i < assetStorePackages.Length; i++)
            {
                if (mobileToolsAvailable)
                {
                    if (!packagesInsideMobileTools.Contains(assetStorePackages[i].folderName))
                    {
                        DrawPack(assetStorePackages[i]);
                    }
                }
                else
                {
                    DrawPack(assetStorePackages[i]);
                }
            }
        }

        AssetState GetAssetState(string folderName)
        {
            AssetState result = AssetState.InProject;

            string path = Common.EditorUtilities.FindFolder("Scripts", folderName);
            if (path == null)
            {
                return AssetState.NotDownloaded;
            }
            if (!File.Exists($"{Application.dataPath}/{path.Replace("Assets/", "")}/Version.txt"))
            {
                nrOfUpdates++;
                return AssetState.UpdateAvailable;
            }

            if (allAssetsVersion != null)
            {
                if (AssetNeedsUpdate(path,folderName))
                {
                    nrOfUpdates++;
                    return AssetState.UpdateAvailable;
                }
            }

            return result;
        }

        private bool AssetNeedsUpdate(string path, string folderName)
        {
            if (allAssetsVersion.assetsVersion.Count == 0)
                return false;
            string filePath = $"{path}/Version.txt";
            StreamReader reader = new StreamReader(filePath);
            int localVersion = JsonUtility.FromJson<Common.AssetVersion>(reader.ReadToEnd()).shortVersion;

            int serverVersion = allAssetsVersion.assetsVersion.First(cond => cond.folderName == folderName).shortVersion;
            reader.Close();
            if (localVersion < serverVersion)
            {
                return true;
            }
            return false;
        }

        void DrawPack(AssetStorePackage pack)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.Space();
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 18;
            style.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(pack.texture, style);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            GUILayout.Label(pack.name, style);
            style.fontSize = 12;
            style.wordWrap = true;
            //style.normal.background = downloadColor;
            GUILayout.Label(pack.description, style);
            EditorGUILayout.EndVertical();
            var oldColor = GUI.backgroundColor;
            string buttonText = "";
            switch (pack.assetState)
            {
                case AssetState.ComingSoon:
                    GUI.backgroundColor = new Color32(190, 190, 190, 255);
                    buttonText = "Coming Soon";
                    break;
                case AssetState.InProject:
                    GUI.backgroundColor = new Color32(253, 195, 71, 255);
                    buttonText = "Owned";
                    break;
                case AssetState.NotDownloaded:
                    GUI.backgroundColor = new Color32(42, 180, 240, 255);
                    buttonText = "Download";
                    break;
                case AssetState.UpdateAvailable:
                    GUI.backgroundColor = new Color32(76, 229, 89, 255);
                    buttonText = "Update Available";
                    break;
            }

            if (GUILayout.Button(buttonText, GUILayout.Width(130), GUILayout.Height(64)))
            {
                updateCheck = false;
                Application.OpenURL(pack.url);
            }

            GUI.backgroundColor = oldColor;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void LoadFile()
        {
            updateResult = "Connecting to server";
            fileLoader = new EditorFileLoaded();
            string url = "https://gleygames.com/versions/AssetVersions-2.txt";
            fileLoader.LoadFile(url);
            EditorApplication.update = MyUpdate;
        }

        private void MyUpdate()
        {
            if (fileLoader.IsDone())
            {
                EditorApplication.update = null;
                LoadCompleted();
            }
        }

        private void LoadCompleted()
        {
            allAssetsVersion = JsonUtility.FromJson<AssetVersions>(fileLoader.GetResult());
            RefreshState();
        }
    }
}
