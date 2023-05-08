//
//  BannerLevelPlayCallbacksWrapper.h
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/IronSource.h>

NS_ASSUME_NONNULL_BEGIN

@protocol BannerLevelPlayCallbacksWrapper <NSObject>

- (void)bannerLevelPlayDidLoad:(ISBannerView *)bannerView withAdInfo:(ISAdInfo *)adInfo;
- (void)bannerLevelPlayDidFailToLoadWithError:(NSError *)error;
- (void)bannerLevelPlayDidClickWithAdInfo:(ISAdInfo *)adInfo;
- (void)bannerLevelPlayDidPresentScreenWithAdInfo:(ISAdInfo *)adInfo;
- (void)bannerLevelPlayDidDismissScreenWithAdInfo:(ISAdInfo *)adInfo;
- (void)bannerLevelPlayDidLeaveApplicationWithAdInfo:(ISAdInfo *)adInfo;

@end

@interface BannerLevelPlayCallbacksWrapper : NSObject<LevelPlayBannerDelegate>

@property (nonatomic, weak) id<BannerLevelPlayCallbacksWrapper> delegate;

- (instancetype) initWithDelegate:(id<BannerLevelPlayCallbacksWrapper>)delegate;

@end

NS_ASSUME_NONNULL_END
