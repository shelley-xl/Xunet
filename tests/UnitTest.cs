namespace Xunet.Tests;

public class UnitTest
{
    [Fact]
    public void SnowflakeTest()
    {
        #region ѩ��ID������
        var id = SnowflakeHelper.NextId();
        Assert.NotEqual(0, id);
        #endregion
    }

    [Fact]
    public void ReflectionTest()
    {
        #region ������չ��
        var user = new User { Name = "����" };
        var name = user.GetProperty<string>("Name");
        Assert.Equal("����", name);
        var namenew = user.SetProperty("Name", "����new");
        name = user.GetProperty<string>("Name");
        Assert.Equal("����new", name);
        #endregion
    }

    [Fact]
    public void RandomTest()
    {
        #region �������չ��
        var randomText = 8.NextString(2);
        Assert.NotNull(randomText);
        #endregion
    }

    [Fact]
    public void StringTest()
    {
        #region String��չ��
        // �����ַ���
        var input = "123456";
        Assert.True(input.IsNotNullOrEmpty());
        Assert.True(input.IsNumber());
        Assert.False(input.IsNullOrEmpty());
        Assert.False("12345678900".IsPhoneNumber());
        Assert.False("12345678900".IsEmail());
        Assert.False("123456789987654321".IsIDCard());
        #endregion
    }

    [Fact]
    public void SecurityTest()
    {
        #region �ӽ�����չ��
        // �����ַ���
        var input = "123456";

        /*
         * MD5 ����
         */
        var md5Value = input.ToMD5Encrypt();
        Assert.Equal("e10adc3949ba59abbe56e057f20f883e", md5Value);

        /*
         * SHA256 ����
         */
        var sha256Value = input.ToSHA256Encrypt();
        Assert.Equal("8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", sha256Value);

        /*
         * AES �ӽ��ܣ�ʹ��Ĭ��key��iv��
         */
        var aesValue = input.ToAESEncrypt();
        var aesText = aesValue.ToAESDecrypt();
        Assert.Equal(input, aesText);

        /*
         * AES �ӽ��ܣ��������key��iv��16����
         */
        var randomKey = 32.NextString(2);
        var randomIv = 16.NextString(2);
        var aesValueRD = input.ToAESEncrypt(randomKey, randomIv, true);
        var aesTextRD = aesValueRD.ToAESDecrypt(randomKey, randomIv, true);
        Assert.Equal(input, aesTextRD);

        /*
         * RSA �ӽ���
         */
        // ��ȡRSA��Կ��˽Կ��2048λ��
        var key = 2048.ToRSASecretKey();
        // ��Կ����
        var rsaValue = input.ToRSAEncrypt(key.PublicKey!);
        // ˽Կ����
        var rsaText = rsaValue.ToRSADecrypt(key.PrivateKey!);
        Assert.Equal(input, rsaText);

        /*
         * Ħ˹����
         */
        var morseValue = input.ToMorseEncrypt();
        var morseText = morseValue.ToMorseDecrypt();
        Assert.Equal(input, morseText);
        #endregion
    }

    [Fact]
    public void AOPTest()
    {
        #region AOP
        IUserService userService = new UserService();
        userService = TransformProxy.GetDynamicProxy(userService);
        var result = userService.Create("Processing", []);
        Assert.Equal("CheckIP=>CheckLogin=>Processing=>LoginLog=>OperateLog", string.Join("=>", result));
        #endregion
    }

    [Fact]
    public void HttpClientFactoryTest()
    {
        #region HttpClientFactory
        HttpClientFactory.AddHttpClient("baidu", x =>
        {
            x.BaseAddress = new Uri("https://www.baidu.com");
        });
        var client = HttpClientFactory.CreateClient("baidu");
        Assert.NotNull(client);
        var result = client.GetAsync("").Result;
        Assert.True(result.IsSuccessStatusCode);
        #endregion
    }
}
