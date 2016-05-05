using UnityEngine;
using System.Collections;

namespace JPush {

	public class CustomEventObj : CustomEvent
	{
		//JPush event types
		public static string EVENT_JPUSH = "event_jpush";
		public static string EVENT_INIT_JPUSH = "event_init_jpush";	// 初始化 JPush
		public static string EVENT_STOP_JPUSH = "event_stop_jpush";	// 停止 JPush 服务
		public static string EVENT_RESUME_JPUSH = "event_resume_jpush";	// 恢复 JPush 服务
		public static string EVENT_GET_REGISTRATION_ID = "event_get_registration_id";	// 获取 registrationId
		public static string EVENT_SET_TAGS = "event_set_tags";	// 设置标签
		public static string EVENT_SET_ALIAS = "event_set_alias";	// 设置别名
		public static string EVENT_SET_ALIAS_AND_TAGS = "event_set_alias_and_tags";	// 同时设置别名和标签
		public static string EVENT_SET_PUSH_TIME = "event_set_push_time";	// 设置推送时间
		public static string EVENT_SET_SILENCE_TIME = "event_set_silence_time";	// 设置推送静默时间
		public static string EVENT_ADD_LOCAL_NOTIFICATION = "event_add_local_notification";	// 添加本地通知
		public static string EVENT_REMOVE_LOCAL_NOTIFICATION = "event_remove_local_notification";	// 删除特定的本地通知
		public static string EVENT_CLEAR_LOCAL_NOTIFICATIONS = "event_clear_local_notifications";	// 清除本地通知
		public static string EVENT_CLEAR_ALL_NOTIFICATIONS = "event_clear_all_notifications";	// 清除所有由极光推送的通知
		public static string EVENT_REQUEST_PERMISSION = "event_request_permission";	// 获取权限(用于 Android 6.0 以上系统)
		public static string EVENT_SET_LATEST_NOTIFICATION_NUM = "event_set_latest_notification_num";	// 设置保留最近通知条数

		// optionally add custom variables instead of using the arguments hashtable
		public int myCustomEventVar1 = 0;
		public bool rockOn = true;

		public CustomEventObj(string eventType = "")
		{
			type = eventType;
		}

	}

}
