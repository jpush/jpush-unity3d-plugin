using UnityEngine;
using System.Collections;
using JPush ;

public class PluginsDemo : MonoBehaviour
{
	string str_unity = "" ;
	bool B_MESSAGE = false ;
	string str_message = "" ;
	
	// Use this for initialization
	void Start ()
	{
		if(JPushBinding.ANDROID_PLATFORM) {
			//TODO			
			gameObject.name = "Main Camera";
					
			JPushBinding.setDebug(true) ;
			JPushBinding.initJPush(gameObject.name , "") ;		
			
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_INIT_JPUSH , gameObject , "initJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_STOP_JPUSH , gameObject , "stopJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_RESUME_JPUSH , gameObject , "resumeJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_TAGS , gameObject , "setTags") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_ALIAS , gameObject , "setAlias") ;
			
		} else if(JPushBinding.IPHONE_PLATFORM) {
			//TODO
		} else {
			//TODO default
		}		
		
	}
		
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Home)){
			beforeQuit() ;
			Application.Quit();
		}		
	}
	
	// remove event listeners
	void OnDestroy ()
	{
		print ("unity3d---onDestroy") ;		
		
		if(JPushBinding.ANDROID_PLATFORM) {
			//TODO
		}
		else if(JPushBinding.IPHONE_PLATFORM) {
			//TODO
		} 
		else {
			//TODO
		}
				
		if (gameObject) {          
			// remove all events
			JPushEventManager.instance.removeAllEventListeners (gameObject);
		}
		
	}
	
	
	void OnGUI ()
	{		
		str_unity = GUILayout.TextField(str_unity, GUILayout.Width(500),GUILayout.Height(150));
							
		if (GUILayout.Button ("initJPush", GUILayout.Height (100))) {
			JPushTriggerManager.triggerInitJPush (CustomEventObj.EVENT_INIT_JPUSH);
		}
		if (GUILayout.Button ("stopJPush", GUILayout.Height (100))) {
			JPushTriggerManager.triggerStopJPush (CustomEventObj.EVENT_STOP_JPUSH);
		}
		if (GUILayout.Button ("resumeJPush", GUILayout.Height (100))) {
			JPushTriggerManager.triggerResumeJPush (CustomEventObj.EVENT_RESUME_JPUSH);
		}
		if (GUILayout.Button ("setTags", GUILayout.Height (100))) {
			JPushTriggerManager.triggerSetTags ( CustomEventObj.EVENT_SET_TAGS , str_unity);
		}
		if (GUILayout.Button ("setAlias", GUILayout.Height (100))) {
			JPushTriggerManager.triggerSetAlias ( CustomEventObj.EVENT_SET_ALIAS , str_unity);
		}
				
		if (GUILayout.Button ("showMessage", GUILayout.Height (100))) {
			if(B_MESSAGE) {
				str_unity = str_message ;
				B_MESSAGE = false ;
			} else {
				//TODO no message
				str_unity = "no message" ;
			}
		}
		
		if (GUILayout.Button ("removeTrigger", GUILayout.Height (100))) {
			// remove a single event
			JPushEventManager.instance.removeEventListener(CustomEventObj.EVENT_SET_PUSH_TIME, gameObject);
		}
		if (GUILayout.Button ("addTrigger", GUILayout.Height (100))) {
			// add a event
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_PUSH_TIME , gameObject , "setPushTime") ;
			
			JPushTriggerManager.triggerSetPushTime(CustomEventObj.EVENT_SET_PUSH_TIME) ;
		}
	}
		
	void initJPush(CustomEventObj evt) {
		Debug.Log("---triggered initjpush----") ;
		JPushBinding.initJPush(gameObject.name , "") ;
		//JPushBridge.initJPush() ;
	}
	
	void stopJPush(CustomEventObj evt) {
		Debug.Log("--triggered stopJPush----") ;
		JPushBinding.stopJPush() ;
	}
	
	void resumeJPush(CustomEventObj evt) {
		Debug.Log("---triggered resumeJPush----") ;
		JPushBinding.resumeJPush() ;
	}
	
	void setTags(CustomEventObj evt) {
		Debug.Log("---triggered setTags----") ;
		string tags = (string)evt.arguments["tags"] ;
		JPushBinding.setTags(tags) ;
	}
	 
	void setAlias(CustomEventObj evt) {
		Debug.Log("---triggered setAlias----") ;
		string alias = (string) evt.arguments["alias"] ;
		JPushBinding.setAlias(alias) ;
	}
	
	void setPushTime(CustomEventObj evt) { 
		Debug.Log("---triggered setPushTime----") ;
		string days = (string) evt.arguments["days"] ;
		string start_time = (string) evt.arguments["start_time"] ;
		string end_time = (string) evt.arguments["end_time"] ;
		JPushBinding.setPushTime(days , start_time , end_time) ;
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
	void recvMessage(string str) {
		Debug.Log("recv----message-----" + str) ; 
		B_MESSAGE = true ;
		str_message = str ;
		
	} 
	
	//开发者自己处理点击通知栏中的通知
	void openNotification(string str) {
		Debug.Log ("recv --- openNotification---" + str) ;
		str_unity = str ;
	}
		
	void beforeQuit(){
		JPushBinding.isQuit() ;
	}
}
