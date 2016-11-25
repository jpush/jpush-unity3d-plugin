# JPush Unity Plugin

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/jpush/jpush-unity3d-plugin)
[![release](https://img.shields.io/badge/release-2.1.0-blue.svg)](https://github.com/jpush/jpush-unity3d-plugin/releases)
[![platforms](https://img.shields.io/badge/platforms-iOS%7CAndroid-lightgrey.svg)](https://github.com/jpush/jpush-unity3d-plugin)
[![weibo](https://img.shields.io/badge/weibo-JPush-blue.svg)](http://weibo.com/jpush?refer_flag=1001030101_&is_all=1)

这是极光官方支持的 JPush Unity 插件（Android &amp; iOS）。

## 集成
运行插件目录下的 JPushUnityPlugin_vX.X.X.unitypackage。

### Android
- 替换 AndroidManifest.xml 里的包名。
- 将 AndroidManifest.xml 中的 JPUSH_APPKEY 值替换成极光控制台应用详情中的 AppKey 值。
- 配置项目里的包名：在 Unity 中选择 *File---Build Settings---Player Settings*，将 *Identification* 选项下的 *Bundle Idenifier* 设置为应用的包名。
- 配置项目的图标。

### iOS
- 在 Unity 游戏场景中，新建一个空的 Gameobject，将其名称修改为 JPushBinding，挂载 JPushBinding.cs。
- 生成 iOS 工程，并打开该工程。
- 添加必要的框架。

        CoreFoundation.framework
        CFNetwork.framework
        SystemConfiguration.framework
        CoreTelephony.framework
        CoreGraphics.framework
        Foundation.framework
        UIKit.framework
        Security.framework
        libz.tbd            // Xcode7 之前为 libz.dylib 之后为 libz.tbd。
        AdSupport.framework // 如需使用广告标识符 IDFA 则添加该库，否则不添加。
        libresolv.tbd
        UserNotifications.framework

- 在 UnityAppController.mm 中添加头文件 JPUSHService.h。

        #import "JPUSHService.h"
        #import <UserNotifications/UserNotifications.h>

        // 如需使用广告标识符 IDFA 则添加该头文件，否则不添加。
        #import <AdSupport/AdSupport.h>

- 在 UnityAppController.mm 的下列方法中添加以下代码：

        - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
        {
           
	    	if ([[UIDevice currentDevice].systemVersion floatValue] >= 10.0) {
		#ifdef NSFoundationVersionNumber_iOS_9_x_Max
	        JPUSHRegisterEntity * entity = [[JPUSHRegisterEntity alloc] init];
   		     entity.types = UNAuthorizationOptionAlert | UNAuthorizationOptionBadge | UNAuthorizationOptionSound;
	        [JPUSHService registerForRemoteNotificationConfig:entity delegate:self];
		#endif
	    }

		#if __IPHONE_OS_VERSION_MAX_ALLOWED > __IPHONE_7_1
		    if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0) {
       	 	//可以添加自定义categories
				[JPUSHService registerForRemoteNotificationTypes:(UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil];
		    } else {
	        //categories 必须为nil
				[JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |  UIRemoteNotificationTypeAlert) categories:nil];
		    }
		#else
	  	 	 //categories 必须为nil
   			 [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |UIRemoteNotificationTypeAlert) categories:nil];
		#endif


        	/*
               不使用 IDFA 启动 SDK。
               参数说明：
                   appKey: 极光官网控制台应用标识。
                   channel: 频道，暂无可填任意。
                   apsForProduction: YES: 发布环境；NO: 开发环境。
             */
            [JPUSHService setupWithOption:launchOptions appKey:@"abcacdf406411fa656ee11c3" channel:@"" apsForProduction:NO];

            /*
                使用 IDFA 启动 SDK（不能与上述方法同时使用）。
                参数说明：
                    appKey: 极光官网控制台应用标识。
                    channel: 频道，暂无可填任意。
                    apsForProduction: YES: 发布环境；NO: 开发环境。
                    advertisingIdentifier: IDFA广告标识符。
             */
            NSString *advertisingId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
            [JPUSHService setupWithOption:launchOptions appKey:@"abcacdf406411fa656ee11c3" channel:@"" apsForProduction:NO advertisingIdentifier:advertisingId];

        	return YES;
        }

在 [JPUSHService setupWithOption:appKey:channel:apsForProduction:advertisingIdentifier:] 方法中：
- appkey: 填写自己应用的 AppKey。
- apsForProduction: 根据所用 Apple 证书的不同填写。
    - YES: 发布环境。
    - NO: 开发环境。
- advertisingIdentifier: 根据自身情况选择是否带有 IDFA 的启动方法，并注释另外一个启动方法。

	- (void)application:(UIApplication *)application 	didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
		// Required.
		[JPUSHService registerDeviceToken:deviceToken];
	}

	- (void)application:(UIApplication *)application 	didReceiveRemoteNotification:(NSDictionary *)userInfo {
		// Required.
		[JPUSHService handleRemoteNotification:userInfo];
	}

	// iOS 10 Support
	- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(NSInteger))completionHandler {
   		 // Required
	    NSDictionary * userInfo = notification.request.content.userInfo;
   		 if([notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
       		 [JPUSHService handleRemoteNotification:userInfo];
    	}
	    [[NSNotificationCenter defaultCenter] postNotificationName:@"JPushPluginReceiveNotification" object:userInfo];
    	completionHandler(UNNotificationPresentationOptionAlert | UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound); // 需要执行这个方法，选择是否提醒用户，有Badge、Sound、Alert三种类型可以选择设置
}

	// iOS 10 Support
	- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)())completionHandler {
   		 // Required
	    NSDictionary * userInfo = response.notification.request.content.userInfo;
   		 if([response.notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
       	 [JPUSHService handleRemoteNotification:userInfo];
	    }
   		 completionHandler();  // 系统要求执行这个方法
	}

## API 说明
### Android
在 Plugins\Android\src 目录下是一些 Java 文件，可以将其引入到 Android 工程中对其进行扩展，重新生成 Jar 包替换掉工程中 Assets\Plugins\Android 目录下的 JPush_Bridge.jar 文件。

> [Android SDK API](/Doc/AndroidAPI.md)。

### iOS
iOS API 在文件 /Plugins/JPushBinding.cs 中，代码 #if UNITY_IPHONE 后面的即为可调用的 iOS API。

> [iOS SDK API](http://docs.jpush.io/client/ios_api/)。

亦可参照以下文档中与 `JPushBinding.cs` 中相同方法名的方法说明：

> [JPush Phonegap Plugin Common API doc](https://github.com/jpush/jpush-phonegap-plugin/blob/master/doc/Common_detail_api.md)

> [JPush Phonegap Plugin iOS API doc](https://github.com/jpush/jpush-phonegap-plugin/blob/master/doc/iOS_API.md)



## 更多
- [JPush 官网文档](http://docs.jiguang.cn/guideline/jmessage_guide/)。
- 有问题可访问[极光社区](http://community.jpush.cn/)搜索和提问。
