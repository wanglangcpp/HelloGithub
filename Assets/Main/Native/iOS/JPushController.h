#import <Foundation/Foundation.h>

@interface JPushController : NSObject
{
}

+ (JPushController *)getInstance;

- (void)onSetAliasWithResCode:(int)iResCode
                         tags:(NSSet*)tags
                        alias:(NSString*)alias;

@end
