# JPush Unity Plugin

[![release](https://img.shields.io/badge/release-3.1.3-blue.svg)](https://github.com/jpush/jpush-unity3d-plugin/releases)
[![platforms](https://img.shields.io/badge/platforms-iOS%7CAndroid-green.svg)](https://github.com/jpush/jpush-unity3d-plugin)

这是极光官方支持的 JPush Unity 插件（Android &amp; iOS）。

## 集成

运行插件目录下的 JPushUnityPlugin_vX.X.X.unitypackage

### Android

1. 替换 AndroidManifest.xml 里的包名。
2. 将 AndroidManifest.xml 中的 JPUSH_APPKEY 值替换成极光控制台应用详情中的 AppKey 值。
3. 配置项目里的包名：在 Unity 中选择 *File---Build Settings---Player Settings*，将 *Identification* 选项下的 *Bundle Idenifier* 设置为应用的包名。
4. 配置项目的图标。

### iOS

1. 生成 iOS 工程，并打开该工程。
2. 添加必要的框架：

    - CoreFoundation.framework
    - CFNetwork.framework
    - SystemConfiguration.framework
    - CoreTelephony.framework
    - CoreGraphics.framework
    - Foundation.framework
    - UIKit.framework
    - Security.framework
    - libz.tbd            // Xcode7 之前为 libz.dylib 之后为 libz.tbd。
    - AdSupport.framework // 如需使用广告标识符 IDFA 则添加该库，否则不添加。
    - libresolv.tbd
    - UserNotifications.framework

3. 在 UnityAppController.mm 中添加头文件 `JPUSHService.h`  。

    ```Objective-C
    #import "JPUSHService.h"
    #import <UserNotifications/UserNotifications.h>

    // 如需使用广告标识符 IDFA 则添加该头文件，否则不添加。
    #import <AdSupport/AdSupport.h>

    @interface UnityAppController ()<JPUSHRegisterDelegate>
    @end
    ```

4. 在 UnityAppController.mm 的下列方法中添加以下代码：

    ```Objective-C
    - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {

      [[JPushEventCache sharedInstance] handFinishLaunchOption:launchOptions];
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
            advertisingIdentifier: IDFA广告标识符。根据自身情况选择是否带有 IDFA 的启动方法，并注释另外一个启动方法。
      */
    //  NSString *advertisingId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    //  [JPUSHService setupWithOption:launchOptions appKey:@"替换成你自己的 Appkey" channel:@"" apsForProduction:NO SadvertisingIdentifier:advertisingId];

      return YES;
    }

    - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
      // Required.
      [JPUSHService registerDeviceToken:deviceToken];
    }

    - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
      // Required.
      [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@"JPushPluginReceiveNotification"];
      [JPUSHService handleRemoteNotification:userInfo];
    }

    - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler {
      [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@"JPushPluginReceiveNotification"];
    }
    ```

## API 说明

Android 与 iOS [通用 API](/Doc/CommonAPI.md)。

### Android

在 Plugins\Android\src 目录下是一些 Java 文件，可以将其引入到 Android 工程中对其进行扩展，重新生成 Jar 包替换掉工程中 Assets\Plugins\Android 目录下的 JPush_Bridge.jar 文件。

[Android API](/Doc/AndroidAPI.md)

### iOS

[iOS API](/Doc/iOSAPI.md)

## 更多

- [JPush 官网文档](http://docs.jiguang.cn/guideline/jpush_guide/)
- 有问题可访问[极光社区](http://community.jpush.cn/)搜索和提问。
