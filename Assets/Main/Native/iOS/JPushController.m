#import "JPushController.h"

static JPushController * instance = nil;

@implementation JPushController

+ (JPushController *) getInstance
{
    @synchronized(self) {
        if (instance == nil) {
            instance = [[self alloc] init];
        }

        return instance;
    }
}

- (void)onSetAliasWithResCode:(int)resultCode
                         tags:(NSSet*)tags
                        alias:(NSString*)alias
{
    NSLog(@"[JPushController onSetAliasWithResCode:tags:alias:] rescode: %d, \ntags: %@, \nalias: %@\n", resultCode, tags , alias);
}

@end
