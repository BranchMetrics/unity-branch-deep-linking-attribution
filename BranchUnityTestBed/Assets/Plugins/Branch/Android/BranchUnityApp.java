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

        //Log.i(TAG, "BranchUnityApp.onCreate()");
        //Branch.enableLogging();

        // TODO: fix the version import, maybe we can do it on build
        // Need to set plugin information earlier than the C# startup
        Branch.registerPlugin("Unity", "0.6.7");

        Branch.disableInstantDeepLinking(true);
        Branch.getAutoInstance(this.getApplicationContext());
    }
}
