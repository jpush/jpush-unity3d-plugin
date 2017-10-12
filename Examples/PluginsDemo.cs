using UnityEngine;
using System.Collections;
using JPush;
using System.Collections.Generic;
using System;


public class PluginsDemo : MonoBehaviour
{
    #if UNITY_ANDROID
    string str_unity = "";
    bool B_MESSAGE = false;
    static string str_message = "";

    // Use this for initialization
    void Start()
    {
        gameObject.name = "Main Camera";
        JPushBinding.InitPush(gameObject.name);
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

        if (GUILayout.Button("stopPush", GUILayout.Height(80)))
        {
            JPushBinding.StopPush();
        }

        if (GUILayout.Button("resumePush", GUILayout.Height(80)))
        {
            JPushBinding.ResumePush();
        }

        if (GUILayout.Button("setTags", GUILayout.Height(80)))
        {
            JPushBinding.SetTags(1, new List<string>() { str_unity });
        }

        if (GUILayout.Button("setAlias", GUILayout.Height(80)))
        {
            JPushBinding.SetAlias(2, str_unity);
        }

        if (GUILayout.Button("addLocalNotification", GUILayout.Height(80)))
        {
            JPushBinding.AddLocalNotification(0, "content", "title", 1, 0, null);
        }

        if (GUILayout.Button("getRegistrationId", GUILayout.Height(80)))
        {
            string registrationId = JPushBinding.GetRegistrationId();
            Debug.Log("------>registrationId: " + registrationId);
        }

        if (GUILayout.Button("showMessage", GUILayout.Height(80)))
        {
            str_unity = str_message;
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
        B_MESSAGE = true;
        str_message = jsonStr;
        str_unity = "有新消息";
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

    #endif

    #if UNITY_IPHONE

    public string rid = "RegistrationID:";
    public string tag1 = "tag1";
    public string tag2 = "tag2";
    public string alias = "alias";
    public string tagAliasResult = "";
    public string notification = "";
    public string message = "";

    void Start ()
    {

    }

    void OnGUI ()
    {
        GUIStyle textFieldStyle = GUI.skin.textField;
        textFieldStyle.fontSize = 40;
        textFieldStyle.fixedWidth = 600;
        textFieldStyle.fixedHeight = 150;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle buttonStyle = GUI.skin.button;
        buttonStyle.fontSize = 40;
        buttonStyle.fixedWidth = 600;
        buttonStyle.fixedHeight = 150;

        GUIStyle labelStyle = new GUIStyle (textFieldStyle);
        labelStyle.fontSize = 28;
        labelStyle.fixedHeight = 100;
        labelStyle.alignment = TextAnchor.MiddleLeft;

        rid = GUILayout.TextField (rid + "\n" + JPush.JPushBinding.RegistrationID(), labelStyle);
        tag1 = GUILayout.TextField(tag1);
        tag2 = GUILayout.TextField(tag2);
        alias = GUILayout.TextField(alias);

        if (GUILayout.Button ("set tag/lias"))
        {
            JPush.JPushBinding._printLocalLog("set tag/alias");
            HashSet<String> tags = new HashSet<String>();
            tags.Add(tag1);
            tags.Add(tag2);
            JPush.JPushBinding.SetTagsWithAlias(tags,alias,(m,n,p)=>{
                tagAliasResult = "iResCode:" + m.ToString();
                tagAliasResult = tagAliasResult + "\ntags:" + n;
                tagAliasResult = tagAliasResult + "\nalias:" + p;
                JPush.JPushBinding._printLocalLog("callback");
            });

        }

        if(tagAliasResult != ""){
            GUILayout.Label(tagAliasResult, labelStyle);
        }

        if(notification != ""){
            GUILayout.Label(notification, labelStyle);
        }
        
        if (message != "") {
            GUILayout.Label(notification, labelStyle);
        }	
        
    }
    
    void OnUpdate()
    {
        
    }

    #endif
}
