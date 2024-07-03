namespace Xunet.Tests.AOP.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class OperateLogAttribute : AfterActionAttribute
{
    public override int Order { get; set; } = 2;

    public override object? AopAction(object?[]? args, object? args2)
    {
        if (args?[1] is List<string> list)
        {
            list.Add("OperateLog");
            return list;
        }
        return null;
    }
}
