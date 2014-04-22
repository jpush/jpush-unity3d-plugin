package com.example.unity3d_jpush_demo;

import java.util.LinkedHashSet;
import java.util.Set;

import android.app.Activity;
import android.text.TextUtils;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.api.TagAliasCallback;

import com.unity3d.player.UnityPlayer;


/**  
 * Copyright © 2014  JPUSH. All rights reserved.
 * @Title: JPushBridge.java
 * @Prject: Unity_002
 * @Package: com.example.unity_002
 * @Description: TODO
 * @author: zhangfl  
 * @date: 2014-4-16 上午9:48:20
 * @version: V1.0  
 */
public class JPushBridge {
	private static JPushBridge jpushBridge = new JPushBridge() ;
	private Activity activity = null;
	public static String gameObjectName = "" ;
	public static String funcName = "" ;
	
	public static boolean ISQUIT = true ;
	
	private Activity getActivity(){
		if(activity == null){
			activity = UnityPlayer.currentActivity; 
		}
		
		return activity;
	}
	
	public static JPushBridge getInstance() {
		if(null == jpushBridge) 
			jpushBridge = new JPushBridge() ;
		ISQUIT = false ;
		return jpushBridge ;
	}
	
	public void isQuit() {
		ISQUIT = true ;
	}
	
	public void setDebug(boolean enable) {
		JPushInterface.setDebugMode(enable) ;
	}
	
	
	public void initJPush(String gameObject , String func) {
		gameObjectName = gameObject ; 
		funcName = func ;
		
		JPushInterface.init(getActivity()) ;
		UnityPlayer.UnitySendMessage(gameObjectName ,funcName , "initJPush:" + gameObject + "---" + func );
	}
	
	public  void stopJPush(String gameObject , String func) {
		JPushInterface.stopPush(getActivity()) ;
		UnityPlayer.UnitySendMessage(gameObject ,func ,  "stopJPush" );
	}
	
	public  void resumeJPush(String gameObject , String func) {
		JPushInterface.resumePush(getActivity()) ;
		UnityPlayer.UnitySendMessage(gameObject ,func ,  "resumeJPush" );
	}
	
	public void setTags(String gameObject , String func ,String unity_tags ) {
		
		if (TextUtils.isEmpty(unity_tags)) {
			return;
		}
		
		String[] sArray = unity_tags.split(",");
		final Set<String> tagSet = new LinkedHashSet<String>();
		for (String sTagItme : sArray) {
			if (!ExampleUtil.isValidTagAndAlias(sTagItme)) {
				return;
			}
			tagSet.add(sTagItme);
		}
		
		UnityPlayer.UnitySendMessage(gameObject , func , "setTags:" + unity_tags);
		
		getActivity().runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				JPushInterface.setTags(getActivity(), tagSet, mTagsCallback) ;
			}
		}) ;
	}
	
	public void setAlias(String gameObject , String func , String unity_alias) {
		
		if (TextUtils.isEmpty(unity_alias)) {
			return;
		}
		if (!ExampleUtil.isValidTagAndAlias(unity_alias)) {
			return;
		}
		final String alias = unity_alias ;
		getActivity().runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				JPushInterface.setAlias(getActivity(), alias, mAliasCallback) ;
			}
		});
		
		UnityPlayer.UnitySendMessage(gameObject , func , "setAlias:" + unity_alias );
	}
	
	private final TagAliasCallback mAliasCallback = new TagAliasCallback() {

		@Override
		public void gotResult(int code, String alias, Set<String> tags) {
			String logs;
			switch (code) {
			case 0:
				logs = "Set alias success";
				break;

			case 6002:
				logs = "Failed to set alias due to timeout. Try again after 60s.";
				
				break;

			default:
				logs = "Failed with errorCode = " + code;
			}

			ExampleUtil.showToast(logs, getActivity());
		}

	};
	
	private final TagAliasCallback mTagsCallback = new TagAliasCallback() {

		@Override
		public void gotResult(int code, String alias, Set<String> tags) {
			String logs;
			switch (code) {
			case 0:
				logs = "Set tags success";
				break;

			case 6002:
				logs = "Failed to set tags due to timeout. Try again after 60s.";
				break;

			default:
				logs = "Failed with errorCode = " + code;
			}

			ExampleUtil.showToast(logs, getActivity());
		}

	};
}
