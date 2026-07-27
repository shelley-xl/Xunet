// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Tests.Aop;

public interface IUserService
{
    [CheckLogin, CheckIP, LoginLog, OperateLog]
    List<string> Create(string name, List<string> list);
}
