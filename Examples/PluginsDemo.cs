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
		JPushBinding.setDebug(true);
		JPushBinding.initJPush(gameObject.name, "");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_INIT_JPUSH, gameObject, "initJPush");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_STOP_JPUSH, gameObject, "stopJPush");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_RESUME_JPUSH, gameObject, "resumeJPush");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_SET_TAGS, gameObject, "setTags");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_SET_ALIAS, gameObject, "setAlias");

		JPushEventManager.instance.addEventListener(
			CustomEventObj.EVENT_ADD_LOCAL_NOTIFICATION, gameObject,
			"addLocalNotification");
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Home))
		{
			beforeQuit();
			Application.Quit();
		}
	}

	// remove event listeners
	void OnDestroy()
	{
		print("unity3d---onDestroy");
		if (gameObject)
		{
			// remove all events
			JPushEventManager.instance.removeAllEventListeners(gameObject);
		}
	}

	void OnGUI()
	{
		str_unity = GUILayout.TextField(str_unity, GUILayout.Width(Screen.width - 80),
			GUILayout.Height(200));

		if (GUILayout.Button("initJPush", GUILayout.Height(80)))
		{
			JPushTriggerManager.triggerInitJPush(CustomEventObj.EVENT_INIT_JPUSH);
		}

		if (GUILayout.Button("stopJPush", GUILayout.Height(80)))
		{
			JPushTriggerManager.triggerStopJPush(CustomEventObj.EVENT_STOP_JPUSH);
		}

		if (GUILayout.Button("resumeJPush", GUILayout.Height(80)))
		{
			JPushTriggerManager.triggerResumeJPush(CustomEventObj.EVENT_RESUME_JPUSH);
		}

		if (GUILayout.Button("setTags", GUILayout.Height(80)))
		{
			JPushTriggerManager.triggerSetTags(CustomEventObj.EVENT_SET_TAGS, str_unity);
		}

		if (GUILayout.Button("setAlias", GUILayout.Height(80)))
		{
			JPushTriggerManager.triggerSetAlias(CustomEventObj.EVENT_SET_ALIAS, str_unity);
		}

		if (GUILayout.Button("addLocalNotification", GUILayout.Height(80)))
		{
			JPushBinding.addLocalNotification(0, "content", "title", 1, 0, null);
		}

		if (GUILayout.Button("getRegistrationId", GUILayout.Height(80)))
		{
			string registrationId = JPushBinding.getRegistrationId();
			Debug.Log("------>registrationId: " + registrationId);
		}

		if (GUILayout.Button("showMessage", GUILayout.Height(80)))
		{
			str_unity = str_message;
			/*if(B_MESSAGE) {
				str_unity = str_message;
				B_MESSAGE = false;
			} else {
				//TODO no message
				str_unity = "no message";
			}*/
		}

		if (GUILayout.Button("addTrigger---setPushTime", GUILayout.Height(80)))
		{
			// add a event
			JPushEventManager.instance.addEventListener(
				CustomEventObj.EVENT_SET_PUSH_TIME, gameObject, "setPushTime");
			string days = "0,1,2,3,4,5,6";
			int start_time = 10;
			int end_time = 18;
			JPushTriggerManager.triggerSetPushTime(
				CustomEventObj.EVENT_SET_PUSH_TIME, days, start_time, end_time);
		}

		if (GUILayout.Button ("removeTrigger---setPushTime", GUILayout.Height(80)))
		{
			// remove a single event
			JPushEventManager.instance.removeEventListener(
				CustomEventObj.EVENT_SET_PUSH_TIME, gameObject);
		}

	}

	void initJPush(CustomEventObj evt)
	{
		Debug.Log("---triggered initjpush----");
		JPushBinding.initJPush(gameObject.name, "");
		//JPushBridge.initJPush();
	}

	void stopJPush(CustomEventObj evt)
	{
		Debug.Log("--triggered stopJPush----");
		JPushBinding.stopJPush();
	}

	void resumeJPush(CustomEventObj evt)
	{
		Debug.Log("---triggered resumeJPush----");
		JPushBinding.resumeJPush();
	}

	void setTags(CustomEventObj evt)
	{
		Debug.Log("---triggered setTags----");
		string tags = (string)evt.arguments["tags"];
		JPushBinding.setTags(tags);
	}

	void setAlias(CustomEventObj evt)
	{
		Debug.Log("---triggered setAlias----");
		string alias = (string) evt.arguments["alias"];
		JPushBinding.setAlias(alias);
	}

	void setPushTime(CustomEventObj evt)
	{
		Debug.Log("---triggered setPushTime----");
		string days = (string) evt.arguments["days"];
		int start_time = (int) evt.arguments["start_time"];
		int end_time = (int) evt.arguments["end_time"];
		JPushBinding.setPushTime(days, start_time, end_time);
	}

	void addLocalNotification(CustomEventObj evt)
	{
		Debug.Log("---triggered addLocalNotification---");
		int builderId = (int) evt.arguments["builderId"];
		string content = (string) evt.arguments["content"];
		string title = (string) evt.arguments["title"];
		int notiId = (int) evt.arguments["notificationId"];
		int broadcastTime = (int) evt.arguments["broadcastTime"];
		string extrasStr = (string) evt.arguments["extras"];
		JPushBinding.addLocalNotification(builderId, content, title, notiId,
			broadcastTime, extrasStr);
	}

	void removeLocalNotification(CustomEventObj evt)
	{
		Debug.Log("---triggered removeLocalNotification---");
		int notiId = (int) evt.arguments["notificationId"];
		JPushBinding.removeLocalNotification(notiId);
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
	//开发者自己处理由JPush推送下来的消息
	void recvMessage(string jsonStr)
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
	void recvNotification(string jsonStr)
	{
		Debug.Log("recv---notification---" + jsonStr);
	}

	//开发者自己处理点击通知栏中的通知
	void openNotification(string jsonStr)
	{
		Debug.Log("recv---openNotification---" + jsonStr);
		str_unity = jsonStr;
	}

	void beforeQuit()
	{
		JPushBinding.isQuit();
	}
	#endif

	#if UNITY_IPHONE
	public string tag1 = "tag1";
	public string tag2 = "tag2";
    public string tag3 = "tag3";

	public string alias = "alias";
	public string result;

	void Start ()
	{

	}

	void OnGUI ()
	{
		tag1 = GUILayout.TextField(tag1, GUILayout.Width(300), GUILayout.Height(150));
		tag2 = GUILayout.TextField(tag2, GUILayout.Width(300), GUILayout.Height(150));
		tag3 = GUILayout.TextField(tag2, GUILayout.Width(300), GUILayout.Height(130));
		if (GUILayout.Button ("set tag/lias", GUILayout.Height (200)))
		{
			JPush.JPushBinding._printLocalLog("set tag/alias");
			HashSet<String> tags = new HashSet<String>();
			tags.Add("tag1");
			tags.Add("tag2");
			tags.Add("tag3");
			JPush.JPushBinding.SetTagsWithAlias(tags,"bieming",(m,n,p)=>{
				result = "respoen" + m.ToString();
				result = "alias"   + p;
				JPush.JPushBinding._printLocalLog("callbakc2");
			});
		}
		GUILayout.Label(result, GUILayout.Width(300), GUILayout.Height(400));
	}

	void OnUpdate()
	{

	}
	#endif

}
