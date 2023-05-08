using System;
public interface IUnityLevelPlayInterstitial
{
    //Mediation Interstitial callbacks
    event Action<IronSourceError, IronSourceAdInfo> OnAdShowFailed;

    event Action<IronSourceError> OnAdLoadFailed;

    event Action<IronSourceAdInfo> OnAdReady;

    event Action<IronSourceAdInfo> OnAdOpened;

    event Action<IronSourceAdInfo> OnAdClosed;

    event Action<IronSourceAdInfo> OnAdShowSucceeded;

    event Action<IronSourceAdInfo> OnAdClicked;
}
