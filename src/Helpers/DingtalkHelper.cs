using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;
using Xunet.Newtonsoft.Json.Linq;

namespace Xunet.Helpers;

/// <summary>
/// 钉钉辅助类
/// </summary>
public static class DingtalkHelper
{
    #region 加签
    private static string Signature(string secret, long timestamp)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        var source = Encoding.UTF8.GetBytes(timestamp + "\n" + secret);
#if NET45_OR_GREATER
        var data = new HMACSHA256(key);
        return System.Net.WebUtility.UrlEncode(Convert.ToBase64String(data.ComputeHash(source)));
#endif
#if NET6_0_OR_GREATER
        var data = HMACSHA256.HashData(key, source);
        return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(data));
#endif
    }
    #endregion

    #region 获取access_token
    /// <summary>
    /// 获取access_token
    /// </summary>
    /// <param name="appkey">appkey</param>
    /// <param name="appsecret">appsecret</param>
    /// <returns></returns>
    public static string GetAccessToken(string appkey, string appsecret)
    {
        var result = $"https://oapi.dingtalk.com/gettoken?appkey={appkey}&appsecret={appsecret}".HttpGet();

        return JObject.Parse(result)["access_token"].ToString();
    }
    #endregion

    #region 发送应用消息
    /// <summary>
    /// 发送应用消息
    /// </summary>
    /// <returns></returns>
    public static string SendMarkdownMessage(SendMarkdownMessageRequest request)
    {
        var obj = new
        {
            agent_id = request.AgentId,
            userid_list = string.Join(",", request.UserIds),
            to_all_user = request.ToAllUser,
            msg = new
            {
                msgtype = "markdown",
                markdown = new
                {
                    title = request.Title,
                    text = $"### {request.Title}\n###### {DateTime.Now:MM月dd日 HH:mm}\n{request.Text}"
                }
            }
        };

        var url = $"https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={request.AccessToken}";

        return url.HttpPost(HttpContentType.ApplicationJson, obj);
    }
    #endregion

    #region 发送群机器人消息
    /// <summary>
    /// 发送群机器人消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string SendMarkdownMessageForGroupRobot(SendMarkdownMessageForGroupRobotRequest request)
    {
        var at_content = new List<string>();
        if (!request.IsAtAll)
        {
            foreach (var userId in request.AtUserIds)
            {
                at_content.Add($"<font color=#4e6ef2>@{userId}</font>");
            }
            foreach (var mobile in request.AtMobiles)
            {
                at_content.Add($"<font color=#4e6ef2>@{mobile}</font>");
            }
        }
        else
        {
            at_content.Add("<font color=#4e6ef2>@所有人</font>");
            request.AtUserIds.Clear();
            request.AtMobiles.Clear();
        }
        var obj = new
        {
            msgtype = "markdown",
            markdown = new
            {
                title = request.Title,
                text = $"{string.Join("", at_content)}\n### {request.Title}\n###### {DateTime.Now:MM月dd日 HH:mm}\n{request.Text}"
            },
            at = new
            {
                atMobiles = request.AtMobiles,
                atUserIds = request.AtUserIds,
                isAtAll = request.IsAtAll
            }
        };
        var timestamp = DateTime.Now.ToLongTimeStamp();
        var sign = Signature(request.Secret, timestamp);
        var url = $"{request.Webhook}&timestamp={timestamp}&sign={sign}";
        return url.HttpPost(HttpContentType.ApplicationJson, obj);
    }
    #endregion
}

#region 发送应用消息请求
/// <summary>
/// 发送应用消息请求
/// </summary>
public class SendMarkdownMessageRequest
{
    /// <summary>
    /// 应用代理Id
    /// </summary>
    public long AgentId { get; set; }
    /// <summary>
    /// 用户UserId
    /// </summary>
    public List<string> UserIds { get; set; }
    /// <summary>
    /// 当设置为false时必须指定UserIds，默认：false
    /// </summary>
    public bool ToAllUser { get; set; } = false;
    /// <summary>
    /// 访问Token
    /// </summary>
    public string AccessToken { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string Text { get; set; }
}

#endregion

#region 发送群机器人消息请求
/// <summary>
/// 发送群机器人消息请求
/// </summary>
public class SendMarkdownMessageForGroupRobotRequest
{
    /// <summary>
    /// Webhook地址
    /// </summary>
    public string Webhook { get; set; }
    /// <summary>
    /// 加签
    /// </summary>
    public string Secret { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// 是否艾特所有人
    /// </summary>
    public bool IsAtAll { get; set; } = false;
    /// <summary>
    /// 艾特用户UserId
    /// </summary>
    public List<string> AtUserIds { get; set; } = new List<string>();
    /// <summary>
    /// 艾特用户手机号
    /// </summary>
    public List<string> AtMobiles { get; set; } = new List<string>();
}

#endregion
