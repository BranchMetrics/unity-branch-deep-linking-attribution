#include "BranchiOSWrapper.h"
#include "Branch.h"
#include "BranchConstants.h"
#include "BranchUniversalObject.h"
#import "UnityAppController.h"


static NSString *_branchKey = @"";
static BranchUnityWrapper *_wrapper = [BranchUnityWrapper sharedInstance];


#pragma mark - Private notification class implementation

@implementation BranchUnityWrapper

+ (BranchUnityWrapper *)sharedInstance
{
    return _wrapper;
}

+ (void)initialize {
    if(!_wrapper) {
        _wrapper = [[BranchUnityWrapper alloc] init];
    }
}

- (id)init {
    if (self = [super init]) {
        UnityRegisterAppDelegateListener(self);
    }
    
    return self;
}

- (void)dealloc {
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

- (void)didFinishLaunching:(NSNotification *)notification {
    self.launchOptions = notification.userInfo;
}

- (void)onOpenURL:(NSNotification *)notification {
    NSURL *openURL = notification.userInfo[@"url"];
    [[Branch getInstance:_branchKey] handleDeepLink:openURL];
}

- (BOOL)continueUserActivity:(NSUserActivity *)userActivity {
    return [[Branch getInstance:_branchKey] continueUserActivity:userActivity];
}

@end


#pragma mark - Converter methods

static NSString *CreateNSString(const char *string) {
    if (string == NULL) {
        return nil;
    }

    return [NSString stringWithUTF8String:string];
}

static NSURL *CreateNSUrl(const char *string) {
    return [NSURL URLWithString:CreateNSString(string)];
}

static NSDate *CreateNSDate(char *strDate) {
    NSString *str = CreateNSString(strDate);
    NSDateFormatter *formatter= [[NSDateFormatter alloc] init];
    formatter.dateFormat = @"yyyy-MM-ddTHH:mm:ssZ";

    return [formatter dateFromString:str];
}

static NSDate *CreateNSDate(NSString *strDate) {
    NSDateFormatter *formatter= [[NSDateFormatter alloc] init];
    formatter.dateFormat = @"yyyy-MM-ddTHH:mm:ssZ";
    
    return [formatter dateFromString:strDate];
}

static NSString *CreateNSStringFromNSDate(NSDate *date) {
    NSDateFormatter *formatter= [[NSDateFormatter alloc] init];
    formatter.dateFormat = @"yyyy-MM-ddTHH:mm:ssZ";
    
    return [formatter stringFromDate:date];
}

static NSDictionary *dictionaryFromJsonString(char *jsonString) {
    NSData *jsonData = [[NSData alloc] initWithBytes:jsonString length:strlen(jsonString)];
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return dictionary;
}

static NSArray *arrayFromJsonString(char *jsonString) {
    NSData *jsonData = [[NSData alloc] initWithBytes:jsonString length:strlen(jsonString)];
    NSArray *array = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return array;
}

static const char *jsonCStringFromDictionary(NSDictionary *dictionary) {
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:kNilOptions error:nil];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

    return [jsonString cStringUsingEncoding:NSUTF8StringEncoding];
}

static NSDictionary *dictFromBranchUniversalObject(BranchUniversalObject *universalObject) {
    NSDictionary *universalObjectDict = [NSDictionary dictionary];
    
    if (universalObject) {
        universalObjectDict = @{
            BRANCH_LINK_DATA_KEY_CANONICAL_IDENTIFIER: universalObject.canonicalIdentifier ? universalObject.canonicalIdentifier : @"",
            BRANCH_LINK_DATA_KEY_OG_TITLE: universalObject.title ? universalObject.title : @"",
            BRANCH_LINK_DATA_KEY_OG_DESCRIPTION: universalObject.contentDescription ? universalObject.contentDescription : @"",
            BRANCH_LINK_DATA_KEY_OG_IMAGE_URL: universalObject.imageUrl ? universalObject.imageUrl : @"",
            BRANCH_LINK_DATA_KEY_CONTENT_TYPE: universalObject.type ? universalObject.type: @"",
            BRANCH_LINK_DATA_KEY_PUBLICLY_INDEXABLE: universalObject.contentIndexMode ? [[NSNumber numberWithInteger:universalObject.contentIndexMode] stringValue]: @"",
            BRANCH_LINK_DATA_KEY_KEYWORDS: universalObject.keywords ? universalObject.keywords : @"",
            BRANCH_LINK_DATA_KEY_CONTENT_EXPIRATION_DATE: universalObject.expirationDate ? @(1000 * [universalObject.expirationDate timeIntervalSince1970]) : @"",
            @"metadata": universalObject.metadata ? universalObject.metadata : @"",
        };
    }

    return universalObjectDict;
}

