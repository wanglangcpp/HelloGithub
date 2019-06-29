#import <Foundation/Foundation.h>
#import "Utility.h"

char * CreateNewCStringWithNSString(NSString * raw)
{
    char * ret;

    if (raw == nil)
    {
        ret = (char *)calloc(sizeof(char), 1);
        ret[0] = '\0';
    }

    const char * cstr = [raw UTF8String];
    ret = (char *)calloc(sizeof(char), strlen(cstr));
    strcpy(ret, cstr);
    return ret;
}
