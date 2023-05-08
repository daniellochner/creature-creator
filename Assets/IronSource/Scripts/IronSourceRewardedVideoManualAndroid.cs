#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
public class IronSourceRewardedVideoManualAndroid : AndroidJavaProxy, IUnityRewardedVideoManual
{

    public IronSourceRewardedVideoManualAndroid() : base(IronSourceConstants.rewardedVideoManualBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityRewardedVideoManualListener", this);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("setUnityRewardedVideoListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError> OnRewardedVideoAdLoadFailed = delegate { };
    public event Action OnRewardedVideoAdReady = delegate { };


    void onRewardedVideoAdReady() {
        if (this.OnRewardedVideoAdReady != null) {
            this.OnRewardedVideoAdReady();
        }
    }

    void onRewardedVideoAdLoadFailed(string args) {
        if (this.OnRewardedVideoAdLoadFailed!=null) {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = IronSourceUtils.getErrorFromErrorObject(argList[0]);
            this.OnRewardedVideoAdLoadFailed(err);
        }
    }
}

#endif
