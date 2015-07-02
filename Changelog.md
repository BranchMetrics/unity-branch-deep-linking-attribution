Branch Unity SDK change log

- v0.1.2 Fixing reverse param order on getShortUrl(params, callback) for Android.

- v0.1.1
  * Adding missed setDebug method.
  * Moving to using runnables instead of Looper.prepare which conflicts with some other JNI items.
  * Adding compiler flags to meta.
  * Recompiling plugin jar with Java 1.6.
