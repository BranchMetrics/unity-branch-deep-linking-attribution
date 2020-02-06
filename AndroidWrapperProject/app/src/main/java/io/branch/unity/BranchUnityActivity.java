package io.branch.unity;

import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;
import android.content.Intent;

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
        this.setIntent(intent);

        BranchUnityWrapper.initSessionWithIntent();
    }
}
