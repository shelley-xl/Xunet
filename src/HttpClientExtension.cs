using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System;

#region HttpClient扩展类
/// <summary>
/// HttpClient扩展类
/// </summary>
public static class HttpClientExtension
{
    #region GET
    /// <summary>
    /// Send a GET request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static string HttpGet(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        return client.GetStringAsync(url).Result;
    }

    /// <summary>
    /// Send a GET request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static Task<string> HttpGetAsync(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        return client.GetStringAsync(url);
    }

    /// <summary>
    /// Send a GET request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static T? HttpGet<T>(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        return client.GetStringAsync(url).Result.DeserializeObject<T>();
    }
    #endregion

    #region POST
    /// <summary>
    /// Send a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static string HttpPost(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        HttpContent? content = null;
        if (param != null && contentType == HttpContentType.ApplicationJson)
        {
            content = new StringContent(param.SerializeObject(), Encoding.UTF8, "application/json");
        }
        if (param != null && contentType == HttpContentType.FormData)
        {
            content = new MultipartFormDataContent();
            var obj = JObject.FromObject(param);
            foreach (var item in obj)
            {
                ((MultipartFormDataContent)content).Add(new StringContent(item.Value!.ToString()), item.Key);
            }
        }
        return client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
    }

    /// <summary>
    /// Send a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static Task<string> HttpPostAsync(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        HttpContent? content = null;
        if (param != null && contentType == HttpContentType.ApplicationJson)
        {
            content = new StringContent(param.SerializeObject(), Encoding.UTF8, "application/json");
        }
        if (param != null && contentType == HttpContentType.FormData)
        {
            content = new MultipartFormDataContent();
            var obj = JObject.FromObject(param);
            foreach (var item in obj)
            {
                ((MultipartFormDataContent)content).Add(new StringContent(item.Value!.ToString()), item.Key);
            }
        }
        return client.PostAsync(url, content).Result.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Send a POST request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static T? HttpPost<T>(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        HttpContent? content = null;
        if (param != null && contentType == HttpContentType.ApplicationJson)
        {
            content = new StringContent(param.SerializeObject(), Encoding.UTF8, "application/json");
        }
        if (param != null && contentType == HttpContentType.FormData)
        {
            content = new MultipartFormDataContent();
            var obj = JObject.FromObject(param);
            foreach (var item in obj)
            {
                ((MultipartFormDataContent)content).Add(new StringContent(item.Value!.ToString()), item.Key);
            }
        }
        return client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result.DeserializeObject<T>();
    }
    #endregion

    #region PUT

    #endregion

    #region DELETE

    #endregion
}

/// <summary>
/// 
/// </summary>
public enum HttpContentType
{
    /// <summary>
    /// application/json
    /// </summary>
    [Description("application/json")]
    ApplicationJson = 1,
    /// <summary>
    /// form-data
    /// </summary>
    [Description("form-data")]
    FormData = 2
}
#endregion