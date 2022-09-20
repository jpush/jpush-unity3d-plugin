package cn.jiguang.unity.push;

import android.content.Context;
import android.text.TextUtils;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;
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

    // 缓存
    static String openNotiStrCache;
    static List<String> receiveNotiStrCache = new ArrayList<String>();
    static List<String> receiveMessageStrCache = new ArrayList<String>();

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
        JPushInterface.setNotificationCallBackEnable(mContext,true);
        JPushInterface.init(mContext);

        if (!receiveNotiStrCache.isEmpty()) {
            for (String noti : receiveNotiStrCache) {
                UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveNotification", noti);
            }
            receiveNotiStrCache.clear();
        }

        if (!TextUtils.isEmpty(openNotiStrCache)) {
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnOpenNotification", openNotiStrCache);
            openNotiStrCache = null;
        }

        if (!receiveMessageStrCache.isEmpty()) {
            for (String msg : receiveMessageStrCache) {
                UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveMessage", msg);
            }
            receiveMessageStrCache.clear();
        }
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

        Set<String> tagSet = JsonUtil.jsonToSet(tagsJsonStr);

        JPushInterface.setTags(mContext, sequence, tagSet);
    }

    public void addTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = JsonUtil.jsonToSet(tagsJsonStr);

        JPushInterface.addTags(mContext, sequence, tagSet);
    }



    public void deleteTags(int sequence, String tagsJsonStr) {
        if (TextUtils.isEmpty(tagsJsonStr)) {
            return;
        }

        Set<String> tagSet = JsonUtil.jsonToSet(tagsJsonStr);

        JPushInterface.deleteTags(mContext, sequence, tagSet);
    }

    public void cleanTags(int sequence) {
        JPushInterface.cleanTags(mContext, sequence);
    }

    public void getAllTags(int sequence) {
        JPushInterface.getAllTags(mContext, sequence);
    }

    public void checkTagBindState(int sequence, String tag) {
        JPushInterface.checkTagBindState(mContext, sequence, tag);
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
                JPushInterface.requestPermission(UnityPlayer.currentActivity);
            }
        });
    }

    /**
     * 设置自定义通知栏样式。
     * 具体可参考：https://docs.jiguang.cn/jpush/client/Android/android_senior/#_11
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
     * 具体可参考：https://docs.jiguang.cn/jpush/client/Android/android_senior/#_11
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

    public boolean getConnectionState() {
        return JPushInterface.getConnectionState(mContext);
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

    //新加


    //动态配置 channel，优先级比 AndroidManifest 里配置的高
    //
    //参数说明
    //
    //context 应用的 ApplicationContext
    //channel 希望配置的 channel，传 null 表示依然使用 AndroidManifest 里配置的 channel
    public  void setChannel(String channel) {
        JPushInterface.setChannel(mContext, channel);
    }

    //接口返回
    //有效的 tag 集合。
    public  String filterValidTags(String jsonTags) {
        if (null == jsonTags){
            return null;
        }
        Set<String> tags = JsonUtil.jsonToSet(jsonTags);
        return JsonUtil.setToJson(JPushInterface.filterValidTags(tags));
    }

    //用于上报用户的通知栏被打开，或者用于上报用户自定义消息被展示等客户端需要统计的事件。
    //参数说明
    //context：应用的 ApplicationContext
    //msgId：推送每一条消息和通知对应的唯一 ID。（ msgId 来源于发送消息和通知的 Extra 字段 JPushInterface.EXTRA_MSG_ID，参考 接收推送消息 Receiver ）
    public void reportNotificationOpened(String msgId) {
        JPushInterface.reportNotificationOpened(mContext, msgId);
    }

    //功能说明
    //
    //设置地理围栏监控周期，最小3分钟，最大1天。默认为15分钟，当距离地理围栏边界小于1000米周期自动调整为3分钟。设置成功后一直使用设置周期，不会进行调整。
    //参数说明
    //
    //context 是应用的 ApplicationContext
    //interval 监控周期，单位是毫秒。
    public void setGeofenceInterval(long interval) {
        JPushInterface.setGeofenceInterval(mContext, interval);
    }

    //功能说明
    //
    //设置最多允许保存的地理围栏数量，超过最大限制后，如果继续创建先删除最早创建的地理围栏。默认数量为10个，允许设置最小1个，最大100个。
    //参数说明
    //
    //context 是应用的 ApplicationContext
    //maxNumber 最多允许保存的地理围栏个数
    public void setMaxGeofenceNumber(int maxNumber) {
        JPushInterface.setMaxGeofenceNumber(mContext,maxNumber);
    }

    //删除指定id的地理围栏
    public void deleteGeofence(String geofenceid){
        JPushInterface.deleteGeofence(mContext,geofenceid);
    }
    //调用此 API 设置手机号码。该接口会控制调用频率，频率为 10s 之内最多 3 次。
    //sequence
    //用户自定义的操作序列号，同操作结果一起返回，用来标识一次操作的唯一性。
    //mobileNumber
    //手机号码。如果传 null 或空串则为解除号码绑定操作。
    //限制：只能以 “+” 或者 数字开头；后面的内容只能包含 “-” 和数字。
    public void setMobileNumber(int sequence, String mobileNumber) {
        JPushInterface.setMobileNumber(mContext,sequence,mobileNumber);
    }

    //JPush SDK 开启和关闭省电模式，默认为关闭。
    //参数说明
    //
    //context 当前应用的 Activity 的上下文
    //enable 是否需要开启或关闭，true 为开启，false 为关闭
    public void setPowerSaveMode(boolean enable) {
        JPushInterface.setPowerSaveMode(mContext,enable);
    }



}
