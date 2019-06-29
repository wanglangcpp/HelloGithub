//
//  LocalNotificationController.h
//  Unity-iPhone
//
//  Created by Antinc on 11/5/15.
//
//

#import <Foundation/Foundation.h>

@interface LocalNotificationController : NSObject
+ (void)registerLocalNotification:(NSString *)dateStr
                        message:(NSString *)alertBody
                            key:(NSString *)key;

+ (void)clearLocalNotification;
@end

