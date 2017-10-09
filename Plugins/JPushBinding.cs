using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE
using System.Collections.Generic;
using LitJson;
#endif

namespace JPush
{
	public class JPushBinding : MonoBehaviour
	{
		#if UNITY_ANDROID
		private static AndroidJavaObject _plugin;
		public static string _gameObject = "";

		private static int notificationDefaults = -1;
		private static int notificationFlags = 16;

		private static readonly int DEFAULT_ALL = -1;
		private static readonly int DEFAULT_SOUND = 1;
		private static readonly int DEFAULT_VIBRATE = 2;
		private static readonly int DEFAULT_LIGHTS = 4;

		private static readonly int FLAG_SHOW_LIGHTS = 1;
		private static readonly int FLAG_ONGOING_EVENT = 2;
		private static readonly int FLAG_INSISTENT = 4;
		private static readonly int FLAG_ONLY_ALERT_ONCE = 8;
		private static readonly int FLAG_AUTO_CANCEL = 16;
		private static readonly int FLAG_NO_CLEAR = 32;
		private static readonly int FLAG_FOREGROUND_SERVICE = 64;

		static JPushBinding()
		{
			using(AndroidJavaClass jpushClass = new AndroidJavaClass("cn.jiguang.unity.push"))
			{
				_plugin = jpushClass.CallStatic<AndroidJavaObject>("getInstance");
			}
		}

		public static void setDebug(bool debug)
		{
			_plugin.Call("setDebug", debug);
		}

		/**
		 * 参数gameObject 代表游戏对象
		 * 参数func 代表回调的方法名
		 */
		public static void initJPush(string gameObject)
		{
			_gameObject = gameObject;
			_plugin.Call("initJPush", gameObject);
		}

		// 停止 JPush 推送服务。
		public static void stopJPush()
		{
			_plugin.Call("stopJPush");
		}

		// 唤醒 JPush 推送服务，使用了 stopJPush 必须调用此方法才能恢复。
		public static void resumeJPush()
		{
			_plugin.Call("resumeJPush");
		}

    // 判断当前 JPush 服务是否停止。
		public static bool isPushStopped()
		{
			return _plugin.Call<bool>("isPushStopped");
		}

		public static string getRegistrationId()
		{
			return _plugin.Call<string>("getRegistrationId");
		}

		public static string filterValidTags(string tags)
		{
			return _plugin.Call<string>("filterValidTags", tags);
		}

		// 设置设备标签。
    // tags 为多个 tag 组成的字符串(tag 为大小写字母,数字,下划线,中文;多个间用逗号分隔)。
		public static void setTags(int sequence, List<string> tags)
		{
      // TODO: 将 tags 转成 json 字符串。

			_plugin.Call("setTags", sequence, tags);
		}

    public static void addTags(int sequence, List<string> tags)
    {
			_plugin.Call("addTags", sequence, tags);
    }

		//设置设备别名.参数 Alias为大小写字母,数字,下划线
		public static void setAlias(int sequence, string alias)
		{
			_plugin.Call("setAlias", sequence, alias);
		}

    public static void deleteAlias(int sequence)
    {
      _plugin.Call("deleteAlias", sequence);
    }

    public static void getAlias(int sequence)
    {
      _plugin.Call("getAlias", sequence);
    }

		/**
		 * 设置允许推送时间。
		 * 参数 days: 为 0-6 之间由","连接而成的字符串
		 * 参数 startHour: 为 0-23 的字符串
		 * 参数 endHour: 为 0-23 的字符串
		 */
		public static void setPushTime(string days, int startHour, int endHour)
		{
		   _plugin.Call("setPushTime", days, start_time, end_time);
		}

		/**
		* 设置通知静默时间。
		* @param: startHour: 静默时段开始时间 - 小时（范围：0 - 23）
		* @param: startMinute: 静默时段开始时间 - 分钟（范围：0 - 59）
		* @param: endHour: 静默时段结束时间 - 小时（范围：0 - 23）
		* @param: endMinute: 静默时段结束时间 -  分钟（范围：0 - 59）
		*/
		public static void setSilenceTime(int startHour, int startMinute, int endHour, int endMinute) 
		{
			_plugin.Call("setSilenceTime", startHour, startMinute, endHour, endMinute);
		}

		public static void setLatestNotificationNumber(int num)
		{
			_plugin.Call("setLatestNotificationNumber", num);
		}

		public static void addLocalNotification(int builderId, string content, string title, int nId,
				int broadcastTime, string extrasStr)
		{
			_plugin.Call("addLocalNotification", builderId, content, title, nId, broadcastTime, extrasStr);
		}

		public static void addLocalNotificationByDate(int builderId, string content, string title, int nId,
				int year, int month, int day, int hour, int minute, int second, string extrasStr)
		{
			_plugin.Call("addLocalNotificationByDate", builderId, content, title, nId,
				year, month, day, hour, minute, second, extrasStr);
		}

