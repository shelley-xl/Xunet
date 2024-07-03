namespace Xunet.Tests.AOP;

public class UserService : IUserService
{
    public List<string> Create(string name, List<string> list)
    {
        list.Add(name);
        return list;
    }
}