static NSDictionary *dictFromBranchLinkProperties(BranchLinkProperties *linkProperties) {
    NSDictionary *linkPropertiesDict = [NSDictionary dictionary];
    
    if (linkProperties) {
        linkPropertiesDict = @{
            @"~tags": linkProperties.tags ? linkProperties.tags : @"",
            @"~feature": linkProperties.feature ? linkProperties.feature : @"",
            @"~alias": linkProperties.alias ? linkProperties.alias : @"",
            @"~channel": linkProperties.channel ? linkProperties.channel : @"",
            @"~stage": linkProperties.stage ? linkProperties.stage : @"",
            @"~duration": linkProperties.matchDuration ? [[NSNumber numberWithInteger:linkProperties.matchDuration] stringValue] : @"",
            @"control_params": linkProperties.controlParams ? linkProperties.controlParams : @""
        };
    }
    
    return linkPropertiesDict;
}

static BranchUniversalObject* branchuniversalObjectFormDict(NSDictionary *universalObjectDict) {
    BranchUniversalObject *universalObject = [[BranchUniversalObject alloc] init];
    
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_CANONICAL_IDENTIFIER]) {
        universalObject.canonicalIdentifier = universalObjectDict[BRANCH_LINK_DATA_KEY_CANONICAL_IDENTIFIER];
    }
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_OG_TITLE]) {
        universalObject.title = universalObjectDict[BRANCH_LINK_DATA_KEY_OG_TITLE];
    }
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_OG_DESCRIPTION]) {
        universalObject.contentDescription = universalObjectDict[BRANCH_LINK_DATA_KEY_OG_DESCRIPTION];
    }
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_OG_IMAGE_URL]) {
        universalObject.imageUrl = universalObjectDict[BRANCH_LINK_DATA_KEY_OG_IMAGE_URL];
    }
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_CONTENT_TYPE]) {
        universalObject.type = universalObjectDict[BRANCH_LINK_DATA_KEY_CONTENT_TYPE];
    }
    
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_PUBLICLY_INDEXABLE]) {
        if (universalObjectDict[BRANCH_LINK_DATA_KEY_PUBLICLY_INDEXABLE] == 0) {
            universalObject.contentIndexMode = ContentIndexModePublic;
        }
        else {
            universalObject.contentIndexMode = ContentIndexModePrivate;
        }
    }
    
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_CONTENT_EXPIRATION_DATE]) {
        universalObject.expirationDate = [NSDate dateWithTimeIntervalSince1970:[universalObjectDict[BRANCH_LINK_DATA_KEY_CONTENT_EXPIRATION_DATE] integerValue]/1000];
    }
    
    if (universalObjectDict[BRANCH_LINK_DATA_KEY_KEYWORDS]) {
        universalObject.keywords = [universalObjectDict[BRANCH_LINK_DATA_KEY_KEYWORDS] copy];
    }
    
    if (universalObjectDict[@"$metadata"]) {
        universalObject.metadata = [universalObjectDict[@"$metadata"] copy];
    }
    
    return universalObject;
}