		public static void removeLocalNotification(int notificationId)
		{
			_plugin.Call("removeLocalNotification", notificationId);
		}

		public static void clearLocalNotifications()
		{
			_plugin.Call("clearLocalNotifications");
		}

		public static void clearAllNotifications()
		{
			_plugin.Call("clearAllNotifications");
		}

		public static void clearNotificationById(int notificationId)
		{
			_plugin.Call("clearNotificationById", notificationId);
		}

		/**
		*	 用于 Android 6.0 及以上系统申请权限。
		*/
		public static void requestPermission()
		{
			_plugin.Call("requestPermission");
		}

		public static void setBasicPushNotificationBuilder()
		{
			// 需要根据自己业务情况修改后再调用。
			int builderId = 1;
			int notiDefaults = notificationDefaults | DEFAULT_ALL;
			int notiFlags = notificationFlags | FLAG_AUTO_CANCEL;
			_plugin.Call("setBasicPushNotificationBuilder", builderId, notiDefaults, notiFlags, null);
		}

		public static void setCustomPushNotificationBuilder()
		{
			// 需要根据自己业务情况修改后再调用。
			int builderId = 1;
			string layoutName = "yourNotificationLayoutName";

			// 指定最顶层状态栏小图标
			string statusBarDrawableName = "yourStatusBarDrawableName";

			// 指定下拉状态栏时显示的通知图标
			string layoutIconDrawableName = "yourLayoutIconDrawableName";

			_plugin.Call("setCustomPushNotificationBuilder", builderId, layoutName, statusBarDrawableName,
          layoutIconDrawableName);
		}

		public static void initCrashHandler()
		{
			_plugin.Call("initCrashHandler");
		}

		public static void stopCrashHandler()
		{
			_plugin.Call("stopCrashHandler");
		}

		#endif

      #if UNITY_IPHONE

      public static Action<int, HashSet<string>, string> _action;

      void Start()
      {
          _registerNetworkDidReceiveMessage();
          _registerNetworkDidReceivePushNotification();
      }

      //---------------------------- tags / alias ----------------------------//

      public static void SetTagsWithAlias(HashSet<String> tags, String alias, Action<int,HashSet<string>,string> callBack)
      {
          String[] arrayTags = new String[tags.Count];
          tags.CopyTo(arrayTags);
          Dictionary<String, object> data = new Dictionary<String, object>();
          data["tags"] = arrayTags;
          data["alias"] = alias;

          String s = LitJson.JsonMapper.ToJson(data);
          _action = callBack;
          _setTagsAlias(s);
      }

      public static void SetTags(HashSet<String> tags)
      {
          String[] arrayTags = new String[tags.Count];
          tags.CopyTo(arrayTags);
          Dictionary<String, object> data = new Dictionary<String, object>();
          data["tags"] = arrayTags;
          String s = LitJson.JsonMapper.ToJson(data);
          _setTags(s);
      }

      public static void SetAlias(String alias)
      {
          JsonData jd = new JsonData();
          jd["alias"] = alias;
          String s = JsonMapper.ToJson(jd);
          _setAlias(s);
      }

      void tagsWihtAliasCallBack(String jsonData)
      {
          _printLocalLog(jsonData);

          JsonData jd = JsonMapper.ToObject(jsonData);
          int responseCode = (int)jd["rescode"];
          String alias = (String)jd["alias"];
          JsonData jdItems = jd["tags"];
          int itemCnt = jdItems.Count;
          HashSet<string> set = new HashSet<string>();

          for(int i = 0; i < itemCnt; i++)
          {
              set.Add((String)jdItems[i]);
          }

          if(_action!=null)
          {
              _action(responseCode, set, alias);
          }
      }

      public static HashSet<String> FilterTags(HashSet<String> tags)
      {
          String[] arrayTags = new String[tags.Count];
          tags.CopyTo(arrayTags);

          Dictionary<String, object> data = new Dictionary<String, object>();
          data ["tags"] = arrayTags;
          String s = JsonMapper.ToJson(data);
          String filterTags = _filterValidTags(s);

          _printLocalLog(filterTags);

          JsonData jd = JsonMapper.ToObject(filterTags);

          JsonData jdItems = jd["tags"];
          int itemCnt = jdItems.Count;
          HashSet<string> set = new HashSet<string>();

          for(int i=0;i<itemCnt;i++)
          {
              set.Add((String)jdItems[i]);
          }

          return set;
      }

      //---------------------------- registrationID ----------------------------//

      public static String RegistrationID(){
          return _registrationID();
      }

      //---------------------------- notification / message ----------------------------//

      void networkDidReceiveMessageCallBack(String parameter){
          JsonData jd = JsonMapper.ToObject(parameter);
          String content = (String) jd["content"];
          _printLocalLog("content:" + content);
      }

