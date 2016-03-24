package com.example.unity3d_jpush_demo;

import android.os.Bundle;
import android.view.WindowManager;
import cn.jpush.android.api.JPushInterface;

import com.unity3d.player.UnityPlayerActivity;


/**
 * Copyright © 2014  JPUSH. All rights reserved.
 * @Title: UnityPluginActivity.java
 * @Prject: unity-jpush-plugin
 * @Package: com.example.jpushdemo
 * @Description: TODO
 * @author: zhangfl
 * @date: 2014-4-16 下午7:07:02
 * @version: V1.0
 */
public class UnityPluginActivity extends UnityPlayerActivity{
	@Override
	protected void onCreate(Bundle arg0) {
		super.onCreate(arg0);

		WindowManager.LayoutParams attr = getWindow().getAttributes();
        attr.flags &= (~WindowManager.LayoutParams.FLAG_FULLSCREEN);
        getWindow().setAttributes(attr);
        getWindow().clearFlags(WindowManager.LayoutParams.FLAG_LAYOUT_NO_LIMITS);
	}

	@Override
	protected void onResume() {
		super.onResume();
		JPushInterface.onResume(UnityPluginActivity.this ) ;
	}

	@Override
	protected void onPause() {
		super.onStop();
		JPushInterface.onPause(UnityPluginActivity.this);
	}
}
