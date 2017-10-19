package cn.jiguang.unity.push;

import android.content.Context;
import android.text.TextUtils;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Set;

import cn.jpush.android.api.JPushMessage;
import cn.jpush.android.service.JPushMessageReceiver;

public class JPushEventReceiver extends JPushMessageReceiver {

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
}