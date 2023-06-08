# 常用工具库

Xunet 是 .NET 的通用工具库，包含扩展方法、辅助类和一些常用组件，用于简化开发和提高工作效率。

支持 .NET Framework 4.5、.NET 6.0、.NET 7.0

[![Xunet](https://img.shields.io/nuget/v/Xunet.svg?style=flat-square)](https://www.nuget.org/packages/Xunet)
[![Xunet](https://img.shields.io/nuget/dt/Xunet.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet?groupby=Version)

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

9、异常处理扩展类

```c#
var result = typeof(TestClass).TryCatch(
    (x) =>
    {
        // 主要业务逻辑
    },
    (x) =>
    {
        // 异常处理
    },
    (x) =>
    {
        // 执行成功
    });

typeof(TestClass).TryFinally(
    (x) => 
    {
        // 主要业务逻辑
    },
    (x) => 
    {
        // finally处理
    });
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

3、读取配置文件

```c#
// 获取配置
var value = AppSettingsHelper.GetValueOrDefault("key");
```

4、性能计时器

```c#
var milliSeconds = StopwatchHelper.Execute(() =>
{
    // do something
    Thread.Sleep(100);
});
```

5、数据计算

```c#
// 获取两个坐标的距离
var distance = MathHelper.GetDistance(1, 2, 3, 4);
```

## 组件

1、日志组件

```c#
LogManager.Info("这是一条测试日志");
```

2、缓存组件

```c#
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

var list = connection.Query<User>("select * from user;").ToList();
```

# 感谢

- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

- [Snowflake](https://github.com/twitter-archive/snowflake)

- [WebSocketSharp](https://github.com/sta/websocket-sharp)

- [TinyPinyin](https://github.com/hstarorg/TinyPinyin.Net)

- [Dapper](https://github.com/DapperLib/Dapper)

- [CSRedis](https://github.com/2881099/csredis)
