JPush Unity3d Plugin
====================

这是极光推送官方支持的 Unity3d 插件（Android &amp; iOS）。

## 导入到 Unity 项目

* 搭建好unity3d(ios/android)开发环境

* 将git下来的jpush-unity3d-plugin.unitypackage导入到Unity中

## 集成 JPush Unity Android SDK

### 基本动作

* 替换 AndroidManifest.xml 里的包名 <br>
  将其中的 “com.jpush.unity3dplugin” 替换成你在 JPush服务器上创建的应用的应用包名

* 配置 Appkey <br>
  将其中的"JPUSH_APPKEY" 值替换成应用详情里应用标志（AppKey）的值

* 配置项目里的包名 <br>
  在 unity 中选择 "File---Build Settings---Player Settings..."，将 "Identification" 选项下的 "Bundle Idenifier*" 设置为应用的包名。

* 运行<br>
  设置OK,直接 "Build&Run" 即可


### API 功能说明

* initJPush <br> 启用JPush推送服务
  
* stopJPush <br> 停止JPush推送服务

* resumeJPush <br> 重新启用JPush推送服务(如果是通过stopJPush来停止的，再次启用必须调用此方法)

* setTags <br> 设置设备标签

* setAlias <br> 设置设备别名

* setPushTime <br> 设置接收推送的时间段

PS:详细信息参见Plugins下的JPushBinding.cs文件


### Example 说明

* 在 unity Assets\Plugins\Demo下的PluginsDemo.cs文件是一个测试脚本，大家在具体使用的时候可以根据需要动态的进行事件的注册与触发。

```
/**
 * 注册事件
 * 
 * CustomEventObj.EVENT_INIT_JPUSH 事件的类型
 * gameObject 作用于哪个游戏对象
 * 第三个参数是当这个事件被触发时回调的函数名,必须得重写
 */
JPushEventManager.instance.addEventListener(CustomEventObj.EVENT_INIT_JPUSH, gameObject, "initJPush");

/**
 * 触发事件
 * CustomEventObj.EVENT_INIT_JPUSH 为触发的事件类型
 */
JPushTriggerManager.triggerInitJPush(CustomEventObj.EVENT_INIT_JPUSH);
```

* 在退出应用的时候，需要调用 beforeQuit() 方法。

* 接受到通知，如果你要查看通知的内容或其他的显示方式，你可以重写“openNotification(string str)” 方法，例如：

```
/**
 * 开发者自己处理点击通知栏中的通知
 * str  为通知的内容，数据格式如下.
    {
          "title": "JPush-Unity-Plugin",
  	    "message": "sdf",
  	    "extras": {
  	        "name": "zfl",
  	        "a": "aaa"
  	    }
  	}
 */

void openNotification(string str) {
	Debug.Log ("recv --- openNotification---" + str) ;
	str_unity = str ;
}
```

* 当收到消息时，你可以通过重写 “recvMessage(string str)”方法，来获取到消息的内容。

```
/**
 * 开发者自己处理由JPush推送下来的消息
 * str  为消息的内容，数据格式如下.
		{
		    "message": "hhh",
		    "extras": {
		        "f": "fff",
		        "q": "qqq",
		        "a": "aaa"
		    }
		}
 */
void recvMessage(string str) {
	Debug.Log("recv----message-----" + str) ; 
	str_message = str ;
} 
```

* 当使用 “JPushTriggerManager.triggerSetTags(CustomEventObj.EVENT_SET_TAGS, str_unity)” 来触发“setTags” 时，
第二个参数的规则：Tag为大小写字母,数字,下划线,中文; 多个用逗号分隔.<br>

* 当使用“JPushTriggerManager.triggerSetAlias(CustomEventObj.EVENT_SET_ALIAS , str_unity)”来触发“setAlias”时，
第二个参数的规则：Alias为大小写字母,数字,下划线.<br>

* 当使用“PushTriggerManager.triggerSetPushTime(CustomEventObj.EVENT_SET_PUSH_TIME , days , start_time , end_time)”
触发“setPushTime”来设置接收推送消息的时间段时.参数days , start_time , end_time 的格式如下：

``` 
string days = "0,1,2,3,4,5,6" ; //任意以“0-6”的组合，中间以“,”分隔的字符串
string start_time = "10" ;      //0-23的整形字符串
string end_time = "18" ;        //0-23的整形字符串
```

* 在Plugins\Android\src目录下是一些java文件，你可以将其引入到android工程中对它们进行扩展.重新生成jar替换掉unity工程中
Assets\Plugins\Android 目录下的JPush_Bridge.jar文件.


## 集成 JPush Unity iOS SDK

* 在Unity3d游戏场景中，新建一个空的 Gameobject，挂载 JPushBinding.c

* 生成ios工程，并打开该工程

* 必要的框架

  ```
    添加CoreTelehony.framework
  ```
  
* 在你的工程中创建一个新的Property List文件

  ```
  并将其命名为PushConfig.plist，填入Portal为你的应用提供的APP_KEY等参数
  ```
  
5. 找到xocde工程Libraries文件夹的APServer.h，拖入project中
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
  - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
      // Required
      [APService registerDeviceToken:deviceToken];
  }
  - (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
      // Required
      [APService handleRemoteNotification:userInfo];
  }
```

