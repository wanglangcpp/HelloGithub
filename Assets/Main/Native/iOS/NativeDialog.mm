#import <Foundation/Foundation.h>
#import "Constant.h"
#import "Utility.h"
#import "CustomAlertViewDelegate.h"
#import "CustomAppController.h"

extern "C"
{
    void NativeDialog_Open(int mode, const char *title, const char *message, BOOL pauseGame,
                           const char *confirmText, const char *cancelText, const char *otherText,
                           int confirmCallbackId, int cancelCallbackId, int otherCallbackId,
                           const char *userData)
    {
        if (mode < 1 || mode > 3)
        {
            mode = 1;
        }

        CustomAlertViewDelegate *delegate = [[CustomAlertViewDelegate alloc] init];
        delegate.pauseGame = pauseGame;
        delegate.confirmCallbackId = confirmCallbackId;
        delegate.cancelCallbackId = cancelCallbackId;
        delegate.otherCallbackId = otherCallbackId;

        UIAlertView* alert = [[UIAlertView alloc] initWithTitle:[NSString stringWithUTF8String:title]
                                                        message:[NSString stringWithUTF8String:message]
                                                       delegate:delegate
                                              cancelButtonTitle:nil
                                              otherButtonTitles:nil];
        delegate.alert = alert;

        [alert addButtonWithTitle:[NSString stringWithUTF8String:confirmText]];

        if (mode > 1)
        {
            [alert addButtonWithTitle:[NSString stringWithUTF8String:cancelText]];
        }

        if (mode > 2)
        {
            [alert addButtonWithTitle:[NSString stringWithUTF8String:otherText]];
        }

        CustomAppController *appController = [[UIApplication sharedApplication] delegate];
        [appController registerObjectForRetain:delegate];
        [alert show];
    }
}