static BranchLinkProperties *branchLinkPropertiesFormDict(NSDictionary *linkPropertiesDict) {
    BranchLinkProperties *linkProperties = [[BranchLinkProperties alloc] init];
    
    if (linkPropertiesDict[@"~tags"]) {
        linkProperties.tags = linkPropertiesDict[@"~tags"];
    }
    if (linkPropertiesDict[@"~feature"]) {
        linkProperties.feature = linkPropertiesDict[@"~feature"];
    }
    if (linkPropertiesDict[@"~alias"]) {
        linkProperties.alias = linkPropertiesDict[@"~alias"];
    }
    if (linkPropertiesDict[@"~channel"]) {
        linkProperties.channel = linkPropertiesDict[@"~channel"];
    }
    if (linkPropertiesDict[@"~stage"]) {
        linkProperties.stage = linkPropertiesDict[@"~stage"];
    }
    if (linkPropertiesDict[@"~duration"]) {
        linkProperties.matchDuration = [linkPropertiesDict[@"~duration"] intValue];
    }
    if (linkPropertiesDict[@"control_params"]) {
        linkProperties.controlParams = [linkPropertiesDict[@"control_params"] copy];
    }
    
    return linkProperties;
}

static callbackWithParams callbackWithParamsForCallbackId(char *callbackId) {
    NSString *callbackString = CreateNSString(callbackId);

    return ^(NSDictionary *params, NSError *error) {
        id errorDictItem = error ? [error description] : [NSNull null];
        id paramsDictItem = params ?: [NSNull null];
        NSDictionary *callbackDict = @{ @"callbackId": callbackString, @"params": paramsDictItem, @"error": errorDictItem };
        
        UnitySendMessage("Branch", "_asyncCallbackWithParams", jsonCStringFromDictionary(callbackDict));
    };
}

static callbackWithStatus callbackWithStatusForCallbackId(char *callbackId) {
    NSString *callbackString = CreateNSString(callbackId);

    return ^(BOOL status, NSError *error) {
        id errorDictItem = error ? [error description] : [NSNull null];
        NSDictionary *callbackDict = @{ @"callbackId": callbackString, @"status": @(status), @"error": errorDictItem };
        
        UnitySendMessage("Branch", "_asyncCallbackWithStatus", jsonCStringFromDictionary(callbackDict));
    };
}

static callbackWithList callbackWithListForCallbackId(char *callbackId) {
    NSString *callbackString = CreateNSString(callbackId);

    return ^(NSArray *list, NSError *error) {
        id errorDictItem = error ? [error description] : [NSNull null];
        NSDictionary *callbackDict = @{ @"callbackId": callbackString, @"list": list, @"error": errorDictItem };
        
        UnitySendMessage("Branch", "_asyncCallbackWithList", jsonCStringFromDictionary(callbackDict));
    };
}

static callbackWithUrl callbackWithUrlForCallbackId(char *callbackId) {
    NSString *callbackString = CreateNSString(callbackId);

    return ^(NSString *url, NSError *error) {
        id errorDictItem = error ? [error description] : [NSNull null];
        NSDictionary *callbackDict = @{ @"callbackId": callbackString, @"url": url, @"error": errorDictItem };
        
        UnitySendMessage("Branch", "_asyncCallbackWithUrl", jsonCStringFromDictionary(callbackDict));
    };
}

static callbackWithBranchUniversalObject callbackWithBranchUniversalObjectForCallbackId(char *callbackId) {
    NSString *callbackString = CreateNSString(callbackId);
    
    return ^(BranchUniversalObject *universalObject, BranchLinkProperties *linkProperties, NSError *error) {
        id errorDictItem = error ? [error description] : [NSNull null];
        
        NSDictionary *params = @{@"universalObject": dictFromBranchUniversalObject(universalObject), @"linkProperties": dictFromBranchLinkProperties(linkProperties)};
        NSDictionary *callbackDict = @{ @"callbackId": callbackString, @"params": params, @"error": errorDictItem };
        
        UnitySendMessage("Branch", "_asyncCallbackWithBranchUniversalObject", jsonCStringFromDictionary(callbackDict));
    };
}

#pragma mark - Key methods

void _setBranchKey(char *branchKey) {
    _branchKey = CreateNSString(branchKey);
}

#pragma mark - InitSession methods

void _initSession() {
    [[Branch getInstance:_branchKey] initSessionWithLaunchOptions:_wrapper.launchOptions];
}

void _initSessionWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] initSessionWithLaunchOptions:_wrapper.launchOptions andRegisterDeepLinkHandler:callbackWithParamsForCallbackId(callbackId)];
}

