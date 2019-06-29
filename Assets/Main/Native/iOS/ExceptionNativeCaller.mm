#import <Foundation/Foundation.h>

extern "C"
{
    void ExceptionNativeCaller_Trigger()
    {
        NSException *exception = [NSException exceptionWithName:@"Native Exception Name" reason:@"Native exception reason." userInfo:nil];
        @throw exception;
    }
}
