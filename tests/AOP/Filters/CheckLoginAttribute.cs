namespace Xunet.Tests.AOP.Filters;

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
