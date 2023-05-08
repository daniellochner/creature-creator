using System;
public interface IUnityLevelPlayRewardedVideo
{
    event Action<IronSourceError, IronSourceAdInfo> OnAdShowFailed;

    event Action<IronSourceAdInfo> OnAdOpened;

    event Action<IronSourceAdInfo> OnAdClosed;

    event Action<IronSourcePlacement, IronSourceAdInfo> OnAdRewarded;

    event Action<IronSourcePlacement, IronSourceAdInfo> OnAdClicked;

    event Action<IronSourceAdInfo> OnAdAvailable;

    event Action OnAdUnavailable;
}
