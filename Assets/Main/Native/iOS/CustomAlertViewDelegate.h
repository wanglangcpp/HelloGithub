#import <Foundation/Foundation.h>

@interface CustomAlertViewDelegate : NSObject <UIAlertViewDelegate>
{

}

@property (nonatomic, assign) BOOL pauseGame;
@property (nonatomic, assign) int confirmCallbackId;
@property (nonatomic, assign) int cancelCallbackId;
@property (nonatomic, assign) int otherCallbackId;
@property (nonatomic, retain) UIAlertView *alert;

@end
