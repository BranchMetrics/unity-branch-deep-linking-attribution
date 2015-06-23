extern "C" {
    #pragma mark - Key methods

    void _setBranchKey(char *branchKey);

    #pragma mark - InitSession methods

    void _initSession();
    void _initSessionAsReferrable(BOOL isReferrable);
    void _initSessionWithCallback(char *callbackId);
    void _initSessionAsReferrableWithCallback(BOOL isReferrable, char *callbackId);

    #pragma mark - Session Item methods

    const char *_getFirstReferringParams();
    const char *_getLatestReferringParams();
    void _resetUserSession();
    void _setIdentity(char *userId);
    void _setIdentityWithCallback(char *userId, char *callbackId);
    void _logout();

    # pragma mark - Configuration methods

    void _setDebug();
    void _setRetryInterval(int retryInterval);
    void _setMaxRetries(int maxRetries);
    void _setNetworkTimeout(int timeout);

    #pragma mark - User Action methods

    void _loadActionCountsWithCallback(char *callbackId);
    void _userCompletedAction(char *action);
    void _userCompletedActionWithState(char *action, char *stateDict);
    int _getTotalCountsForAction(char *action);
    int _getUniqueCountsForAction(char *action);

    #pragma mark - Credit methods

    void _loadRewardsWithCallback(char *callbackId);
    int _getCredits();
    void _redeemRewards(int count);
    int _getCreditsForBucket(char *bucket);
    void _redeemRewardsForBucket(int count, char *bucket);

    void _getCreditHistoryWithCallback(char *callbackId);
    void _getCreditHistoryForBucketWithCallback(char *bucket, char *callbackId);
    void _getCreditHistoryForTransactionWithLengthOrderAndCallback(char *creditTransactionId, int length, int order, char *callbackId);
    void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(char *bucket, char *creditTransactionId, int length, int order, char *callbackId);

    #pragma mark - Content URL methods

    void _getContentUrlWithParamsChannelAndCallback(char *paramsDict, char *channel, char *callbackId);
    void _getContentUrlWithParamsTagsChannelAndCallback(char *paramsDict, char *tagList, char *channel, char *callbackId);

    #pragma mark - Short URL Generation methods

    void _getShortURLWithCallback(char *callbackId);
    void _getShortURLWithParamsAndCallback(char *paramsDict, char *callbackId);
    void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, char *callbackId);
    void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, char *alias, char *callbackId);
    void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, int type, char *callbackId);
    void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(char *paramsDict, char *tagList, char *channel, char *feature, char *stage, int matchDuration, char *callbackId);
    void _getShortURLWithParamsChannelFeatureAndCallback(char *paramsDict, char *channel, char *feature, char *callbackId);
    void _getShortURLWithParamsChannelFeatureStageAndCallback(char *paramsDict, char *channel, char *feature, char *stage, char *callbackId);
    void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(char *paramsDict, char *channel, char *feature, char *stage, char *alias, char *callbackId);
    void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(char *paramsDict, char *channel, char *feature, char *stage, int type, char *callbackId);
    void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(char *paramsDict, char *channel, char *feature, char *stage, int matchDuration, char *callbackId);

    #pragma mark - Referral methods

    void _getReferralUrlWithParamsTagsChannelAndCallback(char *paramsDict, char *tagList, char *channel, char *callbackId);
    void _getReferralUrlWithParamsChannelAndCallback(char *paramsDict, char *channel, char *callbackId);
    void _getReferralCodeWithCallback(char *callbackId);
    void _getReferralCodeWithAmountAndCallback(int amount, char *callbackId);
    void _getReferralCodeWithPrefixAmountAndCallback(char *prefix, int amount, char *callbackId);
    void _getReferralCodeWithAmountExpirationAndCallback(int amount, char *expiration, char *callbackId);
    void _getReferralCodeWithPrefixAmountExpirationAndCallback(char *prefix, int amount, char *expiration, char *callbackId);
    void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(char *prefix, int amount, char *expiration, char *bucket, int calcType, int location, char *callbackId);
    void _validateReferralCodeWithCallback(char *code, char *callbackId);
    void _applyReferralCodeWithCallback(char *code, char *callbackId);
}
