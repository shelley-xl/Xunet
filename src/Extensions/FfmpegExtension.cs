// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Extensions;

using System.Diagnostics;
using System.IO;

#region Ffmpeg扩展类
/// <summary>
/// Ffmpeg扩展类
/// </summary>
public static class FfmpegExtension
{
    #region 获取文件时长
    /// <summary>
    /// 获取文件时长，指定ffmpeg.exe所在文件夹，默认当前目录
    /// </summary>
    /// <param name="sourcePath">源文件</param>
    /// <param name="ffmpegPath">ffmpeg.exe所在文件夹</param>
    /// <returns>时长</returns>
    /// <exception cref="FileNotFoundException">文件未找到异常</exception>
    public static int GetMediaLength(this string sourcePath, string? ffmpegPath = null)
    {
        var ffmpegFileName = Path.Combine(ffmpegPath ?? "", "ffmpeg.exe");
        if (!File.Exists(ffmpegFileName))
        {
            throw new FileNotFoundException("未找到ffmpeg.exe文件");
        }
        using Process pro = new();
        pro.StartInfo.UseShellExecute = false;
        pro.StartInfo.ErrorDialog = false;
        pro.StartInfo.CreateNoWindow = true;
        pro.StartInfo.RedirectStandardError = true;
        pro.StartInfo.FileName = ffmpegFileName;
        pro.StartInfo.Arguments = $"-i \"{sourcePath}\"";
        pro.Start();
        StreamReader errorreader = pro.StandardError;
        pro.WaitForExit(1000);
        string result = errorreader.ReadToEnd();
        if (!string.IsNullOrEmpty(result))
        {
            result = result.Substring(result.IndexOf("Duration: ") + "Duration: ".Length, "00:00:00".Length);
            var h = int.Parse(result.Split(':')[0]);
            var m = int.Parse(result.Split(':')[1]);
            var s = int.Parse(result.Split(':')[2]);
            return h * 3600 + m * 60 + s;
        }
        return 0;
    }
    #endregion

    #region 获取视频截图
    /// <summary>
    /// 获取视频截图，指定ffmpeg.exe所在文件夹，默认当前目录
    /// </summary>
    /// <param name="sourcePath">源文件</param>
    /// <param name="targetPath">目标文件</param>
    /// <param name="ffmpegPath">ffmpeg.exe所在文件夹</param>
    /// <param name="isCrop">是否裁剪，默认居中裁剪</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <param name="seconds">秒/帧</param>
    /// <returns>结果</returns>
    /// <exception cref="FileNotFoundException">文件未找到异常</exception>
    public static string CatchVideoImg(this string sourcePath, string targetPath, string? ffmpegPath = null, bool isCrop = false, int width = 100, int height = 100, int seconds = 1)
    {
        var ffmpegFileName = Path.Combine(ffmpegPath ?? "", "ffmpeg.exe");
        if (!File.Exists(ffmpegFileName))
        {
            throw new FileNotFoundException("未找到ffmpeg.exe文件");
        }
        using Process pro = new();
        pro.StartInfo.UseShellExecute = false;
        pro.StartInfo.ErrorDialog = false;
        pro.StartInfo.CreateNoWindow = true;
        pro.StartInfo.RedirectStandardError = true;
        pro.StartInfo.FileName = ffmpegFileName;
        pro.StartInfo.Arguments = $"-ss {seconds.ToStandardTime()} -y -i \"{sourcePath}\" {(isCrop ? $"-vf crop={width}:{height}" : "")} \"{targetPath}\" -r 1 -vframes 1 -an -f mjpeg";
        pro.Start();
        StreamReader errorreader = pro.StandardError;
        pro.WaitForExit(1000);
        string result = errorreader.ReadToEnd();
        return result;
    }
    #endregion

    #region 执行指定命令
    /// <summary>
    /// 执行指定命令，指定ffmpeg.exe所在文件夹，默认当前目录
    /// </summary>
    /// <param name="ffmpegPath">ffmpeg.exe所在文件夹</param>
    /// <param name="command">执行命令</param>
    /// <returns>结果</returns>
    /// <exception cref="FileNotFoundException">文件未找到异常</exception>
    public static string ExecuteFfmpegCommand(this string ffmpegPath, string command)
    {
        var ffmpegFileName = Path.Combine(ffmpegPath ?? "", "ffmpeg.exe");
        if (!File.Exists(ffmpegFileName))
        {
            throw new FileNotFoundException("未找到ffmpeg.exe文件");
        }
        using Process pro = new();
        pro.StartInfo.UseShellExecute = false;
        pro.StartInfo.ErrorDialog = false;
        pro.StartInfo.CreateNoWindow = true;
        pro.StartInfo.RedirectStandardError = true;
        pro.StartInfo.FileName = ffmpegFileName;
        pro.StartInfo.Arguments = command;
        pro.Start();
        StreamReader errorreader = pro.StandardError;
        pro.WaitForExit(1000);
        string result = errorreader.ReadToEnd();
        return result;
    }
    #endregion
}
#endregion
