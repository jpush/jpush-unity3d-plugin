package cn.jiguang.unity.push;

import android.content.Context;
import android.text.TextUtils;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.Set;
import java.util.regex.Pattern;

import cn.jpush.android.api.BasicPushNotificationBuilder;
import cn.jpush.android.api.CustomPushNotificationBuilder;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.data.JPushLocalNotification;


public class JPushBridge {

    private static JPushBridge jpushBridge;

    private Context mContext;
    static String gameObject;

    public static JPushBridge getInstance() {
        if (jpushBridge == null) {
            jpushBridge = new JPushBridge();
        }
        return jpushBridge;
    }

    public void setDebug(boolean enable) {
        JPushInterface.setDebugMode(enable);
    }

    public void initPush(String gameObject) {
        JPushBridge.gameObject = gameObject;
        mContext = UnityPlayer.currentActivity.getApplicationContext();
        JPushInterface.init(mContext);
    }

    public void stopPush() {
        JPushInterface.stopPush(mContext);
    }

    public void resumePush() {
        JPushInterface.resumePush(mContext);
    }

    public boolean isPushStopped() {
        return JPushInterface.isPushStopped(mContext);
    }

    public String getRegistrationId() {
        return JPushInterface.getRegistrationID(mContext);
    }

    public void initCrashHandler() {
        JPushInterface.initCrashHandler(mContext);
    }

    public void stopCrashHandler() {
        JPushInterface.stopCrashHandler(mContext);
    }

    public void setTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = new LinkedHashSet<String>();

