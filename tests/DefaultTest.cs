using Xunet.Helpers;
using Xunet.Extensions;

namespace Xunet.Tests;

public class DefaultTest
{
    [Fact]
    public void Test()
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

        #region String��չ��
        Assert.True(input.IsNotNullOrEmpty());
        Assert.True(input.IsNumber());
        Assert.False(input.IsNullOrEmpty());
        Assert.False("12345678900".IsPhoneNumber());
        Assert.False("12345678900".IsEmail());
        Assert.False("123456789987654321".IsIDCard());
        #endregion

        #region �������չ��
        var randomText = 8.NextString(2);
        Assert.NotNull(randomText);
        #endregion

        #region ������չ��
        var user = new User { Name = "����" };
        var name = user.GetProperty<string>("Name");
        Assert.Equal("����", name);
        var namenew = user.SetProperty("Name", "����new");
        name = user.GetProperty<string>("Name");
        Assert.Equal("����new", name);
        #endregion

        #region ѩ��ID������
        var id = SnowflakeHelper.NextId();
        Assert.NotEqual(0, id);
        #endregion

        #region ���ܼ�ʱ��������
        var milliSeconds = StopwatchHelper.Execute(() =>
        {
            // do something
            Thread.Sleep(100);
        });
        Assert.True(milliSeconds >= 100);
        #endregion
    }
}
