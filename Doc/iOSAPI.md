# iOS API

- [角标（badge）](#角标badge)
  - [SetBadge(int badge)](#setbadgeint-badge)
  - [ResetBadge()](#resetbadge)
  - [SetApplicationIconBadgeNumber(int badge)](#setapplicationiconbadgenumberint-badge)
  - [GetApplicationIconBadgeNumber()](#getapplicationiconbadgenumber)
- [页面统计](#页面统计)
  - [StartLogPageView(string pageName)](#startlogpageviewstring-pagename)
  - [StopLogPageView(string pageName)](#stoplogpageviewstring-pagename)
  - [BeginLogPageView(string pageName, int duration)](#beginlogpageviewstring-pagename-int-duration)
- [本地通知](#本地通知)
  - [SendLocalNotification(string localParams)](#sendlocalnotificationstring-localparams)
  - [SetLocalNotification(int delay, string content, int badge, string idKey)](#setlocalnotificationint-delay-string-content-int-badge-string-idkey)
  - [DeleteLocalNotificationWithIdentifierKey(string idKey)](#deletelocalnotificationwithidentifierkeystring-idkey)
  - [ClearAllLocalNotifications()](#clearalllocalnotifications)
  - [RemoveNotification(List<string> idKeys, bool delivere)](#removeNotification-idKeys-bool-delivere)
  - [RemoveNotificationAll()](#RemoveNotificationAll)
- [API](#API)
  - [CrashLogON()](#CrashLogON)
  - [SetLatitude(double latitude, double longitude)](#SetLatitude-latitude-double-longitude)

## 角标（badge）

Badge 是 iOS 用来标记应用程序状态的一个数字，出现在程序图标右上角。

JPush 封装了 badge 功能，允许应用上传 badge 值至 JPush 服务器，由 JPush 后台帮助管理每个用户所对应的推送 badge 值，简化了设置推送 badge 的操作。

实际应用中，开发者可以直接对 badge 值做增减操作，无需自己维护用户与 badge 值之间的对应关系。

### SetBadge(int badge)

设置 JPush 服务器中存储的角标（badge）值，该方法不会修改当前应用本地的角标值。

#### 参数说明

- badge: 要设置的角标值，取值范围为 0~9999。

### ResetBadge()

清空 JPush 服务器中存储的 badge 值，等同于 `SetBadge(0)`。

### SetApplicationIconBadgeNumber(int badge)

设置应用本地显示的角标值。

#### 参数说明

- badge: 要设置的角标值，取值范围为 0~9999。

### GetApplicationIconBadgeNumber()

获取应用当前显示的角标数。

#### 代码示例

```csharp
int badge = JPushBinding.GetApplicationIconBadgeNumber();
```

## 页面统计

用于「用户指定页面使用时长」的统计，并上报到极光服务器，在极光 Portal 上展示给开发者。页面统计集成正确，才能够获取正确的页面访问路径、访问深度（PV）的数据。

### StartLogPageView(string pageName)

进入指定页面。

#### 参数说明

- pageName: 指定页面名称。

#### 代码示例

```csharp
JPushBinding.StartLogPageView("login"); // 记录进入到 login 页面。
```

### StopLogPageView(string pageName)

退出指定页面。

#### 参数说明

- pageName: 指定页面名称。

#### 代码示例

```csharp
JPushBinding.StopLogPageView("login"); // 记录退出 login 页面。
```

### BeginLogPageView(string pageName, int duration)

自定义上报指定要统计的页面和用户停留的时间。

#### 参数说明

- pageName: 指定页面名称。
- duration: 在页面停留的时间，单位秒。

## 本地通知

### SendLocalNotification(string localParams)

注册本地通知（推荐使用这个方法）。

#### 参数说明

- localParams 需包含如下字段
  - **id** : Number // 通知的 id, 可用于取消通知
  - **title** : String // 通知标题
  - **content** : String // 通知内容
  - **extra** : Object // extra 字段
  - **fireTime** : Number // 通知触发时间的时间戳（秒）
  - **badge** : Number // 本地推送触发后应用角标的 badge 值 （iOS Only）
  - **soundName** : String // 指定推送的音频文件 （iOS Only）
  - **subtitle** : String // 子标题 （iOS10+ Only）

### SetLocalNotification(int delay, string content, int badge, string idKey)

注册本地通知（不推荐使用该方法，建议使用 SendLocalNotification）。

#### 参数说明

- content: 通知内容；
- badge: 角标数字，如果不需要改变角标就传 -1；
- idKey: 本地推送标识符。

### DeleteLocalNotificationWithIdentifierKey(string idKey)

根据推送标识删除本地通知。

#### 参数说明

- idKey: 本地推送标识符。

### ClearAllLocalNotifications()

清除所有注册的本地通知。

### RemoveNotification(List<string> idKeys, bool delivered)

移除通知中心显示推送和待推送请求。

#### 参数说明

- idKeys: 要查找的idKey列表，null 查找所有；
- badge: ture 显示的通知，false 还没有显示的通知，iOS10以下无效；

### RemoveNotificationAll()

清除所有注册的本地通知。

## API

###  CrashLogON()

用于统计用户应用崩溃日志,如果需要统计 Log 信息，调用该接口。当你需要自己收集错误信息时，切记不要调用该接口。

### SetLatitude(double latitude, double longitude)

地理位置上报。

#### 参数说明

- latitude: 纬度；
- longitude: 经度；