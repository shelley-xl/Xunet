#if NET6_0_OR_GREATER

using System.Linq;
using System.Net;
using Xunet.IP2Region;

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