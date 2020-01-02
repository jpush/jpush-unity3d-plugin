using UnityEngine;
using System.Collections;
using JPush;
using System.Collections.Generic;
using System;

#if UNITY_IPHONE
using LitJson;
#endif

public class PluginsDemo : MonoBehaviour
{
    string str_unity = "";
	  int callbackId = 0;

    // Use this for initialization
    void Start()
    {
        gameObject.name = "Main Camera";
        JPushBinding.SetDebug(true);
        JPushBinding.Init(gameObject.name);
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
            List<string> tags = new List<string> ();
            tags.Add("111");
            tags.Add("222");
			      JPushBinding.SetTags(callbackId++, tags);
        }

        if (GUILayout.Button("setAlias", GUILayout.Height(80)))
        {
            JPushBinding.SetAlias(2, "replaceYourAlias");
        }

        #if UNITY_ANDROID
        if (GUILayout.Button("addLocalNotification", GUILayout.Height(80)))
        {
            // JPushBinding.AddLocalNotification(0, "content", "title", 1, 0, null);
            JPushBinding.AddLocalNotificationByDate(0, "内容", "标题", 1, 2017, 11, 16, 13, 40, 0, "");
        }
        #endif

        if (GUILayout.Button("getRegistrationId", GUILayout.Height(80)))
        {
            string registrationId = JPushBinding.GetRegistrationId();
            Debug.Log("------>registrationId: " + registrationId);
            
        }

        if (GUILayout.Button("addTags", GUILayout.Height(80)))
        {
            List<string> tags = new List<string>(){"addtag1", "addtag2"};
            JPushBinding.AddTags(callbackId++, tags);
        }

        if (GUILayout.Button("deleteTags", GUILayout.Height(80)))
        {
            List<string> tags = new List<string>();
            tags.Add("addtag1");
            tags.Add("addtag2");

            JPushBinding.DeleteTags(callbackId++, tags);
        }

        if (GUILayout.Button("cleanTags", GUILayout.Height(80)))
        {
            JPushBinding.CleanTags(callbackId++);
        }

        if (GUILayout.Button("get all tags", GUILayout.Height(80)))
        {
            JPushBinding.GetAllTags(callbackId++);
        }

        if (GUILayout.Button("getAlias", GUILayout.Height(80)))
        {
            JPushBinding.GetAlias(callbackId++);
            Debug.Log("Alias 将在 OnJPushTagOperateResult 中回调");
        }

        if (GUILayout.Button("check tag is binding", GUILayout.Height(80)))
        {
            JPushBinding.CheckTagBindState(callbackId++,"addtag1");
            Debug.Log("Alias 将在 OnJPushTagOperateResult 中回调");
        }

        if (GUILayout.Button("filterValidTags", GUILayout.Height(80)))
        {
            List<string> tags = new List<string>();
            tags.Add("1");
            tags.Add("2");
            tags.Add("3");
            tags.Add("4");
            List<string> reList = JPushBinding.FilterValidTags(tags);
            string str = string.Join(",", reList.ToArray());
            Debug.Log("filterValidTags reList:" + str);
            str_unity = str;
        }

        if (GUILayout.Button("setMobileNumber", GUILayout.Height(80)))
        {
            JPushBinding.SetMobileNumber(callbackId++, "12345678921");
        }


#if UNITY_ANDROID
        if (GUILayout.Button("setChannel", GUILayout.Height(80)))
        {
            JPushBinding.SetChannel("unityChannel");
        }

#endif

#if UNITY_IPHONE || UNITY_IOS
        if (GUILayout.Button("Trigger local notification after 3 seconds", GUILayout.Height(80)))
        {
            JsonData paramsJ = new JsonData();
            paramsJ["title"] = "the title";
            paramsJ["id"] = "5";
            paramsJ["content"] = "the content";
            paramsJ["badge"] = 9;
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            long ret = Convert.ToInt64(ts.TotalSeconds) + 3;
            paramsJ["fireTime"] = ret;
            paramsJ["subtitle"] = "the subtitle";

            JPushBinding.SendLocalNotification(paramsJ.ToJson());
        }

        if (GUILayout.Button("remove Notification All", GUILayout.Height(80)))
        {
            JPushBinding.RemoveNotificationAll();
        }
#endif
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
    void OnReceiveMessage(string jsonStr)
    {
        Debug.Log("recv----message-----" + jsonStr);
        str_unity = jsonStr;
    }

    /**
     * {
     *	 "title": "notiTitle",
     *   "content": "content",
     *   "extras": {
     *		"key1": "value1",
     *       "key2": "value2"
     * 	}
     * }
     */
    // 获取的是 json 格式数据，开发者根据自己的需要进行处理。
    void OnReceiveNotification(string jsonStr)
    {
        Debug.Log("recv---notification---" + jsonStr);
    }

    //开发者自己处理点击通知栏中的通知
    void OnOpenNotification(string jsonStr)
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

    void OnGetRegistrationId(string result) {
        Debug.Log("JPush on get registration Id: " + result);
        str_unity = "JPush on get registration Id: " + result;		
    }

    void OnMobileNumberOperatorResult(string result)
    {
        Debug.Log("JPush On Mobile Number Operator Result: " + result);
        str_unity = "JPush On Mobile Number Operator Result: " + result;
    }

}
