﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Xunet.Logging;

/// <summary>
/// 日志组件
/// </summary>
public class LogManager
{
    private static DateTime Now => DateTime.Now;

    private static readonly ConcurrentQueue<ConcurrentQueueModel> LogQueue = new();

    /// <summary>
    /// 自定义事件
    /// </summary>
    public static event Action<LogInfo> Event;

    static LogManager()
    {
        Task.Factory.StartNew(() =>
        {
            while (true)
            {
                Pause.WaitOne(1000, true);
                var temp = new List<string[]>();
                foreach (var item in LogQueue)
                {
                    var logMergeContent = string.Concat(item.Msg, Environment.NewLine, "----------------------------------------------------------------------------------------------------------------------", Environment.NewLine);
                    var logArr = temp.FirstOrDefault(d => d[0].Equals(item.LogPath));
                    if (logArr != null)
                    {
                        logArr[1] = string.Concat(logArr[1], logMergeContent);
                    }
                    else
                    {
                        logArr = new[]
                        {
                                item.LogPath,
                                logMergeContent
                            };
                        temp.Add(logArr);
                    }

                    LogQueue.TryDequeue(out _);
                }

                foreach (var item in temp)
                {
                    WriteText(item[0], item[1]);
                }
            }
        }, TaskCreationOptions.LongRunning);
    }

    private static AutoResetEvent Pause => new(false);

