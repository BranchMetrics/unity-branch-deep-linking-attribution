package io.branch.unity;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.Console;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.logging.Logger;
import java.util.Iterator;

import io.branch.indexing.BranchUniversalObject;
import io.branch.referral.util.LinkProperties;
import io.branch.referral.Branch;
import io.branch.referral.BranchError;
import io.branch.referral.SharingHelper;
import io.branch.referral.Defines;
import io.branch.referral.util.ShareSheetStyle;

/**
 * Created by grahammueller on 3/25/15.
 */
public class BranchUnityWrapper {
    public static void setBranchKey(String branchKey) {
        _branchKey = branchKey;
    }

    private static Branch.ShareLinkBuilder linkBuilder = null;
    /**
     * InitSession methods
     */

    public static void initSession() {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSessionWithData(unityActivity.getIntent().getData(), unityActivity);
    }

    public static void initSession(boolean isReferrable) {
        Activity unityActivity = UnityPlayer.currentActivity;
        // No congruent API here, so borrowing from initSession(callback, referrable, data) method
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSession((Branch.BranchReferralInitListener) null, isReferrable, unityActivity.getIntent().getData(), unityActivity);
    }

    public static void initSession(String callbackId) {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).initSession(new BranchReferralInitListenerUnityCallback(callbackId), unityActivity.getIntent().getData(), unityActivity);
    }

    public static void initSession(String callbackId, boolean isReferrable) {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(unityActivity.getApplicationContext(), _branchKey).initSession(new BranchReferralInitListenerUnityCallback(callbackId), isReferrable, unityActivity.getIntent().getData(), unityActivity);
    }

    public static void initSessionWithUniversalObjectCallback(String callbackId) {
        Activity unityActivity = UnityPlayer.currentActivity;
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).initSession(new BranchUniversalReferralInitListenerUnityCallback(callbackId), unityActivity.getIntent().getData(), unityActivity);
    }

    public static void closeSession() {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).closeSession();
    }

    /**
     * Session Item methods
     */

    public static String getFirstReferringParams() {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getFirstReferringParams().toString();
    }

    public static String getFirstReferringBranchUniversalObject() {
        BranchUniversalObject branchUniversalObject = null;
        Branch branchInstance = Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey);
        if (branchInstance != null && branchInstance.getFirstReferringParams() != null) {
            JSONObject firstParam = branchInstance.getFirstReferringParams();
            try {
                if (firstParam.has("+clicked_branch_link") && firstParam.getBoolean("+clicked_branch_link")) {
                    branchUniversalObject = new BranchUniversalObject();

                    if (firstParam.has(Defines.Jsonkey.ContentTitle.getKey())) {
                        branchUniversalObject.setTitle(firstParam.getString(Defines.Jsonkey.ContentTitle.getKey()));
                    }
                    if (firstParam.has(Defines.Jsonkey.CanonicalIdentifier.getKey())) {
                        branchUniversalObject.setCanonicalIdentifier(firstParam.getString(Defines.Jsonkey.CanonicalIdentifier.getKey()));
                    }
                    if (firstParam.has(Defines.Jsonkey.ContentKeyWords.getKey())) {
                        JSONArray keywordJsonArray = firstParam.getJSONArray(Defines.Jsonkey.ContentKeyWords.getKey());
                        for (int i = 0; i < keywordJsonArray.length(); i++) {
                            branchUniversalObject.addKeyWord(keywordJsonArray.get(i).toString());
                        }
                    }
                    if (firstParam.has(Defines.Jsonkey.ContentDesc.getKey())) {
                        branchUniversalObject.setContentDescription(firstParam.getString(Defines.Jsonkey.ContentDesc.getKey()));
                    }
                    if (firstParam.has(Defines.Jsonkey.ContentImgUrl.getKey())) {
                        branchUniversalObject.setContentImageUrl(firstParam.getString(Defines.Jsonkey.ContentImgUrl.getKey()));
                    }
                    if (firstParam.has(Defines.Jsonkey.ContentType.getKey())) {
                        branchUniversalObject.setContentType(firstParam.getString(Defines.Jsonkey.ContentType.getKey()));
                    }
                    if (firstParam.has(Defines.Jsonkey.ContentExpiryTime.getKey())) {
                        branchUniversalObject.setContentExpiration(new Date(firstParam.getLong(Defines.Jsonkey.ContentExpiryTime.getKey())));
                    }

                    Iterator<String> keys = firstParam.keys();
                    while (keys.hasNext()) {
                        String key = keys.next();
                        branchUniversalObject.addContentMetadata(key, firstParam.getString(key));
                    }
                }

            } catch (Exception ignore) {
            }
        }

        return _jsonObjectFromBranchUniversalObject(branchUniversalObject).toString();
    }

    public static String getFirstReferringBranchLinkProperties() {
        LinkProperties linkProperties = null;
        Branch branchInstance = Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey);
        if (branchInstance != null && branchInstance.getFirstReferringParams() != null) {
            JSONObject firstParam = branchInstance.getFirstReferringParams();

            try {
                if (firstParam.has("+clicked_branch_link") && firstParam.getBoolean("+clicked_branch_link")) {
                    linkProperties = new LinkProperties();
                    if (firstParam.has("channel")) {
                        linkProperties.setChannel(firstParam.getString("channel"));
                    }
                    if (firstParam.has("feature")) {
                        linkProperties.setFeature(firstParam.getString("feature"));
                    }
                    if (firstParam.has("stage")) {
                        linkProperties.setStage(firstParam.getString("stage"));
                    }
                    if (firstParam.has("duration")) {
                        linkProperties.setDuration(firstParam.getInt("duration"));
                    }
                    if (firstParam.has("$match_duration")) {
                        linkProperties.setDuration(firstParam.getInt("$match_duration"));
                    }
                    if (firstParam.has("tags")) {
                        JSONArray tagsArray = firstParam.getJSONArray("tags");
                        for (int i = 0; i < tagsArray.length(); i++) {
                            linkProperties.addTag(tagsArray.getString(i));
                        }
                    }

                    Iterator<String> keys = firstParam.keys();
                    while (keys.hasNext()) {
                        String key = keys.next();
                        if (key.startsWith("$")) {
                            linkProperties.addControlParameter(key, firstParam.getString(key));
                        }
                    }
                }
            } catch (Exception ignore) {
            }
        }
        return _jsonObjectFromLinkProperties(linkProperties).toString();
    }

    public static String getLatestReferringParams() {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getLatestReferringParams().toString();
    }

    public static String getLatestReferringBranchUniversalObject() {
        BranchUniversalObject branchUniversalObject = null;
        Branch branchInstance = Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey);

        if (branchInstance != null && branchInstance.getLatestReferringParams() != null) {
            JSONObject latestParam = branchInstance.getLatestReferringParams();
            try {
                if (latestParam.has("+clicked_branch_link") && latestParam.getBoolean("+clicked_branch_link")) {
                    branchUniversalObject = new BranchUniversalObject();

                    if (latestParam.has(Defines.Jsonkey.ContentTitle.getKey())) {
                        branchUniversalObject.setTitle(latestParam.getString(Defines.Jsonkey.ContentTitle.getKey()));
                    }
                    if (latestParam.has(Defines.Jsonkey.CanonicalIdentifier.getKey())) {
                        branchUniversalObject.setCanonicalIdentifier(latestParam.getString(Defines.Jsonkey.CanonicalIdentifier.getKey()));
                    }
                    if (latestParam.has(Defines.Jsonkey.ContentKeyWords.getKey())) {
                        JSONArray keywordJsonArray = latestParam.getJSONArray(Defines.Jsonkey.ContentKeyWords.getKey());
                        for (int i = 0; i < keywordJsonArray.length(); i++) {
                            branchUniversalObject.addKeyWord(keywordJsonArray.get(i).toString());
                        }
                    }
                    if (latestParam.has(Defines.Jsonkey.ContentDesc.getKey())) {
                        branchUniversalObject.setContentDescription(latestParam.getString(Defines.Jsonkey.ContentDesc.getKey()));
                    }
                    if (latestParam.has(Defines.Jsonkey.ContentImgUrl.getKey())) {
                        branchUniversalObject.setContentImageUrl(latestParam.getString(Defines.Jsonkey.ContentImgUrl.getKey()));
                    }
                    if (latestParam.has(Defines.Jsonkey.ContentType.getKey())) {
                        branchUniversalObject.setContentType(latestParam.getString(Defines.Jsonkey.ContentType.getKey()));
                    }
                    if (latestParam.has(Defines.Jsonkey.ContentExpiryTime.getKey())) {
                        branchUniversalObject.setContentExpiration(new Date(latestParam.getLong(Defines.Jsonkey.ContentExpiryTime.getKey())));
                    }

                    Iterator<String> keys = latestParam.keys();
                    while (keys.hasNext()) {
                        String key = keys.next();
                        branchUniversalObject.addContentMetadata(key, latestParam.getString(key));
                    }
                }

            } catch (Exception ignore) {
            }
        }

        return _jsonObjectFromBranchUniversalObject(branchUniversalObject).toString();
    }

    public static String getLatestReferringBranchLinkProperties() {
        LinkProperties linkProperties = null;
        Branch branchInstance = Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey);
        if (branchInstance != null && branchInstance.getLatestReferringParams() != null) {
            JSONObject latestParam = branchInstance.getLatestReferringParams();
            try {
                if (latestParam.has("+clicked_branch_link") && latestParam.getBoolean("+clicked_branch_link")) {
                    linkProperties = new LinkProperties();
                    if (latestParam.has("~channel")) {
                        linkProperties.setChannel(latestParam.getString("~channel"));
                    }
                    if (latestParam.has("~feature")) {
                        linkProperties.setFeature(latestParam.getString("~feature"));
                    }
                    if (latestParam.has("~stage")) {
                        linkProperties.setStage(latestParam.getString("~stage"));
                    }
                    if (latestParam.has("~duration")) {
                        linkProperties.setDuration(latestParam.getInt("~duration"));
                    }
                    if (latestParam.has("$match_duration")) {
                        linkProperties.setDuration(latestParam.getInt("$match_duration"));
                    }
                    if (latestParam.has("~tags")) {
                        JSONArray tagsArray = latestParam.getJSONArray("~tags");
                        for (int i = 0; i < tagsArray.length(); i++) {
                            linkProperties.addTag(tagsArray.getString(i));
                        }
                    }

                    Iterator<String> keys = latestParam.keys();
                    while (keys.hasNext()) {
                        String key = keys.next();
                        if (key.startsWith("$")) {
                            linkProperties.addControlParameter(key, latestParam.getString(key));
                        }
                    }
                }
            } catch (Exception ignore) {
            }
        }
        return _jsonObjectFromLinkProperties(linkProperties).toString();
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

    public static void setDebug() {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setDebug();
    }

    public static void setRetryInterval(int retryInterval) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setRetryInterval(retryInterval);
    }

    public static void setMaxRetries(int maxRetries) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setRetryCount(maxRetries);
    }

    public static void setNetworkTimeout(int timeout) {
        Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).setNetworkTimeout(timeout);
    }

    public static void registerView(String universalObjectDict) {
        try {
            BranchUniversalObject universalObject = _branchUniversalObjectFromJSONObject(new JSONObject(universalObjectDict));
            universalObject.registerView();
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void accountForFacebookSDKPreventingAppLaunch() {

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

    public static int getTotalCountsForAction(String action) {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getTotalCountsForAction(action);
    }

    public static int getUniqueCountsForAction(String action) {
        return Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getUniqueCountsForAction(action);
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

    public static void getShortURL(String parameterDict, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);

            Branch.getInstance(UnityPlayer.currentActivity.getApplicationContext(), _branchKey).getShortUrl(parameters, new BranchReferralInitListenerUnityCallback(callbackId));
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void getShortURLWithBranchUniversalObject(String universalObjectDict, String linkPropertiesDict, String callbackId) {

        try {
            BranchUniversalObject universalObject = _branchUniversalObjectFromJSONObject(new JSONObject(universalObjectDict));
            LinkProperties linkProperties = _linkPropertiesFromJSONObject(new JSONObject(linkPropertiesDict));

            universalObject.generateShortUrl(UnityPlayer.currentActivity.getApplicationContext(), linkProperties, new BranchReferralInitListenerUnityCallback(callbackId));
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
     * Share methods
     */

    public static void shareLink(String parameterDict, String tagList, String message, String feature, String stage, String defaultUrl, String callbackId) {
        try {
            JSONObject parameters = new JSONObject(parameterDict);
            JSONArray tagsJArray = new JSONArray(tagList);

            linkBuilder = new Branch.ShareLinkBuilder(UnityPlayer.currentActivity, parameters);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.FACEBOOK);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.TWITTER);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.MESSAGE);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.EMAIL);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.FLICKR);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.GOOGLE_DOC);
            linkBuilder.addPreferredSharingOption(SharingHelper.SHARE_WITH.WHATS_APP);
            linkBuilder.setMessage(message);
            linkBuilder.setStage(stage);
            linkBuilder.setFeature(feature);
            linkBuilder.setDefaultURL(defaultUrl);
            linkBuilder.setCallback(new BranchReferralInitListenerUnityCallback(callbackId));

            for (int i = 0; i < tagsJArray.length(); i++) {
                linkBuilder.addTag(tagsJArray.getString(i));
            }

            UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    linkBuilder.shareLink();
                }
            });
        }
        catch (JSONException jsone) {
            jsone.printStackTrace();
        }
    }

    public static void shareLinkWithLinkProperties(String universalObjectDict, String linkPropertiesDict, String message, String callbackId) {
        try {
            BranchUniversalObject universalObject = _branchUniversalObjectFromJSONObject(new JSONObject(universalObjectDict));
            LinkProperties linkProperties = _linkPropertiesFromJSONObject(new JSONObject(linkPropertiesDict));

            ShareSheetStyle style = new ShareSheetStyle(UnityPlayer.currentActivity.getApplicationContext(), "", message);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.FACEBOOK);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.TWITTER);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.MESSAGE);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.EMAIL);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.FLICKR);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.GOOGLE_DOC);
            style.addPreferredSharingOption(SharingHelper.SHARE_WITH.WHATS_APP);

            universalObject.showShareSheet(UnityPlayer.currentActivity, linkProperties, style, new BranchReferralInitListenerUnityCallback(callbackId));
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

        if (dateString.length() > 0) {
            SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'");

            try {
                date = format.parse(dateString);
            } catch (ParseException pe) {
                pe.printStackTrace();
            }
        }

        return date;
    }

    private static JSONObject _jsonObjectFromBranchUniversalObject(BranchUniversalObject obj) {
        JSONObject jsonObject = new JSONObject();

        if (obj != null) {
            try {
                jsonObject.put(Defines.Jsonkey.CanonicalIdentifier.getKey(), obj.getCanonicalIdentifier());
                jsonObject.put(Defines.Jsonkey.ContentTitle.getKey(), obj.getTitle());
                jsonObject.put(Defines.Jsonkey.ContentDesc.getKey(), obj.getDescription());
                jsonObject.put(Defines.Jsonkey.ContentImgUrl.getKey(), obj.getImageUrl());
                jsonObject.put(Defines.Jsonkey.ContentType.getKey(), obj.getType());
                jsonObject.put(Defines.Jsonkey.PublicallyIndexable.getKey(), obj.isPublicallyIndexable() ? "0" : "1");
                jsonObject.put(Defines.Jsonkey.ContentKeyWords.getKey(), new JSONArray(obj.getKeywords()));
                jsonObject.put(Defines.Jsonkey.ContentExpiryTime.getKey(), Long.toString(obj.getExpirationTime()));
                jsonObject.put("metadata", new JSONObject(obj.getMetadata()));

            } catch (JSONException jsone) {
                jsone.printStackTrace();
            }
        }

        return jsonObject;
    }

    private static JSONObject _jsonObjectFromLinkProperties(LinkProperties link) {
        JSONObject jsonObject = new JSONObject();

        if (link != null) {
            try {
                jsonObject.put("~tags", new JSONArray(link.getTags()));
                jsonObject.put("~feature", link.getFeature());
                jsonObject.put("~alias", link.getAlias());
                jsonObject.put("~stage", link.getStage());
                jsonObject.put("~duration", link.getMatchDuration());
                jsonObject.put("~channel", link.getChannel());
                jsonObject.put("control_params", new JSONObject(link.getControlParams()));

            } catch (JSONException jsone) {
                jsone.printStackTrace();
            }
        }

        return jsonObject;
    }

    private static BranchUniversalObject _branchUniversalObjectFromJSONObject(JSONObject params) {
        BranchUniversalObject branchUniversalObject = new BranchUniversalObject();

        try {
            if (params.has(Defines.Jsonkey.ContentTitle.getKey())) {
                branchUniversalObject.setTitle(params.getString(Defines.Jsonkey.ContentTitle.getKey()));
            }
            if (params.has(Defines.Jsonkey.CanonicalIdentifier.getKey())) {
                branchUniversalObject.setCanonicalIdentifier(params.getString(Defines.Jsonkey.CanonicalIdentifier.getKey()));
            }
            if (params.has(Defines.Jsonkey.ContentKeyWords.getKey())) {
                JSONArray keywordJsonArray = params.getJSONArray(Defines.Jsonkey.ContentKeyWords.getKey());
                for (int i = 0; i < keywordJsonArray.length(); i++) {
                    branchUniversalObject.addKeyWord(keywordJsonArray.get(i).toString());
                }
            }
            if (params.has(Defines.Jsonkey.ContentDesc.getKey())) {
                branchUniversalObject.setContentDescription(params.getString(Defines.Jsonkey.ContentDesc.getKey()));
            }
            if (params.has(Defines.Jsonkey.ContentImgUrl.getKey())) {
                branchUniversalObject.setContentImageUrl(params.getString(Defines.Jsonkey.ContentImgUrl.getKey()));
            }
            if (params.has(Defines.Jsonkey.ContentType.getKey())) {
                branchUniversalObject.setContentType(params.getString(Defines.Jsonkey.ContentType.getKey()));
            }
            if (params.has(Defines.Jsonkey.ContentExpiryTime.getKey())) {
                branchUniversalObject.setContentExpiration(new Date(Long.decode(params.getString(Defines.Jsonkey.ContentExpiryTime.getKey()))));
            }

            Iterator<String> keys = params.keys();
            while (keys.hasNext()) {
                String key = keys.next();
                branchUniversalObject.addContentMetadata(key, params.getString(key));
            }
        } catch (Exception ignore) {
        }

        return branchUniversalObject;
    }

    private static LinkProperties _linkPropertiesFromJSONObject(JSONObject params) {
        LinkProperties linkProperties = new LinkProperties();

        try {
            if (params.has("~channel")) {
                linkProperties.setChannel(params.getString("~channel"));
            }
            if (params.has("~feature")) {
                linkProperties.setFeature(params.getString("~feature"));
            }
            if (params.has("~stage")) {
                linkProperties.setStage(params.getString("~stage"));
            }
            if (params.has("~duration")) {
                linkProperties.setDuration(Long.valueOf(params.getString("~duration")).intValue());
            }
            if (params.has("~tags")) {
                JSONArray tagsArray = params.getJSONArray("~tags");
                for (int i = 0; i < tagsArray.length(); i++) {
                    linkProperties.addTag(tagsArray.getString(i));
                }
            }

            Iterator<String> keys = params.keys();
                while (keys.hasNext()) {
                String key = keys.next();
                if (key.startsWith("$")) {
                    linkProperties.addControlParameter(key, params.getString(key));
                }
            }
        } catch (Exception ignore) {
        }

        return linkProperties;
    }


    private static String _branchKey;

    /**
     * Callback for Unity
     */

    private static class BranchReferralInitListenerUnityCallback implements Branch.BranchReferralInitListener, Branch.BranchReferralStateChangedListener, Branch.BranchListResponseListener, Branch.BranchLinkCreateListener, Branch.BranchLinkShareListener {
        public BranchReferralInitListenerUnityCallback(String callbackId) {
            _callbackId = callbackId;
        }

        @Override
        public void onInitFinished(JSONObject params, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithParams", branchError, "params", params);
        }

        @Override
        public void onStateChanged(boolean changed, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithStatus", branchError, "status", changed);
        }

        @Override
        public void onReceivingResponse(JSONArray list, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithList", branchError, "list", list);
        }

        @Override
        public void onLinkCreate(String url, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithUrl", branchError, "url", url);
        }

        @Override
        public void onShareLinkDialogLaunched() {

        }

        @Override
        public void onShareLinkDialogDismissed() {

        }

        @Override
        public void onLinkShareResponse(String sharedLink, String sharedChannel, BranchError branchError) {
            try {
                JSONObject params = new JSONObject();
                params.put("sharedLink", sharedLink);
                params.put("sharedChannel", sharedChannel);

                _sendMessageWithWithBranchError("_asyncCallbackWithParams", branchError, "params", params);
            }
            catch (JSONException jsone) {
                jsone.printStackTrace();
            }
        }

        @Override
        public void onChannelSelected(java.lang.String selectedChannel) {
            _sendMessageWithWithBranchError("_asyncCallbackWithParams", null, "selectedChannel", selectedChannel);
        }

        private void _sendMessageWithWithBranchError(String asyncCallbackMethod, BranchError branchError, String extraKey, Object extraValue) {
            try {
                JSONObject responseObject = new JSONObject();
                responseObject.put("callbackId", _callbackId);
                responseObject.put(extraKey, extraValue);
                responseObject.put("error", branchError == null ? null : branchError.getMessage());

                String respString = responseObject.toString();
                UnityPlayer.UnitySendMessage("Branch", asyncCallbackMethod, respString);
            }
            catch (JSONException jsone) {
                // nothing to do here
                jsone.printStackTrace();
            }
        }

        private String _callbackId;
    }

    private static class BranchUniversalReferralInitListenerUnityCallback implements Branch.BranchUniversalReferralInitListener, Branch.BranchReferralStateChangedListener, Branch.BranchListResponseListener, Branch.BranchLinkCreateListener, Branch.BranchLinkShareListener {
        public BranchUniversalReferralInitListenerUnityCallback(String callbackId) {
            _callbackId = callbackId;
        }

        @Override
        public void onInitFinished(BranchUniversalObject branchUniversalObject, LinkProperties linkProperties, BranchError branchError) {
            try {
                JSONObject params = new JSONObject();
                params.put("universalObject", _jsonObjectFromBranchUniversalObject(branchUniversalObject));
                params.put("linkProperties", _jsonObjectFromLinkProperties(linkProperties));

                _sendMessageWithWithBranchError("_asyncCallbackWithBranchUniversalObject", branchError, "params", params);
            }
            catch (JSONException jsone) {
                jsone.printStackTrace();
            }
        }

        @Override
        public void onStateChanged(boolean changed, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithStatus", branchError, "status", changed);
        }

        @Override
        public void onReceivingResponse(JSONArray list, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithList", branchError, "list", list);
        }

        @Override
        public void onLinkCreate(String url, BranchError branchError) {
            _sendMessageWithWithBranchError("_asyncCallbackWithUrl", branchError, "url", url);
        }

        @Override
        public void onShareLinkDialogLaunched() {

        }

        @Override
        public void onShareLinkDialogDismissed() {

        }

        @Override
        public void onLinkShareResponse(String sharedLink, String sharedChannel, BranchError branchError) {
            try {
                JSONObject params = new JSONObject();
                params.put("sharedLink", sharedLink);
                params.put("sharedChannel", sharedChannel);

                _sendMessageWithWithBranchError("_asyncCallbackWithParams", branchError, "params", params);
            }
            catch (JSONException jsone) {
                jsone.printStackTrace();
            }
        }

        @Override
        public void onChannelSelected(java.lang.String selectedChannel) {
            _sendMessageWithWithBranchError("_asyncCallbackWithParams", null, "selectedChannel", selectedChannel);
        }

        private void _sendMessageWithWithBranchError(String asyncCallbackMethod, BranchError branchError, String extraKey, Object extraValue) {
            try {
                JSONObject responseObject = new JSONObject();
                responseObject.put("callbackId", _callbackId);
                responseObject.put(extraKey, extraValue);
                responseObject.put("error", branchError == null ? null : branchError.getMessage());

                String respString = responseObject.toString();
                UnityPlayer.UnitySendMessage("Branch", asyncCallbackMethod, respString);
            }
            catch (JSONException jsone) {
                // nothing to do here
                jsone.printStackTrace();
            }
        }

        private String _callbackId;
    }
}
