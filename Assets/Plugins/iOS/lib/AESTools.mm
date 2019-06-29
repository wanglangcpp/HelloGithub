//
//  AESTools.m
//  Unity-iPhone
//
//  Created by Antinc on 17/12/22.
//
//

#import <Foundation/Foundation.h>
#import <CommonCrypto/CommonCryptor.h>
#import <CommonCrypto/CommonDigest.h>

@implementation AESTools

//size_t const kKeySize = kCCKeySizeAES128;

+ (NSString *) MD5:(NSString *) text
{
    const char *cStr = [text UTF8String];
    unsigned char digest[16];
    unsigned int x=(int)strlen(cStr) ;
    CC_MD5( cStr, x, digest ); // This is the md5 call
    NSMutableString *output = [NSMutableString stringWithCapacity:CC_MD5_DIGEST_LENGTH * 2];
    for(int i = 0; i < CC_MD5_DIGEST_LENGTH; i++)
        [output appendFormat:@"%02x", digest[i]];
    return  output;
}

+ (NSString *)hexStringFromString:(NSData *)myD{
    Byte *bytes = (Byte *)[myD bytes];
    //下面是Byte 转换为16进制。
    NSString *hexStr=@"";
    for(int i=0;i<[myD length];i++)
    {
        NSString *newHexStr = [NSString stringWithFormat:@"%x",bytes[i]&0xff];///16进制数
        if([newHexStr length]==1)
            hexStr = [NSString stringWithFormat:@"%@0%@",hexStr,newHexStr];
        else
            hexStr = [NSString stringWithFormat:@"%@%@",hexStr,newHexStr];
    }
    return hexStr;
}

+ (NSData *)dataFromHexString:(NSString*)text {
    const char *chars = [text UTF8String];
    int i = 0;
    NSUInteger len =text.length;
    NSMutableData *data = [NSMutableData dataWithCapacity:len / 2];
    char byteChars[3] = {'\0','\0','\0'};
    unsigned long wholeByte;
    
    while (i < len) {
        byteChars[0] = chars[i++];
        byteChars[1] = chars[i++];
        wholeByte = strtoul(byteChars, NULL, 16);
        [data appendBytes:&wholeByte length:1];
    }
    return data;
}


//加密
+(NSString *)AESEncrypt:(NSString *)key Encrypttext:(NSString *)text
{
    NSData *contentData = [text dataUsingEncoding:NSUTF8StringEncoding];
    NSUInteger dataLength = contentData.length;
    char keyPtr[kCCKeySizeAES128+1];
    
    memset(keyPtr,0,sizeof(keyPtr));
    [key getCString:keyPtr maxLength:sizeof(keyPtr) encoding:NSUTF8StringEncoding];
    
    // 密文长度 <= 明文长度 + BlockSize
    size_t encryptSize = dataLength + kCCBlockSizeAES128;
    void *encryptedBytes = malloc(encryptSize);
    size_t actualOutSize = 0;
    //NSData *initVector = [@"" dataUsingEncoding:NSUTF8StringEncoding];
    
    CCCryptorStatus cryptStatus = CCCrypt(kCCEncrypt,
                                          kCCAlgorithmAES,
                                          kCCOptionECBMode|kCCOptionPKCS7Padding,
                                          keyPtr,
                                          kCCKeySizeAES128,
                                          nil,
                                          contentData.bytes,
                                          dataLength,
                                          encryptedBytes,
                                          encryptSize,
                                          &actualOutSize);
    if (cryptStatus == kCCSuccess) {
        NSData *dd = [NSData dataWithBytesNoCopy:encryptedBytes length:actualOutSize];
        NSString *result = [self hexStringFromString:dd];
        return [result uppercaseString];
    }
    free(encryptedBytes);
    return nil;
}
//解密
+(NSString *)AESDecrypt:(NSString *)key Decrypttext:(NSString *)text
{
    NSLog(@"SDK AESTools before result:%lu",(unsigned long)text.length);
    NSString * str = [text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]]; //去除掉首尾的空白字符和换行字符
    str = [str stringByReplacingOccurrencesOfString:@"\r" withString:@""];
    str = [str stringByReplacingOccurrencesOfString:@"\n" withString:@""];
    NSLog(@"SDK AESTools before deal result:%lu",(unsigned long)str.length);
    
    NSString * lowerData =[str lowercaseString];
    NSData *contentData = [self dataFromHexString:lowerData];
    NSUInteger dataLength = contentData.length;
    char keyPtr[kCCKeySizeAES128+1];
    
    memset(keyPtr,0,sizeof(keyPtr));
    [key getCString:keyPtr maxLength:sizeof(keyPtr) encoding:NSUTF8StringEncoding];
    
    // 密文长度 <= 明文长度 + BlockSize
    size_t encryptSize = dataLength + kCCBlockSizeAES128;
    void *encryptedBytes = malloc(encryptSize);
    size_t actualOutSize = 0;
    
    CCCryptorStatus cryptStatus = CCCrypt(kCCDecrypt,
                                          kCCAlgorithmAES,
                                          kCCOptionECBMode|kCCOptionPKCS7Padding,
                                          keyPtr,
                                          kCCKeySizeAES128,
                                          nil,
                                          contentData.bytes,
                                          dataLength,
                                          encryptedBytes,
                                          encryptSize,
                                          &actualOutSize);
    if (cryptStatus == kCCSuccess) {
        NSData *dd = [NSData dataWithBytesNoCopy:encryptedBytes length:actualOutSize];
        NSString *result = [[NSString alloc] initWithData:dd encoding:NSUTF8StringEncoding];
        NSLog(@"SDK AESTools result:%lu",(unsigned long)result.length);
        return result;
    }
    free(encryptedBytes);
    return nil;
}


@end
