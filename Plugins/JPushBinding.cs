using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

#if UNITY_IPHONE
using LitJson;
#endif

// @version v3.1.0
namespace JPush
{
    public class JPushBinding : MonoBehaviour
    {
        #if UNITY_ANDROID

        private static AndroidJavaObject _plugin;

        private static int notificationDefaults = -1;
        private static int notificationFlags = 16;

        private static readonly int DEFAULT_ALL = -1;
        private static readonly int DEFAULT_SOUND = 1;
        private static readonly int DEFAULT_VIBRATE = 2;
        private static readonly int DEFAULT_LIGHTS = 4;

        private static readonly int FLAG_SHOW_LIGHTS = 1;
        private static readonly int FLAG_ONGOING_EVENT = 2;
        private static readonly int FLAG_INSISTENT = 4;
        private static readonly int FLAG_ONLY_ALERT_ONCE = 8;
        private static readonly int FLAG_AUTO_CANCEL = 16;
        private static readonly int FLAG_NO_CLEAR = 32;
        private static readonly int FLAG_FOREGROUND_SERVICE = 64;

        static JPushBinding()
        {
            using (AndroidJavaClass jpushClass = new AndroidJavaClass("cn.jiguang.unity.push.JPushBridge"))
            {
                _plugin = jpushClass.CallStatic<AndroidJavaObject>("getInstance");
            }
        }

        #endif

        /// <summary>
        /// 初始化 JPush。
        /// </summary>
        /// <param name="gameObject">游戏对象名。</param>
        public static void Init(string gameObject)
        {
            #if UNITY_ANDROID
            _plugin.Call("initPush", gameObject);

            #elif UNITY_IOS
            _initJpush(gameObject);

            #endif
        }

        /// <summary>
        /// 设置是否开启 Debug 模式。
        /// <para>Debug 模式将会输出更多的日志信息，建议在发布时关闭。</para>
        /// </summary>
        /// <param name="enable">true: 开启；false: 关闭。</param>
        public static void SetDebug(bool enable)
        {
            #if UNITY_ANDROID
            _plugin.Call("setDebug", enable);

            #elif UNITY_IOS
            _setDebugJpush(enable);

            #endif
        }

        /// <summary>
        /// 获取当前设备的 Registration Id。
        /// </summary>
        /// <returns>设备的 Registration Id。</returns>
        public static string GetRegistrationId()
        {
            #if UNITY_ANDROID
            return _plugin.Call<string>("getRegistrationId");

            #elif UNITY_IOS
            return _getRegistrationIdJpush();

            #else
            return "";

            #endif
        }

        /// <summary>
        /// 为设备设置标签（tag）。
        /// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void SetTags(int sequence, List<string> tags)
        {
            string tagsJsonStr = JsonHelper.ToJson<string>(tags);

            #if UNITY_ANDROID
            _plugin.Call("setTags", sequence, tagsJsonStr);

            #elif UNITY_IOS
            _setTagsJpush(sequence, tagsJsonStr);

            #endif
        }

        /// <summary>
        /// 为设备新增标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void AddTags(int sequence, List<string> tags)
        {
            string tagsJsonStr = JsonHelper.ToJson(tags);

            #if UNITY_ANDROID
            _plugin.Call("addTags", sequence, tagsJsonStr);

            #elif UNITY_IOS
            _addTagsJpush(sequence, tagsJsonStr);

            #endif
        }

        /// <summary>
        /// 删除标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void DeleteTags(int sequence, List<string> tags)
        {
            string tagsJsonStr = JsonHelper.ToJson(tags);

            #if UNITY_ANDROID
            _plugin.Call("deleteTags", sequence, tagsJsonStr);

            #elif UNITY_IOS
            _deleteTagsJpush(sequence, tagsJsonStr);

            #endif
        }

        /// <summary>
        /// 清空当前设备设置的标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void CleanTags(int sequence)
        {
            #if UNITY_ANDROID
            _plugin.Call("cleanTags", sequence);

            #elif UNITY_IOS
            _cleanTagsJpush(sequence);

            #endif
        }