void _initSessionAsReferrable(BOOL isReferrable) {
    [[Branch getInstance:_branchKey] initSessionWithLaunchOptions:_wrapper.launchOptions isReferrable:isReferrable];
}

void _initSessionAsReferrableWithCallback(BOOL isReferrable, char *callbackId) {
    [[Branch getInstance:_branchKey] initSessionWithLaunchOptions:_wrapper.launchOptions isReferrable:isReferrable andRegisterDeepLinkHandler:callbackWithParamsForCallbackId(callbackId)];
}

void _initSessionWithUniversalObjectCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] initSessionWithLaunchOptions:_wrapper.launchOptions andRegisterDeepLinkHandlerUsingBranchUniversalObject:callbackWithBranchUniversalObjectForCallbackId(callbackId)];
}

#pragma mark - Session Item methods

const char *_getFirstReferringParams() {
    return jsonCStringFromDictionary([[Branch getInstance:_branchKey] getFirstReferringParams]);
}

const char *_getFirstReferringBranchUniversalObject() {
    BranchUniversalObject* universalObject = [[Branch getInstance:_branchKey] getFirstReferringBranchUniversalObject];
    return jsonCStringFromDictionary(dictFromBranchUniversalObject(universalObject));
}

const char *_getFirstReferringBranchLinkProperties() {
    BranchLinkProperties *linkProperties = [[Branch getInstance:_branchKey] getFirstReferringBranchLinkProperties];
    return jsonCStringFromDictionary(dictFromBranchLinkProperties(linkProperties));
}

const char *_getLatestReferringParams() {
    return jsonCStringFromDictionary([[Branch getInstance:_branchKey] getLatestReferringParams]);
}

const char *_getLatestReferringBranchUniversalObject() {
    BranchUniversalObject *universalObject = [[Branch getInstance:_branchKey]getLatestReferringBranchUniversalObject];
    return jsonCStringFromDictionary(dictFromBranchUniversalObject(universalObject));
}

const char *_getLatestReferringBranchLinkProperties() {
    BranchLinkProperties *linkProperties = [[Branch getInstance:_branchKey] getLatestReferringBranchLinkProperties];
    return jsonCStringFromDictionary(dictFromBranchLinkProperties(linkProperties));
}

void _resetUserSession() {
    [[Branch getInstance:_branchKey] resetUserSession];
}

void _setIdentity(char *userId) {
    [[Branch getInstance:_branchKey] setIdentity:CreateNSString(userId)];
}

void _setIdentityWithCallback(char *userId, char *callbackId) {
    [[Branch getInstance:_branchKey] setIdentity:CreateNSString(userId) withCallback:callbackWithParamsForCallbackId(callbackId)];
}

void _logout() {
    [[Branch getInstance:_branchKey] logout];
}

# pragma mark - Configuation methods

void _setDebug() {
    [[Branch getInstance:_branchKey] setDebug];
}

void _setRetryInterval(int retryInterval) {
    [[Branch getInstance:_branchKey] setRetryInterval:retryInterval];
}

void _setMaxRetries(int maxRetries) {
    [[Branch getInstance:_branchKey] setMaxRetries:maxRetries];
}

void _setNetworkTimeout(int timeout) {
    [[Branch getInstance:_branchKey] setNetworkTimeout:timeout];
}

void _registerView(char *universalObjectJson) {
    NSDictionary *universalObjectDict = dictionaryFromJsonString(universalObjectJson);
    BranchUniversalObject *obj = branchuniversalObjectFormDict(universalObjectDict);
    
    [obj registerView];
}

#pragma mark - User Action methods

void _loadActionCountsWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] loadActionCountsWithCallback:callbackWithStatusForCallbackId(callbackId)];
}

void _userCompletedAction(char *action) {
    [[Branch getInstance:_branchKey] userCompletedAction:CreateNSString(action)];
}

void _userCompletedActionWithState(char *action, char *stateDict) {
    [[Branch getInstance:_branchKey] userCompletedAction:CreateNSString(action) withState:dictionaryFromJsonString(stateDict)];
}

