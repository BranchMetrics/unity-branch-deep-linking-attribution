Branch Unity SDK change log

- 0.4.7
  * Updating Branch to Android version 2.17.0 and iOS version 0.24.1

- 0.4.6
  * Updating Branch to Android version 2.16.0 and iOS version 0.23.2

- 0.4.5
  * Updating Branch to Android version 2.15.0 and iOS version 0.22.5
  
- 0.4.4
  * deleting wrong comments
  
- 0.4.3
  * deleting unused permissions in android manifest
  
- 0.4.2
  * Fixing BranchiOSWrapper.mm

- 0.4.1
  * Updating Branch to Android version 2.14.4 and iOS version 0.22.4
  * Updating BranchUniversalObject
  * Adding new class for metadata (BranchContentMetadata)
  * Adding API for sending branch events
  * Updating Readme

- v0.3.27
  * Updating Branch to iOS version 0.20.3
  * Adding method `setRequestMetadata(string key, string val)`

- v0.3.26
  * Updating Branch to Android version 2.13.1

- v0.3.25
  * Updating Branch to Android version 2.12.2 and iOS version 0.20.2

- v0.3.24
  * @import changed to #import to fix issues "@import when modules are disabled" for Objectvie C and C++

- v0.3.23
  * Updating Branch to iOS version 0.19.5

- v0.3.22
  * Updating Branch to Android version 2.12.1
  * Fix for iOS version

- v0.3.21
  * Updating Branch to Android version 2.12.0 and iOS version 0.18.8
  * Fix for BranchThirdParty.jar

- v0.3.20
  * Fix for Unity 2017

- v0.3.19
  * Updating Branch to Android version 2.11.1 and iOS version 0.17.7

- v0.3.18
  * Updating Branch to iOS version 0.17.6
  * Updated algorithm for android manifest updating

- v0.3.17
  * Updating Branch to Android version 2.10.3 and iOS version 0.17.5

- v0.3.16
  * Updating Branch to Android version 2.10.2

- v0.3.15
  * Updating Branch to Android version 2.8.0 and iOS version 0.15.3
  * Adding cononicalUrl for BranchUniversalObject

- v0.3.14
  * Updating Branch to Android version 2.6.1 and iOS version 0.14.12
  * Fixing RewardsHistoryPanel

- v0.3.13
  * Updating Branch to Android version 2.6.0
  * Fixing AndroidThirdParty
  * Updating BranchThirdParty.jar for Fabric solution

- v0.3.12
  * Updating Branch to Android version 2.5.9 and iOS version 0.13.5
  * Adding strings.xml to AndroidWrapperProject and AndroidThirdParty for avoiding of building error

- v0.3.11
  * Updating Branch to Android version 2.5.7 and iOS version 0.12.27

- v0.3.10
  * Updating Branch to Android version 2.5.5 and iOS version 0.12.24
  * Changing namespace name from MiniJSON to BranchThirdParty_MiniJSON

- v0.3.9
  * Updating Branch to Android version 2.5.2 and iOS version 0.12.19
  * Fix for Android/iOS link-click compatibility when used with BUO.

- v0.3.8
  * Updating Branch to Android version 2.5.1

- v0.3.7
  * Updating Branch to Android version 2.4.7 and iOS version 0.12.16

- v0.3.6
  * Fixing editor script for creating/updating android manifest
  * Fix for crash on sharesheet when user taps cancel(iOS)

- v0.3.5
  * Fixing data retrieving when app is in background (Android)

- v0.3.4
  * Updating Branch to Android version 2.4.5 and iOS version 0.12.11
  * Adding getAutoInstance() for Android platfrom (see README)
  * Fixing of getting deep linking data when app is in background
  * PostProcessBuild is enabled for iOS platfrom only

- v0.3.3
  * Updating Branch to Android version 2.4.4
  
- v0.3.2
  * Updating Branch to Android version 2.4.2 and iOS version 0.12.10
  * Fixing android manifest
  * Fixing "simulate fresh installs" for android
  
- v0.3.1
  * Updating Branch to Android version 2.4.0 and iOS version 0.12.9
  * Returning InitSession methods for iOS