        /// <summary>
        /// 获取当前设备设置的所有标签（tag）。
        /// <para>需要实现 OnJPushTagOperateResult 方法获得操作结果。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void GetAllTags(int sequence)
        {
            #if UNITY_ANDROID
            _plugin.Call("getAllTags", sequence);

            #elif UNITY_IOS
            _getAllTagsJpush(sequence);

            #endif
        }

        /// <summary>
        /// 查询指定标签的绑定状态。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tag">待查询的标签。</param>
        public static void CheckTagBindState(int sequence, string tag)
        {
            #if UNITY_ANDROID
            _plugin.Call("checkTagBindState", sequence, tag);

            #elif UNITY_IOS
            _checkTagBindStateJpush(sequence, tag);

            #endif
        }

        /// <summary>
        /// 设置别名。
        /// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="alias">
        ///     别名。
        ///     <para>有效的别名组成：字母（区分大小写）、数字、下划线、汉字、特殊字符@!#$&*+=.|。</para>
        ///     <para>限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void SetAlias(int sequence, string alias)
        {
            #if UNITY_ANDROID
            _plugin.Call("setAlias", sequence, alias);

            #elif UNITY_IOS
            _setAliasJpush(sequence, alias);

            #endif
        }

        /// <summary>
        /// 删除别名。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void DeleteAlias(int sequence)
        {
            #if UNITY_ANDROID
            _plugin.Call("deleteAlias", sequence);

            #elif UNITY_IOS
            _deleteAliasJpush(sequence);

            #endif
        }

        /// <summary>
        /// 获取当前设备设置的别名。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void GetAlias(int sequence)
        {
            #if UNITY_ANDROID
            _plugin.Call("getAlias", sequence);

            #elif UNITY_IOS
            _getAliasJpush(sequence);

            #endif
        }


        ///接口返回
        ///有效的 tag 集合。
        public static List<string> FilterValidTags(List<string> jsonTags)
        {
            string tagsJsonStr = JsonHelper.ToJson(jsonTags);
            string reJson = null;

            #if UNITY_ANDROID
            reJson = _plugin.Call<string>("filterValidTags", tagsJsonStr);
            #elif UNITY_IOS
            reJson =  _filterValidTagsJpush(tagsJsonStr);
            #endif
            if (null == reJson)
            {
                return new List<string>();
            }

            string[] reStringArray = JsonHelper.FromJson<string>(reJson);

            if (null == reStringArray)
            {
                return new List<string>();
            }

            List<string> reList = new List<string>(); ;
            for (int i = 0; i < reStringArray.Length; i++)
            {
                reList.Add(reStringArray[i]);
            }

            return reList;
        }
        /// <summary>
        /// 设置最多允许保存的地理围栏数量，超过最大限制后，如果继续创建先删除最早创建的地理围栏。默认数量为10个
        /// </summary>
        /// <param name="maxNumber">
        /// Andorid:允许设置最小1个，最大100个.
        /// IOS:iOS系统要求最大不能超过20个，否则会报错。
        /// </param>
        public static void SetMaxGeofenceNumber(int maxNumber)
        {
            #if UNITY_ANDROID
            SetMaxGeofenceNumberAndroid(maxNumber);
            #elif UNITY_IOS
            _setGeofenecMaxCountJpush(maxNumber);
            #endif
        }
        /// <summary>
        /// 删除地理围栏
        /// </summary>
        /// <param name="geofenceid">地理围栏ID</param>
        public static void DeleteGeofence(string geofenceid)
        {
            #if UNITY_ANDROID
            DeleteGeofenceAndroid(geofenceid);
            #elif UNITY_IOS
            _removeGeofenceWithIdentifierJpush(geofenceid);
            #endif
        }

