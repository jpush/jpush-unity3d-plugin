JPush Unity3d Plugin
====================

这是极光推送官方支持的 Unity3d 插件（Android &amp; iOS）。

## 导入到 Unity 项目

* 搭建好unity3d(ios/android)开发环境

* 将git下来的jpush-unity3d-plugin.unitypackage导入到Unity中

## 集成 JPush Unity Android SDK

### 基本动作

* 替换AndroidManifest.xml里的包名  
  将其中的“com.jpush.unity3dplugin”替换成你在JPush服务器上创建的应用的应用包名.

* 配置Appkey  
  将其中的"JPUSH_APPKEY"值替换成应用详情里应用标(AppKey)的值.

* 配置项目里的包名  
  在unity中选择"File---Build Settings---Player Settings...",将"Identification"选项下的"Bundle Idenifier*" 设置为应用的包名.

* 运行  
  设置OK,直接"Build&Run"即可.


### API 功能说明

* initJPush(string gameObject, string func)  
  功能描述：启用JPush推送服务.  
  参数说明：  
  * gameObject 游戏对象; 
  * func 回调的方法名.
  
* stopJPush(string gameObject, string func)    
  功能描述：停止JPush推送服务(必须通过resumeJPush再次启用.)  
  参数说明：
  * gameObject 游戏对象;
  * func 回调的方法名.

* resumeJPush(string gameObject, string func)  
  功能描述：重新启用JPush推送服务(如果是通过stopJPush来停止的,再次启用必须调用此方法).  
  参数说明：
  * gameObject 游戏对象;
  * func 回调的方法名.

* setTags(string gameObject, string func, string tags)  
  功能描述：设置设备标签.  
  参数说明：
  * gameObject 游戏对象;
  * func 回调的方法名;
  * tags 为多个Tag组成的字符串.(Tag为大小写字母,数字,下划线,中文; 多个用逗号分隔).

* setAlias(string gameObject, string func, string alias)  
  功能描述：设置设备别名.  
  参数说明：
  * gameObject 游戏对象;
  * func 回调的方法名;
  * alias 为大小写字母,数字,下划线,中文组成的字符串.

* setPushTime(string gameObject, string func, string days, string start_time, string end_time)   
  功能描述：设置接收推送的时间段.  
  参数说明：
  * gameObject 游戏对象;
  * func 回调的方法名;
  * days 为0-6之间由","连接而成的字符串;  
  * start_time 为0-23的字符串;
  * end_time 为0-23的字符串.

> 提示: 在 Plugins\Android\src 目录下是一些 java 文件,你可以将其引入到 android 工程中对它们进行扩展。重新生成jar替换掉 unity工程中
 Assets\Plugins\Android 目录下的 JPush_Bridge.jar 文件.


### Example 说明

* 在unity Assets\Plugins\Demo下的 PluginsDemo.cs 文件是一个测试脚本,其运行结果如下：

  ![sdf](https://cloud.githubusercontent.com/assets/2249048/2829091/aa181b06-cf9e-11e3-91b5-f7bd83f1647d.png)

* initJPush  
点击按钮会启动JPush推送服务;在JPush服务上推送一个通知可以再通知栏中显示出来.

* stopJPush  
点击按钮会停止JPush推送服务;在JPush服务上推送的任数据都不会接收.

* resumeJPush  
点击按钮会重新启动JPush推送服务.

* setTags  
点击按钮并在输入框中输入tags的内容;如果出现“set tags success” 提示,则表明设置设备标签成功.

* setAlias  
点击按钮并在输入框输入Alias的内容;如果出现"set Alias success" 提示,则表明设置设备别名成功.

* showMessage  
点击按钮后，如果有从服务器推送消息,那么成功的情况下会在输入框中显示出来.

* addTrigger --- setPushTime  
点击按钮,则该应用会监听"setPushTime"事件，并同时出发"setPushTime"设置接收推送的时间段.

* removeTrigger --- setPushTime  
点击按钮，则会将"setPushTime"移除.


## 集成 JPush Unity iOS SDK

* 在Unity3d游戏场景中,新建一个空的 Gameobject,挂载 JPushBinding.c

* 生成ios工程,并打开该工程

* 添加必要的框架：

```
CoreTelehony.framework
```
  
* 在工程中创建一个新的 Property List 文件，并将其命名为 PushConfig.plist，填入Portal 为你的应用提供的 APP_KEY 等参数
  
*  找到 xcode 工程 Libraries 文件夹的 APServer.h，拖入 project 中(或者点击右键，点击 add files to "project name")

* 在 UnityAppController.mm 中添加头文件 APServer.h

```
#import "APService.h"
```

* 在 UnityAppController.mm 中添加监听系统事件，相应地调用 JPush SDK 提供的 API 来实现功能

```
  - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
  {
  // Required
        [APService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
                                                   UIRemoteNotificationTypeSound |
                                                   UIRemoteNotificationTypeAlert)];
  // Required
          [APService setupWithOption:launchOptions];
          return YES;
 }
```
```
  	- (void)application:(UIApplication *)application 	didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
      		// Required
      		[APService registerDeviceToken:deviceToken];
  		}
```
```	
 	 - (void)application:(UIApplication *)application 	didReceiveRemoteNotification:(NSDictionary *)userInfo {
     	 // Required
     	 [APService handleRemoteNotification:userInfo];
 	 }
```

