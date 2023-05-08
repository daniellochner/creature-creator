using System.IO;
using UnityEditor;

[CustomEditor(typeof(IronSourceMediatedNetworkSettings))]
public class IronSourceMediatedNetworkSettingsInspector : UnityEditor.Editor
{
    private static IronSourceMediatedNetworkSettings ironSourceMediatedNetworkSettings;

    public static IronSourceMediatedNetworkSettings IronSourceMediatedNetworkSettings
    {
        get
        {
            if (ironSourceMediatedNetworkSettings == null)
            {
                ironSourceMediatedNetworkSettings = AssetDatabase.LoadAssetAtPath<IronSourceMediatedNetworkSettings>(IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
                if (ironSourceMediatedNetworkSettings == null)
                {
                    IronSourceMediatedNetworkSettings asset = CreateInstance<IronSourceMediatedNetworkSettings>();
                    Directory.CreateDirectory(IronSourceConstants.IRONSOURCE_RESOURCES_PATH);
                    AssetDatabase.CreateAsset(asset, IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
                    ironSourceMediatedNetworkSettings = asset;
                }
            }
            return ironSourceMediatedNetworkSettings;
        }
    }
}