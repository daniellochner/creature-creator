#if UNITY_ANDROID
using System;
using UnityEngine;

public class IronSourceBannerAndroid : AndroidJavaProxy, IUnityBanner
{

    public event Action OnBannerAdLoaded = delegate { };
    public event Action OnBannerAdLeftApplication = delegate { };
    public event Action OnBannerAdScreenDismissed = delegate { };
    public event Action OnBannerAdScreenPresented = delegate { };
    public event Action OnBannerAdClicked = delegate { };
    public event Action<IronSourceError> OnBannerAdLoadFailed = delegate { };

    //implements UnityInterstitialListener java interface and implement banner callbacks
    public IronSourceBannerAndroid() : base(IronSourceConstants.bannerBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityBannerListener", this);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("setUnityBannerListener method doesn't exist, error: " + e.Message);
        }

    }

    void onBannerAdLoaded()
    {
        if (OnBannerAdLoaded != null)
        {
            OnBannerAdLoaded();
        }

    }
    void onBannerAdLoadFailed(String args)
    {
        if (OnBannerAdLoadFailed != null)
        {
            IronSourceError error = IronSourceUtils.getErrorFromErrorObject(args);
            OnBannerAdLoadFailed(error);
        }
    }
    void onBannerAdClicked()
    {
        if (OnBannerAdClicked != null)
        {
            OnBannerAdClicked();
        }
    }
    void onBannerAdScreenPresented()
    {
        if (OnBannerAdScreenPresented != null)
        {
            OnBannerAdScreenPresented();
        }
    }
    void onBannerAdScreenDismissed()
    {
        if (OnBannerAdScreenDismissed != null)
        {
            OnBannerAdScreenDismissed();
        }
    }
    void onBannerAdLeftApplication()
    {
        if (OnBannerAdLeftApplication != null)
        {
            OnBannerAdLeftApplication();
        }
    }
}
#endif
