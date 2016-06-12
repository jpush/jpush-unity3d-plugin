//
//  JPushUnityAnalytics.m
//  test_certifacate
//
//  Created by qinghe on 14-4-15.
//  Copyright (c) 2014年 jpush. All rights reserved.
//

#import "JPushUnityManager.h"
#import "JPUSHService.h"
//region A begin
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
//region A end;

@interface JPushUnityInstnce : NSObject{
@private
}
+(JPushUnityInstnce*)sharedInstance;
@end


//region B bigun;

#if defined(__cplusplus)
extern "C" {
#endif
    static char* MakeHeapString(const char* string) {
        if (!string){
            return NULL;
        }
        char* mem = static_cast<char*>(malloc(strlen(string) + 1));
        if (mem) {
            strcpy(mem, string);
        }
        return mem;
    }
    NSString* CreateNSString (const char* string) {
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

    void _printLocalLog(char *log){

        NSString *nsLog = CreateNSString(log);
        NSLog(@"JPush Unity3d Plugin Log >>>>> %@",nsLog);
    }

    //---------------------------- tags / alias ----------------------------//

    void _setTagsAlias(const char* tagsWithAlias){
        NSString *nsTagsWithAlias = CreateNSString(tagsWithAlias);
        if (![nsTagsWithAlias length]) {
            return;
        }
        NSData       *data = [nsTagsWithAlias dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);

        NSString     *alias     = dict[@"alias"];
        NSArray      *tagsArray = dict[@"tags"];
        NSSet        *tagsSet   = [[NSSet alloc] initWithArray:tagsArray];

        [JPUSHService setTags:tagsSet alias:alias callbackSelector:@selector(tagsAliasCallback:tags:alias:) object:[JPushUnityInstnce sharedInstance]];
    }

    void _setTags(const char * tags){
        NSString *nsTags = CreateNSString(tags);
        if (![nsTags length]) {
            return;
        }
        NSData       *data  = [nsTags dataUsingEncoding:NSUTF8StringEncoding];

        NSDictionary *dict  = APNativeJSONObject(data);
        NSArray      *array = dict[@"tags"];
        NSSet        *set   = [[NSSet alloc] initWithArray:array];

        [JPUSHService setTags:set callbackSelector:@selector(tagsAliasCallback:tags:alias:) object:[JPushUnityInstnce sharedInstance]];
    }

    void _setAlias(const char * alias){
        NSString *nsAlias = CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        NSData       *data =[nsAlias dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendAlias = [dict objectForKey:@"alias"];
        [JPUSHService setAlias:sendAlias callbackSelector:@selector(tagsAliasCallback:tags:alias:) object:[JPushUnityInstnce sharedInstance]];
    }

    const char * _filterValidTags(const char * tags){
        NSString     *nsTags=CreateNSString(tags);
        NSDictionary *dict=APNativeJSONObject([nsTags dataUsingEncoding:NSUTF8StringEncoding]);
        NSArray      *array=[dict objectForKey:@"tags"];
        NSSet        *set=[[NSSet alloc]initWithArray:array];

        NSSet *filterSet =[JPUSHService filterValidTags:set];
        NSArray *filterArray=[filterSet allObjects];
        NSDictionary *filterDict=[[NSDictionary alloc]initWithObjectsAndKeys:filterArray,@"tags", nil];
        NSData *filterData=APNativeJSONData(filterDict);

        NSString *filterTags=[[NSString alloc] initWithData:filterData encoding:NSUTF8StringEncoding];

        return MakeHeapString([filterTags UTF8String]);

    }

    //---------------------------- private ----------------------------//

    NSInteger _integerValue(int intValue){
        NSNumber *n = [NSNumber numberWithInt:intValue];
        return [n integerValue];
    }

    int _intValue(NSInteger integerValue){
        NSNumber *n = [NSNumber numberWithInteger:integerValue];
        return [n intValue];
    }

    //---------------------------- registrationID ----------------------------//

    const char * _registrationID(){

        NSString * registrationID = [JPUSHService registrationID];
        return MakeHeapString([registrationID UTF8String]);

    }

    //---------------------------- notification / message ----------------------------//

    void _registerNetworkDidReceiveMessage(){
        NSNotificationCenter *defaultCenter = [NSNotificationCenter defaultCenter];
        [defaultCenter addObserver:[JPushUnityInstnce sharedInstance]
                          selector:@selector(networkDidRecieveMessage:)
                              name:kJPFNetworkDidReceiveMessageNotification
                            object:nil];
    }


    //---------------------------- badge ----------------------------//

    void _setBadge(const int badge){
        [JPUSHService setBadge:_integerValue(badge)];
    }

    void _resetBadge(){
        [JPUSHService resetBadge];
    }

    void _setApplicationIconBadgeNumber(const int badge){
        [UIApplication sharedApplication].applicationIconBadgeNumber = _integerValue(badge);
    }

    int _getApplicationIconBadgeNumber(){
        return _intValue([UIApplication sharedApplication].applicationIconBadgeNumber);
    }

    //---------------------------- 页面统计 ----------------------------//

    void _startLogPageView(const char *pageName){

        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return ;
        }
        NSData       *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendPageName = dict[@"pageName"];

        [JPUSHService startLogPageView:sendPageName];
    }

    void _stopLogPageView(const char *pageName){

        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return ;
        }
        NSData       *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendPageName = dict[@"pageName"];

        [JPUSHService stopLogPageView:sendPageName];
    }

    void _beginLogPageView(const char *pageName, const int duration){
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return ;
        }
        NSData       *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendPageName = dict[@"pageName"];
        [JPUSHService beginLogPageView:sendPageName duration:duration];
    }

    //---------------------------- 开关日志 ----------------------------//

    void _setDebugMode(){
        [JPUSHService setDebugMode];
    }

    void _setLogOFF(){
        [JPUSHService setLogOFF];
    }

    void _crashLogON(){
        [JPUSHService crashLogON];
    }

    //---------------------------- 本地推送 ----------------------------//

    void _setLocalNotification(int delay, int badge, char *alertBodyAndIdKey){
        NSDate *date = [NSDate dateWithTimeIntervalSinceNow:_integerValue(delay)];

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

    void _deleteLocalNotificationWithIdentifierKey(char *idKey){

        NSString *nsIdKey = CreateNSString(idKey);
        if (![nsIdKey length]) {
            return ;
        }
        NSData       *data =[nsIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendIdkey = dict[@"idKey"];

        [JPUSHService deleteLocalNotificationWithIdentifierKey:sendIdkey];
    }

    void _clearAllLocalNotifications(){
        [JPUSHService clearAllLocalNotifications];
    }

    //---------------------------- 地理位置上报 ----------------------------//

    void _setLocation(char *latitudeAndlongitude){

        NSString *nsLatitudeAndlongitude = CreateNSString(latitudeAndlongitude);
        if (![nsLatitudeAndlongitude length]) {
            return ;
        }
        NSData       *data =[nsLatitudeAndlongitude dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendLatitude = dict[@"latitude"];
        NSString     *sendLongitude = dict[@"longitude"];

        [JPUSHService setLatitude:[sendLatitude doubleValue] longitude:[sendLongitude doubleValue]];
    }




#if defined(__cplusplus)
}
#endif
//region B end
#pragma mark -
#pragma mark - Unity interface

@implementation JPushUnityManager : NSObject
@end
//region C end;
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

- (void)tagsAliasCallback:(int)iResCode tags:(NSSet*)tags alias:(NSString*)alias {

    NSLog(@"rescode: %d, \ntags: %@, \nalias: %@\n", iResCode, tags , alias);
    NSArray *tagsArray=[tags allObjects];

    NSMutableDictionary *dict = [[NSMutableDictionary alloc]init];
    [dict setValue:[[NSNumber alloc] initWithInt:iResCode] forKey:@"rescode"];
    [dict setValue:alias forKey:@"alias"];
    [dict setValue:tagsArray forKey:@"tags"];
    NSData       *data    = APNativeJSONData(dict);
    NSString     *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
    UnitySendMessage("JPushBinding","tagsWihtAliasCallBack",jsonStr.UTF8String);
}

- (void)networkDidRecieveMessage:(NSNotification *)notification {

    NSLog(@"已收到消息%@",notification);
    if (notification.name==kJPFNetworkDidReceiveMessageNotification&&!notification.userInfo){
        
        NSData       *data=APNativeJSONData(notification.userInfo);
        NSString     *jsonStr=[[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage("JPushBinding","networkDidReceiveMessageCallBack",jsonStr.UTF8String);
    }
}

@end


