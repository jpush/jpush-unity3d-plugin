using UnityEngine;
using System.Collections;

namespace JPush
{

	public class JPushTriggerManager : MonoBehaviour
	{
		public static void triggerInitJPush(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerStopJPush(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerResumeJPush(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetTags(string event_type, string tags)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("tags", tags);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetAlias(string event_type, string alias)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("alias", alias);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetPushTime(string event_type, string days,
				string start_time, string end_time)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("days", days);
			evt.arguments.Add("start_time", start_time);
			evt.arguments.Add("end_time", end_time);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerAddLocalNotification(string event_type,
				int builderId, string content, string title, int notificationId,
				int broadcastTime, string extrasStr)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("builderId", builderId);
			evt.arguments.Add("content", content);
			evt.arguments.Add("title", title);
			evt.arguments.Add("notificationId", notificationId);
			evt.arguments.Add("broadcastTime", broadcastTime);
			evt.arguments.Add("extras", extrasStr);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerRemoveLocalNotification(string event_type,
				int notificationId)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("notificationId", notificationId);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerClearLocalNotification(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

	}
}
