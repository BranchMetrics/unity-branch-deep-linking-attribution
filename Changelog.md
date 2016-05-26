Branch Unity SDK change log
-v0.2.7
  * Updating Branch to Android version 1.12.0 and iOS version 0.12.1

-v0.2.6
  * Fixing trouble with Unity5.4.x
  * Adding generation intent-filter for Android Path Prefixs

-v0.2.5
  * Fixing trouble with empty expired data

-v0.2.4
  * Fixing metadata
  * Fixing non-string parameters

-v0.2.3
  * Updating Branch to Android version 1.10.8 and iOS version 0.10.17
  * Adding accountForFacebookSDKPreventingAppLaunch
  * Fixing cold launch

-v0.2.2
  * Updating Branch to Android version 1.10.5 and iOS version 0.10.16
  * Fixing BranchAppController
  * Fixing BranchiOSWrapper

-v0.2.1
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