        public static void SetMobileNumber(int sequence, string mobileNumber)
        {
            #if UNITY_ANDROID
            SetMobileNumberAndroid(sequence,mobileNumber);
            #elif UNITY_IOS
            _setMobileNumberJpush(sequence, mobileNumber);
            #endif
        }

         public static void SetAuth(bool enable)
        {
             #if UNITY_ANDROID
            _plugin.Call("setAuth", enable);
             #elif UNITY_IOS
             _setAuth(enable);
            #endif
        }


#if UNITY_ANDROID

        //-----
        //动态配置 channel，优先级比 AndroidManifest 里配置的高
        //channel 希望配置的 channel，传 null 表示依然使用 AndroidManifest 里配置的 channel
        public static void SetChannel(string channel)
        {
            _plugin.Call("setChannel",channel);
        }


        //用于上报用户的通知栏被打开，或者用于上报用户自定义消息被展示等客户端需要统计的事件。
        //参数说明
        //msgId：推送每一条消息和通知对应的唯一 ID
        public static void ReportNotificationOpened(string msgId)
        {
            _plugin.Call("reportNotificationOpened",msgId);
        }

        //功能说明
        //
        //设置地理围栏监控周期，最小3分钟，最大1天。默认为15分钟，当距离地理围栏边界小于1000米周期自动调整为3分钟。设置成功后一直使用设置周期，不会进行调整。
        //参数说明
        //
        //interval 监控周期，单位是毫秒。
        public static void SetGeofenceInterval(long interval)
        {
            _plugin.Call("setGeofenceInterval", interval);
        }

        //功能说明
        //
        //设置最多允许保存的地理围栏数量，超过最大限制后，如果继续创建先删除最早创建的地理围栏。默认数量为10个，允许设置最小1个，最大100个。
        //参数说明
        //
        //maxNumber 最多允许保存的地理围栏个数
        public static void SetMaxGeofenceNumberAndroid(int maxNumber)
        {
            _plugin.Call("setMaxGeofenceNumber", maxNumber);
        }


        public static void DeleteGeofenceAndroid(string geofenceid)
        {
            _plugin.Call("deleteGeofence", geofenceid);
        }

        //调用此 API 设置手机号码。该接口会控制调用频率，频率为 10s 之内最多 3 次。
        //sequence
        //用户自定义的操作序列号，同操作结果一起返回，用来标识一次操作的唯一性。
        //mobileNumber
        //手机号码。如果传 null 或空串则为解除号码绑定操作。
        //限制：只能以 “+” 或者 数字开头；后面的内容只能包含 “-” 和数字。
        public static void SetMobileNumberAndroid(int sequence, string mobileNumber)
        {
            _plugin.Call("setMobileNumber", sequence, mobileNumber);
        }

        //JPush SDK 开启和关闭省电模式，默认为关闭。
        //参数说明
        //S
        //enable 是否需要开启或关闭，true 为开启，false 为关闭
        public static void SetPowerSaveMode(bool enable)
        {
            _plugin.Call("setPowerSaveMode", enable);
        }


        //-------




        /// <summary>
        /// 停止 JPush 推送服务。 
        /// </summary>
        public static void StopPush()
        {
            _plugin.Call("stopPush");
        }

        /// <summary>
        /// 唤醒 JPush 推送服务，使用了 StopPush 必须调用此方法才能恢复。
        /// </summary>
        public static void ResumePush()
        {
            _plugin.Call("resumePush");
        }

        /// <summary>
        /// 判断当前 JPush 服务是否停止。
        /// </summary>
        /// <returns>true: 已停止；false: 未停止。</returns>
        public static bool IsPushStopped()
        {
            return _plugin.Call<bool>("isPushStopped");
        }

        /// <summary>
        /// 设置允许推送时间。
        /// </summary>
        /// <parm name="days">为 0~6 之间由","连接而成的字符串。</parm>
        /// <parm name="startHour">0~23</parm>
        /// <parm name="endHour">0~23</parm>
        public static void SetPushTime(string days, int startHour, int endHour)
        {
            _plugin.Call("setPushTime", days, startHour, endHour);
        }

