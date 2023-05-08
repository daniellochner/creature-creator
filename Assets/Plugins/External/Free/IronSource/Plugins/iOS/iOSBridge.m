//
//  iOSBridge.m
//  iOSBridge
//
//  Created by Supersonic.
//  Copyright (c) 2015 Supersonic. All rights reserved.
//

#import "iOSBridge.h"
#import <UIKit/UIKit.h>


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

#define BANNER_POSITION_TOP 1
#define BANNER_POSITION_BOTTOM 2

#ifdef __cplusplus
extern "C" {
#endif
    void UnityPause(int pause);
    extern void UnitySendMessage( const char *className, const char *methodName, const char *param );
    
#ifdef __cplusplus
}
#endif

@interface iOSBridge ()
{
    ISBannerView* _bannerView;
    NSInteger _position;
    UIViewController* _bannerViewController;
    BOOL _shouldHideBanner;
}

@property (nonatomic, strong) RewardedVideoLevelPlayCallbacksWrapper *rewardedVideoLevelPlayDelegate;
@property (nonatomic, strong) InterstitialLevelPlayCallbacksWrapper  *interstitialLevelPlayDelegate;
@property (nonatomic, strong) BannerLevelPlayCallbacksWrapper        *bannerLevelPlayDelegate;

@end

static NSString * const EMPTY_STRING = @"";

@implementation iOSBridge
static ISUnityBackgroundCallback backgroundCallback;
static bool pauseGame;


char *const IRONSOURCE_EVENTS = "IronSourceEvents";
char *const IRONSOURCE_REWARDED_VIDEO_EVENTS = "IronSourceRewardedVideoEvents";
char *const IRONSOURCE_INTERSTITIAL_EVENTS = "IronSourceInterstitialEvents";
char *const IRONSOURCE_BANNER_EVENTS = "IronSourceBannerEvents";

+ (iOSBridge *)start {
    static iOSBridge *instance;
    static dispatch_once_t onceToken;
    dispatch_once( &onceToken,
                  ^{
        instance = [iOSBridge new];
    });
    
    return instance;
}

- (instancetype)init {
    if(self = [super init]){
        self.rewardedVideoLevelPlayDelegate = [[RewardedVideoLevelPlayCallbacksWrapper alloc]initWithDelegate:(id)self];
        self.interstitialLevelPlayDelegate = [[InterstitialLevelPlayCallbacksWrapper alloc]initWithDelegate:(id)self];
        self.bannerLevelPlayDelegate = [[BannerLevelPlayCallbacksWrapper alloc]initWithDelegate:(id)self];
        
        [IronSource setRewardedVideoDelegate:self];
        [IronSource setInterstitialDelegate:self];
        [IronSource setISDemandOnlyInterstitialDelegate:self];
        [IronSource setISDemandOnlyRewardedVideoDelegate:self];
        [IronSource setOfferwallDelegate:self];
        [IronSource setBannerDelegate:self];
        [IronSource addImpressionDataDelegate:self];
        [IronSource setConsentViewWithDelegate:self];
        
        //set level play listeneres
        [IronSource setLevelPlayBannerDelegate:self.bannerLevelPlayDelegate];
        [IronSource setLevelPlayInterstitialDelegate:self.interstitialLevelPlayDelegate];
        [IronSource setLevelPlayRewardedVideoDelegate:self.rewardedVideoLevelPlayDelegate];
        
        
        _bannerView = nil;
        _bannerViewController = nil;
        _position = BANNER_POSITION_BOTTOM;
        _shouldHideBanner = NO;
        
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(orientationChanged:)
                                                     name:UIDeviceOrientationDidChangeNotification object:nil];
    }
    
    return self;
}

- (void)setPluginDataWithType:(NSString *)pluginType pluginVersion:(NSString *)version pluginFrameworkVersion:(NSString *)frameworkVersion {
    [ISConfigurations getConfigurations].plugin = pluginType;
    [ISConfigurations getConfigurations].pluginVersion = version;
    [ISConfigurations getConfigurations].pluginFrameworkVersion = frameworkVersion;
}

#pragma mark Base API

- (const char *)getAdvertiserId {
    NSString *advertiserId = [IronSource advertiserId];
    
    return MakeStringCopy(advertiserId);
}

- (void)validateIntegration {
    [ISIntegrationHelper validateIntegration];
}

- (void)shouldTrackNetworkState:(BOOL)flag {
    [IronSource shouldTrackReachability:flag];
}

- (BOOL)setDynamicUserId:(NSString *)dynamicUserId {
    return [IronSource setDynamicUserId:dynamicUserId];
}

- (void)setAdaptersDebug:(BOOL)enabled {
    [IronSource setAdaptersDebug:enabled];
}

- (void)setConsent:(BOOL)consent {
    [IronSource setConsent:consent];
}

- (void)setMetaDataWithKey:(NSString *)key value:(NSString *)value {
    [IronSource setMetaDataWithKey:key value:value];
}

- (void)setMetaDataWithKey:(NSString *)key values:(NSMutableArray *)valuesArray {
    [IronSource setMetaDataWithKey:key values:valuesArray];
}

