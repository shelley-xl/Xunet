using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Xunet.Extensions;

#region 随机数扩展类
/// <summary>
/// 随机数扩展类
/// </summary>
public static class RandomExtension
{
    #region 字符数组
    private static readonly char[] ConstantNumber = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    private static readonly char[] ConstantLowercase = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private static readonly char[] ConstantUppercase = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private static readonly char[] ConstantSpecial = new[] { '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '-', '/', '?', '.', ',', ';', '[', ']', '{', '}', '=' };
    #endregion

    #region 生成随机数
    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回指定范围内的随机数</returns>
    public static int Next(this int minValue, int maxValue)
    {
        return new Random((int)Stopwatch.GetTimestamp()).Next(minValue, maxValue);
    }
    #endregion

    #region 生成随机数
    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回0到最大值的随机数</returns>
    public static int Next(this int maxValue)
    {
        return new Random((int)Stopwatch.GetTimestamp()).Next(maxValue);
    }
    #endregion

    #region 生成随机字符串
    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <param name="level">复杂等级，0纯数字，1数字+字母，2数字+字母+特殊字符</param>
    /// <returns>返回指定长度的随机字符串</returns>
    public static string NextString(this int length, int level = 0)
    {
        char[] array = level switch
        {
            0 => ConstantNumber,
            1 => ConstantNumber.Concat(ConstantLowercase).Concat(ConstantUppercase).ToArray(),
            2 => ConstantNumber.Concat(ConstantLowercase).Concat(ConstantUppercase).Concat(ConstantSpecial).ToArray(),
            _ => ConstantNumber,
        };
        var stringBuilder = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            var index = array.Length.Next();
            stringBuilder.Append(array[index]);
        }
        return stringBuilder.ToString();
    }
    #endregion 
}
#endregion
