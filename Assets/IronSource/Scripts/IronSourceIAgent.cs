using System.Collections.Generic;

public interface IronSourceIAgent
{
	//******************* Base API *******************//

	void onApplicationPause(bool pause);

	string getAdvertiserId();

	void validateIntegration();

	void shouldTrackNetworkState(bool track);

	bool setDynamicUserId(string dynamicUserId);

	void setAdaptersDebug(bool enabled);

	void setMetaData(string key, string value);

	void setMetaData(string key, params string[] values);

	int? getConversionValue();

	void setManualLoadRewardedVideo(bool isOn);

	void setNetworkData(string networkKey, string networkData);

	void SetPauseGame(bool pause);

	//******************* SDK Init *******************//

	void setUserId(string userId);

	void init(string appKey);

	void init(string appKey, params string[] adUnits);

	void initISDemandOnly(string appKey, params string[] adUnits);

	//******************* RewardedVideo API *******************//

	void loadRewardedVideo();

	void showRewardedVideo();

	void showRewardedVideo(string placementName);

	bool isRewardedVideoAvailable();

	bool isRewardedVideoPlacementCapped(string placementName);

	IronSourcePlacement getPlacementInfo(string name);

	void setRewardedVideoServerParams(Dictionary<string, string> parameters);

	void clearRewardedVideoServerParams();

	//******************* RewardedVideo DemandOnly API *******************//

	void showISDemandOnlyRewardedVideo(string instanceId);

	void loadISDemandOnlyRewardedVideo(string instanceId);

	bool isISDemandOnlyRewardedVideoAvailable(string instanceId);

	//******************* Interstitial API *******************//

	void loadInterstitial();

	void showInterstitial();

	void showInterstitial(string placementName);

	bool isInterstitialReady();

	bool isInterstitialPlacementCapped(string placementName);

	//******************* Interstitial DemandOnly API *******************//

	void loadISDemandOnlyInterstitial(string instanceId);

	void showISDemandOnlyInterstitial(string instanceId);

	bool isISDemandOnlyInterstitialReady(string instanceId);

	//******************* Offerwall API *******************//

	void showOfferwall();

	void showOfferwall(string placementName);

	bool isOfferwallAvailable();

	void getOfferwallCredits();

	//******************* Banner API *******************//

	void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position);

	void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName);

	void destroyBanner();

	void displayBanner();

	void hideBanner();

	bool isBannerPlacementCapped(string placementName);

	void setSegment(IronSourceSegment segment);

	void setConsent(bool consent);

	//******************* ConsentView API *******************//

	void loadConsentViewWithType(string consentViewType);

	void showConsentViewWithType(string consentViewType);

	//******************* ILRD API *******************//

	void setAdRevenueData(string dataSource, Dictionary<string, string> impressionData);

	//******************* TestSuite API *******************//

	void launchTestSuite();
}

public static class dataSource
{
	public static string MOPUB { get { return "MoPub"; } }

}


public static class IronSourceAdUnits
{
	public static string REWARDED_VIDEO { get { return "rewardedvideo"; } }

	public static string INTERSTITIAL { get { return "interstitial"; } }

	public static string OFFERWALL { get { return "offerwall"; } }

	public static string BANNER { get { return "banner"; } }
}

public class IronSourceBannerSize
{
	private int width;
	private int height;
	private string description;
	private bool isAdaptive;

	public static IronSourceBannerSize BANNER = new IronSourceBannerSize("BANNER");
	public static IronSourceBannerSize LARGE = new IronSourceBannerSize("LARGE");
	public static IronSourceBannerSize RECTANGLE = new IronSourceBannerSize("RECTANGLE");
	public static IronSourceBannerSize SMART = new IronSourceBannerSize("SMART");

	private IronSourceBannerSize()
	{

	}

	public IronSourceBannerSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		this.description = "CUSTOM";
	}

	public IronSourceBannerSize(string description)
	{
		this.description = description;
		this.width = 0;
		this.height = 0;
	}

	public void SetAdaptive(bool adaptive)
	{
		this.isAdaptive = adaptive;
	}

	public bool IsAdaptiveEnabled()
	{
		return this.isAdaptive;
	}

	public string Description { get { return description; } }
	public int Width { get { return width; } }
	public int Height { get { return height; } }
}

public enum IronSourceBannerPosition
{
	TOP = 1,
	BOTTOM = 2
};
