using System.IO;
using UnityEngine;

public class IronSourceMediatedNetworkSettings : ScriptableObject{
    public static readonly string MEDIATION_SETTINGS_ASSET_PATH = Path.Combine(IronSourceConstants.IRONSOURCE_RESOURCES_PATH, IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME + ".asset");

    [Header("")]
    [Header("AdMob Integration")]
    [SerializeField]
    [Tooltip("This will add AdMob Application ID to AndroidManifest.xml/info.plist")]
    public bool EnableAdmob = false;

    [SerializeField]
    [Tooltip("This Will be added to your AndroidManifest.xml")]
    public string AdmobAndroidAppId = string.Empty;

    [SerializeField]
    [Tooltip("This will be added to your info.plist")]
    public string AdmobIOSAppId = string.Empty;
}