package io.branch.anroidthirdparty;
import android.content.Intent;
import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;

/**
 * Created by antonarhunou on 3/27/17.
 */

public class BranchCustomActivity extends UnityPlayerActivity {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public void onNewIntent(Intent intent) {
        this.setIntent(intent);
    }
}