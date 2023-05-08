using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class IronSourceBannerEvents : MonoBehaviour
{

#if UNITY_ANDROID
    #pragma warning disable CS0067
    public static event Action<IronSourceAdInfo> onAdLoadedEvent;
    public static event Action<IronSourceAdInfo> onAdLeftApplicationEvent;
    public static event Action<IronSourceAdInfo> onAdScreenDismissedEvent;
    public static event Action<IronSourceAdInfo> onAdScreenPresentedEvent;
    public static event Action<IronSourceAdInfo> onAdClickedEvent;
    public static event Action<IronSourceError> onAdLoadFailedEvent;
#endif

#if UNITY_ANDROID
    private IUnityLevelPlayBanner LevelPlaybannerAndroid;
#endif

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        LevelPlaybannerAndroid = new IronSourceBannerLevelPlayAndroid();//sets this.bannerAndroid as listener for Banner(Mediation Only) events in the bridge
        registerBannerEvents();//subscribe to Banner events from this.bannerAndroid

#endif

        gameObject.name = "IronSourceBannerEvents";           //Change the GameObject name to IronSourceEvents.
        DontDestroyOnLoad(gameObject);                 //Makes the object not be destroyed automatically when loading a new scene.
    }

#if UNITY_ANDROID && !UNITY_EDITOR

    private void registerBannerEvents()
    {
        LevelPlaybannerAndroid.OnAdLoaded += (IronSourceAdInfo) =>
        {
            if (onAdLoadedEvent!= null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdLoadedEvent?.Invoke(IronSourceAdInfo);
                });
            }

        };

        LevelPlaybannerAndroid.OnAdClicked += (IronSourceAdInfo) =>
        {
            if (onAdClickedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdClickedEvent?.Invoke(IronSourceAdInfo);
                });
            }
        };

        LevelPlaybannerAndroid.OnAdLoadFailed += (ironSourceError) =>
        {
            if (onAdLoadFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdLoadFailedEvent?.Invoke(ironSourceError);
                });
            }
        };

        LevelPlaybannerAndroid.OnAdLeftApplication += (IronSourceAdInfo) =>
        {
            if (onAdLeftApplicationEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdLeftApplicationEvent?.Invoke(IronSourceAdInfo);
                });
            }
        };

        LevelPlaybannerAndroid.OnAdScreenDismissed += (IronSourceAdInfo) =>
        {
            if (onAdScreenDismissedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdScreenDismissedEvent?.Invoke(IronSourceAdInfo);
                });
            }
        };

        LevelPlaybannerAndroid.OnAdScreenPresented += (IronSourceAdInfo) =>
        {
            if (onAdScreenPresentedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onAdScreenPresentedEvent?.Invoke(IronSourceAdInfo);
                });
            }
        };
    }

#endif

