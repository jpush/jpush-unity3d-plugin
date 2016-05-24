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

		public static void triggerGetRegistrationId(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetAliasAndTags(string event_type,
				string alias, string tags)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("alias", alias);
			evt.arguments.Add("tags", tags);
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
				int startTime, int endTime)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("days", days);
			evt.arguments.Add("startTime", startTime);
			evt.arguments.Add("endTime", endTime);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggertSetSilenceTime(string event_type, int startHour,
				int startMinute, int endHour, int endMinute)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("startHour", startHour);
			evt.arguments.Add("startMinute", startMinute);
			evt.arguments.Add("endHour", endHour);
			evt.arguments.Add("endMinute", endMinute);
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

		public static void triggerClearLocalNotifications(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerClearAllNotifications(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerRequestPermission(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetLatestNotificationNumber(string event_type,
				int num)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			evt.arguments.Add("maxNum", num);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetBasicPushNotificationBuilder(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

		public static void triggerSetCustomPushNotificationBuilder(string event_type)
		{
			CustomEventObj evt = new CustomEventObj(event_type);
			JPushEventManager.instance.dispatchEvent(evt);
		}

	}
}
