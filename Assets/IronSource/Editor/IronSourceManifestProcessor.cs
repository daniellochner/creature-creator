#if UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;


#if UNITY_2018_1_OR_NEWER
public class IronSourceManifestProcessor : IPreprocessBuildWithReport
#else
public class IronSourceManifestProcessor : IPreprocessBuild
#endif
{
    private const string META_APPLICATION_ID = "com.google.android.gms.ads.APPLICATION_ID";
    private const string AD_ID_PERMISSION_ATTR = "com.google.android.gms.permission.AD_ID";
    private const string MANIFEST_PERMISSION = "uses-permission";
    private const string MANIFEST_META_DATA = "meta-data";
    private const string IRONSOURCE_MANIFEST_PATH = "IronSource/Plugins/Android/IronSource.plugin/AndroidManifest.xml";
    private string manifestPath = "";
    private XNamespace ns = "http://schemas.android.com/apk/res/android";

    public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
    {
        if (File.Exists(IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH) || File.Exists(IronSourceMediationSettings.IRONSOURCE_SETTINGS_ASSET_PATH))
        {


            XElement elemManifest = ValidateAndroidManifest();

            XElement elemApplication = elemManifest.Element("application");

            if (File.Exists(IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH))
            {
                string appId = IronSourceMediatedNetworkSettingsInspector.IronSourceMediatedNetworkSettings.AdmobAndroidAppId;

                IEnumerable<XElement> metas = elemApplication.Descendants()
            .Where(elem => elem.Name.LocalName.Equals(MANIFEST_META_DATA));

                if (IronSourceMediatedNetworkSettingsInspector.IronSourceMediatedNetworkSettings.EnableAdmob)
                {

                    XElement elemAdMobEnabled = GetMetaElement(metas, META_APPLICATION_ID);

                    if (appId.Length == 0)
                    {
                        StopBuildWithMessage(
                            "Android AdMob app ID is empty. Please enter your app ID to run ads properly");
                    }
                    else if (!Regex.IsMatch(appId, "^[a-zA-Z0-9-~]*$"))
                    {
                        StopBuildWithMessage(
                            "Android AdMob app ID is not valid. Please enter a valid app ID to run ads properly");
                    }

                    else if (elemAdMobEnabled == null)
                    {
                        elemApplication.Add(CreateMetaElement(META_APPLICATION_ID, appId));
                    }
                    else
                    {
                        elemAdMobEnabled.SetAttributeValue(ns + "value", appId);
                    }

                }
                else if (GetPermissionElement(metas, META_APPLICATION_ID) != null)
                {
                    //remove admob app id in case flag is off
                    GetPermissionElement(metas, META_APPLICATION_ID).Remove();
                }
            }

            if (File.Exists(IronSourceMediationSettings.IRONSOURCE_SETTINGS_ASSET_PATH))
            {
                IEnumerable<XElement> permissons = elemManifest.Descendants().Where(elem => elem.Name.LocalName.Equals(MANIFEST_PERMISSION));

                if (IronSourceMediationSettingsInspector.IronSourceMediationSettings.DeclareAD_IDPermission && GetPermissionElement(permissons, AD_ID_PERMISSION_ATTR) == null)
                {

                    elemManifest.Add(CreatePermissionElement(AD_ID_PERMISSION_ATTR));
                }

                else if (GetPermissionElement(permissons, AD_ID_PERMISSION_ATTR) != null && !IronSourceMediationSettingsInspector.IronSourceMediationSettings.DeclareAD_IDPermission)
                {
                    //remove the permission if flag is false
                    GetPermissionElement(permissons, AD_ID_PERMISSION_ATTR).Remove();
                }
            }
            manifestPath = Path.Combine(Application.dataPath, IRONSOURCE_MANIFEST_PATH);
            elemManifest.Save(manifestPath);

        }
    }

    private XElement CreateMetaElement(string name, object value)
    {
        return new XElement(MANIFEST_META_DATA,
                new XAttribute(ns + "name", name), new XAttribute(ns + "value", value));
    }

    private XElement CreatePermissionElement(string name)
    {
        return new XElement(MANIFEST_PERMISSION,
                new XAttribute(ns + "name", name));
    }

    private XElement GetMetaElement(IEnumerable<XElement> metas, string metaName)
    {
        foreach (XElement elem in metas)
        {
            IEnumerable<XAttribute> attrs = elem.Attributes();
            foreach (XAttribute attr in attrs)
            {
                if (attr.Name.Namespace.Equals(ns)
                        && attr.Name.LocalName.Equals("name") && attr.Value.Equals(metaName))
                {
                    return elem;
                }
            }
        }
        return null;
    }

    private XElement GetPermissionElement(IEnumerable<XElement> manifest, string permissionName)
    {

        foreach (XElement elem in manifest)
        {
            IEnumerable<XAttribute> attrs = elem.Attributes();
            foreach (XAttribute attr in attrs)
            {
                if (attr.Name.Namespace.Equals(ns)
                        && attr.Name.LocalName.Equals("name") && attr.Value.Equals(permissionName))
                {
                    return elem;
                }
            }
        }
        return null;
    }

    private void StopBuildWithMessage(string message)
    {
        string prefix = "[IronSourceApplicationSettings] ";

        EditorUtility.DisplayDialog(
            "IronSource Developer Settings", "Error: " + message, "", "");
#if UNITY_2017_1_OR_NEWER
        throw new System.OperationCanceledException(prefix + message);
#else
        throw new OperationCanceledException(prefix + message);
#endif
    }

    private XElement ValidateAndroidManifest()
    {

        XDocument manifest = null;
        try
        {
            manifestPath = Path.Combine(Application.dataPath, IRONSOURCE_MANIFEST_PATH);
            manifest = XDocument.Load(manifestPath);
        }
#pragma warning disable 0168
        catch (IOException e)
#pragma warning restore 0168
        {
            StopBuildWithMessage("AndroidManifest.xml is missing. Try re-importing the plugin.");
        }

        XElement elemManifest = manifest.Element("manifest");
        if (elemManifest == null)
        {
            StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
        }

        XElement elemApplication = elemManifest.Element("application");
        if (elemApplication == null)
        {
            StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
        }

        return elemManifest;
    }
}

#endif
