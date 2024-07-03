namespace Xunet.Tests.AOP.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class LoginLogAttribute : AfterActionAttribute
{
    public override int Order { get; set; } = 1;

    public override object? AopAction(object?[]? args, object? args2)
    {
        if (args?[1] is List<string> list)
        {
            list.Add("LoginLog");
            return list;
        }
        return null;
    }
}
