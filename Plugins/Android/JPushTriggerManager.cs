using UnityEngine;  
using System.Collections;  

namespace JPush {
	
	public class JPushTriggerManager : MonoBehaviour  
	{  
		public static void triggerInitJPush(string event_type) {
			CustomEventObj evt = new CustomEventObj (event_type);
			JPushEventManager.instance.dispatchEvent (evt); 
		}
		
		public static void triggerStopJPush(string event_type) {
			CustomEventObj evt = new CustomEventObj (event_type);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerResumeJPush(string event_type) {
			CustomEventObj evt = new CustomEventObj (event_type);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerSetTags(string event_type , string tags) {
			CustomEventObj evt = new CustomEventObj (event_type);
			evt.arguments.Add ("tags", tags);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerSetAlias(string event_type , string alias) {
			CustomEventObj evt = new CustomEventObj (event_type);
			evt.arguments.Add ("alias", alias);
			JPushEventManager.instance.dispatchEvent (evt);
		}
		
		public static void triggerSetPushTime(string event_type) {
			CustomEventObj evt = new CustomEventObj (event_type);			
			evt.arguments.Add ("days", "0,1,2,3,4,5,6");
			evt.arguments.Add ("start_time" , "11") ;
			evt.arguments.Add ("end_time" , "20") ;
			JPushEventManager.instance.dispatchEvent (evt);
		}
				
	}  
}