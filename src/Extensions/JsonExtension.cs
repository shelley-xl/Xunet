using Xunet.Newtonsoft.Json;

namespace Xunet;

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
