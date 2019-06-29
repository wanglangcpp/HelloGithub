#import "UnityAppController.h"

@interface CustomAppController : UnityAppController
{
    NSMutableSet *cachedViewsForRetain;
}

- (void)registerObjectForRetain:(id)obj;
- (void)unregisterObjectForRetain:(id)obj;

@end
