using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xunet;

#region String扩展类
/// <summary>
/// String扩展类
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 判断字符串是否为空或者null
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this);

    /// <summary>
    /// 判断字符串是否非空且不为null
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsNotNullOrEmpty(this string? @this) => !string.IsNullOrEmpty(@this);

    /// <summary>
    /// 判断字符串是否为空（含空格）或者null
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string? @this) => string.IsNullOrWhiteSpace(@this);

    /// <summary>
    /// 判断字符串是否非空（含空格）且不为null
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsNotNullOrWhiteSpace(this string? @this) => !string.IsNullOrWhiteSpace(@this);

    /// <summary>
    /// 是否是数字
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsNumber(this string? @this)
        => new Regex(@"\d+").IsMatch(@this ?? "");

    /// <summary>
    /// 是否是手机号
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsPhoneNumber(this string? @this)
    {
        //电信手机号码正则
        string dianxin = @"^1[3578][01379]\d{8}$";
        Regex dReg = new(dianxin);
        //联通手机号正则
        string liantong = @"^1[34578][01256]\d{8}$";
        Regex lReg = new(liantong);
        //移动手机号正则
        string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
        Regex yReg = new(yidong);
        return dReg.IsMatch(@this ?? "") || lReg.IsMatch(@this ?? "") || yReg.IsMatch(@this ?? "");
    }

    /// <summary>
    /// 是否是邮箱
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsEmail(this string? @this)
        => new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").IsMatch(@this ?? "");

    /// <summary>
    /// 是否是IP地址
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsIPAddress(this string? @this)
        => new Regex(@"\d+\.\d+\.\d+\.\d+").IsMatch(@this ?? "");

    /// <summary>
    /// 是否是Url地址
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsUrlAddress(this string? @this)
        => new Regex(@"[a-zA-z]+://[^\s]*").IsMatch(@this ?? "");

    /// <summary>
    /// 是否是身份证号码
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static bool IsIDCard(this string? @this)
        => new Regex(@"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|[7-9]1)\d{4}(19|20|21)\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$").IsMatch(@this ?? "");

    /// <summary>
    /// 获取中文字符
    /// </summary>
    /// <param name="str">原始字符串</param>
    /// <param name="containsSpeChar">是否包含特殊字符~!@#...</param>
    /// <returns></returns>
    public static string GetChineseWord(this string str, bool containsSpeChar = false)
    {
        var spechar = containsSpeChar ? "~!@#$%^&*()_+" : "";
        var matches = Regex.Matches(str, $@"[\u4E00-\u9FFF{spechar}]+", RegexOptions.IgnoreCase);
        var sb = new StringBuilder();
        foreach (Match NextMatch in matches.Cast<Match>())
        {
            sb.Append(NextMatch.Value);
        }
        return sb.ToString();
    }
}
#endregion
