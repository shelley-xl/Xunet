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

#if NET6_0_OR_GREATER

using Microsoft.Extensions.Configuration;
using System;

namespace Xunet.Helpers;

/// <summary>
/// 配置辅助类
/// </summary>
public static class AppSettingsHelper
{
    public static IConfiguration Configuration { get; private set; }
        = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile("appsettings.Development.json", true, true)
        .AddJsonFile("appsettings.Production.json", true, true)
        .AddJsonFile("appsettings.Test.json", true, true)
        .AddJsonFile("appsettings.Stage.json", true, true)
        .Build();

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValueOrDefault(string key, string defaultValue = "")
    {
        string config = Configuration[key];
        return config.IsNullOrEmpty() ? defaultValue : config;
    }
}

#endif
