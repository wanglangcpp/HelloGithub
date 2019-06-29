#import "APService.h"
#import "JPushController.h"

extern "C"
{
    void JPush_SetAlias(const char * alias)
    {
        JPushController * jPushCtl = [JPushController getInstance];
        [APService setAlias:[NSString stringWithUTF8String:alias] callbackSelector:@selector(onSetAliasWithResCode:tags:alias:) object:jPushCtl];
    }
}
