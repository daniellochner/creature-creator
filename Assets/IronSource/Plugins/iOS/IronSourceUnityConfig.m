//
//  iOSBridge
//
//  Created by Ori  on 5/13/15.
//

#import <Foundation/Foundation.h>
#import <IronSource/ISSupersonicAdsConfiguration.h>
#import <IronSource/ISConfigurations.h>

@interface IronSourceUnityConfig:NSObject
//IronSource
- (void) setClientSideCallbacks:(bool)useClientSideCallbacks;
- (void) setLanguage:(NSString *)language;
- (void) setRewardedVideoCustomParams:(NSString *)rvParams;
- (void) setOfferwallCustomParams:(NSString *)owParams;

@end

@implementation IronSourceUnityConfig

//IronSource
- (void) setClientSideCallbacks:(bool)useClientSideCallbacks {
    NSNumber *ucsc = @0;
    if (useClientSideCallbacks)
        ucsc = @1;
    
    [ISSupersonicAdsConfiguration configurations].useClientSideCallbacks = ucsc;
}

- (void) setLanguage:(NSString *)language {
    [ISSupersonicAdsConfiguration configurations].language = language;
}

- (void) setRewardedVideoCustomParams:(NSString *)rvParams {
    NSError *jsonError;
    NSData *objectData = [rvParams dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *json = [NSJSONSerialization JSONObjectWithData:objectData options:NSJSONReadingMutableContainers error:&jsonError];
    if (!jsonError)
        [ISConfigurations configurations].rewardedVideoCustomParameters = json;
}

- (void) setOfferwallCustomParams:(NSString *)owParams {
    NSError *jsonError;
    NSData *objectData = [owParams dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *json = [NSJSONSerialization JSONObjectWithData:objectData options:NSJSONReadingMutableContainers error:&jsonError];
    if (!jsonError)
        [ISConfigurations configurations].offerwallCustomParameters = json;
}



#ifdef __cplusplus
extern "C" {
#endif
    
#define ParseNSStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]
    
    //IronSource
    void CFSetClientSideCallbacks(bool useClientSideCallbacks){
        [[IronSourceUnityConfig new] setClientSideCallbacks:useClientSideCallbacks];
    }
    void CFSetLanguage(const char *language){
        [[IronSourceUnityConfig new] setLanguage:ParseNSStringParam(language)];
    }
    void CFSetRewardedVideoCustomParams(const char *rvParams){
        [[IronSourceUnityConfig new] setRewardedVideoCustomParams:ParseNSStringParam(rvParams)];
    }
    void CFSetOfferwallCustomParams(const char *owParam){
        [[IronSourceUnityConfig new] setOfferwallCustomParams:ParseNSStringParam(owParam)];
    }
    
    
#ifdef __cplusplus
}
#endif

@end
