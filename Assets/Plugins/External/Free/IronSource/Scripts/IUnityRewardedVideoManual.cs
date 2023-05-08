using System;

public interface IUnityRewardedVideoManual
{
    event Action OnRewardedVideoAdReady;

    event Action<IronSourceError> OnRewardedVideoAdLoadFailed;

}