    private static string _logDirectory;
    /// <summary>
    /// 日志存放目录，默认日志放在当前应用程序运行目录下的logs文件夹中
    /// </summary>
    public static string LogDirectory
    {
        get => _logDirectory ?? (Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Any(s => s.Contains("Web.config")) ? AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Logs\" : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
        set
        {
            //自定义目录
            if (!Directory.Exists(value))
            {
                Directory.CreateDirectory(value);
            }
            _logDirectory = value;
        }
    }

    /// <summary>
    /// 写入Info级别的日志
    /// </summary>
    /// <param name="info"></param>
    public static void Info(string info)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(info).ToUpper()}  {info}"
        });
        var log = new LogInfo()
        {
            LogLevel = LogLevel.Info,
            Message = info,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入Info级别的日志
    /// </summary>
    /// <param name="source"></param>
    /// <param name="info"></param>
    public static void Info(string source, string info)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(info).ToUpper()}   {source}  {info}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Info,
            Message = info,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入Info级别的日志
    /// </summary>
    /// <param name="source"></param>
    /// <param name="info"></param>
    public static void Info(Type source, string info)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(info).ToUpper()}   {source.FullName}  {info}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Info,
            Message = info,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入debug级别日志
    /// </summary>
    /// <param name="debug">异常对象</param>
    public static void Debug(string debug)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(debug).ToUpper()}   {debug}"
        });
        LogInfo log = new()
        {
            LogLevel = LogLevel.Debug,
            Message = debug,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入debug级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="debug">异常对象</param>
    public static void Debug(string source, string debug)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(debug).ToUpper()}   {source}  {debug}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Debug,
            Message = debug,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入debug级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="debug">异常对象</param>
    public static void Debug(Type source, string debug)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(debug).ToUpper()}   {source.FullName}  {debug}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Debug,
            Message = debug,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入error级别日志
    /// </summary>
    /// <param name="error">异常对象</param>
    public static void Error(Exception error)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(error).ToUpper()}   {error.Source}  {error.Message}{Environment.NewLine}{error.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Error,
            Message = error.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = error.Source,
            Exception = error,
            ExceptionType = error.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入error级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="error">异常对象</param>
    public static void Error(Type source, Exception error)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(error).ToUpper()}   {source.FullName}  {error.Message}{Environment.NewLine}{error.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Error,
            Message = error.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName,
            Exception = error,
            ExceptionType = error.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入error级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="error">异常信息</param>
    public static void Error(Type source, string error)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(error).ToUpper()}   {source.FullName}  {error}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Error,
            Message = error,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName,
            //Exception = error,
            ExceptionType = error.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入error级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="error">异常对象</param>
    public static void Error(string source, Exception error)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(error).ToUpper()}   {source}  {error.Message}{Environment.NewLine}{error.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Error,
            Message = error.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source,
            Exception = error,
            ExceptionType = error.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入error级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="error">异常信息</param>
    public static void Error(string source, string error)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(error).ToUpper()}   {source}  {error}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Error,
            Message = error,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source,
            //Exception = error,
            ExceptionType = error.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入fatal级别日志
    /// </summary>
    /// <param name="fatal">异常对象</param>
    public static void Fatal(Exception fatal)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(fatal).ToUpper()}   {fatal.Source}  {fatal.Message}{Environment.NewLine}{fatal.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Fatal,
            Message = fatal.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = fatal.Source,
            Exception = fatal,
            ExceptionType = fatal.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入fatal级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="fatal">异常对象</param>
    public static void Fatal(Type source, Exception fatal)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(fatal).ToUpper()}   {source.FullName}  {fatal.Message}{Environment.NewLine}{fatal.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Fatal,
            Message = fatal.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName,
            Exception = fatal,
            ExceptionType = fatal.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入fatal级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="fatal">异常对象</param>
    public static void Fatal(Type source, string fatal)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(fatal).ToUpper()}   {source.FullName}  {fatal}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Fatal,
            Message = fatal,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source.FullName,
            //Exception = fatal,
            ExceptionType = fatal.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入fatal级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="fatal">异常对象</param>
    public static void Fatal(string source, Exception fatal)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(fatal).ToUpper()}   {source}  {fatal.Message}{Environment.NewLine}{fatal.StackTrace}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Fatal,
            Message = fatal.Message,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source,
            Exception = fatal,
            ExceptionType = fatal.GetType().Name
        };
        Event?.Invoke(log);
    }

    /// <summary>
    /// 写入fatal级别日志
    /// </summary>
    /// <param name="source">异常源的类型</param>
    /// <param name="fatal">异常对象</param>
    public static void Fatal(string source, string fatal)
    {
        LogQueue.Enqueue(new ConcurrentQueueModel
        {
            LogPath = GetLogPath(),
            Msg = $"{Now}   [{Environment.CurrentManagedThreadId}]   {nameof(fatal).ToUpper()}   {source}  {fatal}"
        });
        var log = new LogInfo
        {
            LogLevel = LogLevel.Fatal,
            Message = fatal,
            Time = Now,
            ThreadId = Environment.CurrentManagedThreadId,
            Source = source,
            ExceptionType = fatal.GetType().Name
        };
        Event?.Invoke(log);
    }

    private static string GetLogPath()
    {
        string newFilePath;
        var logDir = string.IsNullOrEmpty(LogDirectory) ? Path.Combine(Environment.CurrentDirectory, "logs") : LogDirectory;
        Directory.CreateDirectory(logDir);
        const string extension = ".log";
        var fileNameNotExt = Now.ToString("yyyyMMdd");
        var fileNamePattern = string.Concat(fileNameNotExt, "(*)", extension);
        var filePaths = Directory.GetFiles(logDir, fileNamePattern, SearchOption.TopDirectoryOnly).ToList();

        if (filePaths.Count > 0)
        {
            int fileMaxLen = filePaths.Max(d => d.Length);
            string lastFilePath = filePaths.Where(d => d.Length == fileMaxLen).OrderByDescending(d => d).FirstOrDefault();
            if (new FileInfo(lastFilePath).Length > 1024 * 1024)
            {
                var no = new Regex(@"(?is)(?<=\()(.*)(?=\))").Match(Path.GetFileName(lastFilePath)).Value;
                var parse = int.TryParse(no, out int tempno);
                var formatno = $"({(parse ? (tempno + 1) : tempno)})";
                var newFileName = string.Concat(fileNameNotExt, formatno, extension);
                newFilePath = Path.Combine(logDir, newFileName);
            }
            else
            {
                newFilePath = lastFilePath;
            }
        }
        else
        {
            var newFileName = string.Concat(fileNameNotExt, $"({0})", extension);
            newFilePath = Path.Combine(logDir, newFileName);
        }

        return newFilePath;
    }

    private static void WriteText(string logPath, string logContent)
    {
        try
        {
            if (!File.Exists(logPath))
            {
                File.CreateText(logPath).Close();
            }

            using var sw = File.AppendText(logPath);
            sw.Write(logContent);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}

public class ConcurrentQueueModel
{
    public string LogPath { get; set; }
    public string Msg { get; set; }
}