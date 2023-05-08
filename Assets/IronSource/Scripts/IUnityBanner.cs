using System;
    public interface IUnityBanner
    {

    event Action OnBannerAdLoaded;

    event Action OnBannerAdLeftApplication;

    event Action OnBannerAdScreenDismissed;

    event Action OnBannerAdScreenPresented;

    event Action OnBannerAdClicked;

    event Action<IronSourceError> OnBannerAdLoadFailed;

}
