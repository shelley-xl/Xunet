// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Extensions;

using System;
using System.Security.Cryptography;
using System.Text;

#region 加解密扩展类
/// <summary>
/// 加解密扩展类
/// </summary>
public static class SecurityExtension
{
    #region 获取MD5值
    /// <summary>
    /// 获取MD5值
    /// </summary>
    /// <param name="input">待加密字符串</param>
    /// <param name="toUpper">是否转换成大写</param>
    /// <returns>加密后的字符串</returns>
    public static string ToMD5Encrypt(this string input, bool toUpper = false)
    {
        var buffer = Encoding.UTF8.GetBytes(input);
#if NET45_OR_GREATER
        using var md5 = MD5.Create();
        var MD5buffer = md5.ComputeHash(buffer);
#endif
#if NET6_0_OR_GREATER
        var MD5buffer = MD5.HashData(buffer);
#endif
        var builder = new StringBuilder();
        foreach (var item in MD5buffer)
        {
            if (toUpper) builder.Append(item.ToString("X2"));
            else builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }
    #endregion

    #region 获取SHA256值
    /// <summary>
    /// 获取SHA256值
    /// </summary>
    /// <param name="input">待加密字符串</param>
    /// <param name="toUpper">是否转换成大写</param>
    /// <returns>加密后的字符串</returns>
    public static string ToSHA256Encrypt(this string input, bool toUpper = false)
    {
        var buffer = Encoding.UTF8.GetBytes(input);
#if NET45_OR_GREATER
        using var sha256 = SHA256.Create();
        var SHA256buffer = sha256.ComputeHash(buffer);
#endif
#if NET6_0_OR_GREATER
        var SHA256buffer = SHA256.HashData(buffer);
#endif
        var builder = new StringBuilder();
        foreach (var item in SHA256buffer)
        {
            if (toUpper) builder.Append(item.ToString("X2"));
            else builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }
    #endregion

    #region 获取AES密文
    /// <summary>
    /// 获取AES密文
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="key">秘钥</param>
    /// <param name="iv">偏移量</param>
    /// <param name="isHex">是否返回16进制</param>
    /// <returns>密文</returns>
    public static string ToAESEncrypt(this string plainText, string key = null, string iv = null, bool isHex = false)
    {
        if (key.IsNullOrEmpty()) key = @")O[NB]6,YF}+efcaj{+oESbld8>Z'e9M";
        if (iv.IsNullOrEmpty()) iv = @"L+\~f4,Ir)b$=pkf";
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(plainText);
        var encryptKey = Encoding.UTF8.GetBytes(key!);
        byte[] ivArray = Encoding.UTF8.GetBytes(iv!);
        using var aesAlg = Aes.Create();
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.KeySize = 128;
        aesAlg.Key = encryptKey;
        aesAlg.Padding = PaddingMode.PKCS7;
        aesAlg.IV = ivArray;
        using var encryptor = aesAlg.CreateEncryptor(encryptKey, aesAlg.IV);
        byte[] resultArray = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        if (isHex)
        {
            // 将二进制转换为16进制
            var builder = new StringBuilder();
            for (int i = 0; i < resultArray.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", resultArray[i]));
            }
            return builder.ToString();
        }
        else
        {
            return Convert.ToBase64String(resultArray);
        }
    }
    #endregion

    #region 获取AES明文
    /// <summary>
    /// 获取AES明文
    /// </summary>
    /// <param name="cipherText">密文</param>
    /// <param name="key">秘钥</param>
    /// <param name="iv">偏移量</param>
    /// <param name="isHex">是否解密16进制</param>
    /// <returns>明文</returns>
    public static string ToAESDecrypt(this string cipherText, string key = null, string iv = null, bool isHex = false)
    {
        if (key.IsNullOrEmpty()) key = @")O[NB]6,YF}+efcaj{+oESbld8>Z'e9M";
        if (iv.IsNullOrEmpty()) iv = @"L+\~f4,Ir)b$=pkf";
        var ivByte = Encoding.UTF8.GetBytes(iv!);
        byte[] toEncryptArray;
        if (isHex)
        {
            // 将16进制转换为二进制
            toEncryptArray = new byte[cipherText.Length / 2];
            for (int i = 0; i < toEncryptArray.Length; i++)
            {
                toEncryptArray[i] = Convert.ToByte(cipherText.Substring(i * 2, 2), 16);
            }
        }
        else
        {
            toEncryptArray = Convert.FromBase64String(cipherText);
        }
        var decryptKey = Encoding.UTF8.GetBytes(key!);
        using var aesAlg = Aes.Create();
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.KeySize = 128;
        aesAlg.Key = decryptKey;
        aesAlg.Padding = PaddingMode.PKCS7;
        aesAlg.IV = ivByte;
        using var decryptor = aesAlg.CreateDecryptor(decryptKey, aesAlg.IV);
        byte[] resultArray = decryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Encoding.UTF8.GetString(resultArray);
    }
    #endregion

    #region 获取RSA公钥和私钥
    /// <summary>
    /// 获取RSA公钥和私钥，大小必须以8为增量从384位到16384位
    /// 例如：1024 或 2048
    /// </summary>
    /// <param name="dwKeySize">key大小</param>
    /// <returns>RSA公钥和私钥</returns>
    /// <exception cref="ArgumentException"></exception>
    public static RSASecretKey ToRSASecretKey(this int dwKeySize)
    {
        if (dwKeySize < 384 || dwKeySize > 16384 || dwKeySize % 8 != 0)
            throw new ArgumentException("大小必须以8为增量从384位到16384位");

        var rsaKey = new RSASecretKey();
        using (var rsa = new RSACryptoServiceProvider(dwKeySize))
        {
            rsaKey.PrivateKey = rsa.ToXmlString(true);
            rsaKey.PublicKey = rsa.ToXmlString(false);
        }
        return rsaKey;
    }
    #endregion

    #region 获取RSA密文（公钥加密）
    /// <summary>
    /// 获取RSA密文（公钥加密）
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="xmlPublicKey">公钥</param>
    /// <returns>密文</returns>
    public static string ToRSAEncrypt(this string plainText, string xmlPublicKey)
    {
        string encryptedContent = string.Empty;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(xmlPublicKey);
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
            encryptedContent = Convert.ToBase64String(encryptedData);
        }
        return encryptedContent;
    }
    #endregion

    #region 获取RSA明文（私钥解密）
    /// <summary>
    /// 获取RSA明文（私钥解密）
    /// </summary>
    /// <param name="cipherText">密文</param>
    /// <param name="xmlPrivateKey">私钥</param>
    /// <returns>明文</returns>
    public static string ToRSADecrypt(this string cipherText, string xmlPrivateKey)
    {
        string decryptedContent = string.Empty;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(xmlPrivateKey);
            byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(cipherText), false);
            decryptedContent = Encoding.UTF8.GetString(decryptedData);
        }
        return decryptedContent;
    }
    #endregion

    #region RSA公钥和私钥实体
    /// <summary>
    /// RSA公钥和私钥
    /// </summary>
    public class RSASecretKey
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string? PublicKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string? PrivateKey { get; set; }
    }
    #endregion

    #region 获取摩斯密码
    public static string ToMorseEncrypt(this string plainText)
    {
        int i;
        string ret = string.Empty;
        if (plainText != null && (plainText = plainText.ToUpper()).Length > 0)
            foreach (char asc in plainText)
                if ((i = Find(asc.ToString(), 0)) > -1)
                    ret += " " + CodeTable[i, 1];
        return ret.Trim();
    }
    #endregion

    #region 获取摩斯密码明文
    public static string ToMorseDecrypt(this string cipherText)
    {
        int i;
        string[] splits;
        string ret = string.Empty;
        if (cipherText != null && (splits = cipherText.Split(' ')).Length > 0)
        {
            foreach (string split in splits)
                if ((i = Find(split, 1)) > -1)
                    ret += CodeTable[i, 0];
            return ret;
        }
        return "{#}";
    } 
    #endregion

    #region 查找
    private static int Find(string str, int cols)
    {
        int i = 0, len = CodeTable.Length / 2; // len / rank
        while (i < len)
        {
            if (CodeTable[i, cols] == str)
                return i;
            i++;
        };
        return -1;
    }
    #endregion

    #region 密码表
    private static readonly string[,] CodeTable =
    {
        {"A",".-"},
        {"B","-..."},
        {"C","-.-."},
        {"D","-.."},
        {"E","."},
        {"E","..-.."},
        {"F","..-."},
        {"G","--."},
        {"H","...."},
        {"I",".."},
        {"J",".---"},
        {"K","-.-"},
        {"L",".-.."},
        {"M","--"},
        {"N","-."},
        {"O","---"},
        {"P",".--."},
        {"Q","--.-"},
        {"R",".-."},
        {"S","..."},
        {"T","-"},
        {"U","..-"},
        {"V","...-"},
        {"W",".--"},
        {"X","-..-"},
        {"Y","-.--"},
        {"Z","--.."},
        {"0","-----"},
        {"1",".----"},
        {"2","..---"},
        {"3","...--"},
        {"4","....-"},
        {"5","....."},
        {"6","-...."},
        {"7","--..."},
        {"8","---.."},
        {"9","----."},
        {".",".-.-.-"},
        {",","--..--"},
        {":","---..."},
        {"?","..--.."},
        {"\'",".----."},
        {"-","-....-"},
        {"/","-..-."},
        {"(","-.--."},
        {")","-.--.-"},
        {"\"",".-..-."},
        {"=","-...-"},
        {"+",".-.-."},
        {"*","-..-"},
        {"@",".--.-."},
        {"{UNDERSTOOD}","...-."},
        {"{ERROR}","........"},
        {"{INVITATION TO TRANSMIT}","-.-"},
        {"{WAIT}",".-..."},
        {"{END OF WORK}","...-.-"},
        {"{STARTING SIGNAL}","-.-.-"},
        {" ","\u2423"}
    };
    #endregion
}
#endregion
