// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Tests.Aop.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CheckLoginAttribute : BeforeActionAttribute
{
    public override int Order { get; set; } = 2;

    public override object? AopAction(object?[]? args, object? args2)
    {
        if (args?[1] is List<string> list)
        {
            list.Add("CheckLogin");
            return list;
        }
        return null;
    }
}
