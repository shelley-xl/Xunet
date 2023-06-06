using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xunet.Newtonsoft.Json.Linq;

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
    /// <exception cref="HttpRequestException"></exception>
    public static string HttpGet(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        var result = client.GetAsync(url).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync().Result;
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
        return url.HttpGet(cookies).DeserializeObject<T>();
    }

    /// <summary>
    /// Send a GET request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static async Task<string> HttpGetAsync(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        var result = await client.GetAsync(url);
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return await result.Content.ReadAsStringAsync();
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
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
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
        var result = client.PostAsync(url, content).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync().Result;
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
        return url.HttpPost(contentType, param, cookies).DeserializeObject<T>();
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
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
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
        var result = client.PostAsync(url, content).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync();
    }
    #endregion

    #region PUT
    /// <summary>
    /// Send a Put request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static string HttpPut(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
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
        var result = client.PutAsync(url, content).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync().Result;
    }

    /// <summary>
    /// Send a Put request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static T? HttpPut<T>(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        return url.HttpPut(contentType, param, cookies).DeserializeObject<T>();
    }

    /// <summary>
    /// Send a Put request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="contentType"></param>
    /// <param name="param"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static Task<string> HttpPutAsync(this string url, HttpContentType contentType, object? param = null, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
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
        var result = client.PutAsync(url, content).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync();
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Send a DELETE request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static string HttpDelete(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        var result = client.DeleteAsync(url).Result;
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return result.Content.ReadAsStringAsync().Result;
    }

    /// <summary>
    /// Send a DELETE request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    public static T? HttpDelete<T>(this string url, string? cookies = null)
    {
        return url.HttpDelete(cookies).DeserializeObject<T>();
    }

    /// <summary>
    /// Send a DELETE request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookies"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<string> HttpDeleteAsync(this string url, string? cookies = null)
    {
        var useCookies = cookies.IsNullOrEmpty();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var handler = new HttpClientHandler
        {
            UseCookies = useCookies,
            AllowAutoRedirect = true,
            ClientCertificateOptions = ClientCertificateOption.Automatic
        };
        using var client = new HttpClient(handler);
        if (!useCookies) client.DefaultRequestHeaders.Add("cookie", cookies);
        var result = await client.DeleteAsync(url);
        if (!result.IsSuccessStatusCode)
            throw new HttpRequestException($"Http request error:{(int)result.StatusCode}", null);
        return await result.Content.ReadAsStringAsync();
    }
    #endregion
}

/// <summary>
/// HttpContentType
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