using UnityEngine;
using System.Collections;
using JPush ;
using System.Collections.Generic;
using System;


public class PluginsDemo : MonoBehaviour
{
	#if UNITY_ANDROID
	string str_unity = "" ;
	bool B_MESSAGE = false ;
	string str_message = "" ;
	// Use this for initialization
	void Start ()
	{
			gameObject.name = "Main Camera";
			JPushBinding.setDebug(true) ;
			JPushBinding.initJPush(gameObject.name , "") ;		
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_INIT_JPUSH , gameObject , "initJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_STOP_JPUSH , gameObject , "stopJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_RESUME_JPUSH , gameObject , "resumeJPush") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_TAGS , gameObject , "setTags") ;
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_ALIAS , gameObject , "setAlias") ;
		//TODO
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
		if (gameObject) {          
			// remove all events
			JPushEventManager.instance.removeAllEventListeners (gameObject);
		}		
		//TODO
	}
	void OnGUI ()
	{		
		str_unity = GUILayout.TextField(str_unity, GUILayout.Width(500),GUILayout.Height(150));
							
		if (GUILayout.Button ("initJPush", GUILayout.Height (80))) {
			JPushTriggerManager.triggerInitJPush (CustomEventObj.EVENT_INIT_JPUSH);
		}
		if (GUILayout.Button ("stopJPush", GUILayout.Height (80))) {
			JPushTriggerManager.triggerStopJPush (CustomEventObj.EVENT_STOP_JPUSH);
		}
		if (GUILayout.Button ("resumeJPush", GUILayout.Height (80))) {
			JPushTriggerManager.triggerResumeJPush (CustomEventObj.EVENT_RESUME_JPUSH);
		}
		if (GUILayout.Button ("setTags", GUILayout.Height (80))) {
			JPushTriggerManager.triggerSetTags ( CustomEventObj.EVENT_SET_TAGS , str_unity);
		}
		if (GUILayout.Button ("setAlias", GUILayout.Height (80))) {
			JPushTriggerManager.triggerSetAlias ( CustomEventObj.EVENT_SET_ALIAS , str_unity);
		}
				
		if (GUILayout.Button ("showMessage", GUILayout.Height (80))) {
			if(B_MESSAGE) {
				str_unity = str_message ;
				B_MESSAGE = false ;
			} else {
				//TODO no message
				str_unity = "no message" ;
			}
		}
		
		if (GUILayout.Button ("addTrigger---setPushTime", GUILayout.Height (80))) {
			// add a event
			JPushEventManager.instance.addEventListener (CustomEventObj.EVENT_SET_PUSH_TIME , gameObject , "setPushTime") ;
			
			string days = "0,1,2,3,4,5,6" ;
			string start_time = "10" ;
			string end_time = "18" ;
			JPushTriggerManager.triggerSetPushTime(CustomEventObj.EVENT_SET_PUSH_TIME , days , start_time , end_time) ;
		}
		if (GUILayout.Button ("removeTrigger---setPushTime", GUILayout.Height (80))) {
			// remove a single event
			JPushEventManager.instance.removeEventListener(CustomEventObj.EVENT_SET_PUSH_TIME, gameObject);
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
	#endif
	//#if UNITY_IPHONE
	public string tag1 = "tag1" ;
	public string tag2 = "tag2" ;
    public string tag3 = "tag3" ;

	public string alias = "alias" ;
	public string result;

	void Start ()
	{


	}
	void OnGUI ()
	{
		//tag1=GUI.TextField(Rect(10,10,80,20),tag1);
		//tag2=GUI.TextField(Rect(100,10,80,20),tag2);
		//tag3=GUI.TextField(Rect(190,10,80,20),tag3);

		tag1 = GUILayout.TextField(tag1, GUILayout.Width(100),GUILayout.Height(20));
		tag2 = GUILayout.TextField(tag2, GUILayout.Width(100),GUILayout.Height(20));
		tag3 = GUILayout.TextField(tag2, GUILayout.Width(100),GUILayout.Height(20));
		if (GUILayout.Button ("set tag/lias", GUILayout.Height (30))) {
			HashSet<String> tags=new HashSet<String>();
			tags.Add("tag1");
			tags.Add("tag2");
			tags.Add("tag3");
			JPush.JPushBinding.SetTagsWithAlias(tags,"bieming",(m,n,p)=>{

				result="respoen" +m.ToString();
				result="alias"   +p;
			});
		}
		GUILayout.Label(result, GUILayout.Width(100),GUILayout.Height(20));
	}
	void OnUpdate()
	{

	}


	//#endif


}
