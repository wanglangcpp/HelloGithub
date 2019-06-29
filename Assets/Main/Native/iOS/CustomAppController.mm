#import "CustomAppController.h"
#import "Constant.h"
// #import <Libraries/JPush/iOS/lib/APService.h>
#import <UIKit/UIKit.h>

@implementation CustomAppController

- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application
{
    [super applicationDidReceiveMemoryWarning:application];
    UnitySendMessage(NATIVE_CALLBACK_GO, LOW_MEMORY_MSG, "");
}

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    BOOL ret = [super application:application didFinishLaunchingWithOptions:launchOptions];
    application.applicationIconBadgeNumber = 0;

    if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0) {
        UIUserNotificationType userNotificationTypes = (UIUserNotificationTypeAlert |
                                                        UIUserNotificationTypeBadge |
                                                        UIUserNotificationTypeSound);
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:userNotificationTypes
                                                                                 categories:nil];
        [application registerUserNotificationSettings:settings];
        [application registerForRemoteNotifications];
        //[APService registerForRemoteNotificationTypes:(UIUserNotificationTypeBadge |
        //                                               UIUserNotificationTypeSound |
        //                                               UIUserNotificationTypeAlert)
        //                                   categories:nil];
    } else {
        [application registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
                                                         UIRemoteNotificationTypeAlert |
                                                         UIRemoteNotificationTypeSound)];
        //[APService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
        //                                               UIRemoteNotificationTypeSound |
        //                                               UIRemoteNotificationTypeAlert)
        //                                   categories:nil];
    }

    //[APService setupWithOption:launchOptions];

    cachedViewsForRetain = [[NSMutableSet alloc] init];
    return ret;
}

- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
{
    [super application:application didReceiveRemoteNotification:userInfo];
    // [APService handleRemoteNotification:userInfo];
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler
{
    // IOS 7 Support Required
    // [APService handleRemoteNotification:userInfo];
    completionHandler(UIBackgroundFetchResultNewData);
}

- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
    [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    // [APService registerDeviceToken:deviceToken];
}

- (void)registerObjectForRetain:(id)obj
{
    [cachedViewsForRetain addObject:obj];
}

- (void)unregisterObjectForRetain:(id)obj
{
    [cachedViewsForRetain removeObject:obj];
}

@end

// IMPL_APP_CONTROLLER_SUBCLASS(CustomAppController)
