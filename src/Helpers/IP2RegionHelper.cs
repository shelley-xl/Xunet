// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

#if NET6_0_OR_GREATER

namespace Xunet.Helpers;

using System.Linq;
using System.Net;
using IP2Region;

/// <summary>
/// IP转地区辅助类
/// </summary>
public class IP2RegionHelper
{
    /// <summary>
    /// 根据IP查询（完整）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string Search(string ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return string.Join(" ", result.Split('|').Where(x => x != "0").Select(x => x.Trim()).ToArray());
    }

    /// <summary>
    /// 根据IP查询（国家）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchCountry(string ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return result.Split('|')[0];
    }

    /// <summary>
    /// 根据IP查询（省份）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchProvince(string ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return result.Split('|')[2].Replace("省", "");
    }

    /// <summary>
    /// 根据IP查询（完整）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string Search(IPAddress ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return string.Join(" ", result.Split('|').Where(x => x != "0").Select(x => x.Trim()).ToArray());
    }

    /// <summary>
    /// 根据IP查询（国家）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchCountry(IPAddress ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return result.Split('|')[0];
    }

    /// <summary>
    /// 根据IP查询（省份）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchProvince(IPAddress ip)
    {
        var searcher = new Searcher(CachePolicy.Content);
        var result = searcher.Search(ip);
        return result.Split('|')[2];
    }
}

#endif

#if NET45_OR_GREATER

using System;
using System.Linq;
using System.Net;

namespace Xunet.Helpers;

/// <summary>
/// IP转地区辅助类
/// </summary>
public class IP2RegionHelper
{
    /// <summary>
    /// 根据IP查询（完整）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string Search(string ip)
    {
        throw new NotSupportedException("IP2RegionHelper.Search(string ip) is not supported on .NET Framework.");
    }

    /// <summary>
    /// 根据IP查询（国家）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchCountry(string ip)
    {
        throw new NotSupportedException("IP2RegionHelper.SearchCountry(string ip) is not supported on .NET Framework.");
    }

    /// <summary>
    /// 根据IP查询（省份）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchProvince(string ip)
    {
        throw new NotSupportedException("IP2RegionHelper.SearchProvince(string ip) is not supported on .NET Framework.");
    }

    /// <summary>
    /// 根据IP查询（完整）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string Search(IPAddress ip)
    {
        throw new NotSupportedException("IP2RegionHelper.Search(IPAddress ip) is not supported on .NET Framework.");
    }

    /// <summary>
    /// 根据IP查询（国家）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchCountry(IPAddress ip)
    {
        throw new NotSupportedException("IP2RegionHelper.SearchCountry(IPAddress ip) is not supported on .NET Framework.");
    }

    /// <summary>
    /// 根据IP查询（省份）
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static string SearchProvince(IPAddress ip)
    {
        throw new NotSupportedException("IP2RegionHelper.SearchProvince(IPAddress ip) is not supported on .NET Framework.");
    }
}

#endif