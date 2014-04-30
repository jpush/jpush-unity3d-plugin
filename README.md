JPush Unity3d Plugin
====================

这是极光推送官方支持的 Unity3d 插件（Android &amp; iOS）。

## 导入到 Unity 项目

* 搭建好unity3d(ios/android)开发环境

* 将git下来的jpush-unity3d-plugin.unitypackage导入到Unity中

## 集成 JPush Unity Android SDK

### 基本动作

* 替换AndroidManifest.xml里的包名 <br>
  将其中的“com.jpush.unity3dplugin”替换成你在JPush服务器上创建的应用的应用包名.

* 配置Appkey <br>
  将其中的"JPUSH_APPKEY"值替换成应用详情里应用标(AppKey)的值.

* 配置项目里的包名 <br>
  在unity中选择"File---Build Settings---Player Settings...",将"Identification"选项下的"Bundle Idenifier*" 设置为应用的包名.

* 运行<br>
  设置OK,直接"Build&Run"即可.


### API 功能说明

* initJPush(string gameObject,string func) <br> 
  功能描述：启用JPush推送服务. <br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名.
  
* stopJPush(string gameObject,string func) <br> 
  功能描述：停止JPush推送服务(必须通过resumeJPush再次启用.)<br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名.

* resumeJPush(string gameObject,string func) <br> 
  功能描述：重新启用JPush推送服务(如果是通过stopJPush来停止的,再次启用必须调用此方法).<br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名.

* setTags(string gameObject,string func,string tags) <br> 
  功能描述：设置设备标签.<br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名;tags为多个Tag组成的字符串.(Tag为大小写字母,数字,下划线,中文; 多个用逗号分隔).

* setAlias(string gameObject,string func,string alias) <br> 
  功能描述：设置设备别名.<br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名;alias为大小写字母,数字,下划线,中文组成的字符串.

* setPushTime(string gameObject,string func,string days,string start_time,string end_time) <br> 
  功能描述：设置接收推送的时间段.<br>
  参数说明：gameObject代表游戏对象;func代表回调的方法名;days为0-6之间由","连接而成的字符串;<br>
start_time为0-23的字符串;end_time为0-23的字符串.

* PS:在Plugins\Android\src目录下是一些java文件,你可以将其引入到android工程中对它们进行扩展.重新生成jar替换掉unity工程中
Assets\Plugins\Android目录下的JPush_Bridge.jar文件.


### Example 说明

* 在unity Assets\Plugins\Demo下的PluginsDemo.cs文件是一个测试脚本,其运行结果如下:<br>
  ![sdf](https://cloud.githubusercontent.com/assets/2249048/2829091/aa181b06-cf9e-11e3-91b5-f7bd83f1647d.png)

* 点击"initJPush"按钮会启动JPush推送服务;在JPush服务上推送一个通知可以再通知栏中显示出来.

* 点击"stopJPush"按钮会停止JPush推送服务;在JPush服务上推送的任数据都不会接收.

* 点击"resumeJPush"按钮会重新启动JPush推送服务.

* 点击"setTags"按钮并在输入框中输入tags的内容;如果出现“set tags success” 提示,则表明设置设备标签成功.

* 点击"setAlias"按钮并在输入框输入Alias的内容;如果出现"set Alias success" 提示,则表明设置设备别名成功.

* 点击"showMessage"按钮,如果有从服务器推送消息,那么成功的情况下会在输入框中显示出来.

* 点击"addTrigger---setPushTime"按钮,则该应用会监听"setPushTime"事件.并同时出发"setPushTime"设置接收推送的时间段.

* 点击"removeTrigger---setPushTime"按钮,则会将"setPushTime"移除.


## 集成 JPush Unity iOS SDK

<<<<<<< HEAD
* 在Unity3d游戏场景中，新建一个空的 Gameobject，（重命名为JPushBinding）挂载JPushBinding.c
=======
* 在Unity3d游戏场景中,新建一个空的 Gameobject,挂载 JPushBinding.c
>>>>>>> 58c17c1f62410983296ccaf978e62768ad433ae3

* 生成ios工程,并打开该工程

* 必要的框架

  ```
    添加CoreTelehony.framework
  ```
  
* 在你的工程中创建一个新的Property List文件

  ```
  并将其命名为PushConfig.plist，填入Portal为你的应用提供的APP_KEY等参数
  ```
  
5. 找到xocde工程Libraries文件夹的APServer.h，拖入project中(或者点击右键，点击add files to "project name")
6. 在UnityAppController.mm中添加头文件APServer.h

```
#import "APService.h"
```

* 在UnityAppController.mm 中添加监听系统事件，相应地调用 JPush SDK 提供的 API 来实现功能

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
