using UnityEngine;
using System.Collections;
using JPush;
using System.Collections.Generic;
using System;


public class PluginsDemo : MonoBehaviour
{
    string str_unity = "";

    // Use this for initialization
    void Start()
    {
        gameObject.name = "Main Camera";
        JPushBinding.Init(gameObject.name);
        JPushBinding.SetDebug(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Home))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        str_unity = GUILayout.TextField(str_unity, GUILayout.Width(Screen.width - 80),
        GUILayout.Height(200));

        #if UNITY_ANDROID
        if (GUILayout.Button("stopPush", GUILayout.Height(80)))
        {
            JPushBinding.StopPush();
        }

        if (GUILayout.Button("resumePush", GUILayout.Height(80)))
        {
            JPushBinding.ResumePush();
        }
        #endif

        if (GUILayout.Button("setTags", GUILayout.Height(80)))
        {
            JPushBinding.SetTags(1, new List<string>() { str_unity });
        }

        if (GUILayout.Button("setAlias", GUILayout.Height(80)))
        {
            JPushBinding.SetAlias(2, str_unity);
        }

        #if UNITY_ANDROID
        if (GUILayout.Button("addLocalNotification", GUILayout.Height(80)))
        {
            JPushBinding.AddLocalNotification(0, "content", "title", 1, 0, null);
        }
        #endif

        if (GUILayout.Button("getRegistrationId", GUILayout.Height(80)))
        {
            string registrationId = JPushBinding.GetRegistrationId();
            Debug.Log("------>registrationId: " + registrationId);
        }
    }

    /* data format
     {
        "message": "hhh",
        "extras": {
            "f": "fff",
            "q": "qqq",
            "a": "aaa"
        }
     }
     */
    // 开发者自己处理由 JPush 推送下来的消息。
    void onReceiveMessage(string jsonStr)
    {
        Debug.Log("recv----message-----" + jsonStr);
        str_unity = jsonStr;
    }

    /**
     * {
     *	"title": "notiTitle",
     *   "content": "content",
     *   "extras": {
     *		"key1": "value1",
     *       "key2": "value2"
     * 	}
     * }
     */
    // 获取的是 json 格式数据，开发者根据自己的需要进行处理。
    void onReceiveNotification(string jsonStr)
    {
        Debug.Log("recv---notification---" + jsonStr);
    }

    //开发者自己处理点击通知栏中的通知
    void openNotification(string jsonStr)
    {
        Debug.Log("recv---openNotification---" + jsonStr);
        str_unity = jsonStr;
    }

    /// <summary>
    /// JPush 的 tag 操作回调。
    /// </summary>
    /// <param name="result">操作结果，为 json 字符串。</param>
    void OnJPushTagOperateResult(string result)
    {
        Debug.Log("JPush tag operate result: " + result);
        str_unity = result;
    }

    /// <summary>
    /// JPush 的 alias 操作回调。
    /// </summary>
    /// <param name="result">操作结果，为 json 字符串。</param>
    void OnJPushAliasOperateResult(string result)
    {
        Debug.Log("JPush alias operate result: " + result);
        str_unity = result;
    }
}