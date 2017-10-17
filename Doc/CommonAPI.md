# 通用 API

以下除事件监听之外的方法都基于 `JPushBinding` 对象进行调用。

## 初始化与调试

### Init(string gameObject)

插件初始化。

#### 参数说明

- gameObject: 游戏对象名。所有回调事件都需要在该 Game Object 中实现。

### SetDebug(bool enable)

设置是否开启 Debug 模式。

#### 参数说明

- enable:
  - true: 开启 debug 模式，将会输出更多的 JPush 相关日志。
  - false: 关闭 debug 模式，建议在发布时关闭。

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
- tag: 待查询的标签。

### SetAlias(int sequence, string alias)

设置别名，每个设备只会有一个别名。

注意：该接口是覆盖逻辑，即新的调用会覆盖之前的设置。

#### 参数说明

- sequence: 作为一次操作的唯一标识，会在 `OnJPushTagOperateResult` 回调中一并返回。
- alias: 要设置的别名。
  - 有效的别名组成：字母（区分大小写）、数字、下划线、汉字、特殊字符（@!#$&*+=.|）。
  - 限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。

## 事件监听

监听事件都需要在插件 `init(gameObject)` 方法传入的对应 GameObject 中实现。

### OnGetRegistrationId(string rId)

向极光服务器注册成功，首次得到 Registration Id 时触发。

需要注意该事件只会在首次注册成功时触发，之后如果想要获得 Registration Id，可直接调用 `GetRegistrationId()` 方法。

#### 参数说明

- rId: 该设备的 Registration Id，可作为设备的唯一标识。

### OnReceiveNotification(string notification)

收到通知。

#### 参数说明

- notification: 通知内容的 Json 字符串。

Android 的通知内容格式为：

```json
{
  "title": "通知标题",
  "content": "通知内容",
  "extras": {   // 自定义键值对
    "key1": "value1",
    "key2": "value2"
}
```

iOS 的通知内容格式为：

```json

```

### OnReceiveMessage(string msg)

收到自定义消息。

#### 参数说明

- msg: 自定义消息内容的 Json 字符串。

Android 的通知内容格式为：

```json
{
  "message": "自定义消息内容",
  "extras": {   // 自定义键值对
    "key1": "value1",
    "key2": "value2"
  }
}
```

### OnJPushTagOperateResult(result)

JPush 的标签相关操作回调。

#### 参数说明

- result: Json 格式字符串。格式为：

```json
{
  "sequence": 1, // 调用标签或别名方法时传入的。
  "code": 0,     // 结果码。0：成功；其他：失败（详细说明可参见官网文档）。
  "tag": ["tag1", "tag2"],  // 传入或查询得到的标签，当没有标签时，没有该字段。
  "isBind": true            // 是否已绑定。只有调用 CheckTagBindState 方法时才有该字段。
}
```

### OnJPushAliasOperateResult(result)

JPush 的别名相关操作回调。

#### 参数说明

- result: Json 格式字符串。格式为：

```json
{
  "sequence": 1, // 调用标签或别名方法时传入的。
  "code": 0,     // 结果码。0：成功；其他：失败（详细说明可参见官网文档）。
  "alias": "查询或传入的 alias"
}
```