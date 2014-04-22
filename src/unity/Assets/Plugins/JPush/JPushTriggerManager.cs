using UnityEngine;  
using System.Collections;  

namespace JPush {
	
	public class JPushTriggerManager : MonoBehaviour  
	{  
		public static void triggerInitJPush() {
			CustomEventObj evt = new CustomEventObj (CustomEventObj.EVENT_INIT_JPUSH);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerStopJPush() {
			CustomEventObj evt = new CustomEventObj (CustomEventObj.EVENT_STOP_JPUSH);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerResumeJPush() {
			CustomEventObj evt = new CustomEventObj (CustomEventObj.EVENT_RESUME_JPUSH);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerSetTags(string tags) {
			CustomEventObj evt = new CustomEventObj (CustomEventObj.EVENT_SET_TAGS);
			evt.arguments.Add ("tags", tags);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerSetAlias(string alias) {
			CustomEventObj evt = new CustomEventObj (CustomEventObj.EVENT_SET_ALIAS);
			evt.arguments.Add ("alias", alias);
			JPushEventManager.instance.dispatchEvent (evt);
		}
				
	}  
}