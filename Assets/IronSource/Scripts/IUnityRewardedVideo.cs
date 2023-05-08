using System;

public interface IUnityRewardedVideo
{
    //Rewarded Video mediation callbacks
    event Action<IronSourceError> OnRewardedVideoAdShowFailed;

    event Action OnRewardedVideoAdOpened;

    event Action OnRewardedVideoAdClosed;

    event Action OnRewardedVideoAdStarted;

    event Action OnRewardedVideoAdEnded;

    event Action<IronSourcePlacement> OnRewardedVideoAdRewarded;

    event Action<IronSourcePlacement> OnRewardedVideoAdClicked;

    event Action<bool> OnRewardedVideoAvailabilityChanged;

    //Rewarded Video Demand Only callbacks

    event Action<String> OnRewardedVideoAdOpenedDemandOnlyEvent;

    event Action<String> OnRewardedVideoAdClosedDemandOnlyEvent;

    event Action<String> OnRewardedVideoAdLoadedDemandOnlyEvent;

    event Action<String> OnRewardedVideoAdRewardedDemandOnlyEvent;

    event Action<String, IronSourceError> OnRewardedVideoAdShowFailedDemandOnlyEvent;

    event Action<String> OnRewardedVideoAdClickedDemandOnlyEvent;

    event Action<String, IronSourceError> OnRewardedVideoAdLoadFailedDemandOnlyEvent;

}