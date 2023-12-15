using System;

namespace Xunet.AOP.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class BeforeActionAttribute : Attribute
{
    public abstract int Order { get; set; }
    public abstract object? AopAction(object?[]? args, object? args2);
}
