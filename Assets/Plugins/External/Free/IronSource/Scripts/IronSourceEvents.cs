using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class IronSourceEvents : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_IOS
    delegate void ISUnityBackgroundCallback(string args);
	[DllImport("__Internal")]
	static extern void RegisterCallback(ISUnityBackgroundCallback func);

#endif

#if UNITY_ANDROID
#pragma warning disable CS0067
    public static event Action onSdkInitializationCompletedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourceError> onRewardedVideoAdShowFailedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdOpenedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdClosedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdStartedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdEndedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourcePlacement> onRewardedVideoAdRewardedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourcePlacement> onRewardedVideoAdClickedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<bool> onRewardedVideoAvailabilityChangedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourceError> onRewardedVideoAdLoadFailedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdReadyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdOpenedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdClosedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdLoadedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdRewardedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onRewardedVideoAdShowFailedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdClickedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onRewardedVideoAdLoadFailedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdReadyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action<IronSourceError> onInterstitialAdLoadFailedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdOpenedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdClosedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdShowSucceededEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action<IronSourceError> onInterstitialAdShowFailedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdClickedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdReadyDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdOpenedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdClosedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onInterstitialAdLoadFailedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdClickedDemandOnlyEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onInterstitialAdShowFailedDemandOnlyEvent;

    public static event Action<bool> onOfferwallAvailableEvent;
    public static event Action onOfferwallOpenedEvent;
    public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent;
    public static event Action<IronSourceError> onGetOfferwallCreditsFailedEvent;
    public static event Action onOfferwallClosedEvent;
    public static event Action<IronSourceError> onOfferwallShowFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdLoadedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdLeftApplicationEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdScreenDismissedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdScreenPresentedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdClickedEvent;
    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action<IronSourceError> onBannerAdLoadFailedEvent;

    public static event Action<string> onSegmentReceivedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use OnImpressionDataReady instead.")]
    public static event Action<IronSourceImpressionData> onImpressionSuccessEvent;
#endif

    private const string ERROR_CODE = "error_code";
    private const string ERROR_DESCRIPTION = "error_description";
    private const string INSTANCE_ID_KEY = "instanceId";
    private const string PLACEMENT_KEY = "placement";
    #pragma warning disable CS0067
    public static event Action<IronSourceImpressionData> onImpressionDataReadyEvent;

#if UNITY_ANDROID
    private IUnityInitialization initializationAndroid;
    private IUnityRewardedVideo rewardedVideoAndroid;
    private IUnityRewardedVideoManual rewardedVideoAndroidManual;
    private IUnityInterstitial interstitialAndroid;
    private IUnityOfferwall offerwallAndroid;
    private IUnityBanner bannerAndroid;
    private IUnitySegment segmentAndroid;
    private IUnityImpressionData impressionDataAndroid;
#endif

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        initializationAndroid = new IronSourceInitializationAndroid();//sets this.initialztionAndroid as listener for initialztionAndroid events in the bridge
        registerInitializationEvents(); //subscribe to initialization events from this.initializationAndroid
        rewardedVideoAndroid = new IronSourceRewardedVideoAndroid();//sets this.rewardedVideoAndroid as listener for RV(Mediation& Demand Only) events in the bridge
        registerRewardedVideoEvents();//subscribe to RV events from this.rewardedVideoAndroid
        rewardedVideoAndroidManual = new IronSourceRewardedVideoManualAndroid();
        registerRewardedVideoManualEvents();//subscribe to RV Manual events from this.rewardedVideoAndroid
        registerRewardedVideoDemandOnlyEvents();//subscribe to RV Demand Only events from this.rewardedVideoAndroid
        interstitialAndroid = new IronSourceInterstitialAndroid();//sets this.interstitialAndroid as listener for Interstitia(Mediation& Demand Only) events in the bridge
        registerInterstitialEvents();//subscribe to Interstitial events from this.interstitialAndroid
        registerInterstitialDemandOnlyEvents();//subscribe to Interstitial Demand Only events from this.interstitialAndroid
        offerwallAndroid = new IronSourceOfferwallAndroid();//sets this.offerwallAndroid as listener for Offerwall(Mediation& Demand Only) events in the bridge
        registerOfferwallEvents();//subscribe to Offerwall events from this.offerwallAndroid
        bannerAndroid = new IronSourceBannerAndroid();//sets this.bannerAndroid as listener for Banner(Mediation& Demand Only) events in the bridge
        registerBannerEvents();//subscribe to Banner events from this.bannerAndroid
        impressionDataAndroid = new IronSourceImpressionDataAndroid();//sets this.impressionDataAndroid as listener for Impression Data events in the bridge
        registerImpressionDataEvents();//subscribe to onImpressionSuccess event from this.impressionDataAndroid
        segmentAndroid = new IronSourceSegmentAndroid();//sets this.segmentAndroid as listener for Segment events in the bridge
        registerSegmentEvents();//subscribe to onSegmentRecieved event from this.segmentAndroid

#endif

#if UNITY_IPHONE || UNITY_IOS
    #if !UNITY_EDITOR
        RegisterCallback(FireCallback);
    #endif
#endif
        gameObject.name = "IronSourceEvents";           //Change the GameObject name to IronSourceEvents.
        DontDestroyOnLoad(gameObject);                 //Makes the object not be destroyed automatically when loading a new scene.
    }