int _getTotalCountsForAction(char *action) {
    return (int)[[Branch getInstance:_branchKey] getTotalCountsForAction:CreateNSString(action)];
}

int _getUniqueCountsForAction(char *action) {
    return (int)[[Branch getInstance:_branchKey] getUniqueCountsForAction:CreateNSString(action)];
}

#pragma mark - Credit methods

void _loadRewardsWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] loadRewardsWithCallback:callbackWithStatusForCallbackId(callbackId)];
}

int _getCredits() {
    return (int)[[Branch getInstance:_branchKey] getCredits];
}

void _redeemRewards(int count) {
    [[Branch getInstance:_branchKey] redeemRewards:count];
}

int _getCreditsForBucket(char *bucket) {
    return (int)[[Branch getInstance:_branchKey] getCreditsForBucket:CreateNSString(bucket)];
}

void _redeemRewardsForBucket(int count, char *bucket) {
    [[Branch getInstance:_branchKey] redeemRewards:count forBucket:CreateNSString(bucket)];
}

void _getCreditHistoryWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] getCreditHistoryWithCallback:callbackWithListForCallbackId(callbackId)];
}

void _getCreditHistoryForBucketWithCallback(char *bucket, char *callbackId) {
    [[Branch getInstance:_branchKey] getCreditHistoryForBucket:CreateNSString(bucket) andCallback:callbackWithListForCallbackId(callbackId)];
}

void _getCreditHistoryForTransactionWithLengthOrderAndCallback(char *creditTransactionId, int length, int order, char *callbackId) {
    [[Branch getInstance:_branchKey] getCreditHistoryAfter:CreateNSString(creditTransactionId) number:length order:(BranchCreditHistoryOrder)order andCallback:callbackWithListForCallbackId(callbackId)];
}

void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(char *bucket, char *creditTransactionId, int length, int order, char *callbackId) {
    [[Branch getInstance:_branchKey] getCreditHistoryForBucket:CreateNSString(bucket) after:CreateNSString(creditTransactionId) number:length order:(BranchCreditHistoryOrder)order andCallback:callbackWithListForCallbackId(callbackId)];
}

#pragma mark - Content URL Methods

