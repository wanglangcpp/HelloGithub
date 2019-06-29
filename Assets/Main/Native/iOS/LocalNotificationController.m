//
//  LocalNotificationController.m
//  Unity-iPhone
//
//  Created by Antinc on 11/5/15.
//
//

#import "LocalNotificationController.h"

@implementation LocalNotificationController

+ (void)registerLocalNotification:(NSString *)dateStr
                       message :(NSString *)alertBody
                            key:(NSString *)key
{
    NSDateFormatter* formater = [[NSDateFormatter alloc] init];
    [formater setDateFormat:@"HH:mm:ss"];
    NSDate* date = [formater dateFromString:dateStr];
    UILocalNotification *noti = [[UILocalNotification alloc] init];
    if(noti)
    {
        noti.fireDate = date;
        noti.timeZone = [NSTimeZone defaultTimeZone];
        noti.repeatInterval = NSDayCalendarUnit;
        noti.alertBody = alertBody;
        noti.applicationIconBadgeNumber = 1;
        NSDictionary *infoDic = [NSDictionary dictionaryWithObject:@"test" forKey:key];
        noti.userInfo = infoDic;
        UIApplication *app = [UIApplication sharedApplication];
        [app scheduleLocalNotification:noti];
    }
}

+ (void)clearLocalNotification
{
    UIApplication *app = [UIApplication sharedApplication];
    NSArray *localArr = [app scheduledLocalNotifications];
    if(localArr){
        for(UILocalNotification *noti in localArr){
            [app cancelLocalNotification:noti];
        }
    }
}

@end

