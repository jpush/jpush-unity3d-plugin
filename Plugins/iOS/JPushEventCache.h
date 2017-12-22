//
//  JPushEventCache.h
//  Unity-iPhone
//
//  Created by oshumini on 2017/12/21.
//

#import <Foundation/Foundation.h>

@interface JPushEventCache : NSObject
+ (JPushEventCache *)sharedInstance;

- (void)sendEvent:(NSDictionary *)notification withKey:(NSString *)key;
- (void)scheduleNotificationQueue;
@end

