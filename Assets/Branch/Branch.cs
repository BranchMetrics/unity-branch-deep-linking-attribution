using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

public class Branch : MonoBehaviour {
    public string branchKey;

    public delegate void BranchCallbackWithParams(Dictionary<string, object> parameters, string error);
    public delegate void BranchCallbackWithUrl(string url, string error);
    public delegate void BranchCallbackWithStatus(bool changed, string error);
    public delegate void BranchCallbackWithList(List<object> list, string error);

    #region Public methods

    #region InitSession methods

    /**
     * Just initialize session.
     */
    public static void initSession() {
        _initSession();
    }

    /**
     * Just initialize session, specifying whether is should be referrable.
     */
    public static void  initSession(bool isReferrable) {
        _initSessionAsReferrable(isReferrable);
    }

    /**
     * Initialize session and receive information about how it opened.
     */
    public static void initSession(BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _initSessionWithCallback(callbackId);
    }

    /**
     * Initialize session and receive information about how it opened, specifying whether is should be referrable.
     */
    public static void initSession(bool isReferrable, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _initSessionAsReferrableWithCallback(isReferrable, callbackId);
    }

    /**
     * Close session, necessary for some platforms to track when to cut off a Branch session.
     */
    public static void closeSession() {
        _closeSession();
    }

    #endregion

    #region Session Item methods

    /**
     * Get the referring parameters from the initial install.
     */
    public static Dictionary<string, object> getFirstReferringParams() {
        string firstReferringParamsString = _getFirstReferringParams();

        return MiniJSON.Json.Deserialize(firstReferringParamsString) as Dictionary<string, object>;
    }

    /**
     * Get the referring parameters from the last open.
     */
    public static Dictionary<string, object> getLatestReferringParams() {
        string latestReferringParamsString = _getLatestReferringParams();

        return MiniJSON.Json.Deserialize(latestReferringParamsString) as Dictionary<string, object>;
    }

    /**
     * Reset the current session
     */
    public static void resetUserSession() {
        _resetUserSession();
    }

    /**
     * Specifiy an identity for the current session
     */
    public static void setIdentity(string userId) {
        _setIdentity(userId);
    }

    /**
     * Specifiy an identity for the current session and receive information about the set.
     */
    public static void setIdentity(string userId, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _setIdentityWithCallback(userId, callbackId);
    }

    /**
     * Clear current session
     */
    public static void logout() {
        _logout();
    }

    #endregion

    #region Configuration methods

    /**
     * Puts Branch into debug mode, causing it to log all requests, and more importantly, not reference the hardware ID of the phone so you can register installs after just uninstalling/reinstalling the app.
     *
     * Make sure to remove setDebug before releasing.
     */
    public static void setDebug() {
        _setDebug();
    }

    /**
     * How many seconds between retries
     */
    public static void setRetryInterval(int retryInterval) {
        _setRetryInterval(retryInterval);
    }

    /**
     * How many retries before giving up
     */
    public static void setMaxRetries(int maxRetries) {
        _setMaxRetries(maxRetries);
    }

    /**
     * How long before deeming a request as timed out
     */
    public static void setNetworkTimeout(int timeout) {
        _setNetworkTimeout(timeout);
    }

    #endregion

    #region User Action methods

    /**
     * Load counts for all actions. Callback indicates whether a change has occured in these numbers since you last called this method.
     */
    public static void loadActionCounts(BranchCallbackWithStatus callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _loadActionCountsWithCallback(callbackId);
    }

    /**
     * Mark a custom action completed
     */
    public static void userCompletedAction(string action) {
        _userCompletedAction(action);
    }

    /**
     * Mark a custom action completed with additional custom fields
     */
    public static void userCompletedAction(string action, Dictionary<string, object> state) {
        _userCompletedActionWithState(action, MiniJSON.Json.Serialize(state));
    }

    /**
     * Get the number of counts for an action
     */
    public static int getTotalCountsForAction(string action) {
        return _getTotalCountsForAction(action);
    }

    /**
     * Get the number of "unique" counts for an action
     */
    public static int getUniqueCountsForAction(string action) {
       return _getUniqueCountsForAction(action);
    }

    #endregion

    #region Credit methods

    /**
     * Load reward information. Callback indicates whether these values have changed.
     */
    public static void loadRewards(BranchCallbackWithStatus callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _loadRewardsWithCallback(callbackId);
    }

