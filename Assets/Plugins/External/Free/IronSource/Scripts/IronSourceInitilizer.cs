using UnityEngine;

public class IronSourceInitilizer
{
#if UNITY_IOS || UNITY_ANDROID
    [RuntimeInitializeOnLoadMethod]
    static void Initilize()
    {
        var developerSettings = Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        if (developerSettings != null)
        {
#if UNITY_ANDROID
            string appKey = developerSettings.AndroidAppKey;
#elif UNITY_IOS
        string appKey = developerSettings.IOSAppKey;
#endif
            if (developerSettings.EnableIronsourceSDKInitAPI == true)
            {
                if (appKey.Equals(string.Empty))
                {
                    Debug.LogWarning("IronSourceInitilizer Cannot init without AppKey");
                }
                else
                {
                    IronSource.Agent.init(appKey);
                    IronSource.UNITY_PLUGIN_VERSION = "7.2.1-ri";
                }

            }

            if (developerSettings.EnableAdapterDebug)
            {
                IronSource.Agent.setAdaptersDebug(true);
            }

            if (developerSettings.EnableIntegrationHelper)
            {
                IronSource.Agent.validateIntegration();
            }
        }
    }
#endif

}
