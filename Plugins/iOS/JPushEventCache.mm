//
//  JPushEventCache.m
//  Unity-iPhone
//
//  Created by oshumini on 2017/12/21.
//

#import "JPushEventCache.h"

@interface JPushEventCache()
@property(strong, nonatomic) NSMutableDictionary* eventCache;

@end

@implementation JPushEventCache

+ (JPushEventCache *)sharedInstance {
  static JPushEventCache* sharedInstance = nil;
  static dispatch_once_t onceAPService;
  dispatch_once(&onceAPService, ^{
  
    sharedInstance = [self new];
  });
  
  return sharedInstance;
}

- (instancetype)init {
  self = [super init];
  if (self) {
    _eventCache = @{}.mutableCopy;
  }
  
  return self;
}

- (void)sendEvent:(NSDictionary *)notification withKey:(NSString *)key {
  if (_eventCache[key]) {
    NSMutableArray *arr = _eventCache[key];
    [arr addObject:notification];
  } else {
    _eventCache[key] = @[].mutableCopy;
    NSMutableArray *arr = _eventCache[key];
    [arr addObject:notification];
  }
}

- (void)scheduleNotificationQueue {
  for (NSString *key in _eventCache) {
    for (NSDictionary *notification in _eventCache[key]) {
      [[NSNotificationCenter defaultCenter] postNotificationName:key object:notification];
    }
  }
  [_eventCache removeAllObjects];
}


@end