    /**
     * Get total credit count
     */
    public static int getCredits() {
        return _getCredits();
    }

    /**
     * Get credit count for a specified bucket
     */
    public static int getCredits(string bucket) {
        return _getCreditsForBucket(bucket);
    }

    /**
     * Redeem reward for a specified amount of credits
     */
    public static void redeemRewards(int count) {
        _redeemRewards(count);
    }

    /**
     * Redeem reward for a specified amount of credits and a certain bucket
     */
    public static void redeemRewards(int count, string bucket) {
        _redeemRewardsForBucket(count, bucket);
    }

    /**
     * Get Credit Transaction History items in a list
     */
    public static void getCreditHistory(BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryWithCallback(callbackId);
    }

    /**
     * Get Credit Transaction History items in a list for a specified bucket
     */
    public static void getCreditHistory(string bucket, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryForBucketWithCallback(bucket, callbackId);
    }

    /**
     * Get Credit Transaction History items in a list starting at a specified transaction id, and continuing for a specified number of items, either descending or ascending (0, 1)
     */
    public static void getCreditHistory(string creditTransactionId, int length, int order, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryForTransactionWithLengthOrderAndCallback(creditTransactionId, length, order, callbackId);
    }


    /**
     * Get Credit Transaction History items in a list for a specified bucket starting at a specified transaction id, and continuing for a specified number of items, either descending or ascending (0, 1)
     */
    public static void getCreditHistory(string bucket, string creditTransactionId, int length, int order, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(bucket, creditTransactionId, length, order, callbackId);
    }

    #endregion

    #region Content URL Methods

    /**
     * Get a content url given a set of params and a channel
     */
    public static void getContentURL(Dictionary<string, object> parameters, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getContentUrlWithParamsChannelAndCallback(MiniJSON.Json.Serialize(parameters), channel, callbackId);
    }

