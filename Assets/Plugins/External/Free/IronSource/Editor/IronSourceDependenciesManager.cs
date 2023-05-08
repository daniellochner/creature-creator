using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using IronSourceJSON;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class IronSourceDependenciesManager : EditorWindow
{
    private readonly SortedSet<ProviderInfo> providersSet = new SortedSet<ProviderInfo>(new ProviderInfoComparor());
    private ProviderInfo ironSourceProviderInfo;
    private ProviderInfo unityAdsProviderInfo;
    private UnityWebRequest downloadWebClient;
    private string messageData;
    private IronSourceEditorCoroutines mEditorCoroutines;
    private static string latestUnitySDKVersion;

    private GUIStyle headerStyle;
    private GUIStyle textStyle;
    private GUIStyle boldTextStyle;
    private readonly GUILayoutOption buttonWidth = GUILayout.Width(90);

    public class ProviderInfo
    {
        public Status currentStatues;
        public string providerName;
        public string currentUnityVersion;
        public string latestUnityVersion;
        public string latestUnityAdsVersion;
        public string downloadURL;
        public string displayProviderName;
        public bool isNewProvider;
        public string fileName;
        public Dictionary<string, string> sdkVersionDic;

        public ProviderInfo()
        {
            isNewProvider = false;
            fileName = string.Empty;
            downloadURL = string.Empty;
            currentUnityVersion = IronSourceDependenciesManagerConstants.NONE;
            sdkVersionDic = new Dictionary<string, string>();
        }

        public enum Status
        {
            INSTALLED = 1,
            NONE = 2,
            UPDATED = 3
        }

        public bool SetProviderDataProperties(string provider, Dictionary<string, object> providerData, Dictionary<string, object> providerXML)
        {
            providerName = provider;
            object obj;

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_KEY_NAME, out obj))
            {
                displayProviderName = obj as string;
            }
            else { displayProviderName = providerName; }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_IS_NEW, out obj))
            {
                isNewProvider = bool.Parse(obj as string);
            }

            if (providerXML.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_DOWNLOAD_URL, out obj))
            {
                downloadURL = obj as string;
            }

            if (providerXML.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_FILE_NAME, out obj))
            {
                fileName = obj as string;
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_ANDROID_SDK_VER, out obj))
            {
                sdkVersionDic.Add(IronSourceDependenciesManagerConstants.ANDROID, obj as string);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_IOS_SDK_VER, out obj))
            {
                sdkVersionDic.Add(IronSourceDependenciesManagerConstants.IOS, obj as string);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_UNITY_ADAPTER_VERSION, out obj))
            {
                if ((providerName.ToLower() != IronSourceDependenciesManagerConstants.IRONSOURCE))
                {
                    latestUnityVersion = obj as string;

                }
                else
                {
                    latestUnityVersion = latestUnitySDKVersion;
                }

                downloadURL = downloadURL.Replace(IronSourceDependenciesManagerConstants.UNITY_ADAPTER_MACRO, latestUnityVersion);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_UNITY_ADAPTER_VERSION, out obj))
            {
                if ((providerName.ToLower() == IronSourceDependenciesManagerConstants.UNITYADS))
                {
                    latestUnityAdsVersion = obj as string;
                }
                

                downloadURL = downloadURL.Replace(IronSourceDependenciesManagerConstants.UNITY_ADAPTER_MACRO, latestUnityVersion);
            }

            currentUnityVersion = GetVersionFromXML(fileName);

            if (currentUnityVersion.Equals(IronSourceDependenciesManagerConstants.NONE))
            {
                currentStatues = Status.NONE;
            }

            else
            {
                if (isNewerVersion(currentUnityVersion, latestUnityVersion))
                {
                    currentStatues = Status.INSTALLED;
                }
                else
                {
                    currentStatues = Status.UPDATED;
                }
            }

            return true;
        }
    }

    private static string GetVersionFromXML(string fileName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        string version = IronSourceDependenciesManagerConstants.NONE;
        try
        {
            xmlDoc.LoadXml(File.ReadAllText(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR + fileName));
        }
        catch (Exception)
        {
            return version;
        }
        var unityVersion = xmlDoc.SelectSingleNode(IronSourceDependenciesManagerConstants.IRONSOURCE_XML_PATH);
        if (unityVersion != null)
        {
            return (unityVersion.InnerText);
        }
        File.Delete(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR + fileName);
        return version;
    }


    private IEnumerator SetProviderData()
    {
        UnityWebRequest unityWebRequestLinkJson = UnityWebRequest.Get(IronSourceDependenciesManagerConstants.IRONSOURCE_SDK_XML_LINKS);
        UnityWebRequest unityWebRequesInfoJson = UnityWebRequest.Get(IronSourceDependenciesManagerConstants.IRONSOURCE_SDK_INFO);
        var webRequestLinks = unityWebRequestLinkJson.SendWebRequest();
        var webRequestSDKInfo = unityWebRequesInfoJson.SendWebRequest();

        while (!webRequestLinks.isDone || !webRequestSDKInfo.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }

#if UNITY_2020_1_OR_NEWER
        if (unityWebRequestLinkJson.result != UnityWebRequest.Result.ProtocolError && unityWebRequesInfoJson.result != UnityWebRequest.Result.ProtocolError)
#else
        if (!unityWebRequestLinkJson.isHttpError && !unityWebRequestLinkJson.isNetworkError && !unityWebRequesInfoJson.isError && !unityWebRequesInfoJson.isHttpError)
#endif
        {
            string linksJson = unityWebRequestLinkJson.downloadHandler.text;
            string SDKInfoJson = unityWebRequesInfoJson.downloadHandler.text;
            providersSet.Clear();

            Dictionary<string, object> linksDictionary = new Dictionary<string, object>();
            Dictionary<string, object> SDKInfoDictionary = new Dictionary<string, object>();
            try
            {
                linksDictionary = Json.Deserialize(linksJson) as Dictionary<string, object>;
                SDKInfoDictionary = Json.Deserialize(SDKInfoJson) as Dictionary<string, object>;
            }

            catch (Exception e)
            {
                Debug.Log("Error getting response " + e.ToString());
            }

            if (SDKInfoDictionary != null && SDKInfoDictionary.Count != 0 && linksDictionary != null && linksDictionary.Count != 0)
            {
                string requiredVersion;
                object providersJson;

                ironSourceProviderInfo = new ProviderInfo();
                unityAdsProviderInfo = new ProviderInfo();
                ironSourceProviderInfo.currentUnityVersion = GetVersionFromXML(IronSourceDependenciesManagerConstants.IRONSOURCE_XML);
                
                SDKInfoDictionary.TryGetValue(IronSourceDependenciesManagerConstants.LATEST_SDK_VERSION, out providersJson);
                latestUnitySDKVersion = providersJson.ToString();
                ironSourceProviderInfo.latestUnityVersion = providersJson.ToString();
                

                requiredVersion = (ironSourceProviderInfo.currentUnityVersion == IronSourceDependenciesManagerConstants.NONE) ? ironSourceProviderInfo.latestUnityVersion : ironSourceProviderInfo.currentUnityVersion;

                if (SDKInfoDictionary.TryGetValue(requiredVersion, out providersJson))
                {
                    if (providersJson != null)
                    {
                        foreach (var item in providersJson as Dictionary<string, object>)
                        {
                            ProviderInfo info = new ProviderInfo();

                            object providerXML;
                            var lowerCaseItem = item.Key.ToLower();

                            linksDictionary.TryGetValue(lowerCaseItem, out providerXML);

                            if (info.SetProviderDataProperties(item.Key, item.Value as Dictionary<string, object>, providerXML as Dictionary<string, object>))
                            {
                                if (item.Key.ToLower().Contains(IronSourceDependenciesManagerConstants.IRONSOURCE))
                                {
                                    ironSourceProviderInfo.displayProviderName = info.displayProviderName;
                                    ironSourceProviderInfo.downloadURL = info.downloadURL;
                                    ironSourceProviderInfo.providerName = info.providerName;
                                    ironSourceProviderInfo.sdkVersionDic = info.sdkVersionDic;
                                    ironSourceProviderInfo.fileName = info.fileName;
                                    ironSourceProviderInfo.currentStatues = info.currentStatues;

                                }
                               
                                else
                                {
                                    if (item.Key.ToLower().Contains(IronSourceDependenciesManagerConstants.UNITYADS)) {
                                        if (item.Key.ToLower().Contains(IronSourceDependenciesManagerConstants.UNITYADS))
                                        {

                                            if (File.Exists(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR + IronSourceDependenciesManagerConstants.UNITYADS_XML))
                                            {
                                                unityAdsProviderInfo.currentUnityVersion = GetVersionFromXML(IronSourceDependenciesManagerConstants.UNITYADS_XML);
                                                unityAdsProviderInfo.latestUnityVersion = info.latestUnityAdsVersion;
                                            }
                                            else {
                                                unityAdsProviderInfo.currentUnityVersion = "none";
                                            }


                                            unityAdsProviderInfo.latestUnityVersion = info.latestUnityAdsVersion;
                                            unityAdsProviderInfo.displayProviderName = info.displayProviderName;
                                            unityAdsProviderInfo.downloadURL = info.downloadURL;
                                            unityAdsProviderInfo.providerName = info.providerName;
                                            unityAdsProviderInfo.sdkVersionDic = info.sdkVersionDic;
                                            unityAdsProviderInfo.fileName = info.fileName;
                                            unityAdsProviderInfo.currentStatues = info.currentStatues;

                                        }
                                    } else {
                                        providersSet.Add(info);
                                    }
                                    
                                }
                            }
                        }
                    }
                }

                if (ironSourceProviderInfo.currentStatues == ProviderInfo.Status.INSTALLED || ironSourceProviderInfo.currentStatues == ProviderInfo.Status.NONE)
                {
                    if (SDKInfoDictionary.TryGetValue(IronSourceDependenciesManagerConstants.UPDATE_MSG, out providersJson))
                    {
                        messageData = providersJson.ToString();
                    }
                }
                else
                {
                    if (SDKInfoDictionary.TryGetValue(IronSourceDependenciesManagerConstants.LATEST_MSG, out providersJson))
                    {
                        messageData = providersJson.ToString();
                    }
                }
            }
        }

        Repaint();
    }

    private void CancelDownload()
    {
        // if downloader object is still active
        if (downloadWebClient != null)
        {
            downloadWebClient.Abort();
            return;
        }

        if (mEditorCoroutines != null)
        {
            mEditorCoroutines.StopEditorCoroutine();
            mEditorCoroutines = null;
        }

        downloadWebClient = null;
    }

    public static void ShowISDependenciesManager()
    {
        var win = GetWindowWithRect<IronSourceDependenciesManager>(new Rect(0, 0, IronSourceDependenciesManagerConstants.WIDTH, IronSourceDependenciesManagerConstants.HEIGHT), true);
        win.titleContent = new GUIContent(IronSourceDependenciesManagerConstants.INTERGRATION_MANAGER_TITLE);
        win.Focus();
    }

    void Awake()
    {
        headerStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 14,
            fixedHeight = 20,
            stretchWidth = true,
            fixedWidth = IronSourceDependenciesManagerConstants.WIDTH / 4 + 5,
            clipping = TextClipping.Overflow,
            alignment = TextAnchor.MiddleLeft
        };
        textStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Normal,
            alignment = TextAnchor.MiddleLeft

        };
        boldTextStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold
        };
        CancelDownload();
    }

    private void OnEnable()
    {
        mEditorCoroutines = IronSourceEditorCoroutines.StartEditorCoroutine(SetProviderData());
    }

    private void OnDestroy()
    {
        CancelDownload();
        AssetDatabase.Refresh();
    }

    void DrawProviderItem(ProviderInfo providerData)
    {
        if (!providerData.Equals(default(ProviderInfo)))
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUI.enabled = true;
                bool isNew = providerData.isNewProvider;
                string isNewAddition = isNew ? IronSourceDependenciesManagerConstants.NEW_NETWORK : string.Empty;

                string tooltipText = IronSourceDependenciesManagerConstants.TOOLTIP_LATEST_VERSION + " \n " + providerData.providerName + " " + IronSourceDependenciesManagerConstants.TOOLTIP_ADAPTER_VERSION + " " + providerData.latestUnityVersion;
                if (providerData.sdkVersionDic.TryGetValue(IronSourceDependenciesManagerConstants.ANDROID, out string androidVersion))
                {
                    tooltipText = tooltipText + "\n " + IronSourceDependenciesManagerConstants.TOOLTIP_ANDROID_SDK + " " + androidVersion;
                }
                if (providerData.sdkVersionDic.TryGetValue(IronSourceDependenciesManagerConstants.IOS, out string iosVersion))
                {
                    tooltipText = tooltipText + "\n " + IronSourceDependenciesManagerConstants.TOOLTIP_IOS_SDK + " " + iosVersion;
                }

                EditorGUILayout.LabelField(providerData.displayProviderName + isNewAddition, isNew ? boldTextStyle : textStyle);
                EditorGUILayout.LabelField(providerData.currentUnityVersion, textStyle);
                EditorGUILayout.LabelField(providerData.latestUnityVersion, textStyle);

                string downloadButtonText;

                switch (providerData.currentStatues)
                {
                    case ProviderInfo.Status.NONE:
                        downloadButtonText = IronSourceDependenciesManagerConstants.LABEL_INSTALL;
                        break;
                    case ProviderInfo.Status.INSTALLED:
                        downloadButtonText = IronSourceDependenciesManagerConstants.LABEL_UPDATE;
                        break;

                    default:
                        downloadButtonText = IronSourceDependenciesManagerConstants.LABEL_UPDATED;
                        GUI.enabled = false;
                        break;
                }

                GUIContent gUIContent = new GUIContent
                {
                    text = downloadButtonText,
                    tooltip = tooltipText
                };

                bool btn = GUILayout.Button(gUIContent, buttonWidth);
                if (btn && downloadWebClient == null)
                {
                    GUI.enabled = true;
                    IronSourceEditorCoroutines.StartEditorCoroutine(DownloadFile(providerData.downloadURL));
                }

                GUILayout.Space(5);
                GUI.enabled = true;
            }
        }
    }

    void OnGUI()
    {
        if (ironSourceProviderInfo == null)
        {
            GUILayout.Label(IronSourceDependenciesManagerConstants.ERROR_NOT_AVAILABLE);
            return;
        }

        GUILayout.Space(10);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            DrawSDKHeader();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            DrawProviderItem(ironSourceProviderInfo);
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            DrawProviderItem(unityAdsProviderInfo);
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        GUILayout.Space(15);
        DrawAdaptersHeader();
        GUILayout.Space(15);

        foreach (var provider in providersSet)
        {
            DrawProviderItem(provider);
            GUILayout.Space(2);
        }
        GUILayout.Space(30);
        if (!string.IsNullOrEmpty(messageData))
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.ExpandHeight(true)))
            {
                GUILayout.Space(5);
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
                {
                    EditorGUILayout.SelectableLabel(messageData, EditorStyles.textField, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                }
                GUILayout.Space(5);
            }
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(false)))
            {
                GUILayout.Space(15);
            }
        }

    }

    private void DrawSDKHeader()
    {
        using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
        {
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_CURRENT_SDK, new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                fixedHeight = 20,
                stretchWidth = true,
                fixedWidth = IronSourceDependenciesManagerConstants.WIDTH / 4,
                clipping = TextClipping.Overflow,
                padding = new RectOffset(IronSourceDependenciesManagerConstants.WIDTH / 4 + 15, 0, 0, 0)
            });
            GUILayout.Space(85);
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_LATEST_SDK, new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 13,
                fixedHeight = 20,
                stretchWidth = true,
                fixedWidth = Screen.width / 4,
                clipping = TextClipping.Overflow,
            });
        }
    }

    private void DrawAdaptersHeader()
    {
        using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
        {
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_NETWORK, headerStyle);
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_CURRENT_SDK, headerStyle);
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_LATEST_SDK, headerStyle);
            GUILayout.Space(30);
            EditorGUILayout.LabelField(IronSourceDependenciesManagerConstants.LABEL_Action, headerStyle);
        }
    }

    private IEnumerator DownloadFile(string downloadFileUrl)
    {
        int fileNameIndex = downloadFileUrl.LastIndexOf("/") + 1;
        string downloadFileName = downloadFileUrl.Substring(fileNameIndex);
        string fileDownloading = string.Format("Downloading {0}", downloadFileName);
        string genericFileName = Regex.Replace(downloadFileName, @"_v+(\d\.\d\.\d\.\d|\d\.\d\.\d)", "");
        string path = Path.Combine(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR, genericFileName);
        bool isCancelled = false;
        downloadWebClient = new UnityWebRequest(downloadFileUrl);
        downloadWebClient.downloadHandler = new DownloadHandlerFile(path);
        downloadWebClient.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (downloadWebClient.result != UnityWebRequest.Result.ProtocolError)
#else
        if (!downloadWebClient.isHttpError && !downloadWebClient.isNetworkError)
#endif
        {
            while (!downloadWebClient.isDone)
            {
                yield return new WaitForSeconds(0.1f);
                if (EditorUtility.DisplayCancelableProgressBar("Download Manager", fileDownloading, downloadWebClient.downloadProgress))
                {
                    if (downloadWebClient.error != null)
                    {
                        Debug.LogError(downloadWebClient.error);
                    }
                    CancelDownload();
                    isCancelled = true;
                }
            }
        }
        else
        {
            Debug.LogError("Error Downloading " + genericFileName + " : " + downloadWebClient.error);
        }

        EditorUtility.ClearProgressBar();

        if (genericFileName.EndsWith(".unitypackage") && !isCancelled)
        {
            AssetDatabase.ImportPackage(Path.Combine(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR, genericFileName), true);
        }
        else
        {
            // in case the download was cancelled, delete the file
            if (isCancelled && File.Exists(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR + genericFileName))
            {
                File.Delete(IronSourceDependenciesManagerConstants.IRONSOURCE_DOWNLOAD_DIR + genericFileName);
            }

            IronSourceEditorCoroutines.StartEditorCoroutine(SetProviderData());
        }

        //clean the downloadWebClient object regardless of whether the request succeeded or failed 
        downloadWebClient.Dispose();
        downloadWebClient = null;

        IronSourceEditorCoroutines.StartEditorCoroutine(SetProviderData());
    }

    private static bool isNewerVersion(string current, string latest)
    {
        bool isNewer = false;
        try
        {
            int[] currentVersion = Array.ConvertAll(current.Split('.'), int.Parse);
            int[] remoteVersion = Array.ConvertAll(latest.Split('.'), int.Parse);
            int remoteBuild = 0;
            int curBuild = 0;
            if (currentVersion.Length > 3)
            {
                curBuild = currentVersion[3];
            }
            if (remoteVersion.Length > 3)
            {
                remoteBuild = remoteVersion[3];

            }
            System.Version cur = new System.Version(currentVersion[0], currentVersion[1], currentVersion[2], curBuild);
            System.Version remote = new System.Version(remoteVersion[0], remoteVersion[1], remoteVersion[2], remoteBuild);
            isNewer = cur < remote;
        }
        catch (Exception)
        {

        }
        return isNewer;

    }

    internal class ProviderInfoComparor : IComparer<ProviderInfo>
    {
        public int Compare(ProviderInfo x, ProviderInfo y)
        {
            return x.providerName.CompareTo(y.providerName);
        }
    }
}