        /// <summary>
        /// 设置通知静默时间。
        /// </summary>
        /// <parm name="startHour">0~23</parm>
        /// <parm name="startMinute">0~59</parm>
        /// <parm name="endHour">0~23</parm>
        /// <parm name="endMinute">0~23</parm>
        public static void SetSilenceTime(int startHour, int startMinute, int endHour, int endMinute)
        {
            _plugin.Call("setSilenceTime", startHour, startMinute, endHour, endMinute);
        }

        /// <summary>
        /// 设置保留最近通知条数。
        /// </summary>
        /// <param name="num">要保留的最近通知条数。</param>
        public static void SetLatestNotificationNumber(int num)
        {
            _plugin.Call("setLatestNotificationNumber", num);
        }

        public static void AddLocalNotification(int builderId, string content, string title, int nId,
                int broadcastTime, string extrasStr)
        {
            _plugin.Call("addLocalNotification", builderId, content, title, nId, broadcastTime, extrasStr);
        }

        public static void AddLocalNotificationByDate(int builderId, string content, string title, int nId,
                int year, int month, int day, int hour, int minute, int second, string extrasStr)
        {
            _plugin.Call("addLocalNotificationByDate", builderId, content, title, nId,
                year, month, day, hour, minute, second, extrasStr);
        }

        public static void RemoveLocalNotification(int notificationId)
        {
            _plugin.Call("removeLocalNotification", notificationId);
        }

        public static void ClearLocalNotifications()
        {
            _plugin.Call("clearLocalNotifications");
        }

        public static void ClearAllNotifications()
        {
            _plugin.Call("clearAllNotifications");
        }

        public static void ClearNotificationById(int notificationId)
        {
            _plugin.Call("clearNotificationById", notificationId);
        }

        /// <summary>
        /// 用于 Android 6.0 及以上系统申请权限。
        /// </summary>
        public static void RequestPermission()
        {
            _plugin.Call("requestPermission");
        }

        public static void SetBasicPushNotificationBuilder()
        {
            // 需要根据自己业务情况修改后再调用。
            int builderId = 1;
            int notiDefaults = notificationDefaults | DEFAULT_ALL;
            int notiFlags = notificationFlags | FLAG_AUTO_CANCEL;
            _plugin.Call("setBasicPushNotificationBuilder", builderId, notiDefaults, notiFlags, null);
        }

        public static void SetCustomPushNotificationBuilder()
        {
            // 需要根据自己业务情况修改后再调用。
            int builderId = 1;
            string layoutName = "yourNotificationLayoutName";

            // 指定最顶层状态栏小图标
            string statusBarDrawableName = "yourStatusBarDrawableName";

            // 指定下拉状态栏时显示的通知图标
            string layoutIconDrawableName = "yourLayoutIconDrawableName";

            _plugin.Call("setCustomPushNotificationBuilder", builderId, layoutName, statusBarDrawableName, layoutIconDrawableName);
        }

        public static void InitCrashHandler()
        {
            _plugin.Call("initCrashHandler");
        }

        public static void StopCrashHandler()
        {
            _plugin.Call("stopCrashHandler");
        }

        public static bool GetConnectionState()
        {
            return _plugin.Call<bool>("getConnectionState");
        }

#endif

#if UNITY_IOS

        public static void SetBadge(int badge)
        {
            _setBadgeJpush(badge);
        }

        public static void ResetBadge()
        {
            _resetBadgeJpush();
        }

        public static void SetApplicationIconBadgeNumber(int badge)
        {
            _setApplicationIconBadgeNumberJpush(badge);
        }

        public static int GetApplicationIconBadgeNumber()
        {
            return _getApplicationIconBadgeNumberJpush();
        }

        public static void StartLogPageView(string pageName)
        {
            _startLogPageViewJpush(pageName);
        }

        public static void StopLogPageView(string pageName)
        {
            _stopLogPageViewJpush(pageName);
        }