- (void)setManualLoadRewardedVideo:(BOOL) isOn {
    if (isOn) {
        [IronSource setRewardedVideoManualDelegate:self];
        [IronSource setLevelPlayRewardedVideoManualDelegate:self.rewardedVideoLevelPlayDelegate];
    }
    else {
        NSLog(@"Manual load false is the defualt value");

    }
}

- (void)setNetworkData:(NSString *)networkKey data:(NSString *)networkData {
    NSError* error;
    if (!networkData) {
        return;
    }
    
    NSData *data = [networkData dataUsingEncoding:NSUTF8StringEncoding];
    if (!data) {
        return;
    }
    
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
    if (!dict) {
        return;
    }
    
    [IronSource setNetworkDataWithNetworkKey:networkKey andNetworkData:dict];
}

#pragma mark Init SDK

- (void)setUserId:(NSString *)userId {
    [IronSource setUserId:userId];
}

- (void)initWithAppKey:(NSString *)appKey {
    [IronSource initWithAppKey:appKey delegate:self];
}

- (void)initWithAppKey:(NSString *)appKey adUnits:(NSArray<NSString *> *)adUnits {
    [IronSource initWithAppKey:appKey adUnits:adUnits delegate:self];
}

- (void)initISDemandOnly:(NSString *)appKey adUnits:(NSArray<NSString *> *)adUnits {
    [IronSource initISDemandOnly:appKey adUnits:adUnits];
}

#pragma mark Rewarded Video API

- (void)showRewardedVideo {
    [IronSource showRewardedVideoWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}

- (void)showRewardedVideoWithPlacement:(NSString *)placementName {
    [IronSource showRewardedVideoWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController placement:placementName];
}

- (const char *) getPlacementInfo:(NSString *)placementName {
    char *res = nil;
    
    if (placementName){
        ISPlacementInfo *placementInfo = [IronSource rewardedVideoPlacementInfo:placementName];
        if(placementInfo){
            NSDictionary *dict = @{@"placement_name": [placementInfo placementName],
                                   @"reward_amount": [placementInfo rewardAmount],
                                   @"reward_name": [placementInfo rewardName]};
            
            res = MakeStringCopy([self getJsonFromObj:dict]);
        }
    }
    
    return res;
}

- (BOOL)isRewardedVideoAvailable {
    return [IronSource hasRewardedVideo];
}

- (BOOL)isRewardedVideoPlacementCapped:(NSString *)placementName {
    return [IronSource isRewardedVideoCappedForPlacement:placementName];
}

- (void)setRewardedVideoServerParameters:(NSDictionary *)params {
    [IronSource setRewardedVideoServerParameters:params];
}

- (void)clearRewardedVideoServerParameters {
    [IronSource clearRewardedVideoServerParameters];
}

#pragma mark Rewarded Video Manual Load API

- (void)loadRewardedVideo {
    [IronSource loadRewardedVideo];
}

#pragma mark Rewarded Video DemanOnly API

- (void)showISDemandOnlyRewardedVideo:(NSString *)instanceId {
    [IronSource showISDemandOnlyRewardedVideo:[UIApplication sharedApplication].keyWindow.rootViewController instanceId:instanceId];
}

- (void)loadISDemandOnlyRewardedVideo:(NSString *)instanceId {
    [IronSource loadISDemandOnlyRewardedVideo:instanceId];
}

- (BOOL)isDemandOnlyRewardedVideoAvailable:(NSString *)instanceId {
    return [IronSource hasISDemandOnlyRewardedVideo:instanceId];
}

#pragma mark Init Delegate

- (void)initializationDidComplete {
    UnitySendMessage(IRONSOURCE_EVENTS, "onSdkInitializationCompleted", "");
}

#pragma mark Rewarded Video Delegate

- (void)rewardedVideoHasChangedAvailability:(BOOL)available {
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAvailabilityChanged", (available) ? "true" : "false");
}

- (void)didReceiveRewardForPlacement:(ISPlacementInfo *)placementInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdRewarded", MakeStringCopy([self getJsonFromObj:dict]));
}

- (void)rewardedVideoDidFailToShowWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdShowFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdShowFailed","");
}

- (void)rewardedVideoDidOpen {
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdOpened", "");
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)rewardedVideoDidClose {
    if (pauseGame) {
        UnityPause(0);
    }
    [self centerBanner];
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdClosed", "");
}

- (void)rewardedVideoDidStart {
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdStarted", "");
}

- (void)rewardedVideoDidEnd {
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdEnded", "");
}

- (void)didClickRewardedVideo:(ISPlacementInfo *)placementInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdClicked", MakeStringCopy([self getJsonFromObj:dict]));
}

#pragma mark Rewarded Video Level Play Delegate

- (void)rewardedVideoLevelPlayDidClick:(nonnull ISPlacementInfo *)placementInfo withAdInfo:(nonnull ISAdInfo *)adInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    NSArray *params = @[dict, [self getAdInfoData:adInfo]];
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdClicked", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoLevelPlayDidCloseWithAdInfo:(nonnull ISAdInfo *)adInfo {
    if (pauseGame) {
        UnityPause(0);
    }
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdClosed",[self getAdInfoData:adInfo].UTF8String);
}

