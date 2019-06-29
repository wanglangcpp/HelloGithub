//
//  AesTools.h
//  Unity-iPhone
//
//  Created by Antinc on 17/12/25.
//
//

#import <Foundation/Foundation.h>
#import <CommonCrypto/CommonCryptor.h>

@interface AESTools : NSObject
+(NSString *)AESDecrypt:(NSString *)key Decrypttext:(NSString *)text;
+(NSString *)AESEncrypt:(NSString *)key Encrypttext:(NSString *)text;
+(NSString*)MD5:(NSString*)text;

@end
