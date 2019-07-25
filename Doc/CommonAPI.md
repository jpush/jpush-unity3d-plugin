# 通用 API

以下除事件监听之外的方法都基于 `JPushBinding` 对象进行调用。

- [初始化与调试](#初始化与调试)
  - [Init(string gameObject)](#initstring-gameobject)
  - [SetDebug(bool enable)](#setdebugbool-enable)
  - [GetRegistrationId()](#getregistrationid)
- [标签与别名](#标签与别名)
  - [SetTags(int sequence, List<string> tags)](#settagsint-sequence-liststring-tags)
  - [AddTags(int sequence, List<string> tags)](#addtagsint-sequence-liststring-tags)
  - [DeleteTags(int sequence, List<string> tags)](#deletetagsint-sequence-liststring-tags)
  - [CleanTags(int sequence)](#cleantagsint-sequence)
  - [GetAllTags(int sequence)](#getalltagsint-sequence)
  - [CheckTagBindState(int sequence, string tag)](#checktagbindstateint-sequence-string-tag)
  - [SetAlias(int sequence, string alias)](#setaliasint-sequence-string-alias)
  - [DeleteAlias(int sequence)](#deletealiasint-sequence)
  - [GetAlias(int sequence)](#getaliasint-sequence)
  - [FilterValidTags(List<string>)](#filterValidTags)
- [API](#API)
  - [SetMaxGeofenceNumber(int maxNumber)](#setMaxGeofenceNumber)
  - [SetMobileNumber(int sequence, string mobileNumber)](#setMobileNumber-sequence-string-mobileNumber)
  - [DeleteGeofence(string geofenceid)](#deleteGeofence-string-geofenceid)
- [事件监听](#事件监听)
  - [OnReceiveNotification(string notification)](#onreceivenotificationstring-notification)
  - [OnReceiveMessage(string msg)](#onreceivemessagestring-msg)
  - [OnOpenNotification(string notification)](#onopennotificationstring-notification)
  - [OnJPushTagOperateResult(result)](#onjpushtagoperateresultresult)
  - [OnJPushAliasOperateResult(result)](#onjpushaliasoperateresultresult)
  - [OnMobileNumberOperatorResult(result)](#onjpushMobileNumberoperateresultresult)

## 初始化与调试

### Init(string gameObject)

插件初始化。

#### 参数说明

- gameObject: 代码脚本被挂载的游戏对象名。

### SetDebug(bool enable)

设置是否开启 Debug 模式。

#### 参数说明

- enable:
  - true: 开启 debug 模式，将会输出更多的 JPush 相关日志。
  - false: 关闭 debug 模式，建议在发布时关闭。

### GetRegistrationId()

获取当前设备的 Registration Id。

## 标签与别名

标签（tag）与别名（alias）的相关概念可查看[官方文档](https://docs.jiguang.cn/jpush/client/Android/android_senior/#_1)。

### SetTags(int sequence, List<string> tags)

给当前设备设置标签。

注意该操作是覆盖逻辑，即每次调用会覆盖之前已经设置的标签。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- tags: 标签列表。
  - 有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符（@!#$&*+=.|）。
  - 限制：每个 tag 命名长度限制为 40 字节，单个设备最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。

### AddTags(int sequence, List<string> tags)

给当前设备在已有的基础上新增标签。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- tags: 标签列表。

### DeleteTags(int sequence, List<string> tags)

删除标签。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- tags: 标签列表。

### CleanTags(int sequence)

清空标签。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。

### GetAllTags(int sequence)

获取当前设备的所有标签。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。

### CheckTagBindState(int sequence, string tag)

检查指定标签是否已经绑定。

`OnJPushTagOperateResult` 回调中会附带 `isBind` 属性。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- tag: 待查询的标签。

### SetAlias(int sequence, string alias)

设置别名，每个设备只会有一个别名。

注意：该接口是覆盖逻辑，即新的调用会覆盖之前的设置。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- alias: 要设置的别名。
  - 有效的别名组成：字母（区分大小写）、数字、下划线、汉字、特殊字符（@!#$&*+=.|）。
  - 限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。

### DeleteAlias(int sequence)

删除当前设备设置的别名。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。

### GetAlias(int sequence)

获取当前设备设置的别名。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。

### FilterValidTags(List<string> tags)

过滤非法tag

#### 参数说明

- tags: tag列表

## API

### SetMaxGeofenceNumber(int maxNumber)

设置最多允许保存的地理围栏数量，超过最大限制后，如果继续创建先删除最早创建的地理围栏。默认数量为10个

#### 参数说明

- maxNumber: 允许最大数，
                Andorid:允许设置最小1个，最大100个。
                IOS:iOS系统要求最大不能超过20个，否则会报错。

### SetMobileNumber(int sequence, string mobileNumber)

调用此 API 设置手机号码。该接口会控制调用频率，频率为 10s 之内最多 3 次。

#### 参数说明

- sequence: 用户自定义的操作序列号，同操作结果一起返回，用来标识一次操作的唯一性。
- mobileNumber: 手机号码。如果传 null 或空串则为解除号码绑定操作。 限制：只能以 “+” 或者 数字开头；后面的内容只能包含 “-” 和数字。


### DeleteGeofence(string geofenceid)

删除地理围栏。

#### 参数说明

- geofenceid: 地理围栏ID.


## 事件监听

监听事件都需要在插件 `init(gameObject)` 方法传入的对应 GameObject 中实现。

### OnReceiveNotification(string notification)

收到通知。

#### 参数说明

- notification: 通知内容的 Json 字符串。

Android 的通知内容格式为：

```text
{
  "msgId":"信息id"
  "title": "通知标题",
  "content": "通知内容",
  "extras": {   // 自定义键值对
    "key1": "value1",
    "key2": "value2"
}
```

iOS 的通知内容格式为：

```text
{
  "aps":{
    "alert":"通知内容",
    "badge":1,
    "sound":"default"
  },
  "key1":"value1",
  "key2":"value2",
  "_j_uid":11433016635,
  "_j_msgid":20266199577754012,
  "_j_business":1
}
```

### OnReceiveMessage(string msg)

收到自定义消息。

#### 参数说明

- msg: 自定义消息内容的 Json 字符串。

Android 的通知内容格式为：

```text
{
  "msgId":"信息id"
  "message": "自定义消息内容",
  "extras": {   // 自定义键值对
    "key1": "value1",
    "key2": "value2"
  }
}
```

iOS 的自定义消息内容格式为：

```text
{
  "content": "自定义消息内容",
  "extras": {  // 自定义键值对
    "key1": "value1",
    "key2": "value2"
  }
}
```

### OnOpenNotification(string notification)

点击通知栏通知事件。

#### 参数说明

- notification: 通知内容的 Json 字符串。

Android 的通知内容格式为：

```text
{
  "msgId":"信息id"
  "title": "通知标题",
  "content": "通知内容",
  "extras": {   // 自定义键值对
    "key1": "value1",
    "key2": "value2"
}
```

iOS 的通知内容格式为：

```text
{
  "aps":{
    "alert":"通知内容",
    "badge":1,
    "sound":"default"
  },
  "key1":"value1",
  "key2":"value2",
  "_j_uid":11433016635,
  "_j_msgid":20266199577754012,
  "_j_business":1
}
```

### OnJPushTagOperateResult(result)

JPush 的标签相关操作回调。

#### 参数说明

- result: Json 格式字符串。格式为：

```text
{
  "sequence": 1,            // 调用标签或别名方法时传入的。
  "code": 0,                // 结果码。0：成功；其他：失败（详细说明可参见官网文档）。
  "tag": ["tag1", "tag2"],  // 传入或查询得到的标签，当没有标签时，没有该字段。
  "isBind": true            // 是否已绑定。只有调用 CheckTagBindState 方法时才有该字段。
}
```

### OnJPushAliasOperateResult(result)

JPush 的别名相关操作回调。

#### 参数说明

- result: Json 格式字符串。格式为：

```text
{
  "sequence": 1, // 调用标签或别名方法时传入的。
  "code": 0,     // 结果码。0：成功；其他：失败（详细说明可参见官网文档）。
  "alias": "查询或传入的 alias"
}
```

### OnMobileNumberOperatorResult(result)

JPush 设置电话号码回调

#### 参数说明

- result: Json 格式字符串。格式为：

```text
{
  "sequence": 1, // 调用标签或别名方法时传入的。
  "code": 0,     // 结果码。0：成功；其他：失败（详细说明可参见官网文档）。
}
```