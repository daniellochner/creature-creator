#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
public class IronSourceRewardedVideoLevelPlayAndroid : AndroidJavaProxy, IUnityLevelPlayRewardedVideo
{
    //implements UnityRewardedVideoListener java interface
    public IronSourceRewardedVideoLevelPlayAndroid(): base(IronSourceConstants.LevelPlayRewardedVideoBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityRewardedVideoLevelPlayListener", this);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("setUnityRewardedVideoLevelPlayListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError, IronSourceAdInfo> OnAdShowFailed = delegate { };
    public event Action<IronSourceAdInfo> OnAdOpened = delegate { };
    public event Action<IronSourceAdInfo> OnAdClosed = delegate { };
    public event Action<IronSourcePlacement, IronSourceAdInfo> OnAdRewarded = delegate { };
    public event Action<IronSourcePlacement,IronSourceAdInfo> OnAdClicked = delegate { };
    public event Action<IronSourceAdInfo> OnAdAvailable = delegate { };
    public event Action OnAdUnavailable = delegate { };

    void onAdShowFailed(string description, string data)
    {
        if (this.OnAdShowFailed != null)
        {
            IronSourceError ssp = IronSourceUtils.getErrorFromErrorObject(description);
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdShowFailed(ssp, adInfo);
        }
    }

    void onAdClosed(string data)
    {
        if (this.OnAdClosed != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdClosed(adInfo);
        }

    }

    void onAdOpened(string data)
    {
        if (this.OnAdOpened != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdOpened(adInfo);
        }

    }


    void onAdRewarded(string description, string data)
    {
        if (this.OnAdRewarded != null)
        {
            IronSourcePlacement ssp = IronSourceUtils.getPlacementFromObject(description);
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdRewarded(ssp, adInfo);
        }

    }

    void onAdClicked(string description, string data)
    {
        if (this.OnAdClicked != null)
        {
            IronSourcePlacement ssp = IronSourceUtils.getPlacementFromObject(description);
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdClicked(ssp, adInfo);
        }

    }

    void onAdAvailable( string data)
    {
        

        if (this.OnAdAvailable != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(data);
            this.OnAdAvailable( adInfo);
        }

    }

    void onAdUnavailable()
    {

        if (this.OnAdUnavailable != null)
        {
            this.OnAdUnavailable();
        }

    }

}
#endif