        public static void BeginLogPageView(string pageName, int duration)
        {
            _beginLogPageViewJpush(pageName, duration);
        }

        // 本地通知 -start

        public static void SendLocalNotification(string localParams)
        {
            _sendLocalNotificationJpush(localParams);
        }

        public static void SetLocalNotification(int delay, string content, int badge, string idKey) {
            JsonData jd = new JsonData();
            jd["alertBody"] = content;
            jd["idKey"] = idKey;
            string jsonStr = JsonMapper.ToJson(jd);
            _setLocalNotificationJpush(delay, badge, jsonStr);
        }

        public static void DeleteLocalNotificationWithIdentifierKey(string idKey) {
            JsonData jd = new JsonData();
            jd["idKey"] = idKey;
            string jsonStr = JsonMapper.ToJson(jd);
            _deleteLocalNotificationWithIdentifierKeyJpush(jsonStr);
        }

        public static void ClearAllLocalNotifications() {
            _clearAllLocalNotificationsJpush();
        }

        /// <summary>
        ///  移除通知中心显示推送和待推送请求，
        /// </summary>
        /// <param name="idKeys">要移除的id列表，null 移除所有</param>
        /// <param name="delivered">ture 显示的通知，false 还没有显示的通知，iOS10以下无效</param>
        public static void RemoveNotification(List<string> idKeys, bool delivered)
        {
            string idKeysStr = "";
            if (null != idKeys && idKeys.Count > 0)
            {
                idKeysStr = JsonHelper.ToJson(idKeys);
            }
            _removeNotificationJpush(idKeysStr, delivered);

        }

        public static void RemoveNotificationAll()
        {
            _removeNotificationAllJpush();

        }

        /// <summary>
        ///  查找通知中心显示推送和待推送请求，
        /// </summary>
        /// <param name="idKeys">要查找的id列表，null 查找所有</param>
        /// <param name="delivered">ture 显示的通知，false 还没有显示的通知，iOS10以下无效</param>
        //public static void FindNotification(List<string> idKeys, bool delivered)
        //{
        //    string idKeysStr = "";
        //    if (null != idKeys && idKeys.Count > 0)
        //    {
        //        idKeysStr = JsonHelper.ToJson(idKeys);
        //    }
        //    _findNotification(idKeysStr, delivered);

        //}


        // 本地通知 - end

        //其他 - start

  
        /// <summary>
        /// 用于统计用户应用崩溃日志,如果需要统计 Log 信息，调用该接口。当你需要自己收集错误信息时，切记不要调用该接口。
        /// </summary>
        public static void CrashLogON()
        {
            _crashLogONJpush();

        }

        /// <summary>
        /// 地理位置上报
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">经度</param>
        public static void SetLatitude(double latitude, double longitude)
        {
            _setLatitudeJpush( latitude, longitude);

        }
        //其他 - end


        [DllImport("__Internal")]
        private static extern void _initJpush(string gameObject);

        [DllImport("__Internal")]
        private static extern void _setDebugJpush(bool enable);

        [DllImport("__Internal")]
        private static extern void _setAuth(bool enable);

        [DllImport("__Internal")]
        private static extern string _getRegistrationIdJpush();

        [DllImport("__Internal")]
        private static extern void _setTagsJpush(int sequence, string tags);

        [DllImport("__Internal")]
        private static extern void _addTagsJpush(int sequence, string tags);

        [DllImport("__Internal")]
        private static extern void _deleteTagsJpush(int sequence, string tags);

        [DllImport("__Internal")]
        private static extern void _cleanTagsJpush(int sequence);

        [DllImport("__Internal")]
        private static extern void _getAllTagsJpush(int sequence);

        [DllImport("__Internal")]
        private static extern void _checkTagBindStateJpush(int sequence, string tag);


        [DllImport("__Internal")]
        private static extern string _filterValidTagsJpush(string tags);


        [DllImport("__Internal")]
        private static extern void _setAliasJpush(int sequence, string alias);

