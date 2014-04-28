using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE
using System.Collections.Generic;
using LitJson;
#endif

namespace JPush{

	public class JPushBinding:MonoBehaviour{
		#if UNITY_ANDROID	
		private static AndroidJavaObject _plugin;
		public static string _gameObject = "" ;
		public static string _func ="" ; 
		
		static JPushBinding(){
			using(AndroidJavaClass jpushClass = new AndroidJavaClass("com.example.unity3d_jpush_demo.JPushBridge")){
				_plugin = jpushClass.CallStatic<AndroidJavaObject> ("getInstance");
			}
		}

		public static void setDebug(bool debug){
			_plugin.Call ("setDebug",debug);
		}
		
		public static void initJPush(){
			
		}		
		public static void initJPush(string gameObject , string func ) {
			Debug.Log("unity---initJPush") ;
			
			_gameObject = gameObject ;
			_func = func ;
			_plugin.Call("initJPush" , gameObject , func);
		}
		
		public static void stopJPush(){
			stopJPush(_gameObject , _func) ;			
		}		
		public static void stopJPush(string gameObject , string func){
			Debug.Log("unity---stopJPush") ;	
			_plugin.Call ("stopJPush" , gameObject , func);
		}
		
		public static void resumeJPush(){
			resumeJPush(_gameObject , _func) ;			
		}		
		public static void resumeJPush(string gameObject , string func){
			Debug.Log("unity---resumeJPush") ;			
			_plugin.Call ("resumeJPush" , gameObject , func);

		}
		
		//Tag为大小写字母,数字,下划线,中文; 多个用逗号分隔.
		//Tag can be number, alphabet, underscore, Chinese. Use , to split many tags.
		public static void setTags(string tags){
			setTags(_gameObject , _func , tags) ;
			
		}
		public static void setTags(string gameObject , string func ,string tags){
			Debug.Log("unity---setTags") ;
			_plugin.Call ("setTags" , gameObject , func , tags);
		}
		
		//Alias为大小写字母,数字,下划线; Alias can be number, alphabet, underscore, Chinese.
		public static void setAlias(string alias){
			setAlias(_gameObject , _func , alias) ;
			
		}
		public static void setAlias(string gameObject , string func ,string alias){
			Debug.Log("unity---setAlias") ;		
			_plugin.Call ("setAlias" , gameObject , func , alias);
		}
		
		public static void setPushTime(string days , string start_time , string end_time) {
			setPushTime(_gameObject , _func , days , start_time , end_time) ;
		}
		public static void setPushTime(string gameObject , string func , string days , string start_time , string end_time) {
			Debug.Log("unity---setPushTime") ;		
		   _plugin.Call ("setPushTime" , gameObject , func , days , start_time , end_time);

		}
		
		
		public static void isQuit() {
			Debug.Log("unity---isQuit") ;				
			_plugin.Call ("isQuit" ) ;
		}

		#endif

		#if UNITY_IPHONE

		public static	Action<int,HashSet<string>,string> _action;
		void Start () {	

			_printLocalLog("Start");		
			_registerNetworkDidReceiveMessage();

	   }

		void tagsWihtAliasCallBack(String jsonData)
		{
			_printLocalLog(jsonData);
			
			JsonData jd = JsonMapper.ToObject(jsonData);           
			int respoenCode = (int)jd["rescode"];   
			String alias = (String)jd["alias"];             
			JsonData jdItems = jd["tags"];       
			int itemCnt = jdItems.Count; 
			HashSet<string> set = new HashSet<string> ();

			for(int i=0;i<itemCnt;i++)
			{
				set.Add((String)jdItems[i]);             
			}
			_printLocalLog("rescode:"+respoenCode);
			_printLocalLog("alias:"+alias);
			_printLocalLog("tags:"+set.ToString());
			if(_action!=null)
			{
				_action(respoenCode,set,alias);
			}
		}
		void networkDidReceiveMessageCallBack(String parameter)
		{
			_printLocalLog("networkDidReceiveMessage:"+parameter);
			//		content = "unity3d_democ";
			//		extras =     {
			//		};
			JsonData jd = JsonMapper.ToObject(parameter);           
			String content = (String)jd["content"];             
			_printLocalLog("content:"+content);
			
		}
		
		public static void SetTagsWithAlias(HashSet<String> tags,String alias,Action<int,HashSet<string>,string> callBack){

			String[] arrayTags = new String[tags.Count];
			tags.CopyTo (arrayTags);
			Dictionary<String,object> data = new  Dictionary<String,object> ();
			data["tags"]  = arrayTags;
			data["alias"] = alias;
			
			String s=LitJson.JsonMapper.ToJson(data);
			_action = callBack;
			_setTagsAlias(s);
		}
		public static void SetTags(HashSet<String> tags){

			Hashtable data = new 	Hashtable ();
			data["tags"]  = tags;
			String s=LitJson.JsonMapper.ToJson(data);
			_setTags(s);
		}
		public static void SetAlias(String alias){
			
			JsonData jd = new JsonData ();
			jd ["alias"] = alias;
			String s = JsonMapper.ToJson (jd);
			_setAlias(s);

		}
		[DllImport ("__Internal")]
		public static extern void    _registerNetworkDidReceiveMessage();
		[DllImport ("__Internal")]
		private static extern void   _printLocalLog(String log);
		
		[DllImport ("__Internal")]
		public static extern void    _setTagsAlias(String tagsWithAlias);
		[DllImport ("__Internal")] 
		public static extern void    _setTags(String tags);
		[DllImport ("__Internal")]
		public static extern void    _setAlias(String alias);
		[DllImport ("__Internal")]
		public static extern  String _filterValidTags(String tags);
		[DllImport ("__Internal")]
		public static extern  String _openUDID();
		#endif


	}
}
