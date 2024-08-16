// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

#if NET6_0_OR_GREATER

using System;

namespace Xunet.AOP.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class AfterActionAttribute : Attribute
{
    public abstract int Order { get; set; }
    public abstract object? AopAction(object?[]? args, object? args2);
}

#endif