﻿// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Helpers;

using System;
using System.Diagnostics;

/// <summary>
/// 性能计时器辅助类
/// </summary>
public static class StopwatchHelper
{
    /// <summary>
    /// 执行方法
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static double Execute(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        return sw.ElapsedMilliseconds;
    }
}
