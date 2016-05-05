package com.example.unity3d_jpush_demo;

import android.app.Activity;
import android.content.Context;
import android.os.Looper;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import cn.jpush.android.api.BasicPushNotificationBuilder;
import cn.jpush.android.api.CustomPushNotificationBuilder;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.api.TagAliasCallback;
import cn.jpush.android.data.JPushLocalNotification;


/**
 * Copyright © 2014  JPUSH. All rights reserved.
 *
 * @Title: JPushBridge.java
 * @Prject: Unity_002
 * @Package: com.example.unity_002
 * @Description: TODO
 * @author: zhangfl
 * @date: 2014-4-16 上午9:48:20
 * @version: V1.0
 */
public class JPushBridge {
    private static final String TAG = "JPush";

    private static JPushBridge jpushBridge = new JPushBridge();
    private Activity activity = null;
    public static String gameObjectName = "";
    public static String funcName = "";
    public static boolean isQuit = true;

    private Activity getActivity() {
        if (activity == null) {
            activity = UnityPlayer.currentActivity;
        }
        return activity;
    }

    public static JPushBridge getInstance() {
        if (jpushBridge == null) {
            jpushBridge = new JPushBridge();
        }
        isQuit = false;
        return jpushBridge;
    }

    public void isQuit() {
        isQuit = true;
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

    public String getRegistrationId(String gameObject, String func) {
        UnityPlayer.UnitySendMessage(gameObject, func, "getRegistrationId");
        return JPushInterface.getRegistrationID(getActivity());
    }

    public String filterValidTags(String gameObject, String func, String tags) {
        String[] sArray = tags.split(",");
        final Set<String> tagSet = new LinkedHashSet<String>();
        for (String tag : sArray) {
            if (!isValidTagAndAlias(tag)) {
                return "";
            }
            tagSet.add(tag);
        }
        Set<String> resultTagsSet = JPushInterface.filterValidTags(tagSet);
        StringBuilder resultTags = new StringBuilder();
        for (String tag : resultTagsSet) {
            resultTags.append(tag).append(",");
        }
        String resultStr = String.valueOf(resultTags);
        UnityPlayer.UnitySendMessage(gameObject, func, "filterValidTags");
        return resultStr.substring(0, resultStr.length() - 1);
    }

    public void setTags(String gameObject, String func, String unity_tags) {
        if (TextUtils.isEmpty(unity_tags)) {
            return;
        }

        String[] sArray = unity_tags.split(",");
        final Set<String> tagSet = new LinkedHashSet<String>();
        for (String tag : sArray) {
            if (!isValidTagAndAlias(tag)) {
                return;
            }
            tagSet.add(tag);
        }
        getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                JPushInterface.setTags(getActivity(), tagSet, mTagsCallback);
            }
        });
        UnityPlayer.UnitySendMessage(gameObject, func, "setTags:" + unity_tags);
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

    public void setAliasAndTags(String gameObject, String func, final String alias,
            String tags) {
        if (TextUtils.isEmpty(alias)) {
            setTags(gameObject, func, tags);
            return;
        }
        if (TextUtils.isEmpty(tags)) {
            setAlias(gameObject, func, alias);
            return;
        }
        String[] sArray = tags.split(",");
        Set<String> tagsSet = new LinkedHashSet<String>();
        for (String tag : sArray) {
            tagsSet.add(tag);
        }
        Set<String> resultTagSet = JPushInterface.filterValidTags(tagsSet);
        JPushInterface.setAliasAndTags(getActivity(), alias, resultTagSet,
                mAliasAndTagsCallback);
        UnityPlayer.UnitySendMessage(gameObject, func, "setAliasAndTags: "
                + alias + "; " + tags);
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
            Log.i("JPushBridge", logs);
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
            Log.i("JPushBridge", logs);
        }
    };

    private final TagAliasCallback mAliasAndTagsCallback = new TagAliasCallback() {
        @Override
        public void gotResult(int code, String alias, Set<String> tags) {
            String logs;
            switch (code) {
                case 0:
                    logs = "Set success";
                    break;
                case 6002:
                    logs = "Failed to set due to timeout. Try again after 60s.";
                    break;
                default:
                    logs = "Failed with errorCode = " + code;
            }
            Log.i("JPushBridge", logs);
        }
    };

    public void setPushTime(String gameObject, String func, String days,
            int startHour, int endHour) {
        String[] strDays = days.split(",");
        Set<Integer> daysSet = new HashSet<Integer>();
        for (String str : strDays) {
            if (!isNumeric(str)) {
                return;
            }
            daysSet.add(Integer.parseInt(str));
        }
        JPushInterface.setPushTime(getActivity(), daysSet, startHour, endHour);
        UnityPlayer.UnitySendMessage(gameObject, func, "setPushTime");
    }

    /**
     * 设置通知静默时间。
     *
     * @param gameObject
     * @param func
     * @param startHour   静音时段的开始时间 - 小时(范围： 0 - 23)
     * @param startMinute 静音时段的开始时间 - 分钟(范围: 0 - 59)
     * @param endHour     静音时段的结束时间 - 小时(范围: 0 - 23)
     * @param endMinute   静音时段的结束时间 - 分钟(范围: 0 - 59)
     * @throws Exception
     */
    public void setSilenceTime(String gameObject, String func, int startHour,
            int startMinute, int endHour, int endMinute) throws Exception {
        if (startHour < 0 || startHour > 23 || startMinute < 0 || startMinute > 59) {
            throw new IllegalArgumentException("开始时间不正确");
        }
        if (endHour < 0 || endHour > 23 || endMinute < 0 || endMinute > 59) {
            throw new IllegalArgumentException("结束时间不正确");
        }
        JPushInterface.setSilenceTime(getActivity().getApplicationContext(),
                startHour, startMinute, endHour, endMinute);
        UnityPlayer.UnitySendMessage(gameObject, func, "setSilenceTime");
    }

    public void addLocalNotification(String gameObject, String func, int builderId,
            String content, String title, int notiId, int broadcastTime,
            String extrasStr) {
        try {
            JSONObject extras = TextUtils.isEmpty(extrasStr) ? new JSONObject() : new JSONObject(extrasStr);

            JPushLocalNotification ln = new JPushLocalNotification();
            ln.setBuilderId(builderId);
            ln.setContent(content);
            ln.setTitle(title);
            ln.setNotificationId(notiId);
            ln.setBroadcastTime(System.currentTimeMillis() + broadcastTime);
            ln.setExtras(extras.toString());

            JPushInterface.addLocalNotification(
                    getActivity().getApplicationContext(), ln);
            UnityPlayer.UnitySendMessage(gameObject, func, "addLocalNotification");
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void removeLocalNotification(String gameObject, String func,
            int notificationId) {
        JPushInterface.removeLocalNotification(getActivity().getApplicationContext(),
                notificationId);
        UnityPlayer.UnitySendMessage(gameObject, func, "removeLocalNotification");
    }

    public void clearLocalNotifications(String gameObject, String func) {
        JPushInterface.clearLocalNotifications(getActivity().getApplicationContext());
        UnityPlayer.UnitySendMessage(gameObject, func, "clearLocalNotifications");
    }

    public void clearAllNotifications(String gameObject, String func) {
        JPushInterface.clearAllNotifications(getActivity().getApplicationContext());
        UnityPlayer.UnitySendMessage(gameObject, func, "clearAllNotifications");
    }

    public void clearNotificationById(String gameObject, String func, int notiId) {
        JPushInterface.clearNotificationById(getActivity().getApplicationContext(),
                notiId);
        UnityPlayer.UnitySendMessage(gameObject, func, "clearNotificationById");
    }

    public void requestPermission(String gameObject, String func) {
        getActivity().runOnUiThread(new Runnable() {
            @Override
            public void run() {
                JPushInterface.requestPermission(getActivity());
            }
        });
        UnityPlayer.UnitySendMessage(gameObject, func, "requestPermission");
    }

    /**
     * 设置自定义通知栏样式。
     * 具体可参考：http://docs.jpush.io/client/android_tutorials/#_11
     *
     * @param gameObject
     * @param func
     * @param builderId             样式 Id，代表这种通知样式，服务器推送时需要制定
     * @param notificationDefault
     * @param notificationFlags
     * @param statusBarDrawableName 通知图片名称，需要先放在 Android 资源目录中
     */
    public void setBasicPushNotificationBuilder(String gameObject, String func,
            int builderId, int notificationDefault, int notificationFlags,
            String statusBarDrawableName) {
        BasicPushNotificationBuilder builder = new BasicPushNotificationBuilder(
                getActivity());
        if (notificationDefault != -1) {
            builder.notificationDefaults = notificationDefault;
        }
        if (notificationFlags != 16) {
            builder.notificationFlags = notificationFlags;
        }
        if (TextUtils.isEmpty(statusBarDrawableName)) {
            builder.statusBarDrawable = getResourceId(statusBarDrawableName, "drawable");
        }
        JPushInterface.setPushNotificationBuilder(builderId, builder);
        UnityPlayer.UnitySendMessage(gameObject, func, "setBasicPushNotificationBuilder");
    }

    /**
     * 进一步自定义通知栏，所有资源名称都需要能在资源目录中找到。
     * 具体可参考：http://docs.jpush.io/client/android_tutorials/#_11
     *
     * @param gameObject
     * @param func
     * @param builderId              通知栏样式编号
     * @param layoutName             通知布局样式名称: R.layout.layoutName
     * @param statusBarDrawableName  顶层状态栏小图标
     * @param layoutIconDrawableName 下拉状态栏时的图标
     */
    public void setCustomPushNotificationBuilder(String gameObject, String func,
            int builderId, String layoutName, String statusBarDrawableName,
            String layoutIconDrawableName) {
        int layoutId = getResourceId(layoutName, "layout");
        int iconId = getResourceId("icon", "id");
        int titleId = getResourceId("title", "id");
        int textId = getResourceId("text", "id");
        int statusBarDrawableId = getResourceId(statusBarDrawableName, "drawable");
        int layoutIconDrawableId = getResourceId(layoutIconDrawableName, "drawable");
        CustomPushNotificationBuilder builder = new CustomPushNotificationBuilder(
                getActivity(), layoutId, iconId, titleId, textId);
        if (statusBarDrawableId != 0) {
            builder.statusBarDrawable = statusBarDrawableId;
        }
        if (layoutIconDrawableId != 0) {
            builder.layoutIconDrawable = layoutIconDrawableId;
        }
        JPushInterface.setPushNotificationBuilder(builderId, builder);
        UnityPlayer.UnitySendMessage(gameObject, func, "setCustomPushNotificationBuilder");
    }

    /**
     * 设置最近保留的通知条数
     *
     * @param gameObject
     * @param func
     * @param num        保留的通知条数
     */
    public void setLatestNotificationNumber(String gameObject, String func, int num) {
        JPushInterface.setLatestNotificationNumber(getActivity().getApplicationContext(),
                num);
        UnityPlayer.UnitySendMessage(gameObject, func, "setLatestNotificationNum");
    }

    // 校验Tag Alias 只能是数字,英文字母和中文
    private boolean isValidTagAndAlias(String s) {
        Pattern p = Pattern.compile("^[\u4E00-\u9FA50-9a-zA-Z_-]*$");
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

    private int getResourceId(String resourceName, String type) {
        if (TextUtils.isEmpty(resourceName)) {
            return 0;
        }
        return getActivity().getResources().getIdentifier(resourceName, type,
                getActivity().getPackageName());
    }

}
