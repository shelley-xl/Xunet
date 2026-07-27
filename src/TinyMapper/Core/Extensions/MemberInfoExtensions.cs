namespace Xunet.ObjectMapper.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunet.ObjectMapper.Core.DataStructures;

internal static class MemberInfoExtensions
{
    public static Option<TAttribute> GetAttribute<TAttribute>(this MemberInfo value)
        where TAttribute : Attribute
    {
        return value.GetCustomAttributes(true)
                        .FirstOrDefault(x => x is TAttribute)
                        .ToType<TAttribute>();
    }

    public static List<TAttribute> GetAttributes<TAttribute>(this MemberInfo value)
        where TAttribute : Attribute
    {
        return value.GetCustomAttributes(true).OfType<TAttribute>().ToList();
    }

    public static Type GetMemberType(this MemberInfo value)
    {
        if (value.IsField())
        {
            return ((FieldInfo)value).FieldType;
        }
        if (value.IsProperty())
        {
            return ((PropertyInfo)value).PropertyType;
        }
        if (value.IsMethod())
        {
            return ((MethodInfo)value).ReturnType;
        }
        throw new NotSupportedException();
    }

    public static bool IsField(this MemberInfo value)
    {
#if NET6_0_OR_GREATER
        return value is FieldInfo;
#else
        return value.MemberType == MemberTypes.Field;
#endif
    }

    public static bool IsProperty(this MemberInfo value)
    {
#if NET6_0_OR_GREATER
        return value is PropertyInfo;
#else
        return value.MemberType == MemberTypes.Property;
#endif
    }

    private static bool IsMethod(this MemberInfo value)
    {
#if NET6_0_OR_GREATER
        return value is MethodInfo;
#else
        return value.MemberType == MemberTypes.Method;
#endif

    }
}
