package io.branch.unity;

import android.app.Application;
import io.branch.referral.Branch;
import io.fabric.unity.android.FabricApplication;

/**
 * Created by antonarhunou on 1/16/17.
 */

public class BranchCustomApplication extends FabricApplication {
    @Override
    public void onCreate() {
        super.onCreate();

        Branch.getAutoInstance(this.getApplicationContext());
    }
}