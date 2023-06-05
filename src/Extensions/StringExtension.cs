namespace System;

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
} 
#endregion