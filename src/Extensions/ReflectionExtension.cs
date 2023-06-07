using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace System;

/// <summary>
/// 反射扩展类
/// </summary>
public static class ReflectionExtension
{
    #region 属性字段设置

    private readonly static BindingFlags bf = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    /// <summary>
    /// 执行方法
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="methodName">方法名，区分大小写</param>
    /// <param name="args">方法参数</param>
    /// <typeparam name="T">约束返回的T必须是引用类型</typeparam>
    /// <returns>T类型</returns>
    public static T InvokeMethod<T>(this object obj, string methodName, object[] args)
    {
        return (T)obj.GetType().GetMethod(methodName, args.Select(o => o.GetType()).ToArray()).Invoke(obj, args);
    }

    /// <summary>
    /// 执行方法
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="methodName">方法名，区分大小写</param>
    /// <param name="args">方法参数</param>
    /// <returns>T类型</returns>
    public static void InvokeMethod(this object obj, string methodName, object[] args)
    {
        var type = obj.GetType();
        type.GetMethod(methodName, args.Select(o => o.GetType()).ToArray()).Invoke(obj, args);
    }

    /// <summary>
    /// 设置字段
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="name">字段名</param>
    /// <param name="value">值</param>
    public static void SetField<T>(this T obj, string name, object value) where T : class
    {
        SetProperty(obj, name, value);
    }

    /// <summary>
    /// 获取字段
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="name">字段名</param>
    /// <typeparam name="T">约束返回的T必须是引用类型</typeparam>
    /// <returns>T类型</returns>
    public static T GetField<T>(this object obj, string name)
    {
        return GetProperty<T>(obj, name);
    }

    /// <summary>
    /// 获取所有的字段信息
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <returns>字段信息</returns>
    public static FieldInfo[] GetFields(this object obj)
    {
        FieldInfo[] fieldInfos = obj.GetType().GetFields(bf);
        return fieldInfos;
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="name">属性名</param>
    /// <param name="value">值</param>
    public static string SetProperty<T>(this T obj, string name, object value) where T : class
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.PropertyOrField(parameter, name);
        var before = Expression.Lambda(property, parameter).Compile().DynamicInvoke(obj);
        if (value == before)
        {
            return value?.ToString();
        }

        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            typeof(T).GetProperty(name)?.SetValue(obj, value);
        }
        else
        {
            var valueExpression = Expression.Parameter(property.Type, "v");
            var assign = Expression.Assign(property, valueExpression);
            Expression.Lambda(assign, parameter, valueExpression).Compile().DynamicInvoke(obj, value);
        }

        return before.SerializeObject();
    }

    private static readonly ConcurrentDictionary<string, Delegate> DelegateCache = new();

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <param name="name">属性名</param>
    /// <typeparam name="T">约束返回的T必须是引用类型</typeparam>
    /// <returns>T类型</returns>
    public static T GetProperty<T>(this object obj, string name)
    {
        var type = obj.GetType();
        if (DelegateCache.TryGetValue(type.Name + "." + name, out var func))
        {
            return (T)func.DynamicInvoke(obj);
        }
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.PropertyOrField(parameter, name);
        func = Expression.Lambda(property, parameter).Compile();
        DelegateCache.TryAdd(type.Name + "." + name, func);
        return (T)func.DynamicInvoke(obj);
    }

    /// <summary>
    /// 获取所有的属性信息
    /// </summary>
    /// <param name="obj">反射对象</param>
    /// <returns>属性信息</returns>
    public static PropertyInfo[] GetProperties(this object obj)
    {
        return obj.GetType().GetProperties(bf);
    }

    #endregion 属性字段设置

    #region 获取枚举描述
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetDescriptionFromValue<T>(this T enumValue)
    {
        string value = enumValue.ToString();
        FieldInfo field = enumValue.GetType().GetField(value);
        if (field == null) return value;
        DescriptionAttribute[] objs = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (objs.Length > 0)
        {
            return objs[0].Description;
        }
        return string.Empty;
    }
    #endregion

    #region 获取枚举描述
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetDescriptionFromValue<T>(this List<T> list)
    {
        foreach (var enumValue in list)
        {
            yield return GetDescriptionFromValue(enumValue);
        }
    }
    #endregion

    #region 获取枚举值
    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <typeparam name="T">泛型参数</typeparam>
    /// <param name="enumDescription">枚举描述</param>
    /// <returns></returns>
    public static T GetValueFromDescription<T>(this string enumDescription)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        foreach (var field in type.GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == enumDescription)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == enumDescription)
                    return (T)field.GetValue(null);
            }
        }
        return default;
    }
    #endregion
}

