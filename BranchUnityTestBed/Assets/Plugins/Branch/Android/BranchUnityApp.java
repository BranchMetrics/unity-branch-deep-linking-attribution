package io.branch.unity;

import android.app.Application;
import android.util.Log;

import io.branch.referral.Branch;
//import android.support.multidex.MultiDexApplication;

/**
 * Created by antonarhunou on 1/9/18.
 */

public class BranchUnityApp extends Application {
    //public class BranchUnityApp extends MultiDexApplication {

    private static final String TAG = "BranchSDK.Unity";

    public void onCreate() {
        super.onCreate();

        Log.i(TAG, "BranchUnityApp.onCreate()");
        Branch.enableLogging();
        Branch.disableInstantDeepLinking(true);
        Branch.getAutoInstance(this.getApplicationContext());
    }
}
