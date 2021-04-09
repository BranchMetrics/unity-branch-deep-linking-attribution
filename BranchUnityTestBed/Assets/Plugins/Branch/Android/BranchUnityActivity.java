package io.branch.unity;

import android.content.Intent;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

import io.branch.referral.Defines;

/**
 * Created by antonarhunou on 10/13/16.
 */

public class BranchUnityActivity extends UnityPlayerActivity {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public void onNewIntent(Intent intent) {
    	super.onNewIntent(intent);
    	  //intent.putExtra(Defines.Jsonkey.ForceNewBranchSession.getKey(), true);
        intent.putExtra("branch_force_new_session", true);
        this.setIntent(intent);

        BranchUnityWrapper.initSessionWithIntent();
    }
}
