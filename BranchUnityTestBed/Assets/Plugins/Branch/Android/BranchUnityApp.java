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

        // Some early lifecycle events occur prior to C# runtime startup, which prevents the C# APIs from working properly

        // Enables logging for install/open on app launch
        //Log.i(TAG, "BranchUnityApp.onCreate()");
        //Branch.enableLogging();

        // Set plugin version
        Branch.registerPlugin("Unity", "1.0.0");
        
        Branch.disableInstantDeepLinking(true);
        Branch.getAutoInstance(this.getApplicationContext());
    }
}