- v0.3.0
  * Updating Branch to Android version 2.2.0 and iOS version 0.12.6
  * Updating Testbed
  * Added listOnSpotlight(string universalObject)
  * Deleted initSessionAsReferrable(bool isReferrable)
  * Deleted initSessionWithCallback(string callbackId)    
  * Deleted initSessionAsReferrableWithCallback(bool isReferrable, string callbackId)
  * Deleted closeSession()
  * Deleted getFirstReferringParams()
  * Deleted getLatestReferringParams()
  * Deleted loadActionCountsWithCallback(string callbackId)
  * Deleted getTotalCountsForAction(string action)
  * Deleted getUniqueCountsForAction(string action)
  * Deleted getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId)
  * Deleted getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId)
  * Deleted shareLink(string parameterDict, string tagList, string message, string feature, string stage, string defaultUrl, string callbackId)
  * Deleted getShortURLWithCallback(string callbackId)
getShortURLWithParamsAndCallback(string parametersDict, string callbackId)
  * Deleted getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId)
  * Deleted getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId)
  * Deleted getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId)
  * Deleted getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId)
  * Deleted getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId)
  * Deleted getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId)
  * Deleted getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId)
  * Deleted getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId)
  * Deleted getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId)
  * Deleted getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId)
  * Deleted getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId)
  * Deleted getReferralCodeWithCallback(string callbackId)
  * Deleted getReferralCodeWithAmountAndCallback(int amount, string callbackId)
  * Deleted getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId) 
  * Deleted getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId)
  * Deleted getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId)
  * Deleted getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId)
  * Deleted validateReferralCodeWithCallback(string code, string callbackId)
  * Deleted applyReferralCodeWithCallback(string code, string callbackId)

- v0.2.10
  * Updating Branch to Android version 1.14.3 and iOS version 0.12.3

- v0.2.9
  * Fixing "~tags" parsing
  
- v0.2.8
  * Updating Branch to Android version 1.12.1 and iOS version 0.12.2

- v0.2.7
  * Updating Branch to Android version 1.12.0 and iOS version 0.12.1

- v0.2.6
  * Fixing trouble with Unity5.4.x
  * Adding generation intent-filter for Android Path Prefixs

- v0.2.5
  * Fixing trouble with empty expired data

- v0.2.4
  * Fixing metadata
  * Fixing non-string parameters

- v0.2.3
  * Updating Branch to Android version 1.10.8 and iOS version 0.10.17
  * Adding accountForFacebookSDKPreventingAppLaunch
  * Fixing cold launch

- v0.2.2
  * Updating Branch to Android version 1.10.5 and iOS version 0.10.16
  * Fixing BranchAppController
  * Fixing BranchiOSWrapper

- v0.2.1
  * Updating Branch to Android version 1.10.4 and iOS version 0.10.14  
  * Adding auto management for Branch session
  * Fixing method Branch.SetDebug()
  * Fixing URI Scheme postprocesser (now postprocesser doesn't delete all existing settings)
  * Fixing parsing of BranchUniversalObject and LinkProperties

- v0.2.0
  * Bunch of bug fixes
  * Cleaner integration steps
  * Updating Branch to Android version 1.10.1 and iOS version 0.11.8
  * Added setDebug
  * Support for Universal Links on iOS
  * Support for content analytics
  * Support for native share sheet

- v0.1.4
  * Updating Branch to Android version 1.8.0 and iOS version 0.10.7
  * Updating parameter type "List<string> list" to "List<object> list" in callback BranchCallbackWithList
  * Updating parameter "DateTime expiration" to "DateTime? expiration" in methods getReferralCode
  * Adding share link functional for Android/iOS
  * Adding source code of demo app into BranchUnityWrapper.unitypackage
  * Adding full demo project archive into github
  * Fixing return value in method getTotalCountsForAction
  * Fixing return value in method getUniqueCountsForAction
  * Fixing methods for converting string to date in Android/iOS wrappers
  * Deleting method _runBlockOnThread because this method throws AndroidJavaException: java.lang.RuntimeException: Only one Looper may be created per thread

- v0.1.3 Fixing a potential null pointer in the Android wrapper when no Activity was provided in the init.

- v0.1.2 Fixing reverse param order on getShortUrl(params, callback) for Android.

- v0.1.1
  * Adding missed setDebug method.
  * Moving to using runnables instead of Looper.prepare which conflicts with some other JNI items.
  * Adding compiler flags to meta.
  * Recompiling plugin jar with Java 1.6.