      void networkDidReceivePushNotificationCallBack(String parameter){
          JsonData jd = JsonMapper.ToObject(parameter);
      }

      //---------------------------- badge ----------------------------//

      public static void SetBadge(int badge){
          _setBadge(badge);
      }

      public static void ResetBadge(){
          _resetBadge();
      }

      public static void SetApplicationIconBadgeNumber(int badge){
          _setApplicationIconBadgeNumber(badge);
      }

      public static int GetApplicationIconBadgeNumber(){
          return _getApplicationIconBadgeNumber();
      }

      //---------------------------- 页面统计 ----------------------------//

      public static void StartLogPageView(String pageName){
          JsonData jd = new JsonData();
          jd["pageName"] = pageName;
          String s = JsonMapper.ToJson(jd);
          _startLogPageView(s);
      }

      public static void StopLogPageView(String pageName){
          JsonData jd = new JsonData();
          jd["pageName"] = pageName;
          String s = JsonMapper.ToJson(jd);
          _stopLogPageView(s);
      }

      public static void BeginLogPageView(String pageName, int duration){
          JsonData jd = new JsonData();
          jd["pageName"] = pageName;
          String s = JsonMapper.ToJson(jd);
          _beginLogPageView(s, duration);
      }

      //---------------------------- 开关日志 ----------------------------//

      public static void SetDebugMode() {
          _setDebugMode();
      }

      public static void SetLogOFF(){
          _setLogOFF();
      }

      public static void CrashLogON(){
          _crashLogON();
      }

      //---------------------------- 本地推送 ----------------------------//

      public static void SetLocalNotification(int delay, String alertBody, int badge, String idKey){
          JsonData jd = new JsonData();
          jd["alertBody"] = alertBody;
          jd ["idKey"] = idKey;
          String s = JsonMapper.ToJson(jd);
          _setLocalNotification(delay, badge, s);
      }

      public static void DeleteLocalNotificationWithIdentifierKey(String idKey){
          JsonData jd = new JsonData();
          jd["idKey"] = idKey;
          String s = JsonMapper.ToJson(jd);
          _deleteLocalNotificationWithIdentifierKey(s);
      }

      public static void ClearAllLocalNotifications(){
          _clearAllLocalNotifications();
      }

      //---------------------------- 地理位置上报 ----------------------------//

      public static void SetLocation(String latitude, String longitude){
          JsonData jd = new JsonData();
          jd["latitude"] = latitude;
          jd["longitude"] = longitude;
          String s = JsonMapper.ToJson(jd);
          _setLocation(s);
      }



      //---------------------------- DllImport ----------------------------//

      //--- tags / alias ---//

      [DllImport("__Internal")]
      public static extern void _setTagsAlias(String tagsWithAlias);

      [DllImport("__Internal")]
      public static extern void _setTags(String tags);

      [DllImport("__Internal")]
      public static extern void _setAlias(String alias);

      [DllImport("__Internal")]
      public static extern String _filterValidTags(String tags);

      //--- rid ---//

      [DllImport("__Internal")]
      public static extern String _registrationID();

      //--- notification / message ---//

      [DllImport("__Internal")]
      public static extern void _registerNetworkDidReceiveMessage();

      [DllImport("__Internal")]
      public static extern void _registerNetworkDidReceivePushNotification();

      //--- badge ---//

      [DllImport("__Internal")]
      public static extern void _setBadge(int badge);

      [DllImport("__Internal")]
      public static extern void _resetBadge();

      [DllImport("__Internal")]
      public static extern void _setApplicationIconBadgeNumber(int badge);

      [DllImport("__Internal")]
      public static extern int _getApplicationIconBadgeNumber();

      //--- 页面统计 ---//

      [DllImport("__Internal")]
      public static extern void _startLogPageView(String pageName);

      [DllImport("__Internal")]
      public static extern void _stopLogPageView(String pageName);

      [DllImport("__Internal")]
      public static extern void _beginLogPageView(String pageName, int duration);

      //--- 开关日志 ---//

      [DllImport("__Internal")]
      public static extern void _setDebugMode();

      [DllImport("__Internal")]
      public static extern void _setLogOFF();

      [DllImport("__Internal")]
      public static extern void _crashLogON();

      //--- 本地推送 ---//

      [DllImport("__Internal")]
      public static extern void _setLocalNotification(int delay, int badge, String alertBodyAdnIdKey);

      [DllImport("__Internal")]
      public static extern void _deleteLocalNotificationWithIdentifierKey(String idKey);

      [DllImport("__Internal")]
      public static extern void _clearAllLocalNotifications();

      //--- 地理位置上报 ---//

      [DllImport("__Internal")]
      public static extern void _setLocation(String latitudeAndlongitude);

      //--- log ---//

      [DllImport("__Internal")]
      public static extern void _printLocalLog(String log);

      #endif
    }
}
