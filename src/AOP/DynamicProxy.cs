// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

#if NET6_0_OR_GREATER

using System.Linq;
using System.Reflection;
using Xunet.AOP.Filters;

namespace Xunet.AOP;

public class DynamicProxy<T> : DispatchProxy
{
    public T? Instance { get; set; }
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod.IsDefined(typeof(BeforeActionAttribute), true))
        {
            var attributes = targetMethod
                .GetCustomAttributes<BeforeActionAttribute>(true)
                .OrderBy(x => x.Order)
                .ToArray();
            object? args2 = null;
            foreach (var attribute in attributes)
            {
                args2 = attribute.AopAction(args, args2);
            }
        }

        var result = targetMethod?.Invoke(Instance, args);

        if (targetMethod.IsDefined(typeof(AfterActionAttribute), true))
        {
            var attributes = targetMethod
                .GetCustomAttributes<AfterActionAttribute>(true)
                .OrderBy(x => x.Order)
                .ToArray();
            object? args2 = result;
            foreach (var attribute in attributes)
            {
                args2 = attribute.AopAction(args, args2);
            }
        }

        return result;
    }
}

#endif