        [DllImport("__Internal")]
        private static extern void _deleteAliasJpush(int sequence);

        [DllImport("__Internal")]
        private static extern void _getAliasJpush(int sequence);

        [DllImport("__Internal")]
        private static extern void _setBadgeJpush(int badge);

        [DllImport("__Internal")]
        private static extern void _resetBadgeJpush();

        [DllImport("__Internal")]
        private static extern void _setApplicationIconBadgeNumberJpush(int badge);

        [DllImport("__Internal")]
        private static extern int _getApplicationIconBadgeNumberJpush();

        [DllImport("__Internal")]
        private static extern void _startLogPageViewJpush(string pageName);

        [DllImport("__Internal")]
        private static extern void _stopLogPageViewJpush(string pageName);

        [DllImport("__Internal")]
        private static extern void _beginLogPageViewJpush(string pageName, int duration);

        [DllImport("__Internal")]
        public static extern void _setLocalNotificationJpush(int delay, int badge, string alertBodyAndIdKey);

        [DllImport("__Internal")]
        public static extern void _sendLocalNotificationJpush(string localParams);
        
        [DllImport("__Internal")]
        public static extern void _deleteLocalNotificationWithIdentifierKeyJpush(string idKey);

        [DllImport("__Internal")]
        public static extern void _clearAllLocalNotificationsJpush();



        ///    调用此 API 来设置最大的地理围栏监听个数，默认值为10
        ///    参数说明
        ///    count
        ///    类型要求为NSInteger 类型
        ///    默认值为10
        ///    iOS系统要求最大不能超过20个，否则会报错。
        [DllImport("__Internal")]
        public static extern void _setGeofenecMaxCountJpush(int count);

        /// <summary>
        /// 删除地理围栏
        /// </summary>
        /// <param name="geofenceId">地理围栏ID</param>
        [DllImport("__Internal")]
        public static extern void _removeGeofenceWithIdentifierJpush(string geofenceId);


        //功能说明
        //API 用于统计用户应用崩溃日志
        //调用说明
        //如果需要统计 Log 信息，调用该接口。当你需要自己收集错误信息时，切记不要调用该接口。
        [DllImport("__Internal")]
        public static extern void _crashLogONJpush();

        //   功能说明

        //用于短信补充功能。设置手机号码后，可实现“推送不到短信到”的通知方式，提高推送达到率。
        //参数说明
        //mobileNumber 手机号码。只能以 “+” 或者数字开头，后面的内容只能包含 “-” 和数字，并且长度不能超过 20。如果传 nil 或空串则为解除号码绑定操作
        //completion 响应回调。成功则 error 为空，失败则 error 带有错误码及错误信息，具体错误码详见错误码定义
        //调用说明
        //此接口调用频率有限制，10s 之内最多 3 次。建议在登录成功以后，再调用此接口。结果信息通过 completion 异步返回，也可将completion 设置为 nil 不处理结果信息。
        [DllImport("__Internal")]
        public static extern void _setMobileNumberJpush(int sequence,string mobileNumber);

        [DllImport("__Internal")]
        public static extern void _setLatitudeJpush(double latitude, double longitude);

        /// <summary>
        ///  移除通知中心显示推送和待推送请求，
        /// </summary>
        /// <param name="idKey">要移除的id列表，null 移除所有</param>
        /// <param name="delivered">ture 显示的通知，false 还没有显示的通知，iOS10以下无效</param>
        [DllImport("__Internal")]
        public static extern void _removeNotificationJpush(string idKey, bool delivered);


        [DllImport("__Internal")]
        public static extern void _removeNotificationAllJpush();

        /// <summary>
        ///  查找通知中心显示推送和待推送请求，
        /// </summary>
        /// <param name="idKey">要查找的id列表，null 查找所有</param>
        /// <param name="delivered">ture 显示的通知，false 还没有显示的通知，iOS10以下无效</param>
        //[DllImport("__Internal")]
        //public static extern void _findNotification(string idKey, bool delivered);
#endif
    }
}
