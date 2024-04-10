using Xunet.AOP;
using Xunet.AOP.Filters;

namespace Xunet.Tests;

public class AOPTest
{
    [Fact]
    public void Test()
    {
        IUserService userService = new UserService();
        userService = TransformProxy.GetDynamicProxy(userService);
        var result = userService.Create("Processing", new List<string>());
        Assert.Equal("CheckIP=>CheckLogin=>Processing=>LoginLog=>OperateLog", string.Join("=>", result));
    }
}

public class UserService : IUserService
{
    public List<string> Create(string name, List<string> list)
    {
        list.Add(name);
        return list;
    }
}

public interface IUserService
{
    [CheckLogin, CheckIP, LoginLog, OperateLog]
    List<string> Create(string name, List<string> list);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CheckIPAttribute : BeforeActionAttribute
{
    public override int Order { get; set; } = 1;

    public override object? AopAction(object?[]? args, object? args2)
    {
        if (args?[1] is List<string> list)
        {
            list.Add("CheckIP");
            return list;
        }
        return null;
    }
}

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
