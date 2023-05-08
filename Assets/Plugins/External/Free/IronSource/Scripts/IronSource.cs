using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IronSource : IronSourceIAgent
{
	private IronSourceIAgent _platformAgent;
	private static IronSource _instance;
	public static string UNITY_PLUGIN_VERSION = "7.3.0.1-r";
	private static bool isUnsupportedPlatform;

	private IronSource()
	{
		if (!isUnsupportedPlatform)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			_platformAgent = new UnsupportedPlatformAgent();
#elif (UNITY_IPHONE || UNITY_IOS)
			_platformAgent = new iOSAgent();
#elif UNITY_ANDROID
		_platformAgent = new AndroidAgent ();
#endif
		}

		else
		{
			_platformAgent = new UnsupportedPlatformAgent();
		}
		var ironSourceType = typeof(IronSourceEvents);
		var ironSourceRewardedType = typeof(IronSourceRewardedVideoEvents);
		var ironSourceInterstitialType = typeof(IronSourceInterstitialEvents);
		var ironSourceBannerType = typeof(IronSourceBannerEvents);
		var ironSourceEvents = new GameObject("IronSourceEvents", ironSourceType).GetComponent<IronSourceEvents>(); // Creates IronSourceEvents gameObject
		var ironSourceRewardedVideoEvents = new GameObject("IronSourceRewardedVideoEvents", ironSourceRewardedType).GetComponent<IronSourceRewardedVideoEvents>(); // Creates IronSourceRewardedVideoEvents gameObject
		var ironSourceInterstitialEvents = new GameObject("IronSourceInterstitialEvents", ironSourceInterstitialType).GetComponent<IronSourceInterstitialEvents>(); // Creates IronSourceInterstitialEvents gameObject
		var ironSourceBannerEvents = new GameObject("IronSourceBannerEvents", ironSourceBannerType).GetComponent<IronSourceBannerEvents>(); // Creates IronSourceBannerEvents gameObject
	}

	#region IronSourceIAgent implementation
	public static IronSource Agent
	{
		get
		{
			if (_instance == null)
			{
				_instance = new IronSource();
			}
			return _instance;
		}
	}

	public static string pluginVersion()
	{
		return UNITY_PLUGIN_VERSION;
	}

	public static string unityVersion()
	{
		return Application.unityVersion;
	}

	public static void setUnsupportedPlatform()
	{
		isUnsupportedPlatform = true;
	}

	//******************* Base API *******************//

	public void onApplicationPause(bool pause)
	{
		_platformAgent.onApplicationPause(pause);
	}

	public string getAdvertiserId()
	{
		return _platformAgent.getAdvertiserId();
	}

	public void validateIntegration()
	{
		_platformAgent.validateIntegration();
	}

	public void shouldTrackNetworkState(bool track)
	{
		_platformAgent.shouldTrackNetworkState(track);
	}

	public bool setDynamicUserId(string dynamicUserId)
	{
		return _platformAgent.setDynamicUserId(dynamicUserId);
	}

	public void setAdaptersDebug(bool enabled)
	{
		_platformAgent.setAdaptersDebug(enabled);
	}

	public void setMetaData(string key, string value)
	{
		_platformAgent.setMetaData(key, value);
	}

	public void setMetaData(string key, params string[] values)
	{
		_platformAgent.setMetaData(key, values);
	}

	public int? getConversionValue()
	{
		return _platformAgent.getConversionValue();
	}

	public void setManualLoadRewardedVideo(bool isOn)
	{
		_platformAgent.setManualLoadRewardedVideo(isOn);
	}

	public void setNetworkData(string networkKey, string networkData)
	{
		_platformAgent.setNetworkData(networkKey, networkData);
	}

	public void SetPauseGame(bool pause)
	{
        _platformAgent.SetPauseGame(pause);
	}

	//******************* SDK Init *******************//

	public void setUserId(string userId)
	{
		_platformAgent.setUserId(userId);
	}

	public void init(string appKey)
	{
		_platformAgent.init(appKey);
	}

	public void init(string appKey, params string[] adUnits)
	{
		_platformAgent.init(appKey, adUnits);
	}

    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public void initISDemandOnly(string appKey, params string[] adUnits)
	{
		_platformAgent.initISDemandOnly(appKey, adUnits);
	}

	//******************* RewardedVideo API *******************//

	public void loadRewardedVideo()
	{
		_platformAgent.loadRewardedVideo();
	}


	public void showRewardedVideo()
	{
		_platformAgent.showRewardedVideo();
	}

	public void showRewardedVideo(string placementName)
	{
		_platformAgent.showRewardedVideo(placementName);
	}

	public IronSourcePlacement getPlacementInfo(string placementName)
	{
		return _platformAgent.getPlacementInfo(placementName);
	}

	public bool isRewardedVideoAvailable()
	{
		return _platformAgent.isRewardedVideoAvailable();
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return _platformAgent.isRewardedVideoPlacementCapped(placementName);
	}

	public void setRewardedVideoServerParams(Dictionary<string, string> parameters)
	{
		_platformAgent.setRewardedVideoServerParams(parameters);
	}

	public void clearRewardedVideoServerParams()
	{
		_platformAgent.clearRewardedVideoServerParams();
	}

    //******************* RewardedVideo DemandOnly API *******************//
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public void showISDemandOnlyRewardedVideo(string instanceId)
	{
		_platformAgent.showISDemandOnlyRewardedVideo(instanceId);
	}
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public void loadISDemandOnlyRewardedVideo(string instanceId)
	{
		_platformAgent.loadISDemandOnlyRewardedVideo(instanceId);
	}
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public bool isISDemandOnlyRewardedVideoAvailable(string instanceId)
	{
		return _platformAgent.isISDemandOnlyRewardedVideoAvailable(instanceId);
	}

	//******************* Interstitial API *******************//

	public void loadInterstitial()
	{
		_platformAgent.loadInterstitial();
	}

	public void showInterstitial()
	{
		_platformAgent.showInterstitial();
	}

	public void showInterstitial(string placementName)
	{
		_platformAgent.showInterstitial(placementName);
	}

	public bool isInterstitialReady()
	{
		return _platformAgent.isInterstitialReady();
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return _platformAgent.isInterstitialPlacementCapped(placementName);
	}

    //******************* Interstitial DemandOnly API *******************//
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public void loadISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.loadISDemandOnlyInterstitial(instanceId);
	}
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public void showISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.showISDemandOnlyInterstitial(instanceId);
	}
    [Obsolete("This API has been deprecated as of SDK 7.3.0.1", false)]
    public bool isISDemandOnlyInterstitialReady(string instanceId)
	{
		return _platformAgent.isISDemandOnlyInterstitialReady(instanceId);
	}

	//******************* Offerwall API *******************//

	public void showOfferwall()
	{
		_platformAgent.showOfferwall();
	}

	public void showOfferwall(string placementName)
	{
		_platformAgent.showOfferwall(placementName);
	}

	public void getOfferwallCredits()
	{
		_platformAgent.getOfferwallCredits();
	}

	public bool isOfferwallAvailable()
	{
		return _platformAgent.isOfferwallAvailable();
	}

	//******************* Banner API *******************//

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position)
	{
		_platformAgent.loadBanner(size, position);
	}

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName)
	{
		_platformAgent.loadBanner(size, position, placementName);
	}

	public void destroyBanner()
	{
		_platformAgent.destroyBanner();
	}

	public void displayBanner()
	{
		_platformAgent.displayBanner();
	}

	public void hideBanner()
	{
		_platformAgent.hideBanner();
	}


	public bool isBannerPlacementCapped(string placementName)
	{
		return _platformAgent.isBannerPlacementCapped(placementName);

	}

	public void setSegment(IronSourceSegment segment)
	{
		_platformAgent.setSegment(segment);
	}

	public void setConsent(bool consent)
	{
		_platformAgent.setConsent(consent);
	}

	//******************* ConsentView API *******************//

	public void loadConsentViewWithType(string consentViewType)
	{
		_platformAgent.loadConsentViewWithType(consentViewType);
	}

	public void showConsentViewWithType(string consentViewType)
	{
		_platformAgent.showConsentViewWithType(consentViewType);
	}

	//******************* ILRD API *******************//

	public void setAdRevenueData(string dataSource, Dictionary<string, string> impressionData)
	{
		_platformAgent.setAdRevenueData(dataSource, impressionData);
	}

	//******************* TestSuite API *******************//

	public void launchTestSuite()
	{
		_platformAgent.launchTestSuite();
	}

	#endregion
}