#if !UNITY_ANDROID

    // ******************************* Banner Events *******************************    
    private static event Action<IronSourceAdInfo> _onAdLoadedEvent;

    public static event Action<IronSourceAdInfo> onAdLoadedEvent
    {
        add
        {
            if (_onAdLoadedEvent
         == null || !_onAdLoadedEvent
        .GetInvocationList().Contains(value))
            {
                _onAdLoadedEvent
         += value;
            }
        }

        remove
        {
            if (_onAdLoadedEvent
         != null || _onAdLoadedEvent
        .GetInvocationList().Contains(value))
            {
                _onAdLoadedEvent
         -= value;
            }
        }
    }

    public void onAdLoaded(string args)
    {
        if (_onAdLoadedEvent != null) {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(args);
            _onAdLoadedEvent(adInfo);
        }
        
    }

    private static event Action<IronSourceError> _onAdLoadFailedEvent;

    public static event Action<IronSourceError> onAdLoadFailedEvent
    {
        add
        {
            if (_onAdLoadFailedEvent == null || !_onAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onAdLoadFailedEvent += value;
            }
        }

        remove
        {
            if (_onAdLoadFailedEvent != null && _onAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onAdLoadFailedEvent -= value;
            }
        }
    }

    public void onAdLoadFailed(string description)
    {
        if (_onAdLoadFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onAdLoadFailedEvent(sse);
        }

    }

    private static event Action<IronSourceAdInfo> _onAdClickedEvent;

    public static event Action<IronSourceAdInfo> onAdClickedEvent
    {
        add
        {
            if (_onAdClickedEvent == null || !_onAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onAdClickedEvent += value;
            }
        }

        remove
        {
            if (_onAdClickedEvent != null && _onAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onAdClickedEvent -= value;
            }
        }
    }

    public void onAdClicked(string args)
    {
        if (_onAdClickedEvent != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(args);
            _onAdClickedEvent(adInfo);
        }

    }

    private static event Action<IronSourceAdInfo> _onAdScreenPresentedEvent;

    public static event Action<IronSourceAdInfo> onAdScreenPresentedEvent
    {
        add
        {
            if (_onAdScreenPresentedEvent == null || !_onAdScreenPresentedEvent.GetInvocationList().Contains(value))
            {
                _onAdScreenPresentedEvent += value;
            }
        }

        remove
        {
            if (_onAdScreenPresentedEvent != null && _onAdScreenPresentedEvent.GetInvocationList().Contains(value))
            {
                _onAdScreenPresentedEvent -= value;
            }
        }
    }

    public void onAdScreenPresented(string args)
    {
        if (_onAdScreenPresentedEvent != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(args);
            _onAdScreenPresentedEvent(adInfo);
        }

    }

    private static event Action<IronSourceAdInfo> _onAdScreenDismissedEvent;

    public static event Action<IronSourceAdInfo> onAdScreenDismissedEvent
    {
        add
        {
            if (_onAdScreenDismissedEvent == null || !_onAdScreenDismissedEvent.GetInvocationList().Contains(value))
            {
                _onAdScreenDismissedEvent += value;
            }
        }

        remove
        {
            if (_onAdScreenDismissedEvent != null && _onAdScreenDismissedEvent.GetInvocationList().Contains(value))
            {
                _onAdScreenDismissedEvent -= value;
            }
        }
    }

    public void onAdScreenDismissed(string args)
    {
        if (_onAdScreenDismissedEvent != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(args);
            _onAdScreenDismissedEvent(adInfo);
        }
    }

    private static event Action<IronSourceAdInfo> _onAdLeftApplicationEvent;

    public static event Action<IronSourceAdInfo> onAdLeftApplicationEvent
    {
        add
        {
            if (_onAdLeftApplicationEvent == null || !_onAdLeftApplicationEvent.GetInvocationList().Contains(value))
            {
                _onAdLeftApplicationEvent += value;
            }
        }

        remove
        {
            if (_onAdLeftApplicationEvent != null && _onAdLeftApplicationEvent.GetInvocationList().Contains(value))
            {
                _onAdLeftApplicationEvent -= value;
            }
        }
    }

    public void onAdLeftApplication(string args)
    {
        if (_onAdLeftApplicationEvent != null)
        {
            IronSourceAdInfo adInfo = new IronSourceAdInfo(args);
            _onAdLeftApplicationEvent(adInfo);
        }
    }
#endif

    // ******************************* Helper methods *******************************   

    private IronSourceError getErrorFromErrorObject(object descriptionObject)
    {
        Dictionary<string, object> error = null;
        if (descriptionObject is IDictionary)
        {
            error = descriptionObject as Dictionary<string, object>;
        }
        else if (descriptionObject is String && !String.IsNullOrEmpty(descriptionObject.ToString()))
        {
            error = IronSourceJSON.Json.Deserialize(descriptionObject.ToString()) as Dictionary<string, object>;
        }

        IronSourceError sse = new IronSourceError(-1, "");
        if (error != null && error.Count > 0)
        {
            int eCode = Convert.ToInt32(error[IronSourceConstants.ERROR_CODE].ToString());
            string eDescription = error[IronSourceConstants.ERROR_DESCRIPTION].ToString();
            sse = new IronSourceError(eCode, eDescription);
        }

        return sse;
    }

    private IronSourcePlacement getPlacementFromObject(object placementObject)
    {
        Dictionary<string, object> placementJSON = null;
        if (placementObject is IDictionary)
        {
            placementJSON = placementObject as Dictionary<string, object>;
        }
        else if (placementObject is String)
        {
            placementJSON = IronSourceJSON.Json.Deserialize(placementObject.ToString()) as Dictionary<string, object>;
        }

        IronSourcePlacement ssp = null;
        if (placementJSON != null && placementJSON.Count > 0)
        {
            int rewardAmount = Convert.ToInt32(placementJSON["placement_reward_amount"].ToString());
            string rewardName = placementJSON["placement_reward_name"].ToString();
            string placementName = placementJSON["placement_name"].ToString();

            ssp = new IronSourcePlacement(placementName, rewardName, rewardAmount);
        }

        return ssp;
    }
}