        try {
            JSONObject itemsJsonObj = new JSONObject(tagsJsonStr);
            JSONArray tagsJsonArr = itemsJsonObj.getJSONArray("Items");

            for (int i = 0; i < tagsJsonArr.length(); i++) {
                tagSet.add(tagsJsonArr.getString(i));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JPushInterface.setTags(mContext, sequence, tagSet);
    }

    public void addTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = new LinkedHashSet<String>();

        try {
            JSONObject itemsJsonObj = new JSONObject(tagsJsonStr);
            JSONArray tagsJsonArr = itemsJsonObj.getJSONArray("Items");

            for (int i = 0; i < tagsJsonArr.length(); i++) {
                tagSet.add(tagsJsonArr.getString(i));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JPushInterface.addTags(mContext, sequence, tagSet);
    }

    public void deleteTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = new LinkedHashSet<String>();

        try {
            JSONObject itemsJsonObj = new JSONObject(tagsJsonStr);
            JSONArray tagsJsonArr = itemsJsonObj.getJSONArray("Items");

            for (int i = 0; i < tagsJsonArr.length(); i++) {
                tagSet.add(tagsJsonArr.getString(i));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JPushInterface.deleteTags(mContext, sequence, tagSet);
    }

    public void cleanTags(int sequence) {
      JPushInterface.cleanTags(mContext, sequence);
    }

    public void setAlias(int sequence, String alias) {
        JPushInterface.setAlias(mContext, sequence, alias);
    }

    public void deleteAlias(int sequence) {
        JPushInterface.deleteAlias(mContext, sequence);
    }

    public void getAlias(int sequence) {
        JPushInterface.getAlias(mContext, sequence);
    }

    public void setPushTime(String days, int startHour, int endHour) {
        Set<Integer> daysSet = days == null ? null : new HashSet<Integer>();

        if (!TextUtils.isEmpty(days)) {
            String[] strDays = days.split(",");
            for (String str : strDays) {
                if (!isNumeric(str)) {
                    return;
                }
                daysSet.add(Integer.parseInt(str));
            }
        }
        JPushInterface.setPushTime(mContext, daysSet, startHour, endHour);
    }

    /**
     * 设置通知静默时间。
     *
     * @param startHour   静音时段的开始时间 - 小时(范围：0 - 23)
     * @param startMinute 静音时段的开始时间 - 分钟(范围：0 - 59)
     * @param endHour     静音时段的结束时间 - 小时(范围：0 - 23)
     * @param endMinute   静音时段的结束时间 - 分钟(范围：0 - 59)
     */
    public void setSilenceTime(int startHour, int startMinute, int endHour, int endMinute)
            throws Exception {
        if (startHour < 0 || startHour > 23 || startMinute < 0 || startMinute > 59) {
            throw new IllegalArgumentException("开始时间不正确");
        }

        if (endHour < 0 || endHour > 23 || endMinute < 0 || endMinute > 59) {
            throw new IllegalArgumentException("结束时间不正确");
        }

        JPushInterface.setSilenceTime(mContext, startHour, startMinute, endHour, endMinute);
    }

    public void addLocalNotification(int builderId, String content, String title, int notId,
                                     int broadcastTime, String extrasStr) {
        JPushLocalNotification ln = new JPushLocalNotification();
        ln.setBuilderId(builderId);
        ln.setContent(content);
        ln.setTitle(title);
        ln.setNotificationId(notId);
        ln.setBroadcastTime(System.currentTimeMillis() + broadcastTime * 1000);

        if (!TextUtils.isEmpty(extrasStr)) {
            ln.setExtras(extrasStr);
        }

        JPushInterface.addLocalNotification(mContext, ln);
    }

    public void addLocalNotificationByDate(int builderId, String content, String title, int notId,
                                           int year, int month, int day,
                                           int hour, int minute, int second, String extrasStr) {
        JPushLocalNotification localNotification = new JPushLocalNotification();
        localNotification.setBuilderId(builderId);
        localNotification.setContent(content);
        localNotification.setTitle(title);
        localNotification.setNotificationId(notId);
        localNotification.setBroadcastTime(year, month, day, hour, minute, second);

        if (!TextUtils.isEmpty(extrasStr)) {
            localNotification.setExtras(extrasStr);
        }

        JPushInterface.addLocalNotification(mContext, localNotification);
    }

    public void removeLocalNotification(int notificationId) {
        JPushInterface.removeLocalNotification(mContext, notificationId);
    }

    public void clearLocalNotifications() {
        JPushInterface.clearLocalNotifications(mContext);
    }

    public void clearAllNotifications() {
        JPushInterface.clearAllNotifications(mContext);
    }

    public void clearNotificationById(int notificationId) {
        JPushInterface.clearNotificationById(mContext, notificationId);
    }

    public void requestPermission() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                JPushInterface.requestPermission(mContext);
            }
        });
    }

    /**
     * 设置自定义通知栏样式。
     * 具体可参考：http://docs.jpush.io/client/android_tutorials/#_11
     *
     * @param builderId             样式 Id，代表这种通知样式，服务器推送时需要制定。
     * @param statusBarDrawableName 通知图片名称，需要先放在 Android 资源目录中。
     */
    public void setBasicPushNotificationBuilder(int builderId, int notificationDefault,
                                                int notificationFlags, String statusBarDrawableName) {
        BasicPushNotificationBuilder builder = new BasicPushNotificationBuilder(mContext);

        if (notificationDefault != -1) {
            builder.notificationDefaults = notificationDefault;
        }

        if (notificationFlags != 16) {
            builder.notificationFlags = notificationFlags;
        }

        if (!TextUtils.isEmpty(statusBarDrawableName)) {
            builder.statusBarDrawable = getResourceId(statusBarDrawableName, "drawable");
        }

        JPushInterface.setPushNotificationBuilder(builderId, builder);
    }

    /**
     * 进一步自定义通知栏，所有资源名称都需要能在资源目录中找到。
     * 具体可参考：http://docs.jpush.io/client/android_tutorials/#_11
     *
     * @param builderId              通知栏样式编号
     * @param layoutName             通知布局样式名称: R.layout.layoutName
     * @param statusBarDrawableName  顶层状态栏小图标
     * @param layoutIconDrawableName 下拉状态栏时的图标
     */
    public void setCustomPushNotificationBuilder(int builderId, String layoutName,
                                                 String statusBarDrawableName,
                                                 String layoutIconDrawableName) {
        int layoutId = getResourceId(layoutName, "layout");
        int iconId = getResourceId("icon", "id");
        int titleId = getResourceId("title", "id");
        int textId = getResourceId("text", "id");

        int statusBarDrawableId = getResourceId(statusBarDrawableName, "drawable");
        int layoutIconDrawableId = getResourceId(layoutIconDrawableName, "drawable");

        CustomPushNotificationBuilder builder = new CustomPushNotificationBuilder(mContext,
                layoutId, iconId, titleId, textId);

        if (statusBarDrawableId != 0) {
            builder.statusBarDrawable = statusBarDrawableId;
        }

        if (layoutIconDrawableId != 0) {
            builder.layoutIconDrawable = layoutIconDrawableId;
        }

        JPushInterface.setPushNotificationBuilder(builderId, builder);
    }

    /**
     * 设置最近保留的通知条数。
     *
     * @param num 保留的通知条数。
     */
    public void setLatestNotificationNumber(int num) {
        JPushInterface.setLatestNotificationNumber(mContext, num);
    }

    private boolean isNumeric(String str) {
        Pattern pattern = Pattern.compile("[0-9]*");
        return pattern.matcher(str).matches();
    }

    private int getResourceId(String resourceName, String type) {
        if (TextUtils.isEmpty(resourceName)) {
            return 0;
        }
        return mContext.getResources().getIdentifier(resourceName, type, mContext.getPackageName());
    }
}
