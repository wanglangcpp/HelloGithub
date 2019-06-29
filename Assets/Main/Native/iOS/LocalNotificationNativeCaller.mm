#import "LocalNotificationController.h"

extern "C"
{
    void LocalNotification_Register(const char *dateStr, const char *alertBody, const char *key)
    {
        [LocalNotificationController registerLocalNotification:[NSString stringWithUTF8String:dateStr] message:[NSString stringWithUTF8String:alertBody] key:[NSString stringWithUTF8String:key]];
    }
    
    void LocalNotification_Cancel()
    {
        [LocalNotificationController clearLocalNotification];
    }
}
