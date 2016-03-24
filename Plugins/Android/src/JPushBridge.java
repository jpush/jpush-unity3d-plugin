package com.example.unity3d_jpush_demo;

import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import android.app.Activity;
import android.content.Context;
import android.os.Looper;
import android.text.TextUtils;
import android.widget.Toast;
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
	private static JPushBridge jpushBridge = new JPushBridge();
	private Activity activity = null;
	public static String gameObjectName = "";
	public static String funcName = "";
	public static boolean ISQUIT = true;

	private Activity getActivity() {
		if(activity == null) {
			activity = UnityPlayer.currentActivity;
		}
		return activity;
	}

	public static JPushBridge getInstance() {
		if(jpushBridge == null) {
			jpushBridge = new JPushBridge();
		}
		ISQUIT = false;
		return jpushBridge;
	}

	public void isQuit() {
		ISQUIT = true;
	}

	public void setDebug(boolean enable) {
		JPushInterface.setDebugMode(enable);
	}

	public void initJPush(String gameObject, String func) {
		gameObjectName = gameObject;
		funcName = func;
		JPushInterface.init(getActivity());
		UnityPlayer.UnitySendMessage(gameObjectName, funcName,
	 		"initJPush:" + gameObject + "---" + func);
	}

	public void stopJPush(String gameObject, String func) {
		JPushInterface.stopPush(getActivity());
		UnityPlayer.UnitySendMessage(gameObject, func, "stopJPush");
	}

	public void resumeJPush(String gameObject, String func) {
		JPushInterface.resumePush(getActivity());
		UnityPlayer.UnitySendMessage(gameObject, func, "resumeJPush");
	}

	public void setTags(String gameObject, String func, String unity_tags) {
		if (TextUtils.isEmpty(unity_tags)) {
			return;
		}
		String[] sArray = unity_tags.split(",");
		final Set<String> tagSet = new LinkedHashSet<String>();
		for (String sTagItme : sArray) {
			if (!isValidTagAndAlias(sTagItme)) {
				return;
			}
			tagSet.add(sTagItme);
		}
		UnityPlayer.UnitySendMessage(gameObject, func, "setTags:" + unity_tags);
		getActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				JPushInterface.setTags(getActivity(), tagSet, mTagsCallback);
			}
		});
	}

	public void setAlias(String gameObject, String func, String unity_alias) {
		if (TextUtils.isEmpty(unity_alias)) {
			return;
		}
		if (!isValidTagAndAlias(unity_alias)) {
			return;
		}
		final String alias = unity_alias;
		getActivity().runOnUiThread(new Runnable() {
			@Override
			public void run() {
				JPushInterface.setAlias(getActivity(), alias, mAliasCallback);
			}
		});
		UnityPlayer.UnitySendMessage(gameObject, func, "setAlias:" + unity_alias);
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
			showToast(logs, getActivity());
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
			showToast(logs, getActivity());
		}
	};

	public void setPushTime(String gameObject, String func, String _days,
			String _startime, String _endtime) {
		if(!isNumeric(_startime) || !isNumeric(_endtime)) {
			return;
		}
		String[] strDays = _days.split(",");
		Set<Integer> days = new HashSet<Integer>();
		for(String str : strDays) {
			if(!isNumeric(str)) {
				return;
			}
			days.add(Integer.parseInt(str));
		}
		int starttime = Integer.parseInt(_startime);
		int endtime = Integer.parseInt(_endtime);
		JPushInterface.setPushTime(getActivity(), days, starttime, endtime);
		UnityPlayer.UnitySendMessage(gameObject, func, "setPushTime");
	}

	public void addLocalNotification(String gameObject, String func) {
		//TODO
	}

	public void removeLocalNotification(String gameObject, String func,
 			int notificationId) {
		JPushInterface.removeLocalNotification(getActivity, notificationId);
		UnityPlayer.UnitySendMessage(gameObject, func, "removeLocalNotification");
	}

	public void clearLocalNotification(String gameObject, String func) {
		JPushInterface.clearLocalNotification(getActivity);
		UnityPlayer.UnitySendMessage(gameObject, func, "clearLocalNotification");
	}

	// 校验Tag Alias 只能是数字,英文字母和中文
	private boolean isValidTagAndAlias(String s) {
		Pattern p = Pattern.compile("^[\u4E00-\u9FA50-9a-zA-Z_-]{0,}$");
		Matcher m = p.matcher(s);
		return m.matches();
	}

	private void showToast(final String toast, final Context context) {
		new Thread(new Runnable() {
			@Override
			public void run() {
				Looper.prepare();
				Toast.makeText(context, toast, Toast.LENGTH_SHORT).show();
				Looper.loop();
			}
		}).start();
	}

	private boolean isNumeric(String str) {
		Pattern pattern = Pattern.compile("[0-9]*");
		return pattern.matcher(str).matches();
	}

}
