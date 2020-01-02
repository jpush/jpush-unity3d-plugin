#import "JPushUnityManager.h"
#import "JPUSHService.h"
#import "JPushEventCache.h"

#pragma mark - Utility Function

#if defined(__cplusplus)
extern "C" {
#endif
    extern void       UnitySendMessage(const char* obj, const char* method, const char* msg);
    extern NSString*  CreateNSString (const char* string);
    extern id         APNativeJSONObject(NSData *data);
    extern NSData *   APNativeJSONData(id obj);
#if defined(__cplusplus)
}
#endif

static NSString *gameObjectName = @"";

@interface JPushUnityInstnce : NSObject {
@private
}
+(JPushUnityInstnce*)sharedInstance;
@end


#if defined(__cplusplus)
extern "C" {
#endif
    const char *tagCallbackName_ = "OnJPushTagOperateResult";
    const char *aliasCallbackName_ = "OnJPushAliasOperateResult";
    
    static char *MakeHeapString(const char *string) {
        if (!string){
            return NULL;
        }
        char *mem = static_cast<char*>(malloc(strlen(string) + 1));
        if (mem) {
            strcpy(mem, string);
        }
        return mem;
    }
    
    NSString *CreateNSString (const char *string) {
        return [NSString stringWithUTF8String:(string ? string : "")];
    }
    
    id APNativeJSONObject(NSData *data) {
        if (!data) {
            return nil;
        }
        
        NSError *error = nil;
        id retId = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
        
        if (error) {
            NSLog(@"%s trans data to obj with error: %@", __func__, error);
            return nil;
        }
        
        return retId;
    }
    
    NSData *APNativeJSONData(id obj) {
        NSError *error = nil;
        NSData *data = [NSJSONSerialization dataWithJSONObject:obj options:0 error:&error];
        if (error) {
            NSLog(@"%s trans obj to data with error: %@", __func__, error);
            return nil;
        }
        return data;
    }
    
    NSString *messageAsDictionary(NSDictionary * dic) {
        NSData *data = APNativeJSONData(dic);
        return [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
    }
    
    JPUSHTagsOperationCompletion tagsOperationCompletion = ^(NSInteger iResCode, NSSet *iTags, NSInteger seq) {
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
        [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
        [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
        
        if (iResCode == 0) {
            dic[@"tags"] = [iTags allObjects];
        }
        
        UnitySendMessage([gameObjectName UTF8String], tagCallbackName_, messageAsDictionary(dic).UTF8String);
        
    };
    
    JPUSHAliasOperationCompletion aliasOperationCompletion = ^(NSInteger iResCode, NSString *iAlias, NSInteger seq) {
        NSMutableDictionary* dic = [[NSMutableDictionary alloc] init];
        [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
        [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
        
        if (iResCode == 0) {
            [dic setObject:iAlias forKey:@"alias"];
        }
        
        UnitySendMessage([gameObjectName UTF8String], aliasCallbackName_, messageAsDictionary(dic).UTF8String);
    };
    
    NSInteger integerValue(int intValue) {
        NSNumber *n = [NSNumber numberWithInt:intValue];
        return [n integerValue];
    }
    
    int intValue(NSInteger integerValue) {
        NSNumber *n = [NSNumber numberWithInteger:integerValue];
        return [n intValue];
    }
    // private - end
    
    void _initJpush(char *gameObject) {
        gameObjectName = [NSString stringWithUTF8String:gameObject];
        NSNotificationCenter *msgCenter = [NSNotificationCenter defaultCenter];
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                                                 selector:@selector(networkDidRecieveMessage:)
                                                     name:kJPFNetworkDidReceiveMessageNotification
                                                   object:nil];
        
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                                                 selector:@selector(networkDidRecievePushNotification:)
                                                     name:@"JPushPluginReceiveNotification"
                                                   object:nil];
        
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                                                 selector:@selector(networkOpenPushNotification:)
                                                     name:@"JPushPluginOpenNotification"
                                                   object:nil];
        [[JPushEventCache sharedInstance] scheduleNotificationQueue];
    }
    
    void _setDebugJpush(bool enable) {
        if (enable) {
            [JPUSHService setDebugMode];
        } else {
            [JPUSHService setLogOFF];
        }
    }
    
    const char *_getRegistrationIdJpush() {
        NSString *registrationID = [JPUSHService registrationID];
        return MakeHeapString([registrationID UTF8String]);
    }
    
    // Tag & Alias - start
    
    void _setTagsJpush(int sequence, const char *tags) {
        NSString *nsTags = CreateNSString(tags);
        if (![nsTags length]) {
            return;
        }
        
        NSData *data = [nsTags dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *array = dict[@"Items"];
        NSSet *set = [[NSSet alloc] initWithArray:array];
        
        [JPUSHService setTags:set completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _addTagsJpush(int sequence, char *tags) {
        NSString* tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService addTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteTagsJpush(int sequence, char *tags) {
        NSString *tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService deleteTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _cleanTagsJpush(int sequence) {
        [JPUSHService cleanTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAllTagsJpush(int sequence) {
        [JPUSHService getAllTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _checkTagBindStateJpush(int sequence, char *tag) {
        NSString *nsTag = CreateNSString(tag);
        [JPUSHService validTag:nsTag completion:^(NSInteger iResCode, NSSet *iTags, NSInteger seq, BOOL isBind) {
            NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
            [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
            [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
            
            if (iResCode == 0) {
                [dic setObject:[iTags allObjects] forKey:@"tags"];
                [dic setObject:[NSNumber numberWithBool:isBind] forKey:@"isBind"];
            }
            
            UnitySendMessage([gameObjectName UTF8String], tagCallbackName_, messageAsDictionary(dic).UTF8String);
        } seq:(NSInteger)sequence];
    }
    
    const char * _filterValidTagsJpush(char *tags){
        
        NSString *nsTags = CreateNSString(tags);
        if (![nsTags length]) {
            return nil;
        }
        
        NSData *data = [nsTags dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *array = dict[@"Items"];
        NSSet *set = [[NSSet alloc] initWithArray:array];
        
        NSSet *rSet =  [JPUSHService filterValidTags:set];
        
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
        
        if ([rSet count]) {
            dic[@"Items"] = [rSet allObjects];
        } else {
            return  nil;
        }
        
        return MakeHeapString([messageAsDictionary(dic) UTF8String]);
        
    }
    
    void _setAliasJpush(int sequence, const char * alias){
        NSString *nsAlias = CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        
        [JPUSHService setAlias:nsAlias completion:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAliasJpush(int sequence) {
        [JPUSHService getAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteAliasJpush(int sequence) {
        [JPUSHService deleteAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    // Tag & Alias - end
    
    // 角标处理 - start
    
    void _setBadgeJpush(const int badge){
        [JPUSHService setBadge:integerValue(badge)];
    }
    
    void _resetBadgeJpush(){
        [JPUSHService resetBadge];
    }
    
    void _setApplicationIconBadgeNumberJpush(const int badge){
        [UIApplication sharedApplication].applicationIconBadgeNumber = integerValue(badge);
    }
    
    int _getApplicationIconBadgeNumberJpush(){
        return intValue([UIApplication sharedApplication].applicationIconBadgeNumber);
    }
    
    // 角标处理 - end
    
    // 页面统计 - start
    
    void _startLogPageViewJpush(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService startLogPageView:sendPageName];
    }
    
    void _stopLogPageViewJpush(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService stopLogPageView:sendPageName];
    }
    
    void _beginLogPageViewJpush(const char *pageName, const int duration) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService beginLogPageView:sendPageName duration:duration];
    }
    
    // 页面统计 - end
    
    // 本地通知旧接口 - start
    
    void _setLocalNotificationJpush(int delay, int badge, char *alertBodyAndIdKey){
        NSDate *date = [NSDate dateWithTimeIntervalSinceNow:integerValue(delay)];
        
        NSString *nsalertBodyAndIdKey = CreateNSString(alertBodyAndIdKey);
        if (![nsalertBodyAndIdKey length]) {
            return ;
        }
        NSData       *data =[nsalertBodyAndIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendAlertBody = dict[@"alertBody"];
        NSString     *sendIdkey = dict[@"idKey"];
        
        [JPUSHService setLocalNotification:date alertBody:sendAlertBody badge:badge alertAction:nil identifierKey:sendIdkey userInfo:nil soundName:nil];
    }
    
    void _sendLocalNotificationJpush(char *params) {
        NSString *nsalertBodyAndIdKey = CreateNSString(params);
        if (![nsalertBodyAndIdKey length]) {
            return ;
        }
        NSData       *data =[nsalertBodyAndIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        
        JPushNotificationContent *content = [[JPushNotificationContent alloc] init];
        if (dict[@"title"]) {
            content.title = dict[@"title"];
        }
        
        if (dict[@"subtitle"]) {
            content.subtitle = dict[@"subtitle"];
        }
        
        if (dict[@"content"]) {
            content.body = dict[@"content"];
        }
        
        if (dict[@"badge"]) {
            content.badge = dict[@"badge"];
        }
        
        if (dict[@"action"]) {
            content.action = dict[@"action"];
        }
        
        if (dict[@"extra"]) {
            content.userInfo = dict[@"extra"];
        }
        
        if (dict[@"sound"]) {
            content.sound = dict[@"sound"];
        }
        
        JPushNotificationTrigger *trigger = [[JPushNotificationTrigger alloc] init];
        if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 10.0) {
            if (dict[@"fireTime"]) {
                NSNumber *date = dict[@"fireTime"];
                NSTimeInterval currentInterval = [[NSDate date] timeIntervalSince1970];
                NSTimeInterval interval = [date doubleValue] - currentInterval;
                interval = interval>0?interval:0;
                trigger.timeInterval = interval;
                
            }
        }
        
        else {
            if (dict[@"fireTime"]) {
                NSNumber *date = dict[@"fireTime"];
                trigger.fireDate = [NSDate dateWithTimeIntervalSince1970: [date doubleValue]];
            }
        }
        JPushNotificationRequest *request = [[JPushNotificationRequest alloc] init];
        request.content = content;
        request.trigger = trigger;
        
        if (dict[@"id"]) {
            if ([dict[@"id"] isKindOfClass:[NSString class]])
            {
                request.requestIdentifier = dict[@"id"];
                
            }else{
                NSNumber *identify = dict[@"id"];
                request.requestIdentifier = [identify stringValue];
            }
        }
        request.completionHandler = ^(id result) {
            NSLog(@"result");
        };
        
        [JPUSHService addNotification:request];
    }
    
    void _deleteLocalNotificationWithIdentifierKeyJpush(char *idKey){
        NSString *nsIdKey = CreateNSString(idKey);
        if (![nsIdKey length]) {
            return ;
        }
        NSData       *data =[nsIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendIdkey = dict[@"idKey"];
        
        [JPUSHService deleteLocalNotificationWithIdentifierKey:sendIdkey];
    }
    
    void _clearAllLocalNotificationsJpush(){
        [JPUSHService clearAllLocalNotifications];
    }
    
    
    
    
    void _removeNotificationJpush(char *idKey,bool delivered){
        JPushNotificationIdentifier *identifier = [[JPushNotificationIdentifier alloc] init];
        NSString *nsIdKey = CreateNSString(idKey);
        if (![nsIdKey length]) {
            NSLog(@"![nsIdKey length]");
            identifier.identifiers = nil;
        } else {
            NSData *data = [nsIdKey dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dict = APNativeJSONObject(data);
            NSArray *idKeyArr = dict[@"Items"];
            identifier.identifiers = idKeyArr;
        }
        identifier.delivered = delivered;
        [JPUSHService removeNotification:identifier];
    }
    
    void _removeNotificationAllJpush(){
        [JPUSHService removeNotification:nil];
    }
    
    void _findNotification(char *idKey,bool delivered){
        JPushNotificationIdentifier *identifier = [[JPushNotificationIdentifier alloc] init];
        NSString *nsIdKey = CreateNSString(idKey);
        if (![nsIdKey length]) {
            NSLog(@"![nsIdKey length]");
            identifier.identifiers = nil;
        } else {
            NSData *data = [nsIdKey dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dict = APNativeJSONObject(data);
            NSArray *idKeyArr = dict[@"Items"];
            identifier.identifiers = idKeyArr;
        }
        identifier.delivered = delivered;
        
        identifier.findCompletionHandler = ^(NSArray *results) {
            //results iOS10以下返回UILocalNotification对象数组
            //iOS10以上 根据delivered传入值返回UNNotification或UNNotificationRequest对象数组
            NSLog(@"查找指定通知 - 返回结果为：%@",results);
            //            UnitySendMessage([gameObjectName UTF8String], "OnMobileNumberOperatorResult", messageAsDictionary(dic).UTF8String);
        };
        
        [JPUSHService findNotification:identifier];
    }
    
    
    // 本地通知旧接口 - end
    
    //地理围栏 - start
    
    /**
     //    调用此 API 来设置最大的地理围栏监听个数，默认值为10
     //    参数说明
     //    count
     //    类型要求为NSInteger 类型
     //    默认值为10
     //    iOS系统要求最大不能超过20个，否则会报错。
     */
    void _setGeofenecMaxCountJpush(const int count){
        [JPUSHService setGeofenecMaxCount:integerValue(count)];
    }
    
    //
    void _removeGeofenceWithIdentifierJpush(char *geofenceId){
        NSString *nsGeofenceId = CreateNSString(geofenceId);
        [JPUSHService removeGeofenceWithIdentifier:nsGeofenceId];
    }
    
    
    //地理围栏 - end
    
    //other - start
    /**
     功能说明
     API 用于统计用户应用崩溃日志
     调用说明
     如果需要统计 Log 信息，调用该接口。当你需要自己收集错误信息时，切记不要调用该接口。
     */
    void _crashLogONJpush(){
        [JPUSHService crashLogON];
    }
    
    /**
     功能说明
     
     用于短信补充功能。设置手机号码后，可实现“推送不到短信到”的通知方式，提高推送达到率。
     参数说明
     mobileNumber 手机号码。只能以 “+” 或者数字开头，后面的内容只能包含 “-” 和数字，并且长度不能超过 20。如果传 nil 或空串则为解除号码绑定操作
     completion 响应回调。成功则 error 为空，失败则 error 带有错误码及错误信息，具体错误码详见错误码定义
     调用说明
     此接口调用频率有限制，10s 之内最多 3 次。建议在登录成功以后，再调用此接口。结果信息通过 completion 异步返回，也可将completion 设置为 nil 不处理结果信息。
     */
    void _setMobileNumberJpush(int sequence,char *mobileNumber){
        NSString *nsMobileNumber = CreateNSString(mobileNumber);
        if (![nsMobileNumber length]) {
            return;
        }
        [JPUSHService setMobileNumber:nsMobileNumber completion:^(NSError *error) {
            NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
            [dic setValue:[NSNumber numberWithUnsignedInteger:sequence] forKey:@"sequence"];
            if (!error) {
                [dic setValue:[NSNumber numberWithUnsignedInteger:0] forKey:@"code"];
                UnitySendMessage([gameObjectName UTF8String], "OnMobileNumberOperatorResult", messageAsDictionary(dic).UTF8String);
            }else{
                [dic setValue:[NSNumber numberWithUnsignedInteger:[error code]] forKey:@"code"];
            }
            UnitySendMessage([gameObjectName UTF8String], "OnMobileNumberOperatorResult", messageAsDictionary(dic).UTF8String);
        }];
    }
    
    
    void _setLatitudeJpush(double latitude, double longitude){
        [JPUSHService setLatitude:latitude longitude:longitude];
    }
    
    //other - end
    
#if defined(__cplusplus)
}
#endif

#pragma mark - Unity interface

@implementation JPushUnityManager : NSObject
@end

#pragma mark - Unity instance

@implementation JPushUnityInstnce

static JPushUnityInstnce * _sharedService = nil;

+ (JPushUnityInstnce*)sharedInstance {
    static dispatch_once_t onceAPService;
    dispatch_once(&onceAPService, ^{
        _sharedService = [[JPushUnityInstnce alloc] init];
    });
    return _sharedService;
}

- (void)networkDidRecieveMessage:(NSNotification *)notification {
    if (notification.name == kJPFNetworkDidReceiveMessageNotification && notification.userInfo){
        NSData *data = APNativeJSONData(notification.userInfo);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage([gameObjectName UTF8String], "OnReceiveMessage", jsonStr.UTF8String);
    }
}

- (void)networkDidRecievePushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginReceiveNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage([gameObjectName UTF8String], "OnReceiveNotification", jsonStr.UTF8String);
    }
}

- (void)networkOpenPushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginOpenNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage([gameObjectName UTF8String], "OnOpenNotification", jsonStr.UTF8String);
    }
}
@end
