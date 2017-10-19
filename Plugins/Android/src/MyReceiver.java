package cn.jiguang.unity.push;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.unity3d.player.UnityPlayer;

import cn.jpush.android.api.JPushInterface;

/**
 * 自定义接收器
 * <p>
 * 如果不定义这个 Receiver，则：
 * 1) 默认用户会打开主界面
 * 2) 接收不到自定义消息
 */
public class MyReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {
        Bundle bundle = intent.getExtras();
        if (JPushInterface.ACTION_REGISTRATION_ID.equals(intent.getAction())) {
            String regId = bundle.getString(JPushInterface.EXTRA_REGISTRATION_ID);
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnGetRegistrationId", regId);

        } else if (JPushInterface.ACTION_MESSAGE_RECEIVED.equals(intent.getAction())) {
            String message = bundle.getString(JPushInterface.EXTRA_MESSAGE);
            String extras = bundle.getString(JPushInterface.EXTRA_EXTRA);
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveMessage", msg2str(message, extras));

        } else if (JPushInterface.ACTION_NOTIFICATION_RECEIVED.equals(intent.getAction())) {
            String content = bundle.getString(JPushInterface.EXTRA_ALERT);
            String title = bundle.getString(JPushInterface.EXTRA_NOTIFICATION_TITLE);
            String extras = bundle.getString(JPushInterface.EXTRA_EXTRA);
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnReceiveNotification",
                    noti2str(title, content, extras));

        } else if (JPushInterface.ACTION_NOTIFICATION_OPENED.equals(intent.getAction())) {
            JPushInterface.reportNotificationOpened(context, bundle.getString(JPushInterface.EXTRA_MSG_ID));

            Intent launch = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName());
            launch.addCategory(Intent.CATEGORY_LAUNCHER);
            launch.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            context.startActivity(launch);

            String title = bundle.getString(JPushInterface.EXTRA_NOTIFICATION_TITLE);
            String content = bundle.getString(JPushInterface.EXTRA_ALERT);
            String extras = bundle.getString(JPushInterface.EXTRA_EXTRA);
            UnityPlayer.UnitySendMessage(JPushBridge.gameObject, "OnOpenNotification",
                    noti2str(title, content, extras));
        }
    }

    private static String noti2str(String title, String content, String extras) {
        return ("{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"extras\":" + extras + "}");
    }

    private static String msg2str(String content, String extras) {
        return ("{\"message\":\"" + content + "\",\"extras\":" + extras + "}");
    }
}