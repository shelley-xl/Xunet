namespace Xunet.ObjectMapper.Core;

using System;
using System.Reflection.Emit;
#if NET6_0_OR_GREATER
using System.Reflection;
#endif

internal static class Helpers
{
    internal static bool IsValueType(Type type)
    {
#if NET6_0_OR_GREATER
        return type.GetTypeInfo().IsValueType;
#else
        return type.IsValueType;
#endif
    }

    internal static bool IsPrimitive(Type type)
    {
#if NET6_0_OR_GREATER
        return type.GetTypeInfo().IsPrimitive;
#else
        return type.IsPrimitive;
#endif
    }

    internal static bool IsEnum(Type type)
    {
#if NET6_0_OR_GREATER
        return type.GetTypeInfo().IsEnum;
#else
        return type.IsEnum;
#endif
    }

    internal static bool IsGenericType(Type type)
    {
#if NET6_0_OR_GREATER
        return type.GetTypeInfo().IsGenericType;
#else
        return type.IsGenericType;
#endif
    }

    internal static Type CreateType(TypeBuilder typeBuilder)
    {
#if NET6_0_OR_GREATER
        return typeBuilder.CreateTypeInfo().AsType();
#else
        return typeBuilder.CreateType();
#endif
    }

    internal static Type BaseType(Type type)
    {
#if NET6_0_OR_GREATER
        return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
    }

}