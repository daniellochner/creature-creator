public static class IronSourceDependenciesManagerConstants
{
    internal const string SDK = "sdk";
    internal const int WIDTH = 760;
    internal const int HEIGHT = 760;
    internal const string ANDROID = "Android";
    internal const string IOS = "iOS";
    internal const string IRONSOURCE = "ironsource";
    internal const string UNITYADS = "unityads";
    internal const string NONE = "none";

    //path const
    internal const string IRONSOURCE_SDK_INFO = "https://s3.amazonaws.com/ssa.public/Integration-Manager/IronSourceSDKInfo.json";
    internal const string IRONSOURCE_SDK_XML_LINKS = "https://s3.amazonaws.com/ssa.public/Integration-Manager/IronSourceSDKLinks.json";
    internal const string IRONSOURCE_DOWNLOAD_DIR = "Assets/IronSource/Editor/";
    internal const string IRONSOURCE_XML = "IronSourceSDKDependencies.xml";
    internal const string UNITYADS_XML = "ISUnityAdsAdapterDependencies.xml";
    internal const string IRONSOURCE_XML_PATH = "dependencies/unityversion";


    //xml macro
    internal const string UNITY_ADAPTER_MACRO = "${UnityAdapterVersion}";

    //jsonInfo keys
    internal const string LATEST_SDK_VERSION = "latest";
    internal const string PROVIDER_KEY_NAME = "keyname";
    internal const string PROVIDER_IS_NEW = "isNewProvider";
    internal const string PROVIDER_ANDROID_SDK_VER = "AndroidSDKVersion";
    internal const string PROVIDER_IOS_SDK_VER = "iOSSDKVersion";
    internal const string PROVIDER_UNITY_ADAPTER_VERSION = "UnityAdapterVersion";

    //jsonlinks keys
    internal const string PROVIDER_DOWNLOAD_URL = "DownloadUrl";
    internal const string PROVIDER_FILE_NAME = "FileName";

    //UI constants
    internal const string UPDATE_MSG = "UpdateMessage";
    internal const string LATEST_MSG = "LatestMessage";
    internal const string LABEL_INSTALL = "Install";
    internal const string LABEL_UPDATE = "Update";
    internal const string LABEL_UPDATED = "Updated";
    internal const string LABEL_Action = "Action";
    internal const string LABEL_NETWORK = "Network";
    internal const string LABEL_CURRENT_SDK = "Current Adapter Version";
    internal const string LABEL_LATEST_SDK = "Latest Adapter Version";

    internal const string TOOLTIP_ANDROID_SDK = "Android SDK version";
    internal const string TOOLTIP_IOS_SDK = "iOS SDK version";
    internal const string TOOLTIP_LATEST_VERSION = "Latest Version:";
    internal const string TOOLTIP_ADAPTER_VERSION = "Adapter Version";
    internal const string NEW_NETWORK = " - New Network";

    internal const string INTERGRATION_MANAGER_TITLE = "LevelPlay Integration Manager";
    internal const string ERROR_NOT_AVAILABLE = "SDK and adapters data are not available right now. Try again soon.";
}
