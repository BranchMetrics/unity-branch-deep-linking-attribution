package io.branch.unity;

import android.app.Application;
import io.branch.referral.Branch;
import android.bluetooth.BluetoothSocket;
import io.branch.referral.BranchUtil;
//import android.support.multidex.MultiDexApplication;

/**
 * Created by antonarhunou on 1/9/18.
 */

public class BranchUnityApp extends Application {
//public class BranchUnityApp extends MultiDexApplication {
    public void onCreate() {
        super.onCreate();
        BranchUtil.setPluginType(BranchUtil.PluginType.Unity);
        BranchUtil.setPluginVersion("0.6.0");
        Branch.getAutoInstance(this.getApplicationContext());
        Branch.disableInstantDeepLinking(true);
    }
}
