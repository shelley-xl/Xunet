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