#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceInterstitialLevelPlayAndroid : AndroidJavaProxy, IUnityLevelPlayInterstitial
{

    //implements UnityInterstitialLevelPlayListener java interface
    public IronSourceInterstitialLevelPlayAndroid() : base(IronSourceConstants.LevelPlayinterstitialBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityInterstitialLevelPlayListener", this);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("setUnityInterstitialLevelPlayListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError, IronSourceAdInfo> OnAdShowFailed = delegate { };
    public event Action<IronSourceError> OnAdLoadFailed = delegate { };
    public event Action<IronSourceAdInfo> OnAdReady = delegate { };
    public event Action<IronSourceAdInfo> OnAdOpened = delegate { };
    public event Action<IronSourceAdInfo> OnAdClosed = delegate { };
    public event Action<IronSourceAdInfo> OnAdShowSucceeded = delegate { };
    public event Action<IronSourceAdInfo> OnAdClicked = delegate { };

    void onAdShowFailed(string description, String data)
    {
        if (this.OnAdShowFailed != null)
        {
            IronSourceError ssp = IronSourceUtils.getErrorFromErrorObject(description);
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdShowFailed(ssp, adInfo);
        }
    }

    void onAdReady(String data)
    {
        if (this.OnAdReady != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdReady(adInfo);
        }

    }

    void onAdOpened(String data)
    {
        if (this.OnAdOpened != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdOpened(adInfo);
        }

    }

    void onAdClosed(String data)
    {
        if (this.OnAdClosed != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdClosed(adInfo);
        }

    }

    void onAdShowSucceeded(String data)
    {
        if (this.OnAdShowSucceeded != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdShowSucceeded(adInfo);
        }

    }


    void onAdClicked(String data)
    {
        if (this.OnAdClicked != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdClicked(adInfo);
        }

    }

    void onAdLoadFailed(string args)
    {
        if (this.OnAdLoadFailed != null)
        {
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(args);
            this.OnAdLoadFailed(err);
        }
    }

}
#endif