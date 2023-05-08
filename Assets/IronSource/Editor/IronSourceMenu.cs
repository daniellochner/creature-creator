using System.IO;
using UnityEditor;
using UnityEngine;

public class IronSourceMenu : UnityEditor.Editor
{

   [MenuItem("Ads Mediation/Documentation", false, 0)]
    public static void Documentation()
    {
        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/unity-plugin/");
    }

   
    [MenuItem("Ads Mediation/SDK Change Log", false, 1)]
    public static void ChangeLog()
    {
        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/sdk-change-log/");
    }


    [MenuItem("Ads Mediation/Integration Manager", false , 2)]
    public static void SdkManagerProd()
    {
        IronSourceDependenciesManager.ShowISDependenciesManager();
    }

    [MenuItem("Ads Mediation/Developer Settings/LevelPlay Mediation Settings", false, 3)]
    public static void mediationSettings()
    {
        string path = "Assets/IronSource/Resources";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        var ironSourceMediationSettings = Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        if (ironSourceMediationSettings == null)
        {
            Debug.LogWarning(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME + " can't be found, creating a new one...");
            ironSourceMediationSettings = CreateInstance<IronSourceMediationSettings>();
            AssetDatabase.CreateAsset(ironSourceMediationSettings, IronSourceMediationSettings.IRONSOURCE_SETTINGS_ASSET_PATH);
            ironSourceMediationSettings = Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        }

        Selection.activeObject = ironSourceMediationSettings;
    }

    [MenuItem("Ads Mediation/Developer Settings/Mediated Network Settings", false, 4)]
    public static void mediatedNetworkSettings()
    {
        string path = IronSourceConstants.IRONSOURCE_RESOURCES_PATH;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ironSourceMediatedNetworkSettings = Resources.Load<IronSourceMediatedNetworkSettings>(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME);
        if (ironSourceMediatedNetworkSettings == null)
        {
            Debug.LogWarning(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME + " can't be found, creating a new one...");
            ironSourceMediatedNetworkSettings = CreateInstance<IronSourceMediatedNetworkSettings>();
            AssetDatabase.CreateAsset(ironSourceMediatedNetworkSettings, IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
            ironSourceMediatedNetworkSettings = Resources.Load<IronSourceMediatedNetworkSettings>(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME);
        }

        Selection.activeObject = ironSourceMediatedNetworkSettings;
    }
}