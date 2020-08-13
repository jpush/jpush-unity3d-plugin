# JPush Unity Plugin

[![release](https://img.shields.io/badge/release-3.2.0-blue.svg)](https://github.com/jpush/jpush-unity3d-plugin/releases)
[![platforms](https://img.shields.io/badge/platforms-iOS%7CAndroid-green.svg)](https://github.com/jpush/jpush-unity3d-plugin)

这是极光官方支持的 JPush Unity 插件（Android &amp; iOS）。

## 集成

把Plugins文件夹里的文件合并到您自己的项目Assets/Plugins文件夹下面

### Android

1. 替换 AndroidManifest.xml 里的包名。
2. 将 AndroidManifest.xml 中的 JPUSH_APPKEY 值替换成极光控制台应用详情中的 AppKey 值。
3. 配置项目里的包名：在 Unity 中选择 *File---Build Settings---Player Settings*，将 *Identification* 选项下的 *Bundle Identifier* 设置为应用的包名。

### iOS

1. 生成 iOS 工程，并打开该工程。
2. 添加必要的框架：

   - CFNetwork.framework
   - CoreFoundation.framework
   - CoreTelephony.framework
   - SystemConfiguration.framework
   - CoreGraphics.framework
   - Foundation.framework
   - UIKit.framework
   - Security.framework
   - libz.tbd（Xcode 7 以下版本是 libz.dylib）
   - AdSupport.framework（获取 IDFA 需要；如果不使用 IDFA，请不要添加）
   - UserNotifications.framework（Xcode 8 及以上）
   - libresolv.tbd（JPush 2.2.0 及以上版本需要，Xcode 7 以下版本是 libresolv.dylib）
   - WebKit.framework（JPush 3.3.0 及以上版本需要）


      ​

3. 在 UnityAppController.mm 中添加头文件 `JPUSHService.h`  。

    ```Objective-C
    #import "JPUSHService.h"
    #import "JPushEventCache.h"
    #import <UserNotifications/UserNotifications.h>

    // 如需使用广告标识符 IDFA 则添加该头文件，否则不添加。
    #import <AdSupport/AdSupport.h>

    @interface UnityAppController ()
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
    
5.有些Unity版本在导出Xcode项目之后，需要修改：

```Objective-C
    Preprocessor.h 文件中

    #define UNITY_USES_REMOTE_NOTIFICATIONS 0
    更改为
    #define UNITY_USES_REMOTE_NOTIFICATIONS 1

    否则

    didReceiveRemoteNotification
    didRegisterForRemoteNotificationsWithDeviceToken
    didReceiveRemoteNotification

    都将无法使用
```
## API 说明

Android 与 iOS [通用 API](/Doc/CommonAPI.md)。

### Android

[Android API](/Doc/AndroidAPI.md)

> ./Plugins/Android/jpush-unity-plugin 为插件的 Android 项目，可以使用 Android Studio 打开并进行修改（比如，targetSdkVersion 或者 minSdkVersion），再 build 为 .aar 替换已有的 jpush.aar。

### iOS

[iOS API](/Doc/iOSAPI.md)

## 更多

- [JPush 官网文档](http://docs.jiguang.cn/guideline/jpush_guide/)
- 有问题可访问[极光社区](http://community.jpush.cn/)搜索和提问。
