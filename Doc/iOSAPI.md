# iOS API

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

### SetLocalNotification(int delay, string content, int badge, string idKey)

注册本地通知。

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