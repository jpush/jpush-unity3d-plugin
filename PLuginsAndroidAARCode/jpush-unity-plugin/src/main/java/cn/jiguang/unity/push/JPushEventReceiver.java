package cn.jiguang.unity.push;

import android.content.Context;
import android.content.Intent;
import android.text.TextUtils;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;
import java.util.Set;

import cn.jpush.android.api.CustomMessage;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.api.JPushMessage;
import cn.jpush.android.api.NotificationMessage;
import cn.jpush.android.service.JPushMessageReceiver;

/**
 * 处理 tag/alias 相关 API 的操作结果。
 */
public class JPushEventReceiver extends JPushMessageReceiver {
    private static final String TAG = "JPushEventReceiver";

    @Override
    public void onTagOperatorResult(Context context, JPushMessage jPushMessage) {
        super.onTagOperatorResult(context, jPushMessage);

        JSONObject resultJson = new JSONObject();

        int sequence = jPushMessage.getSequence();
        try {
            resultJson.put("sequence", sequence);
            resultJson.put("code", jPushMessage.getErrorCode());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        if (jPushMessage.getErrorCode() == 0) { // success
            Set<String> tags = jPushMessage.getTags();
            JSONArray tagsJsonArr = new JSONArray();
            for (String tag : tags) {
                tagsJsonArr.put(tag);
            }

            try {
                if (tagsJsonArr.length() != 0) {
                    resultJson.put("tag", tagsJsonArr);
                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

        UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnJPushTagOperateResult", resultJson.toString());
    }

    @Override
    public void onCheckTagOperatorResult(Context context, JPushMessage jPushMessage) {
        super.onCheckTagOperatorResult(context, jPushMessage);

        JSONObject resultJson = new JSONObject();

        int sequence = jPushMessage.getSequence();
        try {
            resultJson.put("sequence", sequence);
            resultJson.put("code", jPushMessage.getErrorCode());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        if (jPushMessage.getErrorCode() == 0) {
            try {
                resultJson.put("tag", jPushMessage.getCheckTag());
                resultJson.put("isBind", jPushMessage.getTagCheckStateResult());
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

        UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnJPushTagOperateResult", resultJson.toString());
    }

    @Override
    public void onAliasOperatorResult(Context context, JPushMessage jPushMessage) {
        super.onAliasOperatorResult(context, jPushMessage);

        JSONObject resultJson = new JSONObject();

        int sequence = jPushMessage.getSequence();
        try {
            resultJson.put("sequence", sequence);
            resultJson.put("code", jPushMessage.getErrorCode());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        if (jPushMessage.getErrorCode() == 0) {
            try {
                if (!TextUtils.isEmpty(jPushMessage.getAlias())) {
                    resultJson.put("alias", jPushMessage.getAlias());
                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

        UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnJPushAliasOperateResult", resultJson.toString());
    }


    @Override
    public void onRegister(Context context, String regId) {
        UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnGetRegistrationId", regId);
    }

    @Override
    public void onMessage(Context context, CustomMessage customMessage) {
        String message = customMessage.message;//bundle.getString(JPushInterface.EXTRA_MESSAGE);
        String extras = customMessage.extra;//bundle.getString(JPushInterface.EXTRA_EXTRA);
        String messageId = customMessage.messageId;
        String msgStr = msg2str(messageId,message, extras);
        if (!TextUtils.isEmpty(JPushBridge.gameObject)) {
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveMessage", msgStr);
        } else {
            JPushBridge.receiveMessageStrCache.add(msgStr);
        }
    }

    @Override
    public void onNotifyMessageArrived(Context context, NotificationMessage notificationMessage) {
        super.onNotifyMessageArrived(context, notificationMessage);
        String content = notificationMessage.notificationContent;//bundle.getString(JPushInterface.EXTRA_ALERT);
        String title = notificationMessage.notificationTitle;//bundle.getString(JPushInterface.EXTRA_NOTIFICATION_TITLE);
        String extras = notificationMessage.notificationExtras;//bundle.getString(JPushInterface.EXTRA_EXTRA);
        String msgId = notificationMessage.msgId;
        String receiveNotiStr = noti2str(msgId,title, content, extras);

        Log.i(TAG, "GameObject: " + JPushBridge.gameObject);

        if (!TextUtils.isEmpty(JPushBridge.gameObject)) {
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveNotification", receiveNotiStr);
        } else {
            JPushBridge.receiveNotiStrCache.add(receiveNotiStr);
        }
    }

    @Override
    public void onNotifyMessageOpened(Context context, NotificationMessage notificationMessage) {
        JPushInterface.reportNotificationOpened(context, notificationMessage.msgId);//bundle.getString(JPushInterface.EXTRA_MSG_ID)

//        Intent launch = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName());
//        if (launch != null) {
//            launch.addCategory(Intent.CATEGORY_LAUNCHER);
//            launch.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_SINGLE_TOP);
//            context.startActivity(launch);
//        }

        String title = notificationMessage.notificationTitle;//bundle.getString(JPushInterface.EXTRA_NOTIFICATION_TITLE);
        String content = notificationMessage.notificationContent;//bundle.getString(JPushInterface.EXTRA_ALERT);
        String extras =notificationMessage.notificationExtras;// bundle.getString(JPushInterface.EXTRA_EXTRA);
        String msgId = notificationMessage.msgId;
        String openNotiStr = noti2str(msgId,title, content, extras);

        Log.i(TAG, "GameObject: " + JPushBridge.gameObject);

        if (!TextUtils.isEmpty(JPushBridge.gameObject)) {
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnOpenNotification", openNotiStr);
        } else {
            JPushBridge.openNotiStrCache = openNotiStr;
        }
    }

    @Override
    public void onMobileNumberOperatorResult(Context context, JPushMessage jPushMessage) {
        super.onMobileNumberOperatorResult(context, jPushMessage);


        JSONObject resultJson = new JSONObject();

        int sequence = jPushMessage.getSequence();
        try {
            resultJson.put("sequence", sequence);
            resultJson.put("code", jPushMessage.getErrorCode());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        if (jPushMessage.getErrorCode() == 0) {
            try {
                if (!TextUtils.isEmpty(jPushMessage.getMobileNumber())) {
                    resultJson.put("mobileNumber", jPushMessage.getMobileNumber());
                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }

        UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnMobileNumberOperatorResult", resultJson.toString());
    }

    private static String noti2str(String msgId,String title, String content, String extras) {

//        return ("{\"msgId:\""+msgId+"\",\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"extras\":" + extras + "}");

        Map<String,String> jsonMap = new HashMap<String, String>();
        jsonMap.put("msgId",msgId);
        jsonMap.put("title",title);
        jsonMap.put("content",content);
        jsonMap.put("extras",extras);
        return toJson(jsonMap);
    }

    private static String msg2str(String msgId,String content, String extras) {
//        return ("{\"msgId:\""+msgId+"\",\"message\":\"" + content + "\",\"extras\":" + extras + "}");
        Map<String,String> jsonMap = new HashMap<String, String>();
        jsonMap.put("msgId",msgId);
        jsonMap.put("message",content);
        jsonMap.put("extras",extras);
        return toJson(jsonMap);
    }

    private static String toJson(Map<String,String> jsonMap){
        StringBuffer buffer = new StringBuffer();
        buffer.append("{");

        if (!jsonMap.isEmpty()){
            Set<Map.Entry<String, String>> entries = jsonMap.entrySet();
            for (Map.Entry<String, String> e:
                    entries) {
                String key = e.getKey();
                String value = e.getValue();
                buffer.append("\"");
                buffer.append(key);
                buffer.append("\"");
                buffer.append(":");
                buffer.append("\"");
                buffer.append(value);
                buffer.append("\"");
                buffer.append(",");
            }
            buffer.deleteCharAt(buffer.length()-1);
        }

        buffer.append("}");
        return buffer.toString();
    }
}