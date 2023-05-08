//
//  InterstitialLevelPlayCallbacksWrapper.h
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/IronSource.h>

NS_ASSUME_NONNULL_BEGIN

@protocol InterstitialLevelPlayCallbacksWrapper <NSObject>

- (void)interstitialLevelPlayDidLoadWithAdInfo:(ISAdInfo *)adInfo;
- (void)interstitialLevelPlayDidFailToLoadWithError:(NSError *)error;
- (void)interstitialLevelPlayDidOpenWithAdInfo:(ISAdInfo *)adInfo;
- (void)interstitialLevelPlayDidCloseWithAdInfo:(ISAdInfo *)adInfo;
- (void)interstitialLevelPlayDidShowWithAdInfo:(ISAdInfo *)adInfo;
- (void)interstitialLevelPlayDidFailToShowWithError:(NSError *)error andAdInfo:(ISAdInfo *)adInfo;
- (void)interstitialLevelPlayDidClickWithAdInfo:(ISAdInfo *)adInfo;

@end

@interface InterstitialLevelPlayCallbacksWrapper : NSObject <LevelPlayInterstitialDelegate>

@property (nonatomic, weak) id<InterstitialLevelPlayCallbacksWrapper> delegate;

- (instancetype) initWithDelegate:(id<InterstitialLevelPlayCallbacksWrapper>)delegate;

@end

NS_ASSUME_NONNULL_END
