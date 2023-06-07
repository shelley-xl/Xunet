namespace System;

#region 日期时间扩展类
/// <summary>
/// 日期时间扩展类
/// </summary>
public static class DateTimeExtension
{
    #region 时间戳起始时间
    /// <summary>
    /// 时间戳起始时间
    /// </summary>
    private readonly static DateTime timeStampStartTime = new(1970, 1, 1, 0, 0, 0, 0);
    #endregion

    #region 获取时间戳
    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>毫秒级时间戳</returns>
    public static long ToLongTimeStamp(this DateTime dateTime)
    {
        return Convert.ToInt64((dateTime.ToUniversalTime() - timeStampStartTime).TotalMilliseconds);
    }
    #endregion

    #region 获取时间戳
    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>秒级时间戳</returns>
    public static int ToTimeStamp(this DateTime dateTime)
    {
        return Convert.ToInt32((dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds);
    }
    #endregion

    #region 获取DateTime
    /// <summary>
    /// 获取DateTime
    /// </summary>
    /// <param name="timeStamp">秒级时间戳</param>
    /// <returns>DateTime</returns>
    public static DateTime ToDateTime(this int timeStamp)
    {
        return timeStampStartTime.AddSeconds(timeStamp).ToLocalTime();
    }
    #endregion

    #region 获取DateTime
    /// <summary>
    /// 获取DateTime
    /// </summary>
    /// <param name="longTimeStamp">毫秒级时间戳</param>
    /// <returns>DateTime</returns>
    public static DateTime ToDateTime(this long longTimeStamp)
    {
        return timeStampStartTime.AddMilliseconds(longTimeStamp).ToLocalTime();
    }
    #endregion

    #region 获取标准日期时间格式
    /// <summary>
    /// 获取标准日期时间格式
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>字符串格式：yyyy-MM-dd HH:mm:ss</returns>
    public static string ToStandardDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
    #endregion

    #region 获取标准日期时间格式
    /// <summary>
    /// 获取标准日期时间格式
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>字符串格式：yyyyMMddHHmmss</returns>
    public static string ToStandardDateTimeStringNumber(this DateTime dateTime)
    {
        return dateTime.ToString("yyyyMMddHHmmss");
    }
    #endregion

    #region 获取标准日期时间格式
    /// <summary>
    /// 获取标准日期时间格式
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>字符串格式：yyyy-MM-dd 00:00:00</returns>
    public static string ToStandardDateTimeStringBegin(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd 00:00:00");
    }
    #endregion

    #region 获取标准日期时间格式
    /// <summary>
    /// 获取标准日期时间格式
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>字符串格式：yyyy-MM-dd 23:59:59</returns>
    public static string ToStandardDateTimeStringEnd(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd 23:59:59");
    }
    #endregion

    #region 获取标准时分秒格式
    /// <summary>
    /// 获取标准时分秒格式
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns>00:00:00</returns>
    public static string ToStandardTime(this int seconds)
    {
        var h = seconds / 3600 % 60;
        var m = seconds / 60 % 60;
        var s = seconds % 60;
        return $"{h:00}:{m:00}:{s:00}";
    }
    #endregion

    #region 获取中文星期
    /// <summary>
    /// 获取中文星期
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>中文星期</returns>
    public static string ToChineseWeek(this DateTime dateTime)
    {
        return dateTime.DayOfWeek switch
        {
            DayOfWeek.Sunday => "星期日",
            DayOfWeek.Monday => "星期一",
            DayOfWeek.Tuesday => "星期二",
            DayOfWeek.Wednesday => "星期三",
            DayOfWeek.Thursday => "星期四",
            DayOfWeek.Friday => "星期五",
            DayOfWeek.Saturday => "星期六",
            _ => "",
        };
    }
    #endregion
}
#endregion