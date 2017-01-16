package io.branch.unity;

import android.app.Application;
import io.branch.referral.Branch;

/**
 * Created by antonarhunou on 1/16/17.
 */

public class BranchCustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();

        Branch.getAutoInstance(this.getApplicationContext());
    }
}