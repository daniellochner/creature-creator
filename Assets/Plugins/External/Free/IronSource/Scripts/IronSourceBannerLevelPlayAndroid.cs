#if UNITY_ANDROID
using System;
using UnityEngine;

public class IronSourceBannerLevelPlayAndroid : AndroidJavaProxy, IUnityLevelPlayBanner
{

    public event Action<IronSourceAdInfo> OnAdLoaded = delegate { };
    public event Action<IronSourceAdInfo> OnAdLeftApplication = delegate { };
    public event Action<IronSourceAdInfo> OnAdScreenDismissed = delegate { };
    public event Action<IronSourceAdInfo> OnAdScreenPresented = delegate { };
    public event Action<IronSourceAdInfo> OnAdClicked = delegate { };
    public event Action<IronSourceError> OnAdLoadFailed = delegate { };

    //implements UnityLevelPlayBannerListener java interface and implement banner callbacks
    public IronSourceBannerLevelPlayAndroid() : base(IronSourceConstants.LevelPlaybannerBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityBannerLevelPlayListener", this);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("setUnityBannerLevelPlayListener method doesn't exist, error: " + e.Message);
        }

    }

    void onAdLoaded(String data)
    {
        if (OnAdLoaded != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            OnAdLoaded(adInfo);
        }

    }
    void onAdLoadFailed(String args)
    {
        if (OnAdLoadFailed != null)
        {
            IronSourceError error = IronSourceUtils.getErrorFromErrorObject(args);
            OnAdLoadFailed(error);
        }
    }
    void onAdClicked(String data)
    {
        if (OnAdClicked != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            OnAdClicked(adInfo);
        }
    }
    void onAdScreenPresented(String data)
    {
        if (OnAdScreenPresented != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            OnAdScreenPresented(adInfo);
        }
    }
    void onAdScreenDismissed(String data)
    {
        if (OnAdScreenDismissed != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            OnAdScreenDismissed(adInfo);
        }
    }
    void onAdLeftApplication(String data)
    {
        if (OnAdLeftApplication != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            OnAdLeftApplication(adInfo);
        }
    }
}
#endif
