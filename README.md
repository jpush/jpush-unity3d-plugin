jpush-unity3d-plugin
====================

JPush's officially supported Unity3d plugin (Android &amp; iOS). 极光推送官方支持的 Unity3d 插件（Android &amp; iOS）。
### 集成
#### 开发者打开开发中心的Unity项目后，双击unitypackge文件，自自动把相应的sdk加到项目相应的位置
### ios集成sdk
1. 在Unity3d游戏场景中，新建一个空的Gameobject，挂载JPushBinding.cs
2. 生成ios工程，并打开该工程
3. 必要的框架

  ```objective-c
    添加CoreTelehony.framework
  ```
  
4. 在你的工程中创建一个新的Property List文件

  ```
  并将其命名为PushConfig.plist，填入Portal为你的应用提供的APP_KEY等参数
  ```
  
5. 找到xocde工程Libraries文件夹的APServer.h，拖入project中
6. 在UnityAppController.mm中添加头文件APServer.h

  ```objective-c
  #import "APService.h"
  ```

7. 在UnityAppController.mm 中添加监听系统事件，相应地调用 JPush SDK 提供的 API 来实现功能

  ```objective-c
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
  - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
      // Required
      [APService registerDeviceToken:deviceToken];
  }
  - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
      // Required
      [APService handleRemoteNotification:userInfo];
  }
  ```