- (void)rewardedVideoLevelPlayDidFailToLoadWithError:(nonnull NSError *)error {
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdLoadFailed", MakeStringCopy([self parseErrorToEvent:error]));
}

- (void)rewardedVideoLevelPlayDidFailToShowWithError:(nonnull NSError *)error andAdInfo:(nonnull ISAdInfo *)adInfo {
    NSArray *params = @[(error) ? [self parseErrorToEvent:error] : @"" , [self getAdInfoData:adInfo]];
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdShowFailed", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoLevelPlayDidLoadWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdReady", [self getAdInfoData:adInfo].UTF8String);
}

- (void)rewardedVideoLevelPlayDidOpenWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdOpened", [self getAdInfoData:adInfo].UTF8String);
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)rewardedVideoLevelPlayDidReceiveRewardForPlacement:(nonnull ISPlacementInfo *)placementInfo withAdInfo:(nonnull ISAdInfo *)adInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    NSArray *params = @[dict, [self getAdInfoData:adInfo]];
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdRewarded", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)hasAvailableAdWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdAvailable", [self getAdInfoData:adInfo].UTF8String);
}

- (void)hasNoAvailableAd {
    UnitySendMessage(IRONSOURCE_REWARDED_VIDEO_EVENTS, "onAdUnavailable","");
    
}

#pragma mark Rewarded Video DemandOnly Delegate

- (void)rewardedVideoDidLoad:(NSString *)instanceId{
    NSArray *params = @[instanceId];
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdLoadedDemandOnly", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoDidFailToLoadWithError:(NSError *)error instanceId:(NSString *)instanceId{
    NSArray *params;
    if (error)
        params = @[instanceId, [self parseErrorToEvent:error]];
    else
        params = @[instanceId,@""];
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdLoadFailedDemandOnly", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoAdRewarded:(NSString *)instanceId {
    NSArray *params = @[instanceId];
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdRewardedDemandOnly", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoDidFailToShowWithError:(NSError *)error instanceId:(NSString *)instanceId {
    
    NSArray *params;
    if (error)
        params = @[instanceId, [self parseErrorToEvent:error]];
    else
        params = @[instanceId,@""];
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdShowFailedDemandOnly", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)rewardedVideoDidOpen:(NSString *)instanceId {
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdOpenedDemandOnly", MakeStringCopy(instanceId));
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)rewardedVideoDidClose:(NSString *)instanceId {
    if (pauseGame) {
        UnityPause(0);
    }
    [self centerBanner];
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdClosedDemandOnly", MakeStringCopy(instanceId));
}

- (void)rewardedVideoDidClick:(NSString *)instanceId {
    NSArray *params = @[instanceId];
    UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdClickedDemandOnly", MakeStringCopy([self getJsonFromObj:params]));
}

#pragma mark Interstitial API

- (void)loadInterstitial {
    [IronSource loadInterstitial];
}

- (void)showInterstitial {
    [IronSource showInterstitialWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}

- (void)showInterstitialWithPlacement:(NSString *)placementName {
    [IronSource showInterstitialWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController placement:placementName];
}

- (BOOL)isInterstitialReady {
    return [IronSource hasInterstitial];
}

- (BOOL)isInterstitialPlacementCapped:(NSString *)placementName {
    return [IronSource isInterstitialCappedForPlacement:placementName];
}

#pragma mark Interstitial DemandOnly API

- (void)loadISDemandOnlyInterstitial:(NSString *)instanceId {
    [IronSource loadISDemandOnlyInterstitial:instanceId];
}

- (void)showISDemandOnlyInterstitial:(NSString *)instanceId {
    [IronSource showISDemandOnlyInterstitial:[UIApplication sharedApplication].keyWindow.rootViewController instanceId:instanceId];
}

- (BOOL)isISDemandOnlyInterstitialReady:(NSString *)instanceId {
    return [IronSource hasISDemandOnlyInterstitial:instanceId];
}

#pragma mark Interstitial Delegate

- (void)interstitialDidLoad {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdReady", "");
}

- (void)interstitialDidFailToLoadWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdLoadFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdLoadFailed","");
}

- (void)interstitialDidOpen {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdOpened", "");
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)interstitialDidClose {
    if (pauseGame) {
        UnityPause(0);
    }
    [self centerBanner];
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdClosed", "");
}

- (void)interstitialDidShow {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdShowSucceeded", "");
}

- (void)interstitialDidFailToShowWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdShowFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdShowFailed","");
}

- (void)didClickInterstitial {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdClicked", "");
}

#pragma mark Interstitial Level Play Delegate

- (void)interstitialLevelPlayDidClickWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdClicked", [self getAdInfoData:adInfo].UTF8String);
}

- (void)interstitialLevelPlayDidCloseWithAdInfo:(nonnull ISAdInfo *)adInfo {
    if (pauseGame) {
        UnityPause(0);
    }
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdClosed", [self getAdInfoData:adInfo].UTF8String);
}

