# Android API

具体 API 代码在 JPushBinding.cs 文件中。

- [停止推送服务](#停止推送服务)
- [恢复推送服务](#恢复推送服务)
- [检查推送服务是否停止](#检查推送服务是否停止)
- [清除通知](#清除通知)
- [设置允许推送时间](#设置允许推送时间)
- [设置通知静默时间](#设置通知静默时间)
- [申请权限接口（用于 Android 6.0 及以上系统）](#申请权限接口用于-android-60-及以上系统)
- [通知栏样式定制](#通知栏样式定制)
- [设置保留最近通知条数](#设置保留最近通知条数)
- [本地通知](#本地通知)

## 停止推送服务

### API - StopJPush

调用了该 API 后，JPush 推送服务完全被停止。具体表现为：

- JPush Service 不在后台运行；
- 收不到推送消息；
- 极光推送所有的其他 API 调用都无效,不能通过 InitJPush 恢复，需要调用 ResumePush 恢复。

#### 代码示例

```csharp
JPushBinding.StopJPush();
```

## 恢复推送服务

### API - ResumePush

调用了此 API 后，极光推送完全恢复正常工作。

#### 代码示例

```csharp
JPushBinding.ResumeJPush();
```

## 检查推送服务是否停止

### API - IsPushStopped

调用此 API 来判断当前 Push Service 是否停止。

#### 代码示例

```csharp
bool isStopped = JPushBinding.IsPushStopped();
```

#### 返回值说明

- true: 调用了 stopJPush() 之后，返回 true；
- false: 初始化后但没有调用 stopJPush() 之前，或者停止后再重新调用了 resumeJPush() ，返回 false;

## 清除通知

### API - ClearAllNotifications

此 API 提供清除通知的功能，包括：清除所有 JPush 展现的通知（不包括非 JPush SDK 展现的）。

#### 代码示例

```csharp
JPushBinding.ClearAllNotifications();
```

### API - ClearNotificationById(int notiId)

此 API 用于清除指定的某个通知。

#### 参数说明

- notiId: 指定通知的 Notification Id。

#### 代码示例

```csharp
JPushBinding.ClearNotificationById(1);  // 1: 指定通知的 notificationId
```

## 设置允许推送时间

### API - SetPushTime(string days, int startHour, int endHour)

默认情况下用户在任何时间都允许推送。即任何时候有推送下来，客户端都会收到，并展示。

开发者可以调用此 API 来设置允许推送的时间。

如果不在该时间段内收到消息，当前的行为是：推送到的通知会被扔掉。

> 这是一个纯粹客户端的实现，所以与客户端时间是否准确、时区等这些，都没有关系。

#### 参数说明

- days: 表示星期中的日期，0 表示星期天，1 表示星期一，以此类推。如果 days 为 null，表示任何时间都可以收到消息；如果为 ""，则表示任何时间都**不能**收到消息。
- startHour: 允许推送的开始时间（24小时制：范围是 0~23）。
- endHour: 允许推送的结束时间（24小时制：范围是 0~23）。

#### 代码示例

```csharp
JPushBinding.SetPushTime("0,1,2,3", 8, 20); // 周日到周三的早上 8 点至晚上 8 点可以推送。
```

## 设置通知静默时间

### API - SetSilenceTime(int startHour, int startMinute, int endHour, int endMinute)

默认情况下用户在收到推送通知时，客户端可能会有震动，响铃等提示。但用户在睡觉、开会等时间点希望为 "免打扰" 模式，也是静音时段的概念。

开发者可以调用此 API 来设置静音时段，如果在该时间段内收到消息，则：不会有铃声和震动。

#### 参数说明

- startHour：静默时段的开始时间 - 小时 （24小时制，范围：0 - 23）。
- startMinute：静默时段的开始时间 - 分钟（范围：0 - 59）。
- endHour：静默时段的结束时间 - 小时 （24小时制，范围：0 - 23）。
- endMinute：静默时段的结束时间 - 分钟（范围：0 - 59）。

#### 代码示例

```csharp
JPushBinding.SetSilenceTime(20, 0, 7, 0);   // 设置晚上 8 点至早上 7 点通知静默。
```

## 申请权限接口（用于 Android 6.0 及以上系统）

### API - RequestPermission()

在 Android 6.0 及以上的系统上，需要去请求一些用到的权限，JPush SDK 用到的一些需要请求如下权限，因为需要这些权限使统计更加精准，功能更加丰富，建议开发者调用。

```xml
"android.permission.READ_PHONE_STATE"
"android.permission.WRITE_EXTERNAL_STORAGE"
"android.permission.READ_EXTERNAL_STORAGE"
"android.permission.ACCESS_FINE_LOCATION"
```

#### 代码示例

```csharp
JPushBinding.RequestPermission();
```

## 通知栏样式定制

大多数情况下，开发者不需要调用这里的定制通知栏 API 来自定义通知栏样式，只需要使用 SDK 默认的即可。

如果想要：

- 改变 Notification 里的铃声、震动、显示与消失行为；
- 自定义通知栏显示样式；
- 不同的 Push 通知，Notification 样式不同；
- 则请使用本通知栏定制 API 提供的能力。

### API - SetBasicPushNotificationBuilder

用于定制 Android Notification 里的 defaults / flags / icon 等基础样式（行为）。

具体可见[官方文档](https://docs.jiguang.cn/jpush/client/Android/android_senior/#_8)。

如果要使用，一定要修改 JPushBinding 中的 SetBasicPushNotificationBuilder 方法，去根据自己的业务逻辑修改代码。

#### 代码示例

```csharp
public static void SetBasicPushNotificationBuilder()
{
    // 需要根据自己业务情况修改后再调用。
    int builderId = 1;  // 定义该样式的 id 为 1，后台推送时就需要也设置相应推送的 builderId 为 1。
    int notiFlags = notificationFlags | FLAG_AUTO_CANCEL;
    int notiDefaults = notificationDefaults | DEFAULT_ALL;

    // statusBarDrawableName: 状态栏通知图标的资源文件名称（R.drawable.statusBarDrawableName）。
    _plugin.Call("setBasicPushNotificationBuilder", builderId, notiDefaults, notiFlags, null);
}

// 使用时直接调用。
JPushBinding.SetBasicPushNotificationBuilder();
```

### API - SetCustomPushNotificationBuilder

继承 Basic 进一步让开发者定制 Notification Layout。

#### 代码示例

```csharp
public static void SetCustomPushNotificationBuilder()
{
    Debug.Log("unity---setCustomPushNotificationBuilder");
    // 需要根据自己业务情况修改后再调用。
    int builderId = 1;
    string layoutName = "yourNotificationLayoutName";
    // 指定最顶层状态栏小图标。
    string statusBarDrawableName = "yourStatusBarDrawableName";
    // 指定下拉状态栏时显示的通知图标。
    string layoutIconDrawableName = "yourLayoutIconDrawableName";

    _plugin.Call("setCustomPushNotificationBuilder", builderId, layoutName, statusBarDrawableName, layoutIconDrawableName);
}

// 使用时直接调用。
JPushBinding.SetCustomPushNotificationBuilder();
```

## 设置保留最近通知条数

### API - SetLatestNotificationNumber(int num)

通过极光推送，推送了很多通知到客户端时，如果用户不去处理，就会有很多保留在那里。

此功能用于限制保留的通知条数，默认为保留最近 5 条通知。

开发者可通过调用此 API 来定义为不同的数量。

> 仅对通知有效。所谓保留最近的，意思是，如果有新的通知到达，之前列表里最老的那条会被移除。
例如，设置为保留最近 5 条通知。假设已经有 5 条显示在通知栏，当第 6 条到达时，第 1 条将会被移除。

#### 代码示例

```csharp
JPushBinding.setLatestNotificationNumber(10);   // 保留最近的 10 条通知。
```

## 本地通知

通过极光推送的SDK，开发者只需要简单调用几个接口，便可以在应用中定时发送本地通知。

> - 本地通知API不依赖于网络，无网条件下依旧可以触发。
> - 本地通知与网络推送的通知是相互独立的，不受保留最近通知条数上限的限制。
> - 本地通知的定时时间是自发送时算起的，不受中间关机等操作的影响。

### API - AddLocalNotification

添加一个本地通知。

#### 接口定义

```csharp
public static void AddLocalNotification(int builderId, string content, string title, int notiId, int broadcastTime, string extrasJsonStr)
```

#### 参数说明

- builderId：通知的样式 ID，0 代表默认样式。
- content：通知内容。
- title：通知标题。
- notiId：通知 ID。
- broadcastTime：广播时间，代表从现有时间开始之后多长时间来发送这条本地通知，单位是秒。
- extrasJsonStr：要在通知中附加的额外 Json 信息。

#### 代码示例

```csharp
// 10 秒之后触发该本地推送。
JPushBinding.AddLocalNotification(0, "content", "title", 1, 10, null);
```

### API - RemoveLocalNotification(notiId)

移除指定本地通知。

#### 参数说明

- notiId: 通知 Id。

#### 代码示例

```csharp
JPushBinding.RemoveLocalNotification(1);    // 1：特定通知的 Notification ID。
```

### API - ClearLocalNotifications

清除所有本地通知。

#### 代码示例

```csharp
JPushBinding.ClearLocalNotifications();
```
