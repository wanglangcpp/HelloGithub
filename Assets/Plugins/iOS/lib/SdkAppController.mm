//
//  CCC.m
//  Unity-iPhone
//
//  Created by Antinc on 17/12/21.
//
//
//
//  SdkAppController.cpp
//  Unity-iPhone
//
//  Created by Antinc on 17/12/20.
//
//
#import "UnityAppController.h"
//#import "SDK/Payment.framework/Headers/Toast+UIView.h"
#import <CommonCrypto/CommonCryptor.h>
#import "Libraries/Main/Native/iOS/CustomAppController.h"
#import <Foundation/Foundation.h>
#import "AesTools.h"


static const NSString * GameObjectName = @"[SdkManager]";
static const NSString * BEHAVIOR = @"Behavior";


//服务器列表（静态）    渠道名:DL
static const NSString *ServerList = @"http://192.168.1.116:8888/server/DL.html";
//static const NSString *ServerList_Out = @"http://admin.waiwang.antinc.cn:8888/Backstage/game/sdk/DL/loadLogin";
static const NSString *ServerList_Out = @"http://world.waiwang.dingliangame.com:8888/Backstage/server/DL.html";
//游戏版本
static const NSString *CheckVersion = @"http://master.genesis.antinc.cn:8080/CheckVersion";
static const NSString *CheckVersion_Out = @"http://world.waiwang.dingliangame.com:8080/CheckVersion";
//资源版本
static const NSString *UpdateResource = @"http://update.genesis.antinc.cn/res/";
static const NSString *UpdateResource_Out = @"http://xkfb.oss-cn-beijing.aliyuncs.com";
//订单号创建
static const NSString *WebOrderIdURL = @"http://192.168.1.116:8888/game/sdk/DL/pay";
static const NSString *WebOrderIdURL_Out = @"http://world.waiwang.dingliangame.com:8888/Backstage/game/sdk/DL/pay";

static NSString *app_code = @"dlgamexukongfengbao";
static const NSNumber *ChannelCode = [NSNumber numberWithInt: 0];
static NSString * KEY = @"dlgame2017112223";

@interface SdkAppController : CustomAppController

-(NSString*)dictToJSONData:(NSDictionary*)infoDict;
-(NSString*)DataTOjsonString:(id)object;
-(void)initConfig:(NSString*)msg;
-(void)initSDK:(NSString*)msg;

@end

#ifdef __cplusplus
extern "C" {
#endif
    
    SdkAppController* controller = NULL;
    
    void unityToIOS(const char *msg)
    {
        NSString * str = [NSString stringWithUTF8String:msg];
        NSLog(@"SDKMangager.u-i----%@",str);
        
        NSData * data = [str dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary * dictData = [NSJSONSerialization JSONObjectWithData: data options:kNilOptions error:nil];
        NSDictionary<NSString *,NSString *> *commandDict = [NSDictionary dictionaryWithObjectsAndKeys:
                                                            @"0",@"initSDK",
                                                            @"4",@"initConfig",nil];
        NSString * msgData = [dictData objectForKey:@"Data"];
        NSString * key = [dictData objectForKey:BEHAVIOR];
        int flag = [[commandDict objectForKey: key] intValue];
        if(flag==0){
            [controller initSDK:msgData];
            return;
        }else if(flag==4){
            [controller initConfig:msgData];
            return;
        }
        
    }
    
    
#ifdef __cplusplus
}
#endif



@implementation SdkAppController

- (id)init
{
    controller = [super init];
    controller = self;
    
    return self;
}

-(NSDictionary*)jsonToDict:(NSString*) jsonStr{
    NSData *jsonData = [jsonStr dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error = nil;
    id jsonObject = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingAllowFragments error:&error];
    if (jsonObject != nil && error == nil){
        return jsonObject;
    }else{// 解析错误
        NSLog(@"dict parse error");
        return nil;
    }
}

- (NSString *)dictToJSONData:(id)theData{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:theData options:NSJSONWritingPrettyPrinted error:&error];
    if ([jsonData length] > 0 && error == nil){
        NSString * tmpStr = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        NSLog(@"dictToJSONData:%@",tmpStr);
        return tmpStr;
    }else{
        return nil;
    }
}

-(NSString*)DataTOjsonString:(id)object
{
    NSString *jsonString = nil;
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:NSJSONWritingPrettyPrinted error:&error];
    if (! jsonData) {
        NSLog(@"Got an error: %@", error);
    } else {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
    return jsonString;
}


//发送到Unity
-(void)sendToUnity:(NSString*)msg ExcuteMothod:(NSString*)methodName{
    const char* cMethodName = [methodName UTF8String];
    const char* str = [msg UTF8String];
    const char* gameObj =[GameObjectName UTF8String];
    NSLog(@"SDK.sendToUnity.i-u---%@---%@",methodName,msg);
    UnitySendMessage(gameObj, cMethodName, str);
}

-(void)initConfig:(NSString*)msg{
    NSLog(@"initConfig: %@", msg);
    NSDictionary<NSString *,NSString *> *dataDict = [NSDictionary dictionaryWithObjectsAndKeys:
    													@"1",@"HasConfig",
    													@"0",@"HasSDK",
                                                        ServerList,@"ServerListURL",
                                                        ServerList_Out,@"ServerListURLOut",
                                                        ChannelCode,@"ChannelCode",
                                                        KEY,@"AesKey", 
                                                        WebOrderIdURL,@"RequestOrderURL",
                                                        WebOrderIdURL_Out,@"RequestOrderURLOut",
                                                        UpdateResource,@"UpdateResourceURL",
                                                        UpdateResource_Out,@"UpdateResourceURLOut",
                                                        CheckVersion,@"CheckVersionURL",
                                                        CheckVersion_Out,@"CheckVersionURLOut",
                                                        app_code,@"AppCode",
                                                        nil];

    NSString * jsonData = [self dictToJSONData:dataDict];
    NSString * result = [jsonData stringByReplacingOccurrencesOfString:@"\\/" withString:@"/"];
    [self sendToUnity:result ExcuteMothod:@"CallbackInitConfig"];
}

-(void)initSDK:(NSString*)msg{
    NSLog(@"initSDK: %@", msg);
    [self sendToUnity:@"0" ExcuteMothod:@"Init"];
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(SdkAppController)