void _getContentUrlWithParamsChannelAndCallback(char *paramsDict, char *channel, char *callbackId) {
    [[Branch getInstance:_branchKey] getContentUrlWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getContentUrlWithParamsTagsChannelAndCallback(char *paramsDict, char *tagList, char *channel, char *callbackId) {
    [[Branch getInstance:_branchKey] getContentUrlWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

#pragma mark - Short URL Generation methods

void _getShortURLWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithBranchUniversalObjectAndCallback(char *universalObjectJson, char *linkPropertiesJson, char *callbackId) {
    NSDictionary *universalObjectDict = dictionaryFromJsonString(universalObjectJson);
    NSDictionary *linkPropertiesDict = dictionaryFromJsonString(linkPropertiesJson);
    
    BranchUniversalObject *obj = branchuniversalObjectFormDict(universalObjectDict);
    BranchLinkProperties *prop = branchLinkPropertiesFormDict(linkPropertiesDict);
    
    [obj getShortUrlWithLinkProperties:prop andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsAndCallback(char *paramsDict, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, char *alias, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andAlias:CreateNSString(alias) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, int type, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andType:(BranchLinkType)type andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, int matchDuration, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andMatchDuration:matchDuration andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsChannelFeatureAndCallback(char *paramsDict, char *channel, char *feature, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsChannelFeatureStageAndCallback(char *paramsDict, char *channel, char *feature, char *stage, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(char *paramsDict, char *channel, char *feature, char *stage, char *alias, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andAlias:CreateNSString(alias) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(char *paramsDict, char *channel, char *feature, char *stage, int type, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andType:(BranchLinkType)type andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(char *paramsDict, char *channel, char *feature, char *stage, int matchDuration, char *callbackId) {
    [[Branch getInstance:_branchKey] getShortURLWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andFeature:CreateNSString(feature) andStage:CreateNSString(stage) andMatchDuration:matchDuration andCallback:callbackWithUrlForCallbackId(callbackId)];
}

#pragma mark - Share Link methods

void _shareLink(char *parameterDict, char *tagList, char *message, char *feature, char *stage, char *defaultUrl, char *callbackId) {
    // Adding a link -- Branch UIActivityItemProvider
    UIActivityItemProvider *itemProvider = [Branch getBranchActivityItemWithParams:dictionaryFromJsonString(parameterDict) feature:CreateNSString(feature) stage:CreateNSString(stage) tags:arrayFromJsonString(tagList)];
    
    // Pass this in the NSArray of ActivityItems when initializing a UIActivityViewController
    UIActivityViewController *shareViewController = [[UIActivityViewController alloc] initWithActivityItems:@[CreateNSString(message), itemProvider] applicationActivities:nil];
    
    // Present the share sheet!
    [GetAppController().rootViewController presentViewController:shareViewController animated:YES completion:nil];
}

void _shareLinkWithLinkProperties(char *universalObjectJson, char *linkPropertiesJson, char *message, char *callbackId) {
    NSDictionary *universalObjectDict = dictionaryFromJsonString(universalObjectJson);
    NSDictionary *linkPropertiesDict = dictionaryFromJsonString(linkPropertiesJson);
    
    BranchUniversalObject *obj = branchuniversalObjectFormDict(universalObjectDict);
    BranchLinkProperties *prop = branchLinkPropertiesFormDict(linkPropertiesDict);
    
    [obj showShareSheetWithLinkProperties:prop andShareText:CreateNSString(message) fromViewController:nil andCallback:nil];
}

#pragma mark - Referral methods

void _getReferralUrlWithParamsTagsChannelAndCallback(char *paramsDict, char *tagList, char *channel, char *callbackId) {
    [[Branch getInstance:_branchKey] getReferralUrlWithParams:dictionaryFromJsonString(paramsDict) andTags:arrayFromJsonString(tagList) andChannel:CreateNSString(channel) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getReferralUrlWithParamsChannelAndCallback(char *paramsDict, char *channel, char *callbackId) {
    [[Branch getInstance:_branchKey] getReferralUrlWithParams:dictionaryFromJsonString(paramsDict) andChannel:CreateNSString(channel) andCallback:callbackWithUrlForCallbackId(callbackId)];
}

void _getReferralCodeWithCallback(char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithCallback:callbackWithParamsForCallbackId(callbackId)];
}

void _getReferralCodeWithAmountAndCallback(int amount, char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithAmount:amount callback:callbackWithParamsForCallbackId(callbackId)];
}

void _getReferralCodeWithPrefixAmountAndCallback(char *prefix, int amount, char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithPrefix:CreateNSString(prefix) amount:amount callback:callbackWithParamsForCallbackId(callbackId)];
}

void _getReferralCodeWithAmountExpirationAndCallback(int amount, char *expiration, char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithAmount:amount expiration:CreateNSDate(expiration) callback:callbackWithParamsForCallbackId(callbackId)];
}

void _getReferralCodeWithPrefixAmountExpirationAndCallback(char *prefix, int amount, char *expiration, char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithPrefix:CreateNSString(prefix) amount:amount expiration:CreateNSDate(expiration) callback:callbackWithParamsForCallbackId(callbackId)];
}

void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(char *prefix, int amount, char *expiration, char *bucket, int calcType, int location, char *callbackId) {
    [[Branch getInstance:_branchKey] getPromoCodeWithPrefix:CreateNSString(prefix) amount:amount expiration:CreateNSDate(expiration) bucket:CreateNSString(bucket) usageType:(BranchPromoCodeUsageType)calcType rewardLocation:(BranchPromoCodeRewardLocation)location callback:callbackWithParamsForCallbackId(callbackId)];
}

void _validateReferralCodeWithCallback(char *code, char *callbackId) {
    [[Branch getInstance:_branchKey] validatePromoCode:CreateNSString(code) callback:callbackWithParamsForCallbackId(callbackId)];
}

void _applyReferralCodeWithCallback(char *code, char *callbackId) {
    [[Branch getInstance:_branchKey] applyPromoCode:CreateNSString(code) callback:callbackWithParamsForCallbackId(callbackId)];
}










