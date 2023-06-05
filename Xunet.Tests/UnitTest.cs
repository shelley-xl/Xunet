namespace Xunet.Tests;

public class UnitTest
{
    [Fact]
    public void Test()
    {
        #region 加解密扩展类
        // 输入字符串
        var input = "123456";

        /*
         * MD5 加密
         */
        var md5Value = input.ToMD5Encrypt();
        Assert.Equal("e10adc3949ba59abbe56e057f20f883e", md5Value);

        /*
         * SHA256 加密
         */
        var sha256Value = input.ToSHA256Encrypt();
        Assert.Equal("8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", sha256Value);

        /*
         * AES 加解密（使用默认key和iv）
         */
        var aesValue = input.ToAESEncrypt();
        var aesText = aesValue.ToAESDecrypt();
        Assert.Equal(input, aesText);

        /*
         * RSA 加解密
         */
        // 获取RSA公钥和私钥（2048位）
        var key = 2048.ToRSASecretKey();
        // 公钥加密
        var rsaValue = input.ToRSAEncrypt(key.PublicKey!);
        // 私钥解密
        var rsaText = rsaValue.ToRSADecrypt(key.PrivateKey!);
        Assert.Equal(input, rsaText);
        #endregion

        #region String扩展类
        Assert.True(input.IsNotNullOrEmpty());
        Assert.True(input.IsNumber());
        Assert.False(input.IsNullOrEmpty());
        Assert.False("12345678900".IsPhoneNumber());
        Assert.False("12345678900".IsEmail());
        Assert.False("123456789987654321".IsIDCard());
        #endregion

        #region 随机数扩展类
        var randomText = 8.NextString(2);
        Assert.NotNull(randomText);
        #endregion
    }
}