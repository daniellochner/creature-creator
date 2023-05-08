//
//  InterstitialLevelPlayCallbacksWrapper.m
//  iOSBridge
//
//  Created by Moshe Aviv Aslanov on 02/05/2022.
//  Copyright © 2022 yossi mozgerashvily. All rights reserved.
//

#import "InterstitialLevelPlayCallbacksWrapper.h"

@implementation InterstitialLevelPlayCallbacksWrapper

-(instancetype)initWithDelegate:(id<InterstitialLevelPlayCallbacksWrapper>)delegate {
    self = [super init];
    
    if (self) {
        _delegate = delegate;
    }
    
    return self;
}

- (void)didClickWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidClickWithAdInfo:adInfo];
}

- (void)didCloseWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidCloseWithAdInfo:adInfo];
}

- (void)didFailToLoadWithError:(NSError *)error {
    [_delegate interstitialLevelPlayDidFailToLoadWithError:error];
}

- (void)didFailToShowWithError:(NSError *)error andAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidFailToShowWithError:error andAdInfo:adInfo];
}

- (void)didLoadWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidLoadWithAdInfo:adInfo];
}

- (void)didOpenWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidOpenWithAdInfo:adInfo];
}

- (void)didShowWithAdInfo:(ISAdInfo *)adInfo {
    [_delegate interstitialLevelPlayDidShowWithAdInfo:adInfo];
}

@end
