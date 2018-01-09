package io.branch.unity;

import android.app.Application;
import io.branch.referral.Branch;

/**
 * Created by antonarhunou on 1/9/18.
 */

public class BranchUnityApp extends Application {
    public void onCreate() {
        super.onCreate();

        Branch.getAutoInstance(this.getApplicationContext());
        Branch.disableInstantDeepLinking(true);
    }
}
