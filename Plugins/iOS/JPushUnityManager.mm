//
//  JPushUnityAnalytics.m
//  test_certifacate
//
//  Created by qinghe on 14-4-15.
//  Copyright (c) 2014年 jpush. All rights reserved.
//

#import "JPushUnityManager.h"
#import "APService.h"
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
    void _registerNetworkDidReceiveMessage(){
    
        NSNotificationCenter *defaultCenter = [NSNotificationCenter defaultCenter];
        [defaultCenter addObserver:[JPushUnityInstnce sharedInstance]
                          selector:@selector(networkDidRecieveMessage:)
                              name:kAPNetworkDidReceiveMessageNotification
                            object:nil];
    }

    
    void _printLocalLog(char *log){
        
        NSString *nsLog=CreateNSString(log);
        NSLog(@"unity3d local log is %@",nsLog);
    }

    void _setTagsAlias(const char* tagsWithAlias){
        NSString *nsTagsWithAlias=CreateNSString(tagsWithAlias);
        if (![nsTagsWithAlias length]) {
            return;
        }
        NSData       *data =[nsTagsWithAlias dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        
        NSString     *alias     = [dict objectForKey:@"alias"];
        NSArray      *tagsArray = [dict objectForKey:@"tags"];
        NSSet        *tagsSet   = [[NSSet alloc] initWithArray:tagsArray];
        SEL sel = @selector(tagsAliasCallback:tags:alias:);
        [APService setTags:tagsSet alias:alias callbackSelector:sel target:[JPushUnityInstnce sharedInstance]];
    }
    void _setTags(const char * tags){
        NSString *nsTags=CreateNSString(tags);
        if (![nsTags length]) {
            return;
        }
        NSData       *data =[nsTags dataUsingEncoding:NSUTF8StringEncoding];
        
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray      *array=[dict objectForKey:@"tags"];
        NSSet        *set=[[NSSet alloc] initWithArray:array];
        SEL sel = @selector(tagsAliasCallback:tags:alias:);
        
        [APService setTags:set callbackSelector:sel object:[JPushUnityInstnce sharedInstance]];
    }
    void _setAlias(const char * alias){
        NSString *nsAlias=CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        NSData       *data =[nsAlias dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendAlias=[dict objectForKey:@"alias"];
        SEL sel = @selector(tagsAliasCallback:tags:alias:);
        [APService setAlias:sendAlias callbackSelector:sel object:[JPushUnityInstnce sharedInstance]];
    }
    const char * _filterValidTags(const char * tags){
        NSString     *nsTags=CreateNSString(tags);
        NSDictionary *dict=APNativeJSONObject([nsTags dataUsingEncoding:NSUTF8StringEncoding]);
        NSArray      *array=[dict objectForKey:@"tags"];
        NSSet        *set=[[NSSet alloc]initWithArray:array];
        
        NSSet *filterSet =[APService filterValidTags:set];
        NSArray *filterArray=[filterSet allObjects];
        NSDictionary *filterDict=[[NSDictionary alloc]initWithObjectsAndKeys:filterArray,@"tags", nil];
        NSData *filterData=APNativeJSONData(filterDict);
        
        NSString *filterTags=[[NSString alloc] initWithData:filterData encoding:NSUTF8StringEncoding];
        
        return MakeHeapString([filterTags UTF8String]);

    }
    const char * _openUDID(){
        
        NSString * nsUDID=[APService openUDID];
        return MakeHeapString([nsUDID UTF8String]);
        
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
    
    NSMutableDictionary *dict=[[NSMutableDictionary alloc]init];
    [dict setValue:[[NSNumber alloc] initWithInt:iResCode] forKey:@"rescode"];
    [dict setValue:alias forKey:@"alias"];
    [dict setValue:tagsArray forKey:@"tags"];
    NSData       *data=APNativeJSONData(dict);
    NSString     *jsonStr=[[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
    UnitySendMessage("JPushBinding","tagsWihtAliasCallBack",jsonStr.UTF8String);
}
- (void)networkDidRecieveMessage:(NSNotification *)notification {
    
    NSLog(@"已收到消息%@",notification);
    if (notification.name==kAPNetworkDidReceiveMessageNotification&&!notification.userInfo){
        
        NSData       *data=APNativeJSONData(notification.userInfo);
        NSString     *jsonStr=[[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage("JPushBinding","networkDidReceiveMessageCallBack",jsonStr.UTF8String);
    }
}
@end




