#if UNITY_ANDROID
using System;
using UnityEngine;

public class IronSourceImpressionDataAndroid : AndroidJavaProxy, IUnityImpressionData
{
    public event Action<IronSourceImpressionData> OnImpressionSuccess = delegate { };
    public event Action<IronSourceImpressionData> OnImpressionDataReady = delegate { };

    //implements UnityImpressionDataListener java interface
    public IronSourceImpressionDataAndroid() : base(IronSourceConstants.impressionDataBridgeListenerClass)
    {
        try
        {
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>(IronSourceConstants.GET_INSTANCE_KEY);
                bridgeInstance.Call("setUnityImpressionDataListener", this);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("setUnityImpressionDataListener method doesn't exist, error: " + e.Message);
        }

    }

    public void onImpressionSuccess(string data)
    {
        if (OnImpressionSuccess != null)
        {
            IronSourceImpressionData impressionData = new IronSourceImpressionData(data);
            OnImpressionSuccess(impressionData);
        }
    }

    public void onImpressionDataReady(string data)
    {
        if (OnImpressionDataReady != null)
        {
            IronSourceImpressionData impressionData = new IronSourceImpressionData(data);
            OnImpressionDataReady(impressionData);
        }
    }

}

#endif