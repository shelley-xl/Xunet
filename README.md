# 常用工具库

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

4、Json扩展类

5、随机数扩展类

6、加解密扩展类

7、String扩展类
