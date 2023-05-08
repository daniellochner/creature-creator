#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
public class IronSourceRewardedVideoAndroid : AndroidJavaProxy, IUnityRewardedVideo
{
    //implements UnityRewardedVideoListener java interface
    public IronSourceRewardedVideoAndroid(): base(IronSourceConstants.rewardedVideoBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityRewardedVideoListener", this);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("setUnityRewardedVideoListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError> OnRewardedVideoAdShowFailed = delegate { };
    public event Action OnRewardedVideoAdOpened = delegate { };
    public event Action OnRewardedVideoAdClosed = delegate { };
    public event Action OnRewardedVideoAdStarted = delegate { };
    public event Action OnRewardedVideoAdEnded = delegate { };
    public event Action<IronSourcePlacement> OnRewardedVideoAdRewarded = delegate { };
    public event Action<IronSourcePlacement> OnRewardedVideoAdClicked = delegate { };
    public event Action<bool> OnRewardedVideoAvailabilityChanged = delegate { };

    public event Action<String> OnRewardedVideoAdOpenedDemandOnlyEvent = delegate { };
    public event Action<String> OnRewardedVideoAdClosedDemandOnlyEvent = delegate { };
    public event Action<String> OnRewardedVideoAdLoadedDemandOnlyEvent = delegate { };
    public event Action<String> OnRewardedVideoAdRewardedDemandOnlyEvent = delegate { };
    public event Action<String, IronSourceError> OnRewardedVideoAdShowFailedDemandOnlyEvent = delegate { };
    public event Action<String> OnRewardedVideoAdClickedDemandOnlyEvent = delegate { };
    public event Action<String, IronSourceError> OnRewardedVideoAdLoadFailedDemandOnlyEvent = delegate { };


    void onRewardedVideoAdShowFailed(string description)
    {
        if (this.OnRewardedVideoAdShowFailed != null)
        {
            IronSourceError ssp = IronSourceUtils.getErrorFromErrorObject(description);
            this.OnRewardedVideoAdShowFailed(ssp);
        }
    }

    void onRewardedVideoAdClosed()
    {
        if (this.OnRewardedVideoAdClosed != null)
        {
            this.OnRewardedVideoAdClosed();
        }

    }

    void onRewardedVideoAdOpened()
    {
        if (this.OnRewardedVideoAdOpened != null)
        {
            this.OnRewardedVideoAdOpened();
        }

    }

    void onRewardedVideoAdStarted()
    {
        if (this.OnRewardedVideoAdStarted != null)
        {
            this.OnRewardedVideoAdStarted();
        }

    }

    void onRewardedVideoAdEnded()
    {
        if (this.OnRewardedVideoAdEnded != null)
        {

            this.OnRewardedVideoAdEnded();
        }

    }

    void onRewardedVideoAdRewarded(string description)
    {
        if (this.OnRewardedVideoAdRewarded != null)
        {
            IronSourcePlacement ssp = IronSourceUtils.getPlacementFromObject(description);
            this.OnRewardedVideoAdRewarded(ssp);
        }

    }

    void onRewardedVideoAdClicked(string description)
    {
        if (this.OnRewardedVideoAdClicked != null)
        {
            IronSourcePlacement ssp = IronSourceUtils.getPlacementFromObject(description);

            this.OnRewardedVideoAdClicked(ssp);
        }

    }

    void onRewardedVideoAvailabilityChanged(string stringAvailable)
    {
        bool isAvailable = (stringAvailable == "true") ? true : false;

        if (this.OnRewardedVideoAvailabilityChanged != null)
        {
            this.OnRewardedVideoAvailabilityChanged(isAvailable);
        }

    }

    void onRewardedVideoAdShowFailedDemandOnly(string args)
    {
        if (this.OnRewardedVideoAdShowFailedDemandOnlyEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            this.OnRewardedVideoAdShowFailedDemandOnlyEvent(instanceId, err);
        }
    }

    void onRewardedVideoAdClosedDemandOnly(string instanceId)
    {
        if (this.OnRewardedVideoAdClosedDemandOnlyEvent != null)
        {
            this.OnRewardedVideoAdClosedDemandOnlyEvent(instanceId);
        }

    }

    void onRewardedVideoAdOpenedDemandOnly(string instanceId)
    {
        if (this.OnRewardedVideoAdOpenedDemandOnlyEvent != null)
        {
            this.OnRewardedVideoAdOpenedDemandOnlyEvent(instanceId);
        }

    }

    void onRewardedVideoAdRewardedDemandOnly(string instanceId)
    {
        if (this.OnRewardedVideoAdRewardedDemandOnlyEvent != null)
        {

            this.OnRewardedVideoAdRewardedDemandOnlyEvent(instanceId);
        }

    }

    void onRewardedVideoAdClickedDemandOnly(string instanceId)
    {
        if (this.OnRewardedVideoAdClickedDemandOnlyEvent != null)
        {
            this.OnRewardedVideoAdClickedDemandOnlyEvent(instanceId);
        }

    }

    void onRewardedVideoAdLoadedDemandOnly(string instanceId)
    {
        if (this.OnRewardedVideoAdLoadedDemandOnlyEvent != null)
        {
            this.OnRewardedVideoAdLoadedDemandOnlyEvent(instanceId);
        }

    }

    void onRewardedVideoAdLoadFailedDemandOnly(string args)
    {
        if (this.OnRewardedVideoAdLoadFailedDemandOnlyEvent != null)
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            this.OnRewardedVideoAdLoadFailedDemandOnlyEvent(instanceId, err);
        }

    }

}
#endif