Branch Unity SDK change log

- v0.1.4 Updating Branch to v.0.8.10
         Updating parameter type "List<string> list" to "List<object> list" in callback BranchCallbackWithList
         Updating parameter "DateTime expiration" to "DateTime? expiration" in methods getReferralCode
         Adding share link functional for Android/iOS
         Adding source code of demo app into BranchUnityWrapper.unitypackage
         Adding full demo project archive into github
         Fixing return value in method getTotalCountsForAction
         Fixing return value in method getUniqueCountsForAction
         Fixing methods for converting string to date in Android/iOS wrappers

- v0.1.3 Fixing a potential null pointer in the Android wrapper when no Activity was provided in the init.

- v0.1.2 Fixing reverse param order on getShortUrl(params, callback) for Android.

- v0.1.1
  * Adding missed setDebug method.
  * Moving to using runnables instead of Looper.prepare which conflicts with some other JNI items.
  * Adding compiler flags to meta.
  * Recompiling plugin jar with Java 1.6.
