//
//  BannerLevelPlayCallbacksWrapper.m
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import "BannerLevelPlayCallbacksWrapper.h"

@implementation BannerLevelPlayCallbacksWrapper

-(instancetype)initWithDelegate:(id<BannerLevelPlayCallbacksWrapper>)delegate {
    self = [super init];
    
    if (self) {
        _delegate = delegate;
    }
    
    return self;
}



- (void)didClickWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate bannerLevelPlayDidClickWithAdInfo:adInfo];
}

- (void)didDismissScreenWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate bannerLevelPlayDidDismissScreenWithAdInfo:adInfo];
}

- (void)didFailToLoadWithError:(NSError *)error {
    [_delegate bannerLevelPlayDidFailToLoadWithError:error];
}

- (void)didLeaveApplicationWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate bannerLevelPlayDidLeaveApplicationWithAdInfo:adInfo];
}

- (void)didLoad:(ISBannerView *)bannerView withAdInfo:(ISAdInfo *)adInfo {
    [_delegate bannerLevelPlayDidLoad:bannerView withAdInfo:adInfo];
}

- (void)didPresentScreenWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate bannerLevelPlayDidPresentScreenWithAdInfo:adInfo];
}

@end
