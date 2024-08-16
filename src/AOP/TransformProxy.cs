// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

#if NET6_0_OR_GREATER

using System.Reflection;

namespace Xunet.AOP;

public class TransformProxy
{
    public static T GetDynamicProxy<T>(T instance)
    {
        object? proxy = DispatchProxy.Create<T, DynamicProxy<T>>();
        if (proxy is DynamicProxy<T> dynamicProxy)
        {
            dynamicProxy.Instance = instance;
        }
        return (T)proxy!;
    }
}

#endif