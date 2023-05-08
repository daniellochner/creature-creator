#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceInterstitialAndroid : AndroidJavaProxy, IUnityInterstitial
{

    //implements UnityInterstitialListener java interface
    public IronSourceInterstitialAndroid() : base(IronSourceConstants.interstitialBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityInterstitialListener", this);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("setUnityInterstitialListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError> OnInterstitialAdShowFailed = delegate { };
    public event Action<IronSourceError> OnInterstitialAdLoadFailed = delegate { };
    public event Action OnInterstitialAdReady = delegate { };
    public event Action OnInterstitialAdOpened = delegate { };
    public event Action OnInterstitialAdClosed = delegate { };
    public event Action OnInterstitialAdShowSucceeded = delegate { };
    public event Action OnInterstitialAdClicked = delegate { };

    public event Action OnInterstitialAdRewarded = delegate { };

    public event Action<string> OnInterstitialAdReadyDemandOnly = delegate { };
    public event Action<string> OnInterstitialAdOpenedDemandOnly = delegate { };
    public event Action<string> OnInterstitialAdClosedDemandOnly = delegate { };
    public event Action<string, IronSourceError> OnInterstitialAdLoadFailedDemandOnly = delegate { };
    public event Action<string> OnInterstitialAdClickedDemandOnly = delegate { };
    public event Action<string, IronSourceError> OnInterstitialAdShowFailedDemandOnly = delegate { };

    void onInterstitialAdShowFailed(string description)
    {
        if (this.OnInterstitialAdShowFailed != null)
        {
            IronSourceError ssp = IronSourceUtils.getErrorFromErrorObject(description);
            this.OnInterstitialAdShowFailed(ssp);
        }
    }

    void onInterstitialAdReady()
    {
        if (this.OnInterstitialAdReady != null)
        {
            this.OnInterstitialAdReady();
        }

    }

    void onInterstitialAdOpened()
    {
        if (this.OnInterstitialAdOpened != null)
        {
            this.OnInterstitialAdOpened();
        }

    }

    void onInterstitialAdClosed()
    {
        if (this.OnInterstitialAdClosed != null)
        {
            this.OnInterstitialAdClosed();
        }

    }

    void onInterstitialAdShowSucceeded()
    {
        if (this.OnInterstitialAdShowSucceeded != null)
        {

            this.OnInterstitialAdShowSucceeded();
        }

    }


    void onInterstitialAdClicked()
    {
        if (this.OnInterstitialAdClicked != null)
        {
            this.OnInterstitialAdClicked();
        }

    }

    void onInterstitialAdLoadFailed(string args)
    {
        if (this.OnInterstitialAdLoadFailed != null)
        {
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(args);
            this.OnInterstitialAdLoadFailed(err);
        }
    }

    void onInterstitialAdRewarded()
    {
        if (this.OnInterstitialAdRewarded != null)
        {
            this.OnInterstitialAdRewarded();
        }

    }

    void onInterstitialAdClickedDemandOnly(string instanceId)
    {
        if (this.OnInterstitialAdClickedDemandOnly != null)
        {
            this.OnInterstitialAdClickedDemandOnly(instanceId);
        }

    }

    void onInterstitialAdClosedDemandOnly(string instanceId)
    {
        if (this.OnInterstitialAdClosedDemandOnly != null)
        {

            this.OnInterstitialAdClosedDemandOnly(instanceId);
        }

    }

    void onInterstitialAdOpenedDemandOnly(string instanceId)
    {
        if (this.OnInterstitialAdOpenedDemandOnly != null)
        {
            this.OnInterstitialAdOpenedDemandOnly(instanceId);
        }

    }

    void onInterstitialAdReadyDemandOnly(string instanceId)
    {
        if (this.OnInterstitialAdReadyDemandOnly != null)
        {
            this.OnInterstitialAdReadyDemandOnly(instanceId);
        }

    }

    void onInterstitialAdLoadFailedDemandOnly(string args)
    {
        if (this.OnInterstitialAdLoadFailedDemandOnly != null)
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            this.OnInterstitialAdLoadFailedDemandOnly(instanceId, err);
        }

    }

    void onInterstitialAdShowFailedDemandOnly(string args)
    {
        if (this.OnInterstitialAdShowFailedDemandOnly != null)
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            this.OnInterstitialAdShowFailedDemandOnly(instanceId, err);
        }

    }


}
#endif