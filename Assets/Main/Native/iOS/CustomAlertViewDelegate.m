#import "CustomAlertViewDelegate.h"
#import "Constant.h"
#import "Utility.h"
#import "CustomAppController.h"

@implementation CustomAlertViewDelegate

#pragma marks UIAlertViewDelegate

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    if (self.pauseGame)
    {
        UnitySendMessage(NATIVE_CALLBACK_GO, TRY_TO_RESUME_GAME, "");
    }

    int callbackId = (buttonIndex == 0 ? self.confirmCallbackId : buttonIndex == 1 ? self.cancelCallbackId : self.otherCallbackId);
    UnitySendMessage(NATIVE_CALLBACK_GO, NATIVE_DIALOG_CLOSE_MSG, CreateNewCStringWithNSString([NSString stringWithFormat:@"%i", callbackId]));
    CustomAppController *appController = [[UIApplication sharedApplication] delegate];
    alertView = nil;
    [appController unregisterObjectForRetain:self];
}

@end