    /**
     * Get a content url given a set of params, tags, and a channel
     */
    public static void getContentURL(Dictionary<string, object> parameters, List<string> tags, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getContentUrlWithParamsTagsChannelAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, callbackId);
    }

    #endregion

    #region Share Link methods

    public static void shareLink(Dictionary<string, object> parameters, List<string> tags, string message, string feature, string stage, string defaultUrl, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _shareLink(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), message, feature, stage, defaultUrl, callbackId);
    }

    #endregion

    #region Short URL Generation methods

    /**
     * Get an arbitrary short url
     */
    public static void getShortURL(BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithCallback(callbackId);
    }

    /**
     * Get a short url given a set of params
     */
    public static void getShortURL(Dictionary<string, object> parameters, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsAndCallback(MiniJSON.Json.Serialize(parameters), callbackId);
    }

    /**
     * Get a short url given a set of params, tags, channel, feature, and stage
     */
    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsTagsChannelFeatureStageAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, callbackId);
    }

    /**
     * Get a short url given a set of params, tags, channel, feature, stage, and alias
     */
    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, string alias, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, alias, callbackId);
    }

    /**
     * Get a short url given a set of params, tags, channel, feature, stage, and type
     */
    public static void getShortURLWithTags(int type, Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, type, callbackId);
    }

    /**
     * Get a short url given a set of params, tags, channel, feature, stage, and matchDuration
     */
    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string>tags, string channel, string feature, string stage, int matchDuration, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, matchDuration, callbackId);
    }

    /**
     * Get a short url given a set of params, channel, and feature
     */
    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsChannelFeatureAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, callbackId);
    }

    /**
     * Get a short url given a set of params, channel, feature, and stage
     */
    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsChannelFeatureStageAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, callbackId);
    }

    /**
     * Get a short url given a set of params, channel, feature, stage, and alias
     */
    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, string alias, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsChannelFeatureStageAliasAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, alias, callbackId);
    }

    /**
     * Get a short url given a set of params, channel, feature, stage, and type
     */
    public static void getShortURL(int type, Dictionary<string, object> parameters, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsChannelFeatureStageTypeAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, type, callbackId);
    }

    /**
     * Get a short url given a set of params, channel, feature, stage, and match duration
     */
    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, int matchDuration, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, matchDuration, callbackId);
    }

    #endregion

    #region Referral Methods

    /**
     * Get a referral url given a set of params, tags, and channel
     */
    public static void getReferralURL(Dictionary<string, object> parameters, List<string> tags, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getReferralUrlWithParamsTagsChannelAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, callbackId);
    }

    /**
     * Get a referral url given a set of params and channel
     */
    public static void getReferralURL(Dictionary<string, object> parameters, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getReferralUrlWithParamsChannelAndCallback(MiniJSON.Json.Serialize(parameters), channel, callbackId);
    }

    /**
     * Get an arbitrary referral code for this session
     */
    public static void getReferralCode(BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getReferralCodeWithCallback(callbackId);
    }

    /**
     * Get a referral code for this session for a specified amount
     */
    public static void getReferralCode(int amount, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getReferralCodeWithAmountAndCallback(amount, callbackId);
    }

    /**
     * Get a referral code for this session with a prefix identifier for a specified amount
     */
    public static void getReferralCode(string prefix, int amount, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getReferralCodeWithPrefixAmountAndCallback(prefix, amount, callbackId);
    }

    /**
     * Get a referral code for this session for a specified amount set to expire at the specified time
     */
    public static void getReferralCode(int amount, DateTime? expiration, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        if (expiration.HasValue) {
            _getReferralCodeWithAmountExpirationAndCallback(amount, expiration.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"), callbackId);
        } else {
            _getReferralCodeWithAmountExpirationAndCallback(amount, "", callbackId);
        }
    }

    /**
     * Get a referral code for this session with a prefix identifier for a specified amount set to expire at the specified time
     */
    public static void getReferralCode(string prefix, int amount, DateTime? expiration, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        if (expiration.HasValue) {
            _getReferralCodeWithPrefixAmountExpirationAndCallback(prefix, amount, expiration.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"), callbackId);
        } else {
            _getReferralCodeWithPrefixAmountExpirationAndCallback(prefix, amount, "", callbackId);
        }
    }

    /**
     * Get a referral code for this session with a prefix identifier for a specified amount set to expire at the specified time in a specified bucket with a specified calculation type and location
     * Calc Type can be one of 0 (unlimited, reward can be applied continually) or 1 (unique, user can only apply a specific code once)
     * Location can be one of 0 (referree, user applying referral code receives credit), 1 (referrer, user who created code receives credit), or 2 (both, both the sender and receiver receive credit)
     */
    public static void getReferralCode(string prefix, int amount, DateTime? expiration, string bucket, int calcType, int location, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        if (expiration.HasValue) {
            _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(prefix, amount, expiration.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"), bucket, calcType, location, callbackId);
        } else {
            _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(prefix, amount, "", bucket, calcType, location, callbackId);
        }
    }

    /**
     * Validate if a referral code exists in Branch system and is still valid. A code is vaild if:
     * 1. It hasn't expired.
     * 2. If its calculation type is uniqe, it hasn't been applied by current user.
     */
    public static void validateReferralCode(string code, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _validateReferralCodeWithCallback(code, callbackId);
    }

    /**
     * Apply a referral code if it exists in Branch system and is still valid (see above). If the code is valid, returns the referral code object in the callback.
     */
    public static void applyReferralCode(string code, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _applyReferralCodeWithCallback(code, callbackId);
    }

    #endregion

    #endregion

    #region Private methods

    public void Awake() {
        name = "Branch";

        DontDestroyOnLoad(gameObject);

        _setBranchKey(branchKey);
    }

    #region Platform Loading Methods

    #if UNITY_IPHONE

    [DllImport ("__Internal")]
    private static extern void _setBranchKey(string branchKey);

    [DllImport ("__Internal")]
    private static extern void _initSession();

    [DllImport ("__Internal")]
    private static extern void _initSessionAsReferrable(bool isReferrable);

    [DllImport ("__Internal")]
    private static extern void _initSessionWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern void _initSessionAsReferrableWithCallback(bool isReferrable, string callbackId);

    private static void _closeSession() { }

    [DllImport ("__Internal")]
    private static extern string _getFirstReferringParams();

    [DllImport ("__Internal")]
    private static extern string _getLatestReferringParams();

    [DllImport ("__Internal")]
    private static extern void _resetUserSession();

    [DllImport ("__Internal")]
    private static extern void _setIdentity(string userId);

    [DllImport ("__Internal")]
    private static extern void _setIdentityWithCallback(string userId, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _logout();

    [DllImport ("__Internal")]
    private static extern void _setDebug();

    [DllImport ("__Internal")]
    private static extern void _setRetryInterval(int retryInterval);

    [DllImport ("__Internal")]
    private static extern void _setMaxRetries(int maxRetries);

    [DllImport ("__Internal")]
    private static extern void _setNetworkTimeout(int timeout);

    [DllImport ("__Internal")]
    private static extern void _loadActionCountsWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern void _userCompletedAction(string action);

    [DllImport ("__Internal")]
    private static extern void _userCompletedActionWithState(string action, string stateDict);

    [DllImport ("__Internal")]
    private static extern int _getTotalCountsForAction(string action);

    [DllImport ("__Internal")]
    private static extern int _getUniqueCountsForAction(string action);

    [DllImport ("__Internal")]
    private static extern void _loadRewardsWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern int _getCredits();

    [DllImport ("__Internal")]
    private static extern void _redeemRewards(int count);

    [DllImport ("__Internal")]
    private static extern int _getCreditsForBucket(string bucket);

    [DllImport ("__Internal")]
    private static extern void _redeemRewardsForBucket(int count, string bucket);

    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForBucketWithCallback(string bucket, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsAndCallback(string parametersDict, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _shareLink(string parameterDict, string tagList, string message, string feature, string stage, string defaultUrl, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithCallback(string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithAmountAndCallback(int amount, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _validateReferralCodeWithCallback(string code, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _applyReferralCodeWithCallback(string code, string callbackId);

    #elif UNITY_ANDROID

    private static void _setBranchKey(string branchKey) {
        BranchAndroidWrapper.setBranchKey(branchKey);
    }

    private static void _initSession() {
        BranchAndroidWrapper.initSession();
    }

    private static void _initSessionAsReferrable(bool isReferrable) {
        BranchAndroidWrapper.initSessionAsReferrable(isReferrable);
    }

    private static void _initSessionWithCallback(string callbackId) {
        BranchAndroidWrapper.initSessionWithCallback(callbackId);
    }

    private static void _closeSession() {
        BranchAndroidWrapper.closeSession();
    }

    private static void _initSessionAsReferrableWithCallback(bool isReferrable, string callbackId) {
        BranchAndroidWrapper.initSessionAsReferrableWithCallback(isReferrable, callbackId);
    }

    private static string _getFirstReferringParams() {
        return BranchAndroidWrapper.getFirstReferringParams();
    }

    private static string _getLatestReferringParams() {
        return BranchAndroidWrapper.getLatestReferringParams();
    }

    private static void _resetUserSession() {
        BranchAndroidWrapper.resetUserSession();
    }

    private static void _setIdentity(string userId) {
        BranchAndroidWrapper.setIdentity(userId);
    }

    private static void _setIdentityWithCallback(string userId, string callbackId) {
        BranchAndroidWrapper.setIdentityWithCallback(userId, callbackId);
    }

    private static void _logout() {
        BranchAndroidWrapper.logout();
    }

    private static void _setDebug() {
        BranchAndroidWrapper.setDebug();
    }

    private static void _setRetryInterval(int retryInterval) {
        BranchAndroidWrapper.setRetryInterval(retryInterval);
    }

    private static void _setMaxRetries(int maxRetries) {
        BranchAndroidWrapper.setMaxRetries(maxRetries);
    }

    private static void _setNetworkTimeout(int timeout) {
        BranchAndroidWrapper.setNetworkTimeout(timeout);
    }

    private static void _loadActionCountsWithCallback(string callbackId) {
        BranchAndroidWrapper.loadActionCountsWithCallback(callbackId);
    }

    private static void _userCompletedAction(string action) {
        BranchAndroidWrapper.userCompletedAction(action);
    }

    private static void _userCompletedActionWithState(string action, string stateDict) {
        BranchAndroidWrapper.userCompletedActionWithState(action, stateDict);
    }

    private static int _getTotalCountsForAction(string action) {
        return BranchAndroidWrapper.getTotalCountsForAction(action);
    }

    private static int _getUniqueCountsForAction(string action) {
        return BranchAndroidWrapper.getUniqueCountsForAction(action);
    }

    private static void _loadRewardsWithCallback(string callbackId) {
        BranchAndroidWrapper.loadRewardsWithCallback(callbackId);
    }

    private static int _getCredits() {
        return BranchAndroidWrapper.getCredits();
    }

    private static void _redeemRewards(int count) {
        BranchAndroidWrapper.redeemRewards(count);
    }

    private static int _getCreditsForBucket(string bucket) {
        return BranchAndroidWrapper.getCreditsForBucket(bucket);
    }

    private static void _redeemRewardsForBucket(int count, string bucket) {
        BranchAndroidWrapper.redeemRewardsForBucket(count, bucket);
    }

    private static void _getCreditHistoryWithCallback(string callbackId) {
        BranchAndroidWrapper.getCreditHistoryWithCallback(callbackId);
    }

    private static void _getCreditHistoryForBucketWithCallback(string bucket, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForBucketWithCallback(bucket, callbackId);
    }

    private static void _getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForTransactionWithLengthOrderAndCallback(creditTransactionId, length, order, callbackId);
    }

    private static void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(bucket, creditTransactionId, length, order, callbackId);
    }

    private static void _getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        BranchAndroidWrapper.getContentUrlWithParamsChannelAndCallback(parametersDict, channel, callbackId);
    }

    private static void _getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        BranchAndroidWrapper.getContentUrlWithParamsTagsChannelAndCallback(parametersDict, tags, channel, callbackId);
    }

    private static void _shareLink(string parameterDict, string tagList, string message, string feature, string stage, string defaultUrl, string callbackId) {
        BranchAndroidWrapper.shareLink(parameterDict, tagList, message, feature, stage, defaultUrl, callbackId);
    }

    private static void _getShortURLWithCallback(string callbackId) {
        BranchAndroidWrapper.getShortURLWithCallback(callbackId);
    }

    private static void _getShortURLWithParamsAndCallback(string parametersDict, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsAndCallback(parametersDict, callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageAndCallback(parametersDict, tags, channel, feature, stage, callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(parametersDict, tags, channel, feature, stage, alias, callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(parametersDict, tags, channel, feature, stage, type, callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(parametersDict, tags, channel, feature, stage, matchDuration, callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureAndCallback(parametersDict, channel, feature, callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageAndCallback(parametersDict, channel, feature, stage, callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageAliasAndCallback(parametersDict, channel, feature, stage, alias, callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageTypeAndCallback(parametersDict, channel, feature, stage, type, callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(parametersDict, channel, feature, stage, matchDuration, callbackId);
    }

    private static void _getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        BranchAndroidWrapper.getReferralUrlWithParamsTagsChannelAndCallback(parametersDict, tags, channel, callbackId);
    }

    private static void _getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        BranchAndroidWrapper.getReferralUrlWithParamsChannelAndCallback(parametersDict, channel, callbackId);
    }

    private static void _getReferralCodeWithCallback(string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithCallback(callbackId);
    }

    private static void _getReferralCodeWithAmountAndCallback(int amount, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithAmountAndCallback(amount, callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountAndCallback(prefix, amount, callbackId);
    }

    private static void _getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithAmountExpirationAndCallback(amount, expiration, callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountExpirationAndCallback(prefix, amount, expiration, callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(prefix, amount, expiration, bucket, calcType, location, callbackId);
    }

    private static void _validateReferralCodeWithCallback(string code, string callbackId) {
        BranchAndroidWrapper.validateReferralCodeWithCallback(code, callbackId);
    }

    private static void _applyReferralCodeWithCallback(string code, string callbackId) {
        BranchAndroidWrapper.applyReferralCodeWithCallback(code, callbackId);
    }

    #else

    private static void _setBranchKey(string branchKey) { }

    private static void _initSession() {
        Debug.Log("Branch is not implemented on this platform");
    }

    private static void _initSessionAsReferrable(bool isReferrable) {
        Debug.Log("Branch is not implemented on this platform");
    }

    private static void _initSessionWithCallback(string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _initSessionAsReferrableWithCallback(bool isReferrable, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _closeSession() { }

    private static string _getFirstReferringParams() {
        return "{}";
    }

    private static string _getLatestReferringParams() {
        return "{}";
    }

    private static void _resetUserSession() { }

    private static void _setIdentity(string userId) { }

    private static void _setIdentityWithCallback(string userId, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _logout() { }

    private static void _setRetryInterval(int retryInterval) { }

    private static void _setMaxRetries(int maxRetries) { }

    private static void _setNetworkTimeout(int timeout) { }

    private static void _loadActionCountsWithCallback(string callbackId) {
        callNotImplementedCallbackForStatusCallback(callbackId);
    }

    private static void _userCompletedAction(string action) { }

    private static void _userCompletedActionWithState(string action, string stateDict) { }

    private static int _getTotalCountsForAction(string action) {
        return 0;
    }

    private static int _getUniqueCountsForAction(string action) {
        return 0;
    }

    private static void _loadRewardsWithCallback(string callbackId) {
        callNotImplementedCallbackForStatusCallback(callbackId);
    }

    private static int _getCredits() {
        return 0;
    }

    private static void _redeemRewards(int count) { }

    private static int _getCreditsForBucket(string bucket) {
        return 0;
    }

    private static void _redeemRewardsForBucket(int count, string bucket) { }

    private static void _getCreditHistoryWithCallback(string callbackId) {
        callNotImplementedCallbackForListCallback(callbackId);
    }

    private static void _getCreditHistoryForBucketWithCallback(string bucket, string callbackId) {
        callNotImplementedCallbackForListCallback(callbackId);
    }

    private static void _getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId) {
        callNotImplementedCallbackForListCallback(callbackId);
    }

    private static void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId) {
        callNotImplementedCallbackForListCallback(callbackId);
    }

    private static void _getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithCallback(string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsAndCallback(string parametersDict, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        callNotImplementedCallbackForUrlCallback(callbackId);
    }

    private static void _getReferralCodeWithCallback(string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _getReferralCodeWithAmountAndCallback(int amount, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _validateReferralCodeWithCallback(string code, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void _applyReferralCodeWithCallback(string code, string callbackId) {
        callNotImplementedCallbackForParamCallback(callbackId);
    }

    private static void callNotImplementedCallbackForParamCallback(string callbackId) {
        var callback = _branchCallbacks[callbackId] as BranchCallbackWithParams;
        callback(null, "Not implemented on this platform");
    }

    private static void callNotImplementedCallbackForUrlCallback(string callbackId) {
        var callback = _branchCallbacks[callbackId] as BranchCallbackWithUrl;
        callback(null, "Not implemented on this platform");
    }

    private static void callNotImplementedCallbackForListCallback(string callbackId) {
        var callback = _branchCallbacks[callbackId] as BranchCallbackWithList;
        callback(null, "Not implemented on this platform");
    }

    private static void callNotImplementedCallbackForStatusCallback(string callbackId) {
        var callback = _branchCallbacks[callbackId] as BranchCallbackWithStatus;
        callback(false, "Not implemented on this platform");
    }

    #endif

    #endregion

    #region Callback management

    public void _asyncCallbackWithParams(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        Dictionary<string, object> parameters = callbackDict.ContainsKey("params") ? callbackDict["params"] as Dictionary<string, object> : null;
        string error = callbackDict.ContainsKey("error") ? callbackDict["error"] as string : null;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithParams;
        callback(parameters, error);
    }

    public void _asyncCallbackWithStatus(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        bool status = callbackDict.ContainsKey("status") ? (callbackDict["status"] as bool?).Value : false;
        string error = callbackDict.ContainsKey("error") ? callbackDict["error"] as string : null;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithStatus;
        callback(status, error);
    }

    public void _asyncCallbackWithList(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        List<object> list = callbackDict.ContainsKey("list") ? callbackDict["list"] as List<object> : null;
        string error = callbackDict.ContainsKey("error") ? callbackDict["error"] as string : null;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithList;
        callback(list, error);
    }

    public void _asyncCallbackWithUrl(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        string url = callbackDict.ContainsKey("url") ? callbackDict["url"] as string : null;
        string error = callbackDict.ContainsKey("error") ? callbackDict["error"] as string : null;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithUrl;
        callback(url, error);
    }

    private static string _getNextCallbackId() {
        return "BranchCallbackId" + (++_nextCallbackId);
    }

    #endregion

    #endregion

    private static int _nextCallbackId = 0;
    private static Dictionary<string, object> _branchCallbacks = new Dictionary<string, object>();
}
