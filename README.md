# 常用工具库

Xunet 是 .NET的通用工具库，包含扩展方法、帮助类和一些中间件，用于简化开发和提高工作效率。

[![Xunet](https://img.shields.io/nuget/v/Xunet.svg?style=flat-square)](https://www.nuget.org/packages/Xunet)

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
// 获取8位数随机字符串
// 参数：level，复杂等级，0纯数字，1数字+字母，2数字+字母+特殊字符
var text = 8.NextString(2);
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
// 判断是否是null或者空字符串
var isNull = "".IsNullOrEmpty();
// 判断是否非null且非空字符串
var isNotNull = "123456".IsNotNullOrEmpty();
```

## 辅助类

1、雪花ID

```c#
// 获取雪花Id
var id1 = SnowflakeHelper.NextId();
// 获取雪花Id字符串
var id2 = SnowflakeHelper.NextIdString();
```