- (void)interstitialLevelPlayDidFailToLoadWithError:(nonnull NSError *)error {
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdLoadFailed", MakeStringCopy([self parseErrorToEvent:error]));
}

- (void)interstitialLevelPlayDidFailToShowWithError:(nonnull NSError *)error andAdInfo:(nonnull ISAdInfo *)adInfo {
    NSArray *params = @[(error) ? [self parseErrorToEvent:error] : @"" , [self getAdInfoData:adInfo]];
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdShowFailed", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)interstitialLevelPlayDidLoadWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdReady", [self getAdInfoData:adInfo].UTF8String);
}

- (void)interstitialLevelPlayDidOpenWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdOpened", [self getAdInfoData:adInfo].UTF8String);
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)interstitialLevelPlayDidShowWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_INTERSTITIAL_EVENTS, "onAdShowSucceeded", [self getAdInfoData:adInfo].UTF8String);
}

#pragma mark Interstitial DemandOnly Delegate

- (void)interstitialDidLoad:(NSString *)instanceId {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdReadyDemandOnly", MakeStringCopy(instanceId));
}

- (void)interstitialDidFailToLoadWithError:(NSError *)error instanceId:(NSString *)instanceId {
    NSArray *parameters;
    if (error)
        parameters = @[instanceId, [self parseErrorToEvent:error]];
    else
        parameters = @[instanceId, @""];
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdLoadFailedDemandOnly",MakeStringCopy([self getJsonFromObj:parameters]));
}

- (void)interstitialDidOpen:(NSString *)instanceId {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdOpenedDemandOnly", MakeStringCopy(instanceId));
    if (pauseGame) {
        UnityPause(1);
    }
}

- (void)interstitialDidClose:(NSString *)instanceId {
    if (pauseGame) {
        UnityPause(0);
    }
    [self centerBanner];
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdClosedDemandOnly", MakeStringCopy(instanceId));
}

- (void)interstitialDidShow:(NSString *)instanceId {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdShowSucceededDemandOnly", MakeStringCopy(instanceId));
}

- (void)interstitialDidFailToShowWithError:(NSError *)error instanceId:(NSString *)instanceId {
    NSArray *parameters;
    if (error)
        parameters = @[instanceId, [self parseErrorToEvent:error]];
    else
        parameters = @[instanceId, @""];
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdShowFailedDemandOnly", MakeStringCopy([self getJsonFromObj:parameters]));
}

- (void)didClickInterstitial:(NSString *)instanceId {
    UnitySendMessage(IRONSOURCE_EVENTS, "onInterstitialAdClickedDemandOnly", MakeStringCopy(instanceId));
}

#pragma mark Offerwall API

- (void)showOfferwall {
    [IronSource showOfferwallWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}

- (void)showOfferwallWithPlacement:(NSString *)placementName {
    [IronSource showOfferwallWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController placement:placementName];
}

- (void)getOfferwallCredits {
    [IronSource offerwallCredits];
}

- (BOOL)isOfferwallAvailable {
    return [IronSource hasOfferwall];
}

#pragma mark Offerwall Delegate

- (void)offerwallHasChangedAvailability:(BOOL)available {
    UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallAvailable", (available) ? "true" : "false");
}

- (void)offerwallDidShow {
    UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallOpened", "");
}

- (void)offerwallDidFailToShowWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallShowFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallShowFailed", "");
}

- (void)offerwallDidClose {
    [self centerBanner];
    UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallClosed", "");
}

- (BOOL)didReceiveOfferwallCredits:(NSDictionary *)creditInfo {
    if(creditInfo)
        UnitySendMessage(IRONSOURCE_EVENTS, "onOfferwallAdCredited", [self getJsonFromObj:creditInfo].UTF8String);
    
    return YES;
}

- (void)didFailToReceiveOfferwallCreditsWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onGetOfferwallCreditsFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onGetOfferwallCreditsFailed", "");
}

#pragma mark ConsentView API

-(void)loadConsentViewWithType:(NSString *)consentViewType {
    [IronSource loadConsentViewWithType: consentViewType];
}

-(void)showConsentViewWithType:(NSString *)consentViewType {
    @synchronized(self) {
        UIViewController* viewController = [UIApplication sharedApplication].keyWindow.rootViewController;
        [IronSource showConsentViewWithViewController:viewController andType:consentViewType];
    }
}

#pragma mark Banner API

- (void)loadBanner:(NSString *)description width:(NSInteger)width height:(NSInteger)height position:(NSInteger)position placement:(NSString *)placement adaptive:(bool) isAdaptive {
    @synchronized(self) {
        _position = position;
        ISBannerSize* size = [self getBannerSize:description width:width height:height];
        size.adaptive = isAdaptive;
        
        _bannerViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
        [IronSource loadBannerWithViewController:_bannerViewController size:size placement:placement];
    }
}

- (void)destroyBanner {
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            if (_bannerView != nil) {
                [IronSource destroyBanner:_bannerView];
                _bannerView = nil;
                _bannerViewController = nil;
                _shouldHideBanner = NO;
            }
        }
    });
}

