using System.Security.Cryptography;
using System.Text;

namespace System;

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
        using (var md5 = MD5.Create())
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            var MD5buffer = md5.ComputeHash(buffer);
            StringBuilder builder = new StringBuilder();
            foreach (var item in MD5buffer)
            {
                if (toUpper) builder.Append(item.ToString("X2"));
                else builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }
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
        using (var sha256 = SHA256.Create())
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            var SHA256buffer = sha256.ComputeHash(buffer);
            StringBuilder builder = new StringBuilder();
            foreach (var item in SHA256buffer)
            {
                if (toUpper) builder.Append(item.ToString("X2"));
                else builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }
    }
    #endregion

    #region 获取AES密文
    /// <summary>
    /// 获取AES密文
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="key">秘钥</param>
    /// <param name="iv">偏移量</param>
    /// <returns>密文</returns>
    public static string ToAESEncrypt(this string plainText, string? key = null, string? iv = null)
    {
        if (key.IsNullOrEmpty()) key = @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
        if (iv.IsNullOrEmpty()) iv = @"L+\~f4,Ir)b$=pkf";
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(plainText);
        var encryptKey = Encoding.UTF8.GetBytes(key!);
        byte[] ivArray = Encoding.UTF8.GetBytes(iv!);
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.KeySize = 128;
            aesAlg.Key = encryptKey;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.IV = ivArray;
            using (var encryptor = aesAlg.CreateEncryptor(encryptKey, aesAlg.IV))
            {
                byte[] resultArray = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray);
            }
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
    /// <returns>明文</returns>
    public static string ToAESDecrypt(this string cipherText, string? key = null, string? iv = null)
    {
        if (key.IsNullOrEmpty()) key = @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
        if (iv.IsNullOrEmpty()) iv = @"L+\~f4,Ir)b$=pkf";
        var ivByte = Encoding.UTF8.GetBytes(iv!);
        byte[] toEncryptArray = Convert.FromBase64String(cipherText);
        var decryptKey = Encoding.UTF8.GetBytes(key!);
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.KeySize = 128;
            aesAlg.Key = decryptKey;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.IV = ivByte;
            using (var decryptor = aesAlg.CreateDecryptor(decryptKey, aesAlg.IV))
            {
                byte[] resultArray = decryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }
    }
    #endregion

    #region 获取RSA公钥和私钥
    /// <summary>
    /// 获取RSA公钥和私钥，大小必须以8为增量从384位到16384位
    /// </summary>
    /// <param name="dwKeySize">key大小</param>
    /// <returns>RSA公钥和私钥</returns>
    public static RSASecretKey ToRSASecretKey(this int dwKeySize)
    {
        RSASecretKey rsaKey = new RSASecretKey();
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize))
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
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
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
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
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
}
#endregion