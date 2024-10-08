﻿// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Helpers;

using Snowflake;

/// <summary>
/// 雪花ID辅助类
/// </summary>
public class SnowflakeHelper
{
    #region Private Fields
    private static readonly IdWorker _worker = new(1, 1);
    private static readonly object _obj = new();
    #endregion

    #region Public Methods
    /// <summary>
    /// 获取long类型唯一ID
    /// </summary>
    /// <returns></returns>
    public static long NextId()
    {
        lock (_obj)
        {
            return _worker.NextId();
        }
    }

    /// <summary>
    /// 获取字符串类型唯一ID
    /// </summary>
    /// <returns></returns>
    public static string NextIdString()
    {
        return NextId().ToString();
    } 
    #endregion
}
