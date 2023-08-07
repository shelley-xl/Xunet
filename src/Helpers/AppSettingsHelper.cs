#if NET45_OR_GREATER

using System.Configuration;

namespace Xunet.Helpers;

/// <summary>
/// 配置辅助类
/// </summary>
public static class AppSettingsHelper
{
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValueOrDefault(string key, string defaultValue = "")
    {
        return ConfigurationManager.AppSettings.Get(key) ?? defaultValue;
    }
}

#endif
