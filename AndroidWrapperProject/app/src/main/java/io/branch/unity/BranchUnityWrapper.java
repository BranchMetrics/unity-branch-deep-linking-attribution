package io.branch.unity;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.logging.Logger;

import io.branch.referral.Branch;
import io.branch.referral.BranchError;

/**
 * Created by grahammueller on 3/25/15.
 */
public class BranchUnityWrapper {
    public static void setBranchKey(String branchKey) {
        _branchKey = branchKey;
    }

    /**
     * InitSession methods
     */

    public static void initSession() {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSessionWithData(unityActivity.getIntent().getData());
    }

    public static void initSession(boolean isReferrable) {
        Activity unityActivity = UnityPlayer.currentActivity;
        // No congruent API here, so borrowing from initSession(callback, referrable, data) method
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSession(null, isReferrable, unityActivity.getIntent().getData());
    }

    public static void initSession(String callbackId) {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).initSession(new BranchReferralInitListenerUnityCallback(callbackId), unityActivity.getIntent().getData());
    }

    public static void initSession(String callbackId, boolean isReferrable) {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSession(new BranchReferralInitListenerUnityCallback(callbackId), isReferrable, unityActivity.getIntent().getData());
    }

    /**
     * Session Item methods
     */

    public static String getFirstReferringParams() {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getFirstReferringParams().toString();
    }

    public static String getLatestReferringParams() {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getLatestReferringParams().toString();
    }

    public static void resetUserSession() {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).resetUserSession();
    }

    public static void setIdentity(String userId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setIdentity(userId);
    }

    public static void setIdentity(String userId, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setIdentity(userId, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void logout() {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).logout();
    }

    /**
     * Configuration methods
     */

    public static void setRetryInterval(int retryInterval) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setRetryInterval(retryInterval);
    }

    public static void setMaxRetries(int maxRetries) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setRetryCount(maxRetries);
    }

    public static void setNetworkTimeout(int timeout) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setNetworkTimeout(timeout);
    }

    /**
     * User Action methods
     */

    public static void loadActionCounts(String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).loadActionCounts(new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void userCompletedAction(String action) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).userCompletedAction(action);
    }

    public static void userCompletedAction(String action, String stateDict) {
        try {
            JSONObject state = new JSONObject(stateDict);
            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).userCompletedAction(action, state);
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getTotalCountsForAction(String action) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getTotalCountsForAction(action);
    }

    public static void getUniqueCountsForAction(String action) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getUniqueCountsForAction(action);
    }

    /**
     * Credit methods
     */

    public static void loadRewards(String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).loadRewards(new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static int getCredits() {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCredits();
    }

    public static void redeemRewards(int count) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).redeemRewards(count);
    }

    public static int getCreditsForBucket(String bucket) {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCreditsForBucket(bucket);
    }

    public static void redeemRewards(String bucket, int count) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).redeemRewards(bucket, count);
    }

    public static void getCreditHistory(String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCreditHistory(new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getCreditHistory(String bucket, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCreditHistory(bucket, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getCreditHistory(String creditTransactionId, int length, int order, String callbackId) {
        Branch.CreditHistoryOrder creditHistoryOrder = order == 0 ? Branch.CreditHistoryOrder.kMostRecentFirst : Branch.CreditHistoryOrder.kLeastRecentFirst;

        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCreditHistory(creditTransactionId, length, creditHistoryOrder, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getCreditHistory(String bucket, String creditTransactionId, int length, int order, String callbackId) {
        Branch.CreditHistoryOrder creditHistoryOrder = order == 0 ? Branch.CreditHistoryOrder.kMostRecentFirst : Branch.CreditHistoryOrder.kLeastRecentFirst;

        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getCreditHistory(bucket, creditTransactionId, length, creditHistoryOrder, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    /**
     * Content URL methods
     */

    public static void getContentUrl(String parameterDict, String channel, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getContentUrl(channel, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getContentUrl(String parameterDict, String tagList, String channel, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getContentUrl(tags, channel, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    /**
     * Short URL Generation methods
     */

    public static void getShortURL(String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getShortURL(String callbackId, String parameterDict) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURLWithTags(String parameterDict, String tagList, String channel, String feature, String stage, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(tags, channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURLWithTags(String parameterDict, String tagList, String channel, String feature, String stage, String alias, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(alias, tags, channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURLWithTags(int type, String parameterDict, String tagList, String channel, String feature, String stage, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(type, tags, channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURLWithTags(String parameterDict, String tagList, String channel, String feature, String stage, int matchDuration, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(tags, channel, feature, stage, parameters, matchDuration, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURL(String parameterDict, String channel, String feature, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            // null stage. this method seems to be non-corresponding to iOS
            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(channel, feature, null, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURL(String parameterDict, String channel, String feature, String stage, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURL(String parameterDict, String channel, String feature, String stage, String alias, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(alias, channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURL(int type, String parameterDict, String channel, String feature, String stage, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(type, channel, feature, stage, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURL(String parameterDict, String channel, String feature, String stage, int matchDuration, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(channel, feature, stage, parameters, matchDuration, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    /**
     * Referral methods
     */

    public static void getReferralUrl(String parameterDict, String tagList, String channel, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);
            List<String> tags = new ArrayList<String>(); // Sad Panda for inconsistent typing :(
            for (int i = 0; i < tagsJArray.length(); i++) {
                tags.add(tagsJArray.getString(i));
            }

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralUrl(tags, channel, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getReferralUrl(String parameterDict, String channel, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralUrl(channel, parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getReferralCode(String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getReferralCode(int amount, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(amount, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getReferralCode(String prefix, int amount, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(prefix, amount, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getReferralCode(int amount, String expiration, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(amount, _dateFromString(expiration), new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getReferralCode(String prefix, int amount, String expiration, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(prefix, amount, _dateFromString(expiration), new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void getReferralCode(String prefix, int amount, String expiration, String bucket, int calcType, int location, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getReferralCode(prefix, amount, _dateFromString(expiration), bucket, calcType, location, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void validateReferralCode(String code, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).validateReferralCode(code, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    public static void applyReferralCode(String code, String callbackId) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).applyReferralCode(code, new BranchReferralInitListenerUnityCallback(callbackId));
    }

    /**
     * Util methods
     */

    private static Date _dateFromString(String dateString) {
        Date date = null;
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'");

        try {
            date = format.parse(dateString);
        }
        catch (ParseException pe) {
           pe.printStackTrace();
        }

        return date;
    }

    private static String _branchKey;

    /**
     * Callback for Unity
     */

    private static class BranchReferralInitListenerUnityCallback implements Branch.BranchReferralInitListener, Branch.BranchReferralStateChangedListener, Branch.BranchListResponseListener, Branch.BranchLinkCreateListener {
        public BranchReferralInitListenerUnityCallback(String callbackId) {
            _callbackId = callbackId;
        }

        @Override
        public void onInitFinished(JSONObject params, BranchError branchError) {
            _sendMessageWithWithBranchError(branchError, "params", params);
        }

        @Override
        public void onStateChanged(boolean changed, BranchError branchError) {
            _sendMessageWithWithBranchError(branchError, "status", changed);
        }

        @Override
        public void onReceivingResponse(JSONArray list, BranchError branchError) {
            _sendMessageWithWithBranchError(branchError, "list", list);
        }

        @Override
        public void onLinkCreate(String url, BranchError branchError) {
            _sendMessageWithWithBranchError(branchError, "url", url);
        }

        private void _sendMessageWithWithBranchError(BranchError branchError, String extraKey, Object extraValue) {
            try {
                JSONObject responseObject = new JSONObject();
                responseObject.put("callbackId", _callbackId);
                responseObject.put(extraKey, extraValue);
                responseObject.put("error", branchError == null ? null : branchError.getMessage());

                String respString = responseObject.toString();
                UnityPlayer.UnitySendMessage("Branch", "_asyncCallbackWithParams", respString);
            }
            catch (JSONException jsone) {
                // nothing to do here
                jsone.printStackTrace();
            }
        }

        private String _callbackId;
    }
}
