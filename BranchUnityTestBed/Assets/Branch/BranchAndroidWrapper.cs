using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class BranchAndroidWrapper {
    #if UNITY_ANDROID
    
    public static void setBranchKey(String branchKey) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setBranchKey", branchKey);
		});
    }
    
    #region InitSession methods
    
    public static void initSession() {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("initSession");
		});
    }
    
    public static void initSessionAsReferrable(bool isReferrable) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("initSession", isReferrable);
		});
    }
    
    public static void initSessionWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("initSession", callbackId);
		});
    }
    
    public static void initSessionAsReferrableWithCallback(bool isReferrable, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("initSession", callbackId, isReferrable);
		});
    }

	//TODO
	public static void initSessionWithUniversalObjectCallback(string callbackId) {
		_runBlockOnThread(() => {
			_getBranchClass().CallStatic("initSessionWithUniversalObjectCallback", callbackId);
		});
	}

    public static void closeSession() {
		_runBlockOnThread(() => {
        	_getBranchClass ().CallStatic ("closeSession");
		});
    }
    
    #endregion
    
    #region Session Item methods
    
    public static string getFirstReferringParams() {
        return _getBranchClass().CallStatic<string>("getFirstReferringParams");
    }
    
	public static string getFirstReferringBranchUniversalObject() {
		return _getBranchClass().CallStatic<string>("getFirstReferringBranchUniversalObject");
	}
	
	public static string getFirstReferringBranchLinkProperties() {
		return _getBranchClass().CallStatic<string>("getFirstReferringBranchLinkProperties");
	}

    public static string getLatestReferringParams() {
		return _getBranchClass().CallStatic<string>("getLatestReferringParams");
    }
    
	public static string getLatestReferringBranchUniversalObject() {
		return _getBranchClass().CallStatic<string>("getLatestReferringBranchUniversalObject");
	}
	
	public static string getLatestReferringBranchLinkProperties() {
		return _getBranchClass().CallStatic<string>("getLatestReferringBranchLinkProperties");
	}

    public static void resetUserSession() {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("resetUserSession");
		});
    }
    
    public static void setIdentity(string userId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setIdentity", userId);
		});
    }
    
    public static void setIdentityWithCallback(string userId, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setIdentity", userId, callbackId);
		});
    }
    
    public static void logout() {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("logout");
		});
    }
    
    #endregion
    
    #region Configuration methods

    public static void setDebug() {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setDebug");
		});
    }
    
    public static void setRetryInterval(int retryInterval) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setRetryInterval", retryInterval);
		});
    }
    
    public static void setMaxRetries(int maxRetries) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setMaxRetries", maxRetries);
		});
    }
    
    public static void setNetworkTimeout(int timeout) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("setNetworkTimeout", timeout);
		});
    }
    
	public static void registerView(string universalObject) {
		_runBlockOnThread(() => {
			_getBranchClass().CallStatic("registerView", universalObject);
		});
	}
	
	#endregion
    
    #region User Action methods
    
    public static void loadActionCountsWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("loadActionCounts", callbackId);
		});
    }
    
    public static void userCompletedAction(string action) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("userCompletedAction", action);
		});
    }
    
    public static void userCompletedActionWithState(string action, string stateDict) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("userCompletedAction", action, stateDict);
		});
    }
    
    public static int getTotalCountsForAction(string action) {
        return _getBranchClass().CallStatic<int>("getTotalCountsForAction", action);
    }
    
    public static int getUniqueCountsForAction(string action) {
        return _getBranchClass().CallStatic<int>("getUniqueCountsForAction", action);
    }
    
    #endregion
    
    #region Credit methods
    
    public static void loadRewardsWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("loadRewards", callbackId);
		});
    }
    
    public static int getCredits() {
        return _getBranchClass().CallStatic<int>("getCredits");
    }
    
    public static void redeemRewards(int count) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("redeemRewards", count);
		});
    }
    
    public static int getCreditsForBucket(string bucket) {
        return _getBranchClass().CallStatic<int>("getCreditsForBucket", bucket);
    }
    
    public static void redeemRewardsForBucket(int count, string bucket) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("redeemRewards", bucket, count);
		});
    }
    
    public static void getCreditHistoryWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getCreditHistory", callbackId);
		});
    }
    
    public static void getCreditHistoryForBucketWithCallback(string bucket, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getCreditHistory", bucket, callbackId);
		});
    }
    
    public static void getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getCreditHistory", creditTransactionId, length, order, callbackId);
		});
    }
    
    public static void getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getCreditHistory", bucket, creditTransactionId, length, order, callbackId);
		});
    }
    
    #endregion
    
    #region Content URL methods
    
    public static void getContentUrlWithParamsChannelAndCallback(string parameterDict, string channel, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getContentUrlWithParamsChannelAndCallback", parameterDict, channel, callbackId);
		});
    }
    
    public static void getContentUrlWithParamsTagsChannelAndCallback(string parameterDict, string tagList, string channel, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getContentUrlWithParamsTagsChannelAndCallback", parameterDict, tagList, channel, callbackId);
		});
    }

	#endregion

	#region Share Link methods

	public static void shareLink(String parameterDict, String tagList, String message, String feature, String stage, String defaultUrl, String callbackId) {
		_runBlockOnThread(() => {
			_getBranchClass().CallStatic("shareLink", parameterDict, tagList, message, feature, stage, defaultUrl, callbackId);
		});
	}

	public static void shareLinkWithLinkProperties(string universalObject, string linkProperties, string message, string callbackId) {
		_runBlockOnThread(() => {
			_getBranchClass().CallStatic("shareLinkWithLinkProperties", universalObject, linkProperties, message, callbackId);
		});
	}

    #endregion
    
    #region Short URL Generation methods
    
    public static void getShortURLWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", callbackId);
		});
    }
    
    public static void getShortURLWithParamsAndCallback(string parameterDict, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", parameterDict, callbackId);
		});
    }
    
	public static void getShortURLWithBranchUniversalObjectAndCallback(string universalObject, string linkProperties, string callbackId) {
		_runBlockOnThread(() => {
			_getBranchClass().CallStatic("getShortURLWithBranchUniversalObject", universalObject, linkProperties, callbackId);
		});
	}

    public static void getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parameterDict, string tagList, string channel, string feature, string stage, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURLWithTags", parameterDict, tagList, channel, feature, stage, callbackId);
		});
    }
    
    public static void getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parameterDict, string tagList, string channel, string feature, string stage, string alias, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURLWithTags", parameterDict, tagList, channel, feature, stage, alias, callbackId);
		});
    }
    
    public static void getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parameterDict, string tagList, string channel, string feature, string stage, int type, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURLWithTags", type, parameterDict, tagList, channel, feature, stage,  callbackId);
		});
    }
    
    public static void getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parameterDict, string tagList, string channel, string feature, string stage, int matchDuration, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURLWithTags", parameterDict, tagList, channel, feature, stage, matchDuration, callbackId);
		});
    }
    
    public static void getShortURLWithParamsChannelFeatureAndCallback(string parameterDict, string channel, string feature, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", parameterDict, channel, feature, callbackId);
		});
    }
    
    public static void getShortURLWithParamsChannelFeatureStageAndCallback(string parameterDict, string channel, string feature, string stage, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", parameterDict, channel, feature, stage, callbackId);
		});
    }
    
    public static void getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parameterDict, string channel, string feature, string stage, string alias, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", parameterDict, channel, feature, stage, alias, callbackId);
		});
    }
    
    public static void getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parameterDict, string channel, string feature, string stage, int type, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", type, parameterDict, channel, feature, stage, callbackId);
		});
    }
    
    public static void getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parameterDict, string channel, string feature, string stage, int matchDuration, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getShortURL", parameterDict, channel, feature, stage, matchDuration, callbackId);
		});
    }
    
    #endregion
    
    #region Referral methods
    
    public static void getReferralUrlWithParamsTagsChannelAndCallback(string parameterDict, string tagList, string channel, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralUrl", parameterDict, tagList, channel, callbackId);
		});
    }
    
    public static void getReferralUrlWithParamsChannelAndCallback(string parameterDict, string channel, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralUrl", parameterDict, channel, callbackId);
		});
    }
    
    public static void getReferralCodeWithCallback(string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", callbackId);
		});
    }
    
    public static void getReferralCodeWithAmountAndCallback(int amount, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", amount, callbackId);
		});
    }
    
    public static void getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", prefix, amount, callbackId);
		});
    }
    
    public static void getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", amount, expiration, callbackId);
		});
    }
    
    public static void getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", prefix, amount, expiration, callbackId);
		});
    }
    
    public static void getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("getReferralCode", prefix, amount, expiration, bucket, calcType, location, callbackId);
		});
    }
    
    public static void validateReferralCodeWithCallback(string code, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("validateReferralCode", code, callbackId);
		});
    }
    
    public static void applyReferralCodeWithCallback(string code, string callbackId) {
		_runBlockOnThread(() => {
        	_getBranchClass().CallStatic("applyReferralCode", code, callbackId);
		});
    }
    
    #endregion
    
    #region Utility methods
    
	private static AndroidJavaClass _getBranchClass() {
		if (_branchClass == null) {
			_branchClass = new AndroidJavaClass("io/branch/unity/BranchUnityWrapper");
		}
		
		return _branchClass;
	}
	
	private static void _runBlockOnThread(Action runBlock) {
		var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		activity.Call("runOnUiThread", new AndroidJavaRunnable(runBlock));
	}
    
    #endregion
    
    private static AndroidJavaClass _branchClass;
    
    #endif
}
