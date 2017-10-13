#import "JPushUnityManager.h"
#import "JPUSHService.h"

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


@interface JPushUnityInstnce : NSObject {
@private
}
+(JPushUnityInstnce*)instance;
@end


#if defined(__cplusplus)
extern "C" {
#endif
    
    // private - start
    const char *gameObject_;
    
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
            [dic setObject:[iTags allObjects] forKey:@"tags"];
        }
        
        UnitySendMessage(gameObject_, tagCallbackName_, messageAsDictionary(dic).UTF8String);
    };
    
    JPUSHAliasOperationCompletion aliasOperationCompletion = ^(NSInteger iResCode, NSString *iAlias, NSInteger seq) {
        NSMutableDictionary* dic = [[NSMutableDictionary alloc] init];
        [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
        [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
        
        if (iResCode == 0) {
            [dic setObject:iAlias forKey:@"alias"];
        }
        
        UnitySendMessage(gameObject_, aliasCallbackName_, messageAsDictionary(dic).UTF8String);
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

    void _init(char *gameObject) {
        gameObject_ = gameObject;
        
        NSNotificationCenter *msgCenter = [NSNotificationCenter defaultCenter];
        [msgCenter addObserver:[JPushUnityInstnce instance]
                          selector:@selector(networkDidRecieveMessage:)
                              name:kJPFNetworkDidReceiveMessageNotification
                            object:nil];
        
        NSNotificationCenter *notiCenter = [NSNotificationCenter defaultCenter];
        [notiCenter addObserver:[JPushUnityInstnce instance]
                          selector:@selector(networkDidRecievePushNotification:)
                              name:@"JPushPluginReceiveNotification"
                            object:nil];
    }
    
    void _setDebug(bool enable) {
        if (enable) {
            [JPUSHService setDebugMode];
        } else {
            [JPUSHService setLogOFF];
        }
    }
    
    const char *_getRegistrationId() {
        NSString *registrationID = [JPUSHService registrationID];
        return MakeHeapString([registrationID UTF8String]);
    }

    // Tag & Alias - start
    
    void _setTags(int sequence, const char *tags) {
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
    
    void _addTags(int sequence, char *tags) {
        NSString* tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService addTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteTags(int sequence, char *tags) {
        NSString *tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService deleteTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _cleanTags(int sequence) {
        [JPUSHService cleanTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAllTags(int sequence) {
        [JPUSHService getAllTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }

    void _checkTagBindState(int sequence, char *tag) {
        NSString *nsTag = CreateNSString(tag);
        [JPUSHService validTag:nsTag completion:^(NSInteger iResCode, NSSet *iTags, NSInteger seq, BOOL isBind) {
            NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
            [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
            [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
            
            if (iResCode == 0) {
                [dic setObject:[iTags allObjects] forKey:@"tags"];
                [dic setObject:[NSNumber numberWithBool:isBind] forKey:@"isBind"];
            }
            
            UnitySendMessage(gameObject_, tagCallbackName_, messageAsDictionary(dic).UTF8String);
        } seq:(NSInteger)sequence];
    }

    void _setAlias(int sequence, const char * alias){
        NSString *nsAlias = CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        
        [JPUSHService setAlias:nsAlias completion:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAlias(int sequence) {
        [JPUSHService getAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteAlias(int sequence) {
        [JPUSHService deleteAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    // Tag & Alias - end

    // 角标处理 - start
    
    void _setBadge(const int badge){
        [JPUSHService setBadge:integerValue(badge)];
    }

    void _resetBadge(){
        [JPUSHService resetBadge];
    }

    void _setApplicationIconBadgeNumber(const int badge){
        [UIApplication sharedApplication].applicationIconBadgeNumber = integerValue(badge);
    }

    int _getApplicationIconBadgeNumber(){
        return intValue([UIApplication sharedApplication].applicationIconBadgeNumber);
    }
    
    // 角标处理 - end

    // 页面统计 - start
    
    void _startLogPageView(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService startLogPageView:sendPageName];
    }

    void _stopLogPageView(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService stopLogPageView:sendPageName];
    }

    void _beginLogPageView(const char *pageName, const int duration) {
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
    
    // 本地通知 - start
    
    void _addNotification(char * notificationJsonStr) {
        NSString *nsNotification = CreateNSString(notificationJsonStr);
        NSData *data =[nsNotification dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        
        NSDictionary *notiContent = dict[@"content"];
        NSDictionary *notiTrigger = dict[@"trigger"];
        
        JPushNotificationContent *content = [[JPushNotificationContent alloc] init];
        JPushNotificationTrigger *trigger = [[JPushNotificationTrigger alloc] init];
        
        // 设置通知内容 - start
        if (notiContent[@"title"]) {
            content.title = notiContent[@"title"];
        }
        
        if (notiContent[@"subtitle"]) {
            content.subtitle = notiContent[@"subtitle"];
        }
        
        if (notiContent[@"body"]) {
            content.body = notiContent[@"body"];
        }
        
        if (notiContent[@"badge"]) {
            content.badge = notiContent[@"badge"];
        }
        
        if (notiContent[@"sound"]) {
            content.sound = notiContent[@"sound"];
        }
        
        if (notiContent[@"categoryIdentifier"]) {
            content.categoryIdentifier = notiContent[@"categoryIdentifier"];
        }
        // 设置通知内容 - end
        
        // 设置触发时间 - start
        if (notiTrigger[@"timeInterval"]) {
            // TODO
//            trigger.timeInterval = [notiTrigger ];
        }
        
        if (notiTrigger[@"repeat"]) {
//            trigger.repeat =
        }
        // 设置触发时间 - end
    }
    
    // 本地通知 - end

    
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
        UnitySendMessage(gameObject_, "networkDidReceiveMessageCallBack",jsonStr.UTF8String);
    }
}

- (void)networkDidRecievePushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginReceiveNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage(gameObject_, "networkDidReceivePushNotificationCallBack",jsonStr.UTF8String);
    }
}
@end