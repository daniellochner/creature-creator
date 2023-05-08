using System;
public interface IUnityLevelPlayRewardedVideoManual
{
    event Action<IronSourceAdInfo> OnAdReady;

    event Action<IronSourceError> OnAdLoadFailed;
}
