#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceOfferwallAndroid : AndroidJavaProxy, IUnityOfferwall
{

    //implements UnityOfferwallListener java interface
    public IronSourceOfferwallAndroid() : base(IronSourceConstants.offerwallBridgeListenerClass)
    {
        try
        {    
            using (var pluginClass = new AndroidJavaClass(IronSourceConstants.bridgeClass))
            {
                var bridgeInstance = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                bridgeInstance.Call("setUnityOfferwallListener", this);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("setUnityOfferwallListener method doesn't exist, error: " + e.Message);
        }
    }

    public event Action<IronSourceError> OnOfferwallShowFailed = delegate { };
    public event Action OnOfferwallOpened = delegate { };
    public event Action OnOfferwallClosed = delegate { };
    public event Action<IronSourceError> OnGetOfferwallCreditsFailed = delegate { };
    public event Action<Dictionary<string, object>> OnOfferwallAdCredited = delegate { };
    public event Action<bool> OnOfferwallAvailable = delegate { };

    public void onOfferwallOpened ()
    {
        if (this.OnOfferwallOpened != null) {
            this.OnOfferwallOpened ();
        }
    }


    public void onOfferwallShowFailed (string description)
    {
        if (OnOfferwallShowFailed != null) {
            IronSourceError sse = IronSourceUtils.getErrorFromErrorObject (description);
            OnOfferwallShowFailed (sse);
        }
    }

    
    public void onOfferwallClosed ()
    {
        if (OnOfferwallClosed != null) {
            OnOfferwallClosed ();
        }
    }

    
    public void onGetOfferwallCreditsFailed (string description)
    {
        if (OnGetOfferwallCreditsFailed != null) {
            IronSourceError sse = IronSourceUtils.getErrorFromErrorObject (description);
            OnGetOfferwallCreditsFailed (sse);

        }
    }

    
    public void onOfferwallAdCredited (string json)
    {
        if (OnOfferwallAdCredited != null)
            OnOfferwallAdCredited (IronSourceJSON.Json.Deserialize (json) as Dictionary<string,object>);
    }

    
    public void onOfferwallAvailable (string stringAvailable)
    {
        bool isAvailable = (stringAvailable == "true") ? true : false;
        if (OnOfferwallAvailable != null)
            OnOfferwallAvailable (isAvailable);
    }


}
#endif