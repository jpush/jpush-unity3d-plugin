# Android API

具体 API 代码在 JPushBinding.cs 文件中。

- [初始化推送服务](#初始化推送服务)
- [停止推送服务](#停止推送服务)
- [恢复推送服务](#恢复推送服务)
- [获取 RegistrationID](#获取-registrationid)
- [回调事件](#回调事件)
- [设置别名与标签相关](#设置别名与标签相关)
- [清除通知](#清除通知)
- [设置允许推送时间](#设置允许推送时间)
- [设置通知静默时间](#设置通知静默时间)
- [申请权限接口（用于 Android 6.0 及以上系统）](#申请权限接口用于-android-60-及以上系统)
- [通知栏样式定制](#通知栏样式定制)
- [设置保留最近通知条数](#设置保留最近通知条数)
- [本地通知](#本地通知)

## 初始化推送服务
### API - initJPush

只有在初始化了之后，才能调用下面的一系列 API。

#### 接口定义

	public static void initJPush(string gameObject, string func)

#### 代码示例：

    void onStart()
    {
		gameObject.name = "Main Camera";  // 示例
        JPushBinding.initJPush(gameObject.name, "");
    }


## 停止推送服务
### API - stopJPush
调用了该 API 后，JPush 推送服务完全被停止。具体表现为：

- JPush Service 不在后台运行；
- 收不到推送消息；
- 极光推送所有的其他 API 调用都无效,不能通过 initJPush 恢复，需要调用 resumePush 恢复。

#### 接口定义

	public static void stopJPush()

#### 代码示例

    JPushBinding.stopJPush();

## 恢复推送服务
### API - resumeJPush
调用了此 API 后，极光推送完全恢复正常工作。

#### 接口定义

	public static void resumeJPush()

#### 代码示例

    JPushBinding.resumeJPush();

## 获取 RegistrationID
### API - getRegistrationId
调用此 API 来取得应用程序对应的 RegistrationID。 只有当应用程序成功注册到 JPush 的服务器时才返回对应的值，否则返回空字符串。


**Registration ID** 定义：
集成了 JPush SDK 的应用程序在第一次成功注册到 JPush 服务器时，JPush 服务器会给客户端返回一个唯一的该设备的标识 - RegistrationID。JPush SDK 会以广播的形式发送 RegistrationID 到应用程序。

应用程序可以把此 RegistrationID 保存以自己的应用服务器上，然后就可以根据 RegistrationID 来向设备推送消息或者通知。
#### 接口定义

	public static string getRegistrationId()

#### 代码示例

    string registrationId = JPushBinding.getRegistrationId();

## 回调事件
### API - recvNotification
收到通知时系统会执行该方法，开发者根据需要自行定义。

#### 代码示例

	/**
	* {
	*	"title": "notiTitle",
	*   "content": "content",
	*   "extras": {
	*		"key1": "value1",
	*       "key2": "value2"
	* 	}
	* }
	*/
	void recvNotification(string jsonStr) {
		// 开发者根据自己需要定义收到通知后的操作。
	}

### API - openNotification
当打开通知时，执行该方法。

#### 代码示例

	/**
	* {
	*	"title": "notiTitle",
	*   "content": "content",
	*   "extras": {
	*		"key1": "value1",
	*       "key2": "value2"
	* 	}
	* }
	*/
	void openNotification(string jsonStr)
	{
		// 开发者根据需要定义打开通知后的操作。
	}

### API - recvMessage
当收到自定义消息时，执行该方法。

#### 代码示例

	/**
	* {
	*   "message": "message",
	*   "extras": {
	*		"key1": "value1",
	*       "key2": "value2"
	* 	}
	* }
	*/
	void recvMessage(string jsonStr)
	{
		// 开发者根据需要定义收到自定义消息后的操作。
	}


## 设置别名与标签

### API - filterValidTags
调用此 API 检查 tags 是否可用并返回可用的 tag 字符串。

> 使用建议
设置 tags 时，如果其中一个 tag 无效，则整个设置过程失败。
如果 App 的 tags 会在运行过程中动态设置，并且存在对 JPush SDK tag 规定的无效字符，
则有可能一个 tag 无效导致这次调用里所有的 tags 更新失败。
这时你可以调用本方法 filterValidTags 来过滤掉无效的 tags，得到有效的 tags，
再调用 JPush SDK 的 set tags / alias 方法。

#### 接口定义

	public static string filterValidTags(string tags)

#### 代码示例

    string validTags = JPushBinding.filterValidTags("yourTag1, yourTag2");

### API - setTags
调用此 API 来设置标签。

需要理解的是，这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。

> 使用建议
如果待设置的 alias / tags 是动态的，有可能在调用 setAliasAndTags 时因为 alias / tags 无效而整调用失败。
调用此方法只设置 tags，可以排除可能无效的 alias 对本次调用的影响。

#### 接口定义

    public static void setTags(string tags)

#### 参数定义

- tags
    - 空数组或列表表示取消之前的设置。
    - 每次调用至少设置一个 tag，覆盖之前的设置，不是新增。
    - 有效的标签组成：字母（区分大小写）、数字、下划线、汉字。
    - 限制：每个 tag 命名长度限制为 40 字节，最多支持设置 100 个 tag，但总长度不得超过 1K 字节（判断长度需采用UTF-8编码）。
        - 单个设备最多支持设置 100 个 tag，App 全局 tag 数量无限制。

#### 代码示例

    JPushBinding.setTags("yourTag1, yourTags2");

### API - setAlias
调用此 API 来设置别名。

需要理解的是，这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。

#### 接口定义

	public static void setAlias(string alias)

#### 参数定义

- alias
    - ""（空字符串）表示取消之前的设置。
    - 每次调用设置有效的别名，覆盖之前的设置。
    - 有效的别名组成：字母（区分大小写）、数字、下划线、汉字。
    - 限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。

#### 代码示例

    JPushBinding.setAlias("yourAlias");

### API - setAliasAndTags
调用此 API 来同时设置别名与标签。

需要理解的是，这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。

在之前调用过后，如果需要再次改变别名与标签，只需要重新调用此 API 即可。

#### 接口定义

	public static void setAliasAndTags(string alias, string tags)

#### 参数定义

- alias
    - null 此次调用不设置此值（注：不是指的字符串"null"）。
    - ""（空字符串）表示取消之前的设置。
    - 每次调用设置有效的别名，覆盖之前的设置。
    - 有效的别名组成：字母（区分大小写）、数字、下划线、汉字。
    - 限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。

- tags
    - null 此次调用不设置此值（注：不是指的字符串"null"）。
    - 空数组或列表表示取消之前的设置。
    - 每次调用至少设置一个 tag，覆盖之前的设置，不是新增。
    - 有效的标签组成：字母（区分大小写）、数字、下划线、汉字。
    - 限制：每个 tag 命名长度限制为 40 字节，最多支持设置 100 个 tag，但总长度不得超过 1K 字节（判断长度需采用 UTF-8 编码）。

 ### 代码示例

    JPushBinding.setAliasAndTags("yourAlias", "yourTags1, yourTags2");


## 清除通知
### API - clearAllNotifications
此 API 提供清除通知的功能，包括：清除所有 JPush 展现的通知（不包括非 JPush SDK 展现的）。

#### 接口定义

	public static void clearAllNotifications()

#### 代码示例

    JPushBinding.clearAllNotifications();

### API - clearNotificationById
此 API 用于清除指定的某个通知。

#### 接口定义

	public static void clearNotificationById(int notiId)

#### 代码示例

    JPushBinding.clearNotificationById(1); // 1: 指定通知的 notificationId


## 设置允许推送时间
### API - setPushTime
默认情况下用户在任何时间都允许推送。即任何时候有推送下来，客户端都会收到，并展示。

开发者可以调用此 API 来设置允许推送的时间。

如果不在该时间段内收到消息，当前的行为是：推送到的通知会被扔掉。

> 这是一个纯粹客户端的实现。
 所以与客户端时间是否准确、时区等这些，都没有关系。

#### 接口定义

	public static void setPushTime(string days, int startHour, int endHour)

#### 参数说明
- days: 表示星期中的日期，0 表示星期天，1 表示星期一，以此类推。如果 days 为 null，表示任何时间都可以收到消息；如果为 ""，则表示任何时间都**不能**收到消息。
- startHour: 允许推送的开始时间（24小时制：范围是 0 - 23）。
- endHour: 允许推送的结束时间（24小时制：范围是 0 - 23）。

#### 代码示例

    JPushBinding.setPushTime("0,1,2,3", 8, 20); // 周日到周三的早上8点至晚上8点可以推送。


## 设置通知静默时间
### API - setSilenceTime
默认情况下用户在收到推送通知时，客户端可能会有震动，响铃等提示。但用户在睡觉、开会等时间点希望为 "免打扰" 模式，也是静音时段的概念。

开发者可以调用此 API 来设置静音时段，如果在该时间段内收到消息，则：不会有铃声和震动。

#### 接口定义

		public static void setSilenceTime(int startHour, int startMinute, int endHour, int endMinute)

#### 参数说明

- startHour：静默时段的开始时间 - 小时 （24小时制，范围：0 - 23 ）；
- startMinute：静默时段的开始时间 - 分钟（范围：0 - 59 ）；
- endHour：静默时段的结束时间 - 小时 （24小时制，范围：0 - 23 ）；
- endMinute：静默时段的结束时间 - 分钟（范围：0 - 59 ）；

#### 代码示例

    JPushBinding.setSilenceTime(20, 0, 7, 0);   // 设置晚上8点至早上7点通知静默。

## 申请权限接口（用于 Android 6.0 及以上系统）
### API - requestPermission
在 Android 6.0 及以上的系统上，需要去请求一些用到的权限，JPush SDK 用到的一些需要请求如下权限，因为需要这些权限使统计更加精准，功能更加丰富，建议开发者调用。

    "android.permission.READ_PHONE_STATE"
    "android.permission.WRITE_EXTERNAL_STORAGE"
    "android.permission.READ_EXTERNAL_STORAGE"
    "android.permission.ACCESS_FINE_LOCATION"

#### 接口定义

	public static void requestPermission()

#### 代码示例

    JPushBinding.requestPermission();


## 通知栏样式定制
大多数情况下，开发者不需要调用这里的定制通知栏 API 来自定义通知栏样式，只需要使用 SDK 默认的即可。

如果您想：

- 改变 Notification 里的铃声、震动、显示与消失行为；
- 自定义通知栏显示样式；
- 不同的 Push 通知，Notification 样式不同；
- 则请使用本通知栏定制 API 提供的能力。


### API - setBasicPushNotificationBuilder
用于定制 Android Notification 里的 defaults / flags / icon 等基础样式（行为）。

如果要使用，一定要修改 JPushBinding 中的 setBasicPushNotificationBuilder 方法，去根据自己的业务逻辑修改代码。

#### 接口定义

	public static void setBasicPushNotificationBuilder()

#### 代码示例

    public static void setBasicPushNotificationBuilder(string gameObject, string func)
    {
        Debug.Log("unity---setBasicPushNotificationBuilder");
        // 需要根据自己业务情况修改后再调用。
        int builderId = 1;  // 定义该样式的 id 为 1，后台推送时就需要也设置相应推送的 builderId 为 1。
        int notiFlags = notificationFlags | FLAG_AUTO_CANCEL;
        int notiDefaults = notificationDefaults | DEFAULT_ALL;

        // statusBarDrawableName: 状态栏通知图标的资源文件名称（R.drawable.statusBarDrawableName）。
        _plugin.Call("setBasicPushNotificationBuilder", gameObject, func,
            builderId, notiDefaults, notiFlags, "statusBarDrawableName");
    }

    // 使用时直接调用
    JPushBinding.setBasicPushNotificationBuilder();


### API - setCustomPushNotificationBuilder
继承 Basic 进一步让开发者定制 Notification Layout。

#### 接口定义

	public static void setCustomPushNotificationBuilder()

#### 代码示例

    public static void setCustomPushNotificationBuilder(string gameObject,
            string func)
    {
        Debug.Log("unity---setCustomPushNotificationBuilder");
        // 需要根据自己业务情况修改后再调用。
        int builderId = 1;
        string layoutName = "yourNotificationLayoutName";
        // 指定最顶层状态栏小图标。
        string statusBarDrawableName = "yourStatusBarDrawableName";
        // 指定下拉状态栏时显示的通知图标。
        string layoutIconDrawableName = "yourLayoutIconDrawableName";

        _plugin.Call("setCustomPushNotificationBuilder", builderId,
            layoutName, statusBarDrawableName, layoutIconDrawableName);
    }

    // 使用时直接调用
    JPushBinding.setCustomPushNotificationBuilder();


## 设置保留最近通知条数
### API - setLatestNotificationNumber

通过极光推送，推送了很多通知到客户端时，如果用户不去处理，就会有很多保留在那里。

此功能用于限制保留的通知条数，默认为保留最近 5 条通知。

开发者可通过调用此 API 来定义为不同的数量。

> 仅对通知有效。所谓保留最近的，意思是，如果有新的通知到达，之前列表里最老的那条会被移除。
例如，设置为保留最近 5 条通知。假设已经有 5 条显示在通知栏，当第 6 条到达时，第 1 条将会被移除。

#### 接口定义

	public static void setLatestNotificationNumber(int num)

#### 代码示例

    JPushBinding.setLatestNotificationNumber(10);   // 保留最近的 10 条通知。


## 本地通知

通过极光推送的SDK，开发者只需要简单调用几个接口，便可以在应用中定时发送本地通知。

> - 本地通知API不依赖于网络，无网条件下依旧可以触发。
> - 本地通知与网络推送的通知是相互独立的，不受保留最近通知条数上限的限制。
> - 本地通知的定时时间是自发送时算起的，不受中间关机等操作的影响。

### API - addLocalNotification
添加一个本地通知。

#### 接口定义

	public static void addLocalNotification(int builderId, string content,
			string title, int notiId, int broadcastTime, string extrasJsonStr)

#### 代码示例

    JPushBinding.addLocalNotification(1, "content", "title", 1, 0, "yourJsonStr");

### API - removeLocalNotification
移除指定本地通知。

#### 接口定义

	public static void removeLocalNotification(int notiId)

#### 代码示例

    JPushBinding.removeLocalNotification(1);    // 1：特定 Notification ID。

### API - clearLocalNotifications
清除所有本地通知。

#### 接口定义

	public static void clearLocalNotifications()

#### 代码示例

    JPushBinding.clearLocalNotifications();
