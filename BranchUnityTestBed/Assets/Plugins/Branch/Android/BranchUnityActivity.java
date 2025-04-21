package io.branch.unity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.app.Activity;

import com.unity3d.player.UnityPlayerActivity;

import io.branch.referral.Defines;

/**
 * Created by antonarhunou on 10/13/16.
 */
public class BranchUnityActivity extends Activity {

    private static final String TAG = "BranchSDK.Unity";

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Use reflection to detect which Unity activity is available
        try {
            Class.forName("com.unity3d.player.UnityPlayerGameActivity");
            // Start or use GameActivity-based logic
        } catch (ClassNotFoundException e) {
            // Fall back to PlayerActivity logic
        }
    }

    @Override
    public void onStart() {
        super.onStart();
        //Log.i(TAG, "BranchUnityActivity.onStart()");
        BranchUnityWrapper.initSession(this);
    }

    @Override
    public void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        //Log.i(TAG, "BranchUnityActivity.onNewIntent(Intent intent)");

        // Unity also triggers a session call from C#, so ignore this to avoid a duplicate call
        //BranchUnityWrapper.initSessionWithIntent();
    }
}
