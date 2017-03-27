package io.branch.anroidthirdparty;
import io.branch.referral.Branch;
import io.fabric.unity.android.FabricApplication;

/**
 * Created by antonarhunou on 3/27/17.
 */

public class BranchCustomApplication extends FabricApplication {
    @Override
    public void onCreate() {
        super.onCreate();

        Branch.getAutoInstance(this.getApplicationContext());
    }
}