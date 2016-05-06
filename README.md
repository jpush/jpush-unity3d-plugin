JPush Unity3d Plugin

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/jpush/jpush-unity3d-plugin)
[![release](https://img.shields.io/badge/release-2.1.0-blue.svg)](https://github.com/jpush/jpush-unity3d-plugin/releases)
[![platforms](https://img.shields.io/badge/platforms-iOS%7CAndroid-lightgrey.svg)](https://github.com/jpush/jpush-unity3d-plugin)
[![weibo](https://img.shields.io/badge/weibo-JPush-blue.svg)](http://weibo.com/jpush?refer_flag=1001030101_&is_all=1)

这是极光推送官方支持的 Unity3d 插件（Android &amp; iOS）。

## 导入到 Unity 项目

* 搭建好 unity3d(iOS/Android)开发环境。

* 将 git 下来的 jpush-unity3d-plugin/Plugins 导入到 Unity 中。

## 集成 JPush Unity Android SDK

### 基本动作

* 替换 AndroidManifest.xml 里的包名：将其中的“com.jpush.unity3dplugin”`全部`替换成你在JPush服务器上创建的应用的应用包名。

* 配置Appkey：将其中的"JPUSH_APPKEY"值替换成应用详情里应用标 AppKey 的值。

* 配置项目里的包名：在 unity 中选择"File---Build Settings---Player Settings...",将"Identification"选项下的"Bundle Idenifier*" 设置为应用的包名.

* 运行：设置 OK，直接 Build&Run 即可。


### API 说明

> 在 Plugins\Android\src 目录下是一些 java 文件，你可以将其引入到 Android 工程中对它们进行扩展，重新生成 jar 替换掉 unity 工程中
 Assets\Plugins\Android 目录下的 JPush_Bridge.jar 文件。

 [Android API 详细文档](/Doc/AndroidAPI.md)


### Example 说明

在 unity Assets\Plugins\Demo下的 PluginsDemo.cs 文件是一个测试脚本，其运行结果如下：

  ![sdf](https://cloud.githubusercontent.com/assets/2249048/2829091/aa181b06-cf9e-11e3-91b5-f7bd83f1647d.png)

* initJPush  
点击按钮会启动 JPush 推送服务，在 JPush 服务上推送一个通知可以在通知栏中显示出来。

* stopJPush  
点击按钮会停止 JPush 推送服务，在 JPush 服务上推送的任何数据都不会接收。

* resumeJPush  
点击按钮会重新启动 JPush 推送服务。

* setTags  
点击按钮并在输入框中输入 tags 的内容，如果出现“set tags success”提示，则表明设置设备标签成功。

* setAlias  
点击按钮并在输入框输入 alias 的内容，如果出现"set alias success"提示，则表明设置设备别名成功。

* showMessage
点击按钮后，如果有从服务器推送消息，那么成功的情况下会在输入框中显示出来。

* addTrigger --- setPushTime  
点击按钮，则该应用会监听"setPushTime"事件，并同时出发"setPushTime"设置接收推送的时间段。

* removeTrigger --- setPushTime  
点击按钮，则会将"setPushTime"移除。


## 集成 JPush Unity iOS SDK

* 在 Unity3d 游戏场景中,新建一个空的 Gameobject，将其名称修改为 JPushBinding，挂载 JPushBinding.cs。

* 生成 iOS 工程，并打开该工程。

* 添加必要的框架：

```
CoreFoundation.framework
CFNetwork.framework
SystemConfiguration.framework
CoreTelephony.framework
CoreGraphics.framework
Foundation.framework
UIKit.framework
Security.framework
libz.tbd//如原先为 libz.dylib 则替换为 libz.tbd
AdSupport.framework//如需使用广告标识符 IDFA 则添加该库，否则不添加
```

* 在 UnityAppController.mm 中添加头文件 JPUSHService.h：

```
#import "JPUSHService.h"
//#import <AdSupport/AdSupport.h>//如需使用广告标识符 IDFA 则添加该头文件，否则不添加
```

* 在 UnityAppController.mm 中下列方法中添加以下代码：

```
  - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
  {
  // Required
	#if __IPHONE_OS_VERSION_MAX_ALLOWED > __IPHONE_7_1
	    if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0) {
	        //可以添加自定义categories
	        [JPUSHService registerForRemoteNotificationTypes:(UIUserNotificationTypeBadge |
	                                                       UIUserNotificationTypeSound |
	                                                       UIUserNotificationTypeAlert)
	                                           categories:nil];
	    } else {
	        //categories 必须为nil
	        [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
	                                                       UIRemoteNotificationTypeSound |
	                                                       UIRemoteNotificationTypeAlert)
	                                           categories:nil];
	    }
	#else
	    //categories 必须为nil
	    [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
	                                                   UIRemoteNotificationTypeSound |
	                                                   UIRemoteNotificationTypeAlert)
	                                       categories:nil];
	#endif

	/* 不使用 IDFA 启动 sdk
       参数说明：
            appKey：极光官网控制台应用标识
            channel：频道，暂无可填任意
            apsForProduction：YES发布环境/NO开发环境
     */
    [JPUSHService setupWithOption:launchOptions appKey:@"abcacdf406411fa656ee11c3" channel:@"" apsForProduction:NO];


    /* 使用 IDFA 启动 sdk （不与上述方法同时使用）
       参数说明：
            appKey：极光官网控制台应用标识
            channel：频道，暂无可填任意
            apsForProduction：YES发布环境/NO开发环境
            advertisingIdentifier：IDFA广告标识符
     */
    //NSString *advertisingId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    //[JPUSHService setupWithOption:launchOptions appKey:@"abcacdf406411fa656ee11c3" channel:@"" apsForProduction:NO advertisingIdentifier:advertisingId];

	return YES;

 }
```
```
  	- (void)application:(UIApplication *)application 	didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
      		// Required
      		[JPUSHService registerDeviceToken:deviceToken];
  		}
```
```
 	 - (void)application:(UIApplication *)application 	didReceiveRemoteNotification:(NSDictionary *)userInfo {
     	 // Required
     	 [JPUSHService handleRemoteNotification:userInfo];
 	 }
```
### iOS Example 说明
* 新建一个 Unity3d 的工程，将 Examples/PluginsDemo.cs 拖到 Main Camera 对象上生成相应的 iOS 项目即可。
