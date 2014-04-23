using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace JPush{

	public class JPushBinding{
		private static AndroidJavaObject _plugin;
		public static string _gameObject = "" ;
		public static string _func ="" ; 
		public static bool ANDROID_PLATFORM = false ;		
		public static bool IPHONE_PLATFORM = false ;
		
		static JPushBinding(){
			Debug.Log("--------unity static JPushBridge---------" );
						
			if(Application.platform == RuntimePlatform.Android) {
				ANDROID_PLATFORM = true ;
				using(AndroidJavaClass jpushClass = new AndroidJavaClass("com.example.unity3d_jpush_demo.JPushBridge"))
					_plugin = jpushClass.CallStatic<AndroidJavaObject> ("getInstance");
			}
			else if(Application.platform == RuntimePlatform.IPhonePlayer) {
				//TODO
				IPHONE_PLATFORM = true ;
			}
			else {
				//TODO
			}
		}
		
		public static void setDebug(bool debug){
			if(ANDROID_PLATFORM) 
				_plugin.Call ("setDebug",debug);
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
						
		}
		
		public static void initJPush(){
			initJPush(_gameObject , _func) ;
			
		}		
		public static void initJPush(string gameObject , string func ) {
			Debug.Log("unity---initJPush") ;
					
			if(ANDROID_PLATFORM) {
				_gameObject = gameObject ;
				_func = func ;
				_plugin.Call("initJPush" , gameObject , func);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
		}
		
		public static void stopJPush(){
			stopJPush(_gameObject , _func) ;			
		}		
		public static void stopJPush(string gameObject , string func){
			Debug.Log("unity---stopJPush") ;
			
			if(ANDROID_PLATFORM) {
				_plugin.Call ("stopJPush" , gameObject , func);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
			
		}

		public static void resumeJPush(){
			resumeJPush(_gameObject , _func) ;			
		}		
		public static void resumeJPush(string gameObject , string func){
			Debug.Log("unity---resumeJPush") ;
			
			if(ANDROID_PLATFORM) {
				_plugin.Call ("resumeJPush" , gameObject , func);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
			
		}
		
		//Tag为大小写字母,数字,下划线,中文; 多个用逗号分隔.
		//Tag can be number, alphabet, underscore, Chinese. Use , to split many tags.
		public static void setTags(string tags){
			setTags(_gameObject , _func , tags) ;
		
		}
		public static void setTags(string gameObject , string func ,string tags){
			Debug.Log("unity---setTags") ;
			
			if(ANDROID_PLATFORM) {
				_plugin.Call ("setTags" , gameObject , func , tags);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
		
		}
		
		//Alias为大小写字母,数字,下划线; Alias can be number, alphabet, underscore, Chinese.
		public static void setAlias(string alias){
			setAlias(_gameObject , _func , alias) ;
		
		}
		public static void setAlias(string gameObject , string func ,string alias){
			Debug.Log("unity---setAlias") ;		
		
			if(ANDROID_PLATFORM) {
				_plugin.Call ("setAlias" , gameObject , func , alias);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
		}
		
		public static void setPushTime(string days , string start_time , string end_time) {
			setPushTime(_gameObject , _func , days , start_time , end_time) ;
		}
		public static void setPushTime(string gameObject , string func , string days , string start_time , string end_time) {
			Debug.Log("unity---setPushTime") ;		
		
			if(ANDROID_PLATFORM) {
				_plugin.Call ("setPushTime" , gameObject , func , days , start_time , end_time);
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
		}
		
		
		public static void isQuit() {
			Debug.Log("unity---isQuit") ;	
			
			if(ANDROID_PLATFORM) {
				_plugin.Call ("isQuit" ) ;
			}
			else if(IPHONE_PLATFORM) {
				//TODO
			}
			else {
				//TODO
			}
		}
				
	}
}