- (void)displayBanner {
    _shouldHideBanner = NO;
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            if (_bannerView != nil) {
                [_bannerView setHidden:_shouldHideBanner];
            }
        }
    });
}

- (void)hideBanner {
    _shouldHideBanner = YES;
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            if (_bannerView != nil) {
                [_bannerView setHidden:_shouldHideBanner];
            }
        }
    });
}

- (BOOL)isBannerPlacementCapped:(NSString *)placementName {
    return [IronSource isBannerCappedForPlacement:placementName];
}

- (ISBannerSize *) getBannerSize:(NSString *)description width:(NSInteger)width height:(NSInteger)height {
    if ([description isEqualToString:@"CUSTOM"]) {
        return [[ISBannerSize alloc] initWithWidth:width andHeight:height];
    }
    if ([description isEqualToString:@"SMART"]) {
        return ISBannerSize_SMART;
    }
    if ([description isEqualToString:@"RECTANGLE"]) {
        return ISBannerSize_RECTANGLE;
    }
    if ([description isEqualToString:@"LARGE"]) {
        return ISBannerSize_LARGE;
    }
    else {
        return ISBannerSize_BANNER;
    }
}

#pragma mark Banner Delegate

- (CGPoint)getBannerCenter:(NSInteger)position rootView:(UIView *)rootView {
    CGFloat y;
    if (position == BANNER_POSITION_TOP) {
        y = (_bannerView.frame.size.height / 2);
        if (@available(ios 11.0, *)) {
            y += rootView.safeAreaInsets.top;
        }
    }
    else {
        y = rootView.frame.size.height - (_bannerView.frame.size.height / 2);
        if (@available(ios 11.0, *)) {
            y -= rootView.safeAreaInsets.bottom;
        }
    }
    
    return CGPointMake(rootView.frame.size.width / 2, y);
}

- (void)bannerDidLoad:(ISBannerView *)bannerView {
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            _bannerView = bannerView;
            [_bannerView setAccessibilityLabel:@"bannerContainer"];
            [_bannerView setHidden:_shouldHideBanner];
            
            _bannerView.center = [self getBannerCenter:_position rootView:_bannerViewController.view];
            [_bannerViewController.view addSubview:_bannerView];
            
            UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdLoaded", "");
        }
    });
}

- (void)bannerDidFailToLoadWithError:(NSError *)error {
    if (error)
        UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdLoadFailed", MakeStringCopy([self parseErrorToEvent:error]));
    else
        UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdLoadFailed", "");
}

- (void)didClickBanner {
    UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdClicked", "");
}

- (void)bannerWillPresentScreen {
    UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdScreenPresented", "");
}

- (void)bannerDidDismissScreen {
    UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdScreenDismissed", "");
}

- (void)bannerWillLeaveApplication {
    UnitySendMessage(IRONSOURCE_EVENTS, "onBannerAdLeftApplication", "");
}


- (void)centerBanner {
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            if (_bannerView != nil && _bannerViewController != nil) {
                _bannerView.center = [self getBannerCenter:_position rootView:_bannerViewController.view];
            }
        }
    });
}

- (void)orientationChanged:(NSNotification *)notification {
    [self centerBanner];
}

#pragma mark Banner Level Play Delegate


- (void)bannerLevelPlayDidClickWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdClicked", [self getAdInfoData:adInfo].UTF8String);
    
}

- (void)bannerLevelPlayDidDismissScreenWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdScreenDismissed", [self getAdInfoData:adInfo].UTF8String);
    
}

- (void)bannerLevelPlayDidFailToLoadWithError:(nonnull NSError *)error {
    UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdLoadFailed", (error) ? MakeStringCopy([self parseErrorToEvent:error]):"");
}

- (void)bannerLevelPlayDidLeaveApplicationWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdLeftApplication", [self getAdInfoData:adInfo].UTF8String);
}

- (void)bannerLevelPlayDidLoad:(nonnull ISBannerView *)bannerView withAdInfo:(nonnull ISAdInfo *)adInfo {
    dispatch_async(dispatch_get_main_queue(), ^{
        @synchronized(self) {
            _bannerView = bannerView;
            [_bannerView setAccessibilityLabel:@"bannerContainer"];
            [_bannerView setHidden:_shouldHideBanner];
            
            _bannerView.center = [self getBannerCenter:_position rootView:_bannerViewController.view];
            [_bannerViewController.view addSubview:_bannerView];
            
            UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdLoaded", [self getAdInfoData:adInfo].UTF8String);
        }
    });
}

- (void)bannerLevelPlayDidPresentScreenWithAdInfo:(nonnull ISAdInfo *)adInfo {
    UnitySendMessage(IRONSOURCE_BANNER_EVENTS, "onAdScreenPresented", [self getAdInfoData:adInfo].UTF8String);
}

#pragma mark Helper methods

