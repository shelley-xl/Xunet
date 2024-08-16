// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Extensions;

using Newtonsoft.Json;

#region Json扩展类
/// <summary>
/// Json扩展类
/// </summary>
public static class JsonExtension
{
    /// <summary>
    /// Json序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string SerializeObject<T>(this T obj) => JsonConvert.SerializeObject(obj);

    /// <summary>
    /// Json反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? DeserializeObject<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
} 
#endregion
