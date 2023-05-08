//
//  RewardedVideoLevelPlayCallbacksWrapper.m
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import "RewardedVideoLevelPlayCallbacksWrapper.h"

@implementation RewardedVideoLevelPlayCallbacksWrapper
-(instancetype)initWithDelegate:(id<RewardedVideoLevelPlayCallbacksWrapper>)delegate {
    self = [super init];
    
    if (self) {
        _delegate = delegate;
    }
    
    return self;
}


- (void)didClick:(ISPlacementInfo *)placementInfo withAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidClick:placementInfo withAdInfo:adInfo];
}

- (void)didCloseWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidCloseWithAdInfo:adInfo];
}

- (void)didFailToShowWithError:(NSError *)error andAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidFailToShowWithError:error andAdInfo:adInfo];
}

- (void)didOpenWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidOpenWithAdInfo:adInfo];
}

- (void)didReceiveRewardForPlacement:(ISPlacementInfo *)placementInfo withAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidReceiveRewardForPlacement:placementInfo withAdInfo:adInfo];
}

- (void)hasAvailableAdWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate hasAvailableAdWithAdInfo:adInfo];
}

- (void)hasNoAvailableAd {
    [_delegate hasNoAvailableAd];
}

- (void)didFailToLoadWithError:(NSError *)error {
    [_delegate rewardedVideoLevelPlayDidFailToLoadWithError:error];
}

- (void)didLoadWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate rewardedVideoLevelPlayDidLoadWithAdInfo:adInfo];
}

@end