- (void) setSegment:(NSString*) segmentJSON {
    [IronSource setSegmentDelegate:self];
    ISSegment *segment = [[ISSegment alloc] init];
    NSError* error;
    if (!segmentJSON)
        return;
    
    NSData *data = [segmentJSON dataUsingEncoding:NSUTF8StringEncoding];
    if (!data)
        return;
    
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
    
    if (!dict)
        return;
    
    NSMutableArray *allKeys = [[dict allKeys] mutableCopy];
    for (id key in allKeys)
    {
        NSString* keyString = (NSString*)key;
        NSString *object = [dict objectForKey: keyString];
        if ([keyString isEqualToString:@"age"]){
            segment.age = [object intValue] ;
        }
        else if([keyString isEqualToString:@"gender"]){
            if([object caseInsensitiveCompare:@"male"] == NSOrderedSame)
                segment.gender = IRONSOURCE_USER_MALE ;
            else if([object caseInsensitiveCompare:@"female"] == NSOrderedSame)
                segment.gender = IRONSOURCE_USER_FEMALE;
            
        }
        else if ([keyString isEqualToString:@"level"]){
            segment.level =  [object intValue];
        }
        else if ([keyString isEqualToString:@"isPaying"]){
            segment.paying = [object boolValue];
        }
        else if ([keyString isEqualToString:@"userCreationDate"]){
            NSDate *date = [NSDate dateWithTimeIntervalSince1970: [object longLongValue]/1000];
            segment.userCreationDate = date;
            
        }
        else if ([keyString isEqualToString:@"segmentName"]){
            segment.segmentName = object;
            
        } else if ([keyString isEqualToString:@"iapt"]){
            segment.iapTotal = [object doubleValue];
        }
        else{
            [segment setCustomValue:object forKey:keyString];
        }
        
    }
    
    [IronSource setSegment:segment];
}

- (void)didReceiveSegement:(NSString *)segment{
    UnitySendMessage(IRONSOURCE_EVENTS, "onSegmentReceived", MakeStringCopy(segment));
}

- (NSString *)parseErrorToEvent:(NSError *)error{
    if (error){
        NSString* codeStr =  [NSString stringWithFormat:@"%ld", (long)[error code]];
        
        NSDictionary *dict = @{@"error_description": [error localizedDescription],
                               @"error_code": codeStr};
        
        return [self getJsonFromObj:dict];
    }
    return nil;
}

- (NSString *)getJsonFromObj:(id)obj {
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:obj options:0 error:&error];
        if (!jsonData) {
            NSLog(@"Got an error: %@", error);
            return @"";
        } else {
            NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            return jsonString;
        }
}

- (NSString *) getAdInfoData:(ISAdInfo *) adinfo{
    if (adinfo!=nil) {
        NSString *adInfoString = [NSString stringWithFormat:@"%@", adinfo];
        return adInfoString;
    }
    return EMPTY_STRING;
}

#pragma mark ImpressionData Delegate

- (void)impressionDataDidSucceed:(ISImpressionData *)impressionData {
    if (backgroundCallback!=nil) {
             const char * serializedParameters = [self getJsonFromObj:[impressionData all_data]].UTF8String;
             backgroundCallback(serializedParameters);
         }
    UnitySendMessage(IRONSOURCE_EVENTS, "onImpressionSuccess", [self getJsonFromObj:[impressionData all_data]].UTF8String);
    
}

#pragma mark ConsentView Delegate

- (void)consentViewDidAccept:(NSString *)consentViewType {
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidAccept", MakeStringCopy(consentViewType));
}

- (void)consentViewDidDismiss:(NSString *)consentViewType {
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidDismiss", MakeStringCopy(consentViewType));
}

- (void)consentViewDidFailToLoadWithError:(NSError *)error consentViewType:(NSString *)consentViewType {
    NSArray *params;
    if (error)
        params = @[consentViewType, [self parseErrorToEvent:error]];
    else
        params = @[consentViewType, @""];
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidFailToLoadWithError", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)consentViewDidLoadSuccess:(NSString *)consentViewType {
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidLoadSuccess", MakeStringCopy(consentViewType));
}

- (void)consentViewDidFailToShowWithError:(NSError *)error consentViewType:(NSString *)consentViewType {
    NSArray *params;
    if (error)
        params = @[consentViewType, [self parseErrorToEvent:error]];
    else
        params = @[consentViewType, @""];
    
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidFailToShowWithError", MakeStringCopy([self getJsonFromObj:params]));
}

- (void)consentViewDidShowSuccess:(NSString *)consentViewType {
    UnitySendMessage(IRONSOURCE_EVENTS, "onConsentViewDidShowSuccess", MakeStringCopy(consentViewType));
}

#pragma mark ConversionValue API

-(const char *) getConversionValue {
    NSNumber *conversionValue = [IronSource getConversionValue];
    char *res = MakeStringCopy([conversionValue stringValue]);
    return res;
}

#pragma mark ILRD API
- (void)setAdRevenueData:(NSString *)dataSource impressionData:(NSData *)impressionData {
    [IronSource setAdRevenueDataWithDataSource:dataSource impressionData:impressionData];
}

