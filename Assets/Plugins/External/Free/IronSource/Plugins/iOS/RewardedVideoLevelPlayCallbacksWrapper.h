//
//  RewardedVideoLevelPlayCallbacksWrapper.h
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/IronSource.h>

NS_ASSUME_NONNULL_BEGIN

@protocol RewardedVideoLevelPlayCallbacksWrapper <NSObject>

- (void)rewardedVideoLevelPlayDidLoadWithAdInfo:(ISAdInfo *)adInfo;
- (void)rewardedVideoLevelPlayDidFailToLoadWithError:(NSError *)error;
- (void)hasAvailableAdWithAdInfo:(ISAdInfo *)adInfo;
- (void)hasNoAvailableAd;
- (void)rewardedVideoLevelPlayDidReceiveRewardForPlacement:(ISPlacementInfo *)placementInfo withAdInfo:(ISAdInfo *)adInfo;
- (void)rewardedVideoLevelPlayDidFailToShowWithError:(NSError *)error andAdInfo:(ISAdInfo *)adInfo;
- (void)rewardedVideoLevelPlayDidOpenWithAdInfo:(ISAdInfo *)adInfo;
- (void)rewardedVideoLevelPlayDidCloseWithAdInfo:(ISAdInfo *)adInfo;
- (void)rewardedVideoLevelPlayDidClick:(ISPlacementInfo *)placementInfo withAdInfo:(ISAdInfo *)adInfo;

@end

@interface RewardedVideoLevelPlayCallbacksWrapper : NSObject <LevelPlayRewardedVideoManualDelegate,LevelPlayRewardedVideoDelegate>

@property (nonatomic, weak) id<RewardedVideoLevelPlayCallbacksWrapper>delegate;

- (instancetype) initWithDelegate:(id<RewardedVideoLevelPlayCallbacksWrapper>)delegate;

@end

NS_ASSUME_NONNULL_END