#if UNITY_ANDROID && !UNITY_EDITOR
    private void registerInitializationEvents()
    {
        initializationAndroid.OnSdkInitializationCompletedEvent += () =>
        {
            if (onSdkInitializationCompletedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onSdkInitializationCompletedEvent?.Invoke();
                });
            }

        };
    }

    private void registerBannerEvents()
    {
        bannerAndroid.OnBannerAdLoaded += () =>
        {
            if (onBannerAdLoadedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdLoadedEvent?.Invoke();
                });
            }

        };

        bannerAndroid.OnBannerAdClicked += () =>
        {
            if (onBannerAdClickedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdClickedEvent?.Invoke();
                });
            }
        };

        bannerAndroid.OnBannerAdLoadFailed += (ironSourceError) =>
        {
            if (onBannerAdLoadFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdLoadFailedEvent?.Invoke(ironSourceError);
                });
            }
        };

        bannerAndroid.OnBannerAdLeftApplication += () =>
        {
            if (onBannerAdLeftApplicationEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdLeftApplicationEvent?.Invoke();
                });
            }
        };

        bannerAndroid.OnBannerAdScreenDismissed += () =>
        {
            if (onBannerAdScreenDismissedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdScreenDismissedEvent?.Invoke();
                });
            }
        };

        bannerAndroid.OnBannerAdScreenPresented += () =>
        {
            if (onBannerAdScreenPresentedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onBannerAdScreenPresentedEvent?.Invoke();
                });
            }
        };
    }

    //subscribe to IronSourceInterstitialAndroid IS Mediation & rewarded Interstitial events and notify to subscribed events inside the app
    private void registerInterstitialEvents()
    {
        interstitialAndroid.OnInterstitialAdClicked += () =>
        {
            if (onInterstitialAdClickedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdClickedEvent?.Invoke();
                });
            }
        };

        interstitialAndroid.OnInterstitialAdReady += () =>
        {
            if (onInterstitialAdReadyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdReadyEvent?.Invoke();
                });
            }
        };

        interstitialAndroid.OnInterstitialAdClosed += () =>
        {
            if (onInterstitialAdClosedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdClosedEvent?.Invoke();
                });
            }
        };

        interstitialAndroid.OnInterstitialAdOpened += () =>
        {
            if (onInterstitialAdOpenedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdOpenedEvent?.Invoke();
                });
            }
        };

        interstitialAndroid.OnInterstitialAdLoadFailed += (ironsourceError) =>
        {
            if (onInterstitialAdLoadFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdLoadFailedEvent?.Invoke(ironsourceError);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdShowFailed += (ironSourceError) =>
        {
            if (onInterstitialAdShowFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdShowFailedEvent?.Invoke(ironSourceError);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdShowSucceeded += () =>
        {
            if (onInterstitialAdShowSucceededEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdShowSucceededEvent?.Invoke();
                });
            }
        };

    }

    //subscribe to IronSourceInterstitialAndroid IS Demand Only events and notify to subscribed events inside the app
    private void registerInterstitialDemandOnlyEvents()
    {
        interstitialAndroid.OnInterstitialAdReadyDemandOnly += (instanceId) =>
        {
            if (onInterstitialAdReadyDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdReadyDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdClosedDemandOnly += (instanceId) =>
        {
            if (onInterstitialAdClosedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdClosedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdOpenedDemandOnly += (instanceId) =>
        {
            if (onInterstitialAdOpenedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdOpenedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdClickedDemandOnly += (instanceId) =>
        {
            if (onInterstitialAdClickedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdClickedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdLoadFailedDemandOnly += (instanceId, ironSourceError) =>
        {
            if (onInterstitialAdLoadFailedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdLoadFailedDemandOnlyEvent?.Invoke(instanceId, ironSourceError);
                });
            }
        };

        interstitialAndroid.OnInterstitialAdShowFailedDemandOnly += (instanceId, ironSourceError) =>
        {
            if (onInterstitialAdShowFailedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onInterstitialAdShowFailedDemandOnlyEvent?.Invoke(instanceId, ironSourceError);
                });
            }
        };

    }

    private void registerOfferwallEvents()
    {
        offerwallAndroid.OnOfferwallOpened += () =>
        {
            if (onOfferwallOpenedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onOfferwallOpenedEvent?.Invoke();
                });
            }
        };

        offerwallAndroid.OnOfferwallShowFailed += (error) =>
        {
            if (onOfferwallShowFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onOfferwallShowFailedEvent?.Invoke(error);
                });
            }

        };

        offerwallAndroid.OnOfferwallClosed += () =>
        {
            if (onOfferwallClosedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onOfferwallClosedEvent?.Invoke();
                });
            }
        };

        offerwallAndroid.OnOfferwallAvailable += (isAvailable) =>
        {
            if (onOfferwallAvailableEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onOfferwallAvailableEvent?.Invoke(isAvailable);
                });
            }
        };

        offerwallAndroid.OnOfferwallAdCredited += (dic) =>
        {
            if (onOfferwallAdCreditedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onOfferwallAdCreditedEvent?.Invoke(dic);
                });
            }
        };

        offerwallAndroid.OnGetOfferwallCreditsFailed += (error) =>
        {
            if (onGetOfferwallCreditsFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onGetOfferwallCreditsFailedEvent?.Invoke(error);
                });
            }
        };
    }

    //subscribe to IronSourceSegmentAndroid onSegmentRecieved event and notify to subscribed event inside the app
    private void registerSegmentEvents()
    {
        segmentAndroid.OnSegmentRecieved += (segmentName) =>
        {
            if (onSegmentReceivedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onSegmentReceivedEvent?.Invoke(segmentName);
                });
            }
        };
    }

    //subscribe to IronSourceImpressionDatandroid onSegmentRecieved event and notify to subscribed event inside the app
    private void registerImpressionDataEvents()
    {
        impressionDataAndroid.OnImpressionSuccess += (impressionData) =>
        {

            if (onImpressionSuccessEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onImpressionSuccessEvent?.Invoke(impressionData);
                });
            }
        };

        impressionDataAndroid.OnImpressionDataReady += (impressionData) =>
        {
            if (onImpressionDataReadyEvent != null)
            {
                onImpressionDataReadyEvent?.Invoke(impressionData);
            }
        };
    }

    //subscribe to IronSourceRewardedVideoAndroid RV Demand Only events and notify to subscribed events inside the app
    public void registerRewardedVideoDemandOnlyEvents()
    {

        rewardedVideoAndroid.OnRewardedVideoAdClosedDemandOnlyEvent += (instanceId) =>
        {
            if (onRewardedVideoAdClosedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdClosedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdLoadedDemandOnlyEvent += (instanceId) =>
        {
            if (onRewardedVideoAdLoadedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdLoadedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdOpenedDemandOnlyEvent += (instanceId) =>
        {
            if (onRewardedVideoAdOpenedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdClickedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdClickedDemandOnlyEvent += (instanceId) =>
        {
            if (onRewardedVideoAdClickedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdClickedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdRewardedDemandOnlyEvent += (instanceId) =>
        {
            if (onRewardedVideoAdRewardedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdRewardedDemandOnlyEvent?.Invoke(instanceId);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdLoadFailedDemandOnlyEvent += (instanceId, error) =>
        {
            if (onRewardedVideoAdLoadFailedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdLoadFailedDemandOnlyEvent?.Invoke(instanceId, error);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAdShowFailedDemandOnlyEvent += (instanceId, error) =>
        {
            if (onRewardedVideoAdShowFailedDemandOnlyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdShowFailedDemandOnlyEvent?.Invoke(instanceId, error);
                });
            }
        };
    }

    //subscribe to IronSourceRewardedVideoAndroid RV Mediation events and notify to subscribed events inside the app
    private void registerRewardedVideoEvents()
    {
        rewardedVideoAndroid.OnRewardedVideoAdClicked += (IronSourcePlacement) =>
        {
            if (onRewardedVideoAdClickedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdClickedEvent?.Invoke(IronSourcePlacement);
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdShowFailed += (IronSourceError) =>
        {
            if (onRewardedVideoAdShowFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdShowFailedEvent?.Invoke(IronSourceError);
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdOpened += () =>
        {
            if (onRewardedVideoAdOpenedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdOpenedEvent?.Invoke();
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdClosed += () =>
        {
            if (onRewardedVideoAdClosedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdClosedEvent?.Invoke();
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdStarted += () =>
        {
            if (onRewardedVideoAdStartedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdStartedEvent?.Invoke();
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdEnded += () =>
        {
            if (onRewardedVideoAdEndedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdEndedEvent?.Invoke();
                });
            }
        };
        rewardedVideoAndroid.OnRewardedVideoAdRewarded += (IronSourcePlacement) =>
        {
            if (onRewardedVideoAdRewardedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdRewardedEvent?.Invoke(IronSourcePlacement);
                });
            }
        };

        rewardedVideoAndroid.OnRewardedVideoAvailabilityChanged += (isAvailable) =>
        {
            if (onRewardedVideoAvailabilityChangedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAvailabilityChangedEvent?.Invoke(isAvailable);
                });
            }
        };
    }
    // ******************************* RewardedVideo Manual Load Events *******************************

    public void registerRewardedVideoManualEvents()
    {
        rewardedVideoAndroidManual.OnRewardedVideoAdReady += () =>
        {
            if (onRewardedVideoAdReadyEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdReadyEvent?.Invoke();
                });
            }
        };

        rewardedVideoAndroidManual.OnRewardedVideoAdLoadFailed += (IronSourceError) =>
        {
            if (onRewardedVideoAdLoadFailedEvent != null)
            {
                IronSourceEventsDispatcher.executeAction(() =>
                {
                    onRewardedVideoAdLoadFailedEvent?.Invoke(IronSourceError);
                });
            }
        };
    }



#endif

#if !UNITY_ANDROID

#if UNITY_IPHONE || UNITY_IOS


    [AOT.MonoPInvokeCallback(typeof(ISUnityBackgroundCallback))]
    public static void FireCallback(string args)
    {
        if (onImpressionDataReadyEvent != null)
        {
            InvokeEvent(onImpressionDataReadyEvent, args);
        }
    }
#endif

    // ******************************* Init Event *******************************

    private static event Action _onSdkInitializationCompletedEvent;

    public static event Action onSdkInitializationCompletedEvent
    {
        add
        {
            if (_onSdkInitializationCompletedEvent == null || !_onSdkInitializationCompletedEvent.GetInvocationList().Contains(value))
            {
                _onSdkInitializationCompletedEvent += value;
            }
        }

        remove
        {
            if (_onSdkInitializationCompletedEvent != null && _onSdkInitializationCompletedEvent.GetInvocationList().Contains(value))
            {
                _onSdkInitializationCompletedEvent -= value;
            }
        }
    }

    public void onSdkInitializationCompleted(string empty)
    {
        if (_onSdkInitializationCompletedEvent != null)
        {
            _onSdkInitializationCompletedEvent();
        }
    }

    // ******************************* Rewarded Video Events *******************************
    private static event Action<IronSourceError> _onRewardedVideoAdShowFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourceError> onRewardedVideoAdShowFailedEvent
    {
        add
        {
            if (_onRewardedVideoAdShowFailedEvent == null || !_onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdShowFailedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdShowFailedEvent != null && _onRewardedVideoAdShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdShowFailedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdShowFailed(string description)
    {
        if (_onRewardedVideoAdShowFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onRewardedVideoAdShowFailedEvent(sse);
        }
    }

    private static event Action _onRewardedVideoAdOpenedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdOpenedEvent
    {
        add
        {
            if (_onRewardedVideoAdOpenedEvent == null || !_onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdOpenedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdOpenedEvent != null && _onRewardedVideoAdOpenedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdOpenedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdOpened(string empty)
    {
        if (_onRewardedVideoAdOpenedEvent != null)
        {
            _onRewardedVideoAdOpenedEvent();
        }
    }

    private static event Action _onRewardedVideoAdClosedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdClosedEvent
    {
        add
        {
            if (_onRewardedVideoAdClosedEvent == null || !_onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClosedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdClosedEvent != null && _onRewardedVideoAdClosedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClosedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdClosed(string empty)
    {
        if (_onRewardedVideoAdClosedEvent != null)
        {
            _onRewardedVideoAdClosedEvent();
        }
    }

    private static event Action _onRewardedVideoAdStartedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdStartedEvent
    {
        add
        {
            if (_onRewardedVideoAdStartedEvent == null || !_onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdStartedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdStartedEvent != null && _onRewardedVideoAdStartedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdStartedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdStarted(string empty)
    {
        if (_onRewardedVideoAdStartedEvent != null)
        {
            _onRewardedVideoAdStartedEvent();
        }
    }

    private static event Action _onRewardedVideoAdEndedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdEndedEvent
    {
        add
        {
            if (_onRewardedVideoAdEndedEvent == null || !_onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdEndedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdEndedEvent != null && _onRewardedVideoAdEndedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdEndedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdEnded(string empty)
    {
        if (_onRewardedVideoAdEndedEvent != null)
        {
            _onRewardedVideoAdEndedEvent();
        }
    }

    private static event Action<IronSourcePlacement> _onRewardedVideoAdRewardedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourcePlacement> onRewardedVideoAdRewardedEvent
    {
        add
        {
            if (_onRewardedVideoAdRewardedEvent == null || !_onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdRewardedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdRewardedEvent != null && _onRewardedVideoAdRewardedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdRewardedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdRewarded(string description)
    {
        if (_onRewardedVideoAdRewardedEvent != null)
        {
            IronSourcePlacement ssp = getPlacementFromObject(description);
            _onRewardedVideoAdRewardedEvent(ssp);
        }
    }

    private static event Action<IronSourcePlacement> _onRewardedVideoAdClickedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourcePlacement> onRewardedVideoAdClickedEvent
    {
        add
        {
            if (_onRewardedVideoAdClickedEvent == null || !_onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClickedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdClickedEvent != null && _onRewardedVideoAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClickedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdClicked(string description)
    {
        if (_onRewardedVideoAdClickedEvent != null)
        {
            IronSourcePlacement ssp = getPlacementFromObject(description);
            _onRewardedVideoAdClickedEvent(ssp);
        }
    }

    private static event Action<bool> _onRewardedVideoAvailabilityChangedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<bool> onRewardedVideoAvailabilityChangedEvent
    {
        add
        {
            if (_onRewardedVideoAvailabilityChangedEvent == null || !_onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAvailabilityChangedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAvailabilityChangedEvent != null && _onRewardedVideoAvailabilityChangedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAvailabilityChangedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAvailabilityChanged(string stringAvailable)
    {
        bool isAvailable = (stringAvailable == "true") ? true : false;
        if (_onRewardedVideoAvailabilityChangedEvent != null)
            _onRewardedVideoAvailabilityChangedEvent(isAvailable);
    }

    // ******************************* RewardedVideo DemandOnly Events *******************************

    private static event Action<string> _onRewardedVideoAdLoadedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdLoadedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdLoadedDemandOnlyEvent == null || !_onRewardedVideoAdLoadedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdLoadedDemandOnlyEvent != null && _onRewardedVideoAdLoadedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdLoadedDemandOnly(string instanceId)
    {
        if (_onRewardedVideoAdLoadedDemandOnlyEvent != null)
        {
            _onRewardedVideoAdLoadedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string, IronSourceError> _onRewardedVideoAdLoadFailedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onRewardedVideoAdLoadFailedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdLoadFailedDemandOnlyEvent == null || !_onRewardedVideoAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadFailedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdLoadFailedDemandOnlyEvent != null && _onRewardedVideoAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadFailedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdLoadFailedDemandOnly(string args)
    {
        if (_onRewardedVideoAdLoadFailedDemandOnlyEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            _onRewardedVideoAdLoadFailedDemandOnlyEvent(instanceId, err);
        }
    }

    private static event Action<string> _onRewardedVideoAdOpenedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdOpenedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdOpenedDemandOnlyEvent == null || !_onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdOpenedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdOpenedDemandOnlyEvent != null && _onRewardedVideoAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdOpenedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdOpenedDemandOnly(string instanceId)
    {
        if (_onRewardedVideoAdOpenedDemandOnlyEvent != null)
        {
            _onRewardedVideoAdOpenedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string> _onRewardedVideoAdClosedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdClosedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdClosedDemandOnlyEvent == null || !_onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClosedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdClosedDemandOnlyEvent != null && _onRewardedVideoAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClosedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdClosedDemandOnly(string instanceId)
    {
        if (_onRewardedVideoAdClosedDemandOnlyEvent != null)
        {
            _onRewardedVideoAdClosedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string> _onRewardedVideoAdRewardedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdRewardedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdRewardedDemandOnlyEvent == null || !_onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdRewardedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdRewardedDemandOnlyEvent != null && _onRewardedVideoAdRewardedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdRewardedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdRewardedDemandOnly(string instanceId)
    {
        if (_onRewardedVideoAdRewardedDemandOnlyEvent != null)
        {
            _onRewardedVideoAdRewardedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string, IronSourceError> _onRewardedVideoAdShowFailedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onRewardedVideoAdShowFailedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdShowFailedDemandOnlyEvent == null || !_onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdShowFailedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdShowFailedDemandOnlyEvent != null && _onRewardedVideoAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdShowFailedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdShowFailedDemandOnly(string args)
    {
        if (_onRewardedVideoAdShowFailedDemandOnlyEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            _onRewardedVideoAdShowFailedDemandOnlyEvent(instanceId, err);
        }
    }

    private static event Action<string> _onRewardedVideoAdClickedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onRewardedVideoAdClickedDemandOnlyEvent
    {
        add
        {
            if (_onRewardedVideoAdClickedDemandOnlyEvent == null || !_onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClickedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdClickedDemandOnlyEvent != null && _onRewardedVideoAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdClickedDemandOnlyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdClickedDemandOnly(string instanceId)
    {
        if (_onRewardedVideoAdClickedDemandOnlyEvent != null)
        {
            _onRewardedVideoAdClickedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string> _onSegmentReceivedEvent;
    public static event Action<string> onSegmentReceivedEvent
    {
        add
        {
            if (_onSegmentReceivedEvent == null || !_onSegmentReceivedEvent.GetInvocationList().Contains(value))
            {
                _onSegmentReceivedEvent += value;
            }
        }

        remove
        {
            if (_onSegmentReceivedEvent != null && _onSegmentReceivedEvent.GetInvocationList().Contains(value))
            {
                _onSegmentReceivedEvent -= value;
            }
        }
    }

    public void onSegmentReceived(string segmentName)
    {
        if (_onSegmentReceivedEvent != null)
            _onSegmentReceivedEvent(segmentName);
    }

    // ******************************* Interstitial Events *******************************

    private static event Action _onInterstitialAdReadyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdReadyEvent
    {
        add
        {
            if (_onInterstitialAdReadyEvent == null || !_onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdReadyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdReadyEvent != null && _onInterstitialAdReadyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdReadyEvent -= value;
            }
        }
    }

    public void onInterstitialAdReady()
    {
        if (_onInterstitialAdReadyEvent != null)
            _onInterstitialAdReadyEvent();
    }

    private static event Action<IronSourceError> _onInterstitialAdLoadFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action<IronSourceError> onInterstitialAdLoadFailedEvent
    {
        add
        {
            if (_onInterstitialAdLoadFailedEvent == null || !_onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdLoadFailedEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdLoadFailedEvent != null && _onInterstitialAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdLoadFailedEvent -= value;
            }
        }
    }

    public void onInterstitialAdLoadFailed(string description)
    {
        if (_onInterstitialAdLoadFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onInterstitialAdLoadFailedEvent(sse);
        }
    }

    private static event Action _onInterstitialAdOpenedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdOpenedEvent
    {
        add
        {
            if (_onInterstitialAdOpenedEvent == null || !_onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdOpenedEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdOpenedEvent != null && _onInterstitialAdOpenedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdOpenedEvent -= value;
            }
        }
    }

    public void onInterstitialAdOpened(string empty)
    {
        if (_onInterstitialAdOpenedEvent != null)
        {
            _onInterstitialAdOpenedEvent();
        }
    }

    private static event Action _onInterstitialAdClosedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdClosedEvent
    {
        add
        {
            if (_onInterstitialAdClosedEvent == null || !_onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClosedEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdClosedEvent != null && _onInterstitialAdClosedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClosedEvent -= value;
            }
        }
    }

    public void onInterstitialAdClosed(string empty)
    {
        if (_onInterstitialAdClosedEvent != null)
        {
            _onInterstitialAdClosedEvent();
        }
    }

    private static event Action _onInterstitialAdShowSucceededEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdShowSucceededEvent
    {
        add
        {
            if (_onInterstitialAdShowSucceededEvent == null || !_onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowSucceededEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdShowSucceededEvent != null && _onInterstitialAdShowSucceededEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowSucceededEvent -= value;
            }
        }
    }

    public void onInterstitialAdShowSucceeded(string empty)
    {
        if (_onInterstitialAdShowSucceededEvent != null)
        {
            _onInterstitialAdShowSucceededEvent();
        }
    }

    private static event Action<IronSourceError> _onInterstitialAdShowFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action<IronSourceError> onInterstitialAdShowFailedEvent
    {
        add
        {
            if (_onInterstitialAdShowFailedEvent == null || !_onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowFailedEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdShowFailedEvent != null && _onInterstitialAdShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowFailedEvent -= value;
            }
        }
    }

    public void onInterstitialAdShowFailed(string description)
    {
        if (_onInterstitialAdShowFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onInterstitialAdShowFailedEvent(sse);
        }
    }

    private static event Action _onInterstitialAdClickedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceInterstitialEvents listener instead.", false)]
    public static event Action onInterstitialAdClickedEvent
    {
        add
        {
            if (_onInterstitialAdClickedEvent == null || !_onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClickedEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdClickedEvent != null && _onInterstitialAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClickedEvent -= value;
            }
        }
    }

    public void onInterstitialAdClicked(string empty)
    {
        if (_onInterstitialAdClickedEvent != null)
        {
            _onInterstitialAdClickedEvent();
        }
    }

    // ******************************* Interstitial DemanOnly Events *******************************

    private static event Action<string> _onInterstitialAdReadyDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdReadyDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdReadyDemandOnlyEvent == null || !_onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdReadyDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdReadyDemandOnlyEvent != null && _onInterstitialAdReadyDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdReadyDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdReadyDemandOnly(string instanceId)
    {
        if (_onInterstitialAdReadyDemandOnlyEvent != null)
            _onInterstitialAdReadyDemandOnlyEvent(instanceId);
    }


    private static event Action<string, IronSourceError> _onInterstitialAdLoadFailedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onInterstitialAdLoadFailedDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdLoadFailedDemandOnlyEvent == null || !_onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdLoadFailedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdLoadFailedDemandOnlyEvent != null && _onInterstitialAdLoadFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdLoadFailedDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdLoadFailedDemandOnly(string args)
    {
        if (_onInterstitialAdLoadFailedDemandOnlyEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            _onInterstitialAdLoadFailedDemandOnlyEvent(instanceId, err);
        }
    }

    private static event Action<string> _onInterstitialAdOpenedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdOpenedDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdOpenedDemandOnlyEvent == null || !_onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdOpenedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdOpenedDemandOnlyEvent != null && _onInterstitialAdOpenedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdOpenedDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdOpenedDemandOnly(string instanceId)
    {
        if (_onInterstitialAdOpenedDemandOnlyEvent != null)
        {
            _onInterstitialAdOpenedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string> _onInterstitialAdClosedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdClosedDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdClosedDemandOnlyEvent == null || !_onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClosedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdClosedDemandOnlyEvent != null && _onInterstitialAdClosedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClosedDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdClosedDemandOnly(string instanceId)
    {
        if (_onInterstitialAdClosedDemandOnlyEvent != null)
        {
            _onInterstitialAdClosedDemandOnlyEvent(instanceId);
        }
    }

    private static event Action<string, IronSourceError> _onInterstitialAdShowFailedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string, IronSourceError> onInterstitialAdShowFailedDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdShowFailedDemandOnlyEvent == null || !_onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowFailedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdShowFailedDemandOnlyEvent != null && _onInterstitialAdShowFailedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdShowFailedDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdShowFailedDemandOnly(string args)
    {
        if (_onInterstitialAdLoadFailedDemandOnlyEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError sse = getErrorFromErrorObject(argList[1]);
            string instanceId = argList[0].ToString();
            _onInterstitialAdShowFailedDemandOnlyEvent(instanceId, sse);
        }
    }

    private static event Action<string> _onInterstitialAdClickedDemandOnlyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public static event Action<string> onInterstitialAdClickedDemandOnlyEvent
    {
        add
        {
            if (_onInterstitialAdClickedDemandOnlyEvent == null || !_onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClickedDemandOnlyEvent += value;
            }
        }

        remove
        {
            if (_onInterstitialAdClickedDemandOnlyEvent != null && _onInterstitialAdClickedDemandOnlyEvent.GetInvocationList().Contains(value))
            {
                _onInterstitialAdClickedDemandOnlyEvent -= value;
            }
        }
    }

    public void onInterstitialAdClickedDemandOnly(string instanceId)
    {
        if (_onInterstitialAdClickedDemandOnlyEvent != null)
        {
            _onInterstitialAdClickedDemandOnlyEvent(instanceId);
        }
    }

    // ******************************* Offerwall Events ******************************* 

    private static event Action _onOfferwallOpenedEvent;

    public static event Action onOfferwallOpenedEvent
    {
        add
        {
            if (_onOfferwallOpenedEvent == null || !_onOfferwallOpenedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallOpenedEvent += value;
            }
        }

        remove
        {
            if (_onOfferwallOpenedEvent != null && _onOfferwallOpenedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallOpenedEvent -= value;
            }
        }
    }

    public void onOfferwallOpened(string empty)
    {
        if (_onOfferwallOpenedEvent != null)
        {
            _onOfferwallOpenedEvent();
        }
    }

    private static event Action<IronSourceError> _onOfferwallShowFailedEvent;

    public static event Action<IronSourceError> onOfferwallShowFailedEvent
    {
        add
        {
            if (_onOfferwallShowFailedEvent == null || !_onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallShowFailedEvent += value;
            }
        }

        remove
        {
            if (_onOfferwallShowFailedEvent != null && _onOfferwallShowFailedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallShowFailedEvent -= value;
            }
        }
    }

    public void onOfferwallShowFailed(string description)
    {
        if (_onOfferwallShowFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onOfferwallShowFailedEvent(sse);
        }
    }

    private static event Action _onOfferwallClosedEvent;

    public static event Action onOfferwallClosedEvent
    {
        add
        {
            if (_onOfferwallClosedEvent == null || !_onOfferwallClosedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallClosedEvent += value;
            }
        }

        remove
        {
            if (_onOfferwallClosedEvent != null && _onOfferwallClosedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallClosedEvent -= value;
            }
        }
    }

    public void onOfferwallClosed(string empty)
    {
        if (_onOfferwallClosedEvent != null)
        {
            _onOfferwallClosedEvent();
        }
    }

    private static event Action<IronSourceError> _onGetOfferwallCreditsFailedEvent;

    public static event Action<IronSourceError> onGetOfferwallCreditsFailedEvent
    {
        add
        {
            if (_onGetOfferwallCreditsFailedEvent == null || !_onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
            {
                _onGetOfferwallCreditsFailedEvent += value;
            }
        }

        remove
        {
            if (_onGetOfferwallCreditsFailedEvent != null && _onGetOfferwallCreditsFailedEvent.GetInvocationList().Contains(value))
            {
                _onGetOfferwallCreditsFailedEvent -= value;
            }
        }
    }

    public void onGetOfferwallCreditsFailed(string description)
    {
        if (_onGetOfferwallCreditsFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onGetOfferwallCreditsFailedEvent(sse);

        }
    }

    private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

    public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent
    {
        add
        {
            if (_onOfferwallAdCreditedEvent == null || !_onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallAdCreditedEvent += value;
            }
        }

        remove
        {
            if (_onOfferwallAdCreditedEvent != null && _onOfferwallAdCreditedEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallAdCreditedEvent -= value;
            }
        }
    }

    public void onOfferwallAdCredited(string json)
    {
        if (_onOfferwallAdCreditedEvent != null)
            _onOfferwallAdCreditedEvent(IronSourceJSON.Json.Deserialize(json) as Dictionary<string, object>);
    }

    private static event Action<bool> _onOfferwallAvailableEvent;

    public static event Action<bool> onOfferwallAvailableEvent
    {
        add
        {
            if (_onOfferwallAvailableEvent == null || !_onOfferwallAvailableEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallAvailableEvent += value;
            }
        }

        remove
        {
            if (_onOfferwallAvailableEvent != null && _onOfferwallAvailableEvent.GetInvocationList().Contains(value))
            {
                _onOfferwallAvailableEvent -= value;
            }
        }
    }

    public void onOfferwallAvailable(string stringAvailable)
    {
        bool isAvailable = (stringAvailable == "true") ? true : false;
        if (_onOfferwallAvailableEvent != null)
            _onOfferwallAvailableEvent(isAvailable);
    }

    // ******************************* Banner Events *******************************    
    private static event Action _onBannerAdLoadedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdLoadedEvent
    {
        add
        {
            if (_onBannerAdLoadedEvent == null || !_onBannerAdLoadedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLoadedEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdLoadedEvent != null && _onBannerAdLoadedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLoadedEvent -= value;
            }
        }
    }

    public void onBannerAdLoaded()
    {
        if (_onBannerAdLoadedEvent != null)
            _onBannerAdLoadedEvent();
    }

    private static event Action<IronSourceError> _onBannerAdLoadFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action<IronSourceError> onBannerAdLoadFailedEvent
    {
        add
        {
            if (_onBannerAdLoadFailedEvent == null || !_onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLoadFailedEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdLoadFailedEvent != null && _onBannerAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLoadFailedEvent -= value;
            }
        }
    }

    public void onBannerAdLoadFailed(string description)
    {
        if (_onBannerAdLoadFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onBannerAdLoadFailedEvent(sse);
        }

    }

    private static event Action _onBannerAdClickedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdClickedEvent
    {
        add
        {
            if (_onBannerAdClickedEvent == null || !_onBannerAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdClickedEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdClickedEvent != null && _onBannerAdClickedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdClickedEvent -= value;
            }
        }
    }

    public void onBannerAdClicked()
    {
        if (_onBannerAdClickedEvent != null)
            _onBannerAdClickedEvent();
    }

    private static event Action _onBannerAdScreenPresentedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdScreenPresentedEvent
    {
        add
        {
            if (_onBannerAdScreenPresentedEvent == null || !_onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdScreenPresentedEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdScreenPresentedEvent != null && _onBannerAdScreenPresentedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdScreenPresentedEvent -= value;
            }
        }
    }

    public void onBannerAdScreenPresented()
    {
        if (_onBannerAdScreenPresentedEvent != null)
            _onBannerAdScreenPresentedEvent();
    }

    private static event Action _onBannerAdScreenDismissedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdScreenDismissedEvent
    {
        add
        {
            if (_onBannerAdScreenDismissedEvent == null || !_onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdScreenDismissedEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdScreenDismissedEvent != null && _onBannerAdScreenDismissedEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdScreenDismissedEvent -= value;
            }
        }
    }

    public void onBannerAdScreenDismissed()
    {
        if (_onBannerAdScreenDismissedEvent != null)
            _onBannerAdScreenDismissedEvent();
    }

    private static event Action _onBannerAdLeftApplicationEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceBannerEvents listener instead.", false)]
    public static event Action onBannerAdLeftApplicationEvent
    {
        add
        {
            if (_onBannerAdLeftApplicationEvent == null || !_onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLeftApplicationEvent += value;
            }
        }

        remove
        {
            if (_onBannerAdLeftApplicationEvent != null && _onBannerAdLeftApplicationEvent.GetInvocationList().Contains(value))
            {
                _onBannerAdLeftApplicationEvent -= value;
            }
        }
    }

    public void onBannerAdLeftApplication()
    {
        if (_onBannerAdLeftApplicationEvent != null)
            _onBannerAdLeftApplicationEvent();
    }

    private static event Action<IronSourceImpressionData> _onImpressionSuccessEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use OnImpressionDataReady instead.")]
    public static event Action<IronSourceImpressionData> onImpressionSuccessEvent
    {
        add
        {
            if (_onImpressionSuccessEvent == null || !_onImpressionSuccessEvent.GetInvocationList().Contains(value))
            {
                _onImpressionSuccessEvent += value;
            }
        }

        remove
        {
            if (_onImpressionSuccessEvent != null && _onImpressionSuccessEvent.GetInvocationList().Contains(value))
            {
                _onImpressionSuccessEvent -= value;
            }
        }
    }

    public void onImpressionSuccess(string args)
    {
        IronSourceImpressionData impressionData = new IronSourceImpressionData(args);


        if (_onImpressionSuccessEvent != null)
        {
            _onImpressionSuccessEvent(impressionData);
        }
    }

    // ******************************* RewardedVideo Manual Load Events *******************************

    private static event Action<IronSourceError> _onRewardedVideoAdLoadFailedEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action<IronSourceError> onRewardedVideoAdLoadFailedEvent
    {
        add
        {
            if (_onRewardedVideoAdLoadFailedEvent == null || !_onRewardedVideoAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadFailedEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdLoadFailedEvent != null && _onRewardedVideoAdLoadFailedEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdLoadFailedEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdLoadFailed(string description)
    {
        if (_onRewardedVideoAdLoadFailedEvent != null)
        {
            IronSourceError sse = getErrorFromErrorObject(description);
            _onRewardedVideoAdLoadFailedEvent(sse);
        }
    }

    private static event Action _onRewardedVideoAdReadyEvent;

    [Obsolete("This API has been deprecated as of SDK 7.3.0. Please use the alternate API in IronSourceRewardedVideoEvents listener instead.", false)]
    public static event Action onRewardedVideoAdReadyEvent
    {
        add
        {
            if (_onRewardedVideoAdReadyEvent == null || !_onRewardedVideoAdReadyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdReadyEvent += value;
            }
        }

        remove
        {
            if (_onRewardedVideoAdReadyEvent != null && _onRewardedVideoAdReadyEvent.GetInvocationList().Contains(value))
            {
                _onRewardedVideoAdReadyEvent -= value;
            }
        }
    }

    public void onRewardedVideoAdReady(string empty)
    {
        if (_onRewardedVideoAdReadyEvent != null)
        {
            _onRewardedVideoAdReadyEvent();
        }
    }

#endif

    // ******************************* ConsentView Callbacks *******************************   

    //iOS callbacks only - in order to prevent using macro for iOS it's not only iOS
    private static event Action<string, IronSourceError> _onConsentViewDidFailToLoadWithErrorEvent;

    public static event Action<string, IronSourceError> onConsentViewDidFailToLoadWithErrorEvent
    {
        add
        {
            if (_onConsentViewDidFailToLoadWithErrorEvent == null || !_onConsentViewDidFailToLoadWithErrorEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidFailToLoadWithErrorEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidFailToLoadWithErrorEvent != null && _onConsentViewDidFailToLoadWithErrorEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidFailToLoadWithErrorEvent -= value;
            }
        }
    }

    public void onConsentViewDidFailToLoadWithError(string args)
    {
        if (_onConsentViewDidFailToLoadWithErrorEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = getErrorFromErrorObject(argList[1]);
            string consentViewType = argList[0].ToString();
            _onConsentViewDidFailToLoadWithErrorEvent(consentViewType, err);
        }
    }

    private static event Action<string, IronSourceError> _onConsentViewDidFailToShowWithErrorEvent;

    public static event Action<string, IronSourceError> onConsentViewDidFailToShowWithErrorEvent
    {
        add
        {
            if (_onConsentViewDidFailToShowWithErrorEvent == null || !_onConsentViewDidFailToShowWithErrorEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidFailToShowWithErrorEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidFailToShowWithErrorEvent != null && _onConsentViewDidFailToShowWithErrorEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidFailToShowWithErrorEvent -= value;
            }
        }
    }

    public void onConsentViewDidFailToShowWithError(string args)
    {
        if (_onConsentViewDidFailToShowWithErrorEvent != null && !String.IsNullOrEmpty(args))
        {
            List<object> argList = IronSourceJSON.Json.Deserialize(args) as List<object>;
            IronSourceError err = getErrorFromErrorObject(argList[1]);
            string consentViewType = argList[0].ToString();
            _onConsentViewDidFailToShowWithErrorEvent(consentViewType, err);
        }
    }

    private static event Action<string> _onConsentViewDidAcceptEvent;

    public static event Action<string> onConsentViewDidAcceptEvent
    {
        add
        {
            if (_onConsentViewDidAcceptEvent == null || !_onConsentViewDidAcceptEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidAcceptEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidAcceptEvent != null && _onConsentViewDidAcceptEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidAcceptEvent -= value;
            }
        }
    }

    public void onConsentViewDidAccept(string consentViewType)
    {
        if (_onConsentViewDidAcceptEvent != null)
        {
            _onConsentViewDidAcceptEvent(consentViewType);
        }
    }

    private static event Action<string> _onConsentViewDidDismissEvent;

    public static event Action<string> onConsentViewDidDismissEvent
    {
        add
        {
            if (_onConsentViewDidDismissEvent == null || !_onConsentViewDidDismissEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidDismissEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidDismissEvent != null && _onConsentViewDidDismissEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidDismissEvent -= value;
            }
        }
    }

    public void onConsentViewDidDismiss(string consentViewType)
    {
        if (_onConsentViewDidDismissEvent != null)
        {
            _onConsentViewDidDismissEvent(consentViewType);
        }
    }

    private static event Action<string> _onConsentViewDidLoadSuccessEvent;

    public static event Action<string> onConsentViewDidLoadSuccessEvent
    {
        add
        {
            if (_onConsentViewDidLoadSuccessEvent == null || !_onConsentViewDidLoadSuccessEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidLoadSuccessEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidLoadSuccessEvent != null && _onConsentViewDidLoadSuccessEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidLoadSuccessEvent -= value;
            }
        }
    }

    public void onConsentViewDidLoadSuccess(string consentViewType)
    {
        if (_onConsentViewDidLoadSuccessEvent != null)
        {
            _onConsentViewDidLoadSuccessEvent(consentViewType);
        }
    }

    private static event Action<string> _onConsentViewDidShowSuccessEvent;

    public static event Action<string> onConsentViewDidShowSuccessEvent
    {
        add
        {
            if (_onConsentViewDidShowSuccessEvent == null || !_onConsentViewDidShowSuccessEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidShowSuccessEvent += value;
            }
        }

        remove
        {
            if (_onConsentViewDidShowSuccessEvent != null && _onConsentViewDidShowSuccessEvent.GetInvocationList().Contains(value))
            {
                _onConsentViewDidShowSuccessEvent -= value;
            }
        }
    }

    public void onConsentViewDidShowSuccess(string consentViewType)
    {
        if (_onConsentViewDidShowSuccessEvent != null)
        {
            _onConsentViewDidShowSuccessEvent(consentViewType);
        }
    }



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
            int eCode = Convert.ToInt32(error[ERROR_CODE].ToString());
            string eDescription = error[ERROR_DESCRIPTION].ToString();
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




    // Invoke ImpressionDataReady Events

    private static void InvokeEvent(Action<IronSourceImpressionData> evt, String args)
    {
        IronSourceImpressionData impressionData = new IronSourceImpressionData(args);
        evt(impressionData);
    }
}