#pragma mark TestSuite API
- (void)launchTestSuite {
    [IronSource launchTestSuite:[UIApplication sharedApplication].keyWindow.rootViewController];
}

#pragma mark - C Section

#ifdef __cplusplus
extern "C" {
#endif
    
    void RegisterCallback(ISUnityBackgroundCallback func){
            backgroundCallback=func;
        }
    void RegisterPauseGameFunction(bool func){
        pauseGame=func;
    }
    
    void CFSetPluginData(const char *pluginType, const char *pluginVersion, const char *pluginFrameworkVersion){
        [[iOSBridge start] setPluginDataWithType:GetStringParam(pluginType) pluginVersion:GetStringParam(pluginVersion) pluginFrameworkVersion:GetStringParam(pluginFrameworkVersion)];
    }
    
    const char *CFGetAdvertiserId(){
        return [[iOSBridge start] getAdvertiserId];
    }
    
    void CFValidateIntegration(){
        [[iOSBridge start] validateIntegration];
    }
    
    void CFShouldTrackNetworkState(bool flag){
        [[iOSBridge start] shouldTrackNetworkState:flag];
    }
    
    bool CFSetDynamicUserId(char *dynamicUserId){
        return [[iOSBridge start] setDynamicUserId:GetStringParam(dynamicUserId)];
    }
    
    void CFSetAdaptersDebug(bool enabled){
        [[iOSBridge start] setAdaptersDebug:enabled];
    }
    
    void CFSetUserId(char *userId){
        return [[iOSBridge start] setUserId:GetStringParam(userId)];
    }
    
    void CFSetConsent (bool consent) {
        [[iOSBridge start] setConsent:consent];
    }
    
    void CFSetMetaData (char *key, char *value) {
        [[iOSBridge start] setMetaDataWithKey:GetStringParam(key) value:GetStringParam(value)];
    }
    
    void CFSetMetaDataWithValues (char *key,const char *values[]) {
        NSMutableArray *valuesArray = [NSMutableArray new];
        if(values != nil ) {
            int i = 0;
            
            while (values[i] != nil) {
                [valuesArray addObject: [NSString stringWithCString: values[i] encoding:NSASCIIStringEncoding]];
                i++;
            }
            
            [[iOSBridge start] setMetaDataWithKey:GetStringParam(key) values:valuesArray];
        }
    }
    
    void CFSetManualLoadRewardedVideo(bool isOn) {
        [[iOSBridge start] setManualLoadRewardedVideo:isOn];
    }
    
    void CFSetNetworkData (char *networkKey, char *networkData) {
        [[iOSBridge start] setNetworkData:GetStringParam(networkKey) data:GetStringParam(networkData)];
    }
    
#pragma mark Init SDK
    
    void CFInit(const char *appKey){
        [[iOSBridge start] initWithAppKey:GetStringParam(appKey)];
    }
    
    void CFInitWithAdUnits(const char *appKey, const char *adUnits[]){
        NSMutableArray *adUnitsArray = [NSMutableArray new];
        
        if(adUnits != nil ) {
            int i = 0;
            
            while (adUnits[i] != nil) {
                [adUnitsArray addObject: [NSString stringWithCString: adUnits[i] encoding:NSASCIIStringEncoding]];
                i++;
            }
            
            [[iOSBridge start] initWithAppKey:GetStringParam(appKey) adUnits:adUnitsArray];
        }
    }
    
    void CFInitISDemandOnly(const char *appKey, const char *adUnits[]){
        NSMutableArray *adUnitsArray = [NSMutableArray new];
        
        if(adUnits != nil ) {
            int i = 0;
            
            while (adUnits[i] != nil) {
                [adUnitsArray addObject: [NSString stringWithCString: adUnits[i] encoding:NSASCIIStringEncoding]];
                i++;
            }
            [[iOSBridge start] initISDemandOnly:GetStringParam(appKey) adUnits:adUnitsArray];
        }
    }
    
#pragma mark RewardedVideo API
    
    void CFLoadRewardedVideo() {
        [[iOSBridge start] loadRewardedVideo];
    }
    
    void CFShowRewardedVideo(){
        [[iOSBridge start] showRewardedVideo];
    }
    
    void CFShowRewardedVideoWithPlacementName(char *placementName){
        [[iOSBridge start] showRewardedVideoWithPlacement:GetStringParam(placementName)];
    }
    
    const char *CFGetPlacementInfo(char *placementName){
        return [[iOSBridge start] getPlacementInfo:GetStringParam(placementName)];
    }
    
    bool CFIsRewardedVideoAvailable(){
        return [[iOSBridge start] isRewardedVideoAvailable];
    }
    
    bool CFIsRewardedVideoPlacementCapped(char *placementName){
        return [[iOSBridge start] isRewardedVideoPlacementCapped:GetStringParam(placementName)];
    }
    
    void CFSetRewardedVideoServerParameters(char *jsonString) {
        NSData *data = [GetStringParam(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        if (!data) {
            return;
        }
        
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
        if (dict) {
            [[iOSBridge start] setRewardedVideoServerParameters:dict];
        }
    }
    
    void CFClearRewardedVideoServerParameters() {
        [[iOSBridge start] clearRewardedVideoServerParameters];
    }
    
#pragma mark RewardedVideo DemandOnly API
    
    void CFShowISDemandOnlyRewardedVideo(char * instanceId){
        [[iOSBridge start] showISDemandOnlyRewardedVideo:GetStringParam(instanceId)];
    }
    
    void CFLoadISDemandOnlyRewardedVideo(char * instanceId) {
        [[iOSBridge start] loadISDemandOnlyRewardedVideo:GetStringParam(instanceId)];
    }
    
    bool CFIsDemandOnlyRewardedVideoAvailable(char * instanceId) {
        return [[iOSBridge start] isDemandOnlyRewardedVideoAvailable:GetStringParam(instanceId)];
    }
    
#pragma mark Interstitial API
    
    void CFLoadInterstitial(){
        [[iOSBridge start] loadInterstitial];
    }
    
    void CFShowInterstitial(){
        [[iOSBridge start] showInterstitial];
    }
    
    void CFShowInterstitialWithPlacementName(char *placementName){
        [[iOSBridge start] showInterstitialWithPlacement:GetStringParam(placementName)];
    }
    
    bool CFIsInterstitialReady(){
        return [[iOSBridge start] isInterstitialReady];
    }
    
    bool CFIsInterstitialPlacementCapped(char *placementName){
        return [[iOSBridge start] isInterstitialPlacementCapped:GetStringParam(placementName)];
    }
    
#pragma mark Interstitial DemandOnly API
    
    void CFLoadISDemandOnlyInterstitial(char * instanceId) {
        [[iOSBridge start] loadISDemandOnlyInterstitial:GetStringParam(instanceId)];
    }
    
    void CFShowISDemandOnlyInterstitial(char * instanceId) {
        [[iOSBridge start] showISDemandOnlyInterstitial:GetStringParam(instanceId)];
        
    }
    
    bool CFIsDemandOnlyInterstitialReady(char * instanceId) {
        return [[iOSBridge start] isISDemandOnlyInterstitialReady:GetStringParam(instanceId)];
    }
    
#pragma mark Offerwall API
    
    void CFShowOfferwall(){
        [[iOSBridge start] showOfferwall];
    }
    
    void CFShowOfferwallWithPlacementName(char *placementName){
        [[iOSBridge start] showOfferwallWithPlacement:GetStringParam(placementName)];
    }
    
    void CFGetOfferwallCredits(){
        [[iOSBridge start] getOfferwallCredits];
    }
    
    bool CFIsOfferwallAvailable(){
        return [[iOSBridge start] isOfferwallAvailable];
    }
    
#pragma mark Banner API
    
    void CFLoadBanner(char* description, int width, int height, int position, char* placementName, bool isAdaptive){
        [[iOSBridge start] loadBanner:GetStringParam(description) width:width height:height position:position placement:GetStringParam(placementName) adaptive:isAdaptive];
    }
    
    void CFDestroyBanner (){
        [[iOSBridge start] destroyBanner];
    }
    
    void CFDisplayBanner (){
        [[iOSBridge start] displayBanner];
    }
    
    void CFHideBanner (){
        [[iOSBridge start] hideBanner];
    }
    
    bool CFIsBannerPlacementCapped (char *placementName){
        return [[iOSBridge start] isBannerPlacementCapped:GetStringParam(placementName)];
    }
    
#pragma mark Segment API
    
    void CFSetSegment (char* jsonString) {
        [[iOSBridge start] setSegment:GetStringParam(jsonString)];
    }
    
#pragma mark ConsentView API
    
    void CFLoadConsentViewWithType (char* consentViewType){
        [[iOSBridge start] loadConsentViewWithType:GetStringParam(consentViewType)];
    }
    
    void CFShowConsentViewWithType (char* consentViewType){
        [[iOSBridge start] showConsentViewWithType:GetStringParam(consentViewType)];
    }
    
#pragma mark ConversionValue API
    
    const char *CFGetConversionValue(){
        return [[iOSBridge start] getConversionValue];
    }
    
#pragma mark ILRD API
    void  CFSetAdRevenueData(char* datasource,char* impressiondata){
        NSData *data=[GetStringParam(impressiondata)dataUsingEncoding:NSUTF8StringEncoding];
        if (!data) {
            return;
        }
        return [[iOSBridge start] setAdRevenueData:GetStringParam(datasource)impressionData:data];
    }

#pragma mark TestSuite API
    void CFLaunchTestSuite(){
        [[iOSBridge start] launchTestSuite];
    }

#pragma mark - ISRewardedVideoManualDelegate methods
    
    
    - (void)rewardedVideoDidLoad {
        UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdReady", "");
    }
    
    - (void)rewardedVideoDidFailToLoadWithError:(NSError *)error {
        if (error)
            UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdLoadFailed", MakeStringCopy([self parseErrorToEvent:error]));
        else
            UnitySendMessage(IRONSOURCE_EVENTS, "onRewardedVideoAdLoadFailed","");
        
    }
    
    
#ifdef __cplusplus
}
#endif

@end

