namespace Xunet.Tests.AOP;

public interface IUserService
{
    [CheckLogin, CheckIP, LoginLog, OperateLog]
    List<string> Create(string name, List<string> list);
}
