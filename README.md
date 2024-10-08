# Xunet

Xunet is a general purpose tool library for .NET,contains extension methods,helper classes and some components,which is used to simplify development and improve work efficiency.

Support .NET Framework 4.5+、.NET 6.0、.NET 7.0、.NET 8.0

[![Nuget](https://img.shields.io/nuget/v/Xunet.svg?style=flat-square)](https://www.nuget.org/packages/Xunet)
[![Downloads](https://img.shields.io/nuget/dt/Xunet.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet?groupby=Version)
[![License](https://img.shields.io/github/license/shelley-xl/Xunet.svg)](https://github.com/shelley-xl/Xunet/blob/master/LICENSE)
![Vistors](https://visitor-badge.laobi.icu/badge?page_id=https://github.com/shelley-xl/Xunet)

## 扩展类

1、日期时间扩展类

```c#
// 获取当前时间戳
var timeStamp = DateTime.Now.ToTimeStamp();
// 时间戳转DateTime
var dateTime = timeStamp.ToDateTime();
```

2、Ffmpeg扩展类

```c#
// 音频文件路径
var audio_path = "音频文件路径";
// 获取音频文件时长
var length = audio_path.GetMediaLength("Ffmpeg.exe所在目录，默认当前目录");
// 视频文件路径
var video_path = "视频文件路径";
// 获取视频截图
var image = video_path.CatchVideoImg("目标存储路径", "Ffmpeg.exe所在目录，默认当前目录");
```

3、HttpClient扩展类

```c#
// 接口地址
var url = "https://api.demo.com/test";
// GET请求
var result = await url.HttpGetAsync();
// POST请求（支持application/json和form-data）
var result2 = await url.HttpPostAsync(HttpContentType.ApplicationJson, 
new { id = "参数1", name = "参数2" });
// PUT请求（支持application/json和form-data）
var result2 = await url.HttpPutAsync(HttpContentType.ApplicationJson, 
new { id = "参数1", name = "参数2" });
// DELETE请求
var result = await url.HttpDeleteAsync();

// 简单HttpClientFactory实现
// 添加
HttpClientFactory.AddHttpClient("baidu", x =>
{
    x.BaseAddress = new Uri("https://www.baidu.com");
});
// 创建
var client = HttpClientFactory.CreateClient("baidu");
// 使用
var result = client.GetAsync(url).Result;
```

4、Json扩展类

```c#
// object对象
var data = new { code = 0, message = "" };
// JSON序列化
var json = data.SerializeObject();
// JSON反序列化
var obj = json.DeserializeObject<object>();
```

5、随机数扩展类

```c#
// 获取0~10以内的随机数（方式一）
var randomNum1 = 0.Next(10);
// 获取0~10以内的随机数（方式二）
var randomNum2 = 10.Next();
// 获取8位数随机字符串
// 参数：level，复杂等级，0纯数字，1数字+字母，2数字+字母+特殊字符
var randomText = 8.NextString(2);
```

6、加解密扩展类

```c#
// 输入字符串
var input = "123456";
// MD5加密
var md5Value = input.ToMD5Encrypt();
// SHA256加密
var sha256Value = input.ToSHA256Encrypt();
// AES加密
var aesValue = input.ToAESEncrypt();
// AES解密
var aesText = aesValue.ToAESDecrypt();
// RSA加密
// 获取RSA公钥和私钥（2048位）
var key = 2048.ToRSASecretKey();
// 公钥加密
var rsaValue = input.ToRSAEncrypt(key.PublicKey);
// 私钥解密
var rsaText = rsaValue.ToRSADecrypt(key.PrivateKey);
```

7、String扩展类

```c#
// 输入字符串
var input = "123456";
// 判断是否是null或者空字符串
var isNull = input.IsNullOrEmpty();
// 判断是否非null且非空字符串
var isNotNull = input.IsNotNullOrEmpty();
// 判断是否是手机号
var isTel = input.IsPhoneNumber();
// ...
```

8、反射扩展类

```c#
// 对象
var user = new User { Name = "徐来" };
// 获取Name属性
var name = user.GetProperty<string>("Name");
// 设置Name属性
var namenew = user.SetProperty("Name", "徐来new");
```

## 辅助类

1、雪花ID

```c#
// 雪花Id（long类型）
var id1 = SnowflakeHelper.NextId();
// 雪花Id（字符串）
var id2 = SnowflakeHelper.NextIdString();
```

2、拼音助手

```c#
// 获取中文全拼
var pinyin = PinyinHelper.GetPinyin("徐来");
```

3、性能计时器

```c#
var milliSeconds = StopwatchHelper.Execute(() =>
{
    // do something
    Thread.Sleep(100);
});
```

4、IP转地区

```c#
// 说明：仅支持.NET 6.0 及以上版本
// 需要依赖ip2region.xdb文件
// https://github.com/lionsoul2014/ip2region/blob/master/data/ip2region.xdb

// 根据IP查询地区
var region = IP2RegionHelper.Search("ip地址");
```

5、钉钉

```c#
// 发送应用消息
var access_token = DingtalkHelper.GetAccessToken("appkey", "appsecret");
DingtalkHelper.SendMarkdownMessage(new SendMarkdownMessageRequest
{
    AgentId = 123456,
    AccessToken = access_token,
    Title = "恐龙抗狼",
    Text = "我没K，我没K，我没K，布鲁布鲁biu~",
    UserIds = new List<string> { "userId001" }
});

// 发送群机器人消息
DingtalkHelper.SendMarkdownMessageForGroupRobot(new SendMarkdownMessageForGroupRobotRequest
{
    Webhook = "",
    Secret = "",
    Title = "恐龙抗狼",
    Text = "我没K，我没K，我没K，布鲁布鲁biu~",
    AtUserIds = new List<string> { "userId001" }
});
```

6、Redis

```c#
// 初始化Redis缓存
RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1:6379"));
// 设置缓存
await RedisHelper.SetAsync("user", new User { Name = "徐来" });
// 获取缓存
var user = await RedisHelper.GetAsync<User>("user");
```

7、机器

```c#
// 获取本机IP地址
var localIPAddress = MachineHelper.GetLocalIPAddress();
// 获取公网IP地址
var publicIPAddress = MachineHelper.GetPublicIPAddress();
// 获取机器码
var machineCode = MachineHelper.GetMachineCode();
// 获取随机端口号
var port = MachineHelper.GetRandomPort();
// 检查端口号是否被占用
var isPortInUse = MachineHelper.IsPortInUse(port);
// 获取注册码
var registerCode = MachineHelper.GetRegisterCode("32位秘钥", machineCode, 30);
// 验证注册码是否有效
var verifyRegisterCode = MachineHelper.VerifyRegisterCode("32位秘钥", registerCode);
```

## 组件

1、日志组件

```c#
LogManager.Info("这是一条测试日志");
```

2、缓存组件

```c#
// 使用CSRedis;
using Xunet.CSRedis;

// 初始化Redis缓存
RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1:6379"));
// 设置缓存
await RedisHelper.SetAsync("user", new User { Name = "徐来" });
// 获取缓存
var user = await RedisHelper.GetAsync<User>("user");
```

3、ORM组件

```c#
// 使用Dapper
using Xunet.Dapper;

// 查询
var list = await connection.QueryAsync<User>("select * from user;");
```

4、定时任务组件

```c#
// 使用FluentScheduler
using Xunet.FluentScheduler;

// 初始化定时任务
JobManager.Initialize();
// 添加定时任务
JobManager.AddJob(() => DoWork(), (x) =>
{
    x.WithName("JobName");
    x.ToRunEvery(1).Days().At(10, 30); // 每天的10:30执行
    x.ToRunEvery(1).Weekdays().At(10, 30); // 每周一到周五的10:30执行
    x.ToRunEvery(1).Weeks().On(DayOfWeek.Friday).At(10, 30); // 每周五的10:30执行
    x.ToRunEvery(1).Months().On(18).At(10, 30).WeekdaysOnly(); // 每月18号的10:30执行（仅周一到周五）
});
// 扩展：基于TimeCrontab，集成后支持Cron表达式
JobManager.AddJob(() => DoWork(), (x) =>
{
    x.WithName("JobName");
    x.ToRunWithCron("30 10 * * ?"); // 每天的10:30执行
});
```

5、Excel组件

```c#
// 使用MiniExcel
using Xunet.MiniExcels;

var list = MiniExcel.Query<User>(path).ToList();
```

6、AOP组件

```c#
// 说明：仅支持.NET 6.0 及以上版本
using Xunet.AOP;
using Xunet.AOP.Filters;

IUserService userService = new UserService();
userService = TransformProxy.GetDynamicProxy(userService);
var result = userService.Create("Processing", new List<string>());

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
```

## 感谢

- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

- [Snowflake](https://github.com/twitter-archive/snowflake)

- [WebSocketSharp](https://github.com/sta/websocket-sharp)

- [TinyPinyin](https://github.com/hstarorg/TinyPinyin.Net)

- [Dapper](https://github.com/DapperLib/Dapper)

- [CSRedis](https://github.com/2881099/csredis)

- [FluentScheduler](https://github.com/fluentscheduler/FluentScheduler)

- [TimeCrontab](https://github.com/MonkSoul/TimeCrontab)

- [MiniExcel](https://github.com/mini-software/MiniExcel)

## 更新日志

[CHANGELOG](CHANGELOG.md)
