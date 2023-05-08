using System;

public interface IUnityInterstitial
{
    //Mediation Interstitial callbacks
    event Action<IronSourceError> OnInterstitialAdShowFailed;

    event Action<IronSourceError> OnInterstitialAdLoadFailed;

    event Action OnInterstitialAdReady;

    event Action OnInterstitialAdOpened;

    event Action OnInterstitialAdClosed;

    event Action OnInterstitialAdShowSucceeded;

    event Action OnInterstitialAdClicked;

    //Rewarded Interstitial callback
    event Action OnInterstitialAdRewarded;

    //Demand Only Interstitial callbacks
    event Action<String> OnInterstitialAdReadyDemandOnly;

    event Action<String> OnInterstitialAdOpenedDemandOnly;

    event Action<String> OnInterstitialAdClosedDemandOnly;

    event Action<String, IronSourceError> OnInterstitialAdLoadFailedDemandOnly;

    event Action<String> OnInterstitialAdClickedDemandOnly;

    event Action<String, IronSourceError> OnInterstitialAdShowFailedDemandOnly;

}