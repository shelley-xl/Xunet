using System;
using System.Collections;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Xunet.Helpers;

/// <summary>
/// 机器辅助类
/// </summary>
public class MachineHelper
{
    /// <summary>
    /// 获取当前机器的本机IP地址
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    /// <summary>
    /// 获取当前机器的公网IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetPublicIPAddress()
    {
        return "http://ip.42.pl/raw".HttpGet();
    }

    /// <summary>
    /// 获取当前机器的机器码
    /// 生成规则：MD5(MAC地址 + CPU序列号 + 硬盘ID)
    /// </summary>
    /// <param name="md5">是否生成MD5</param>
    /// <returns></returns>
    public static string GetMachineCode()
    {
        var mac = GetMacAddress();
        var cpuId = GetCpuId();
        var diskId = GetDiskId();
        var machineCode = $"{mac}{cpuId}{diskId}";
        return machineCode.ToMD5Encrypt(true);
    }

    /// <summary>
    /// 获取当前机器的注册码
    /// 生成规则：AES(机器码 + 过期时间戳 + 注册时间戳)
    /// </summary>
    /// <param name="key">32位秘钥</param>
    /// <param name="expireDays">过期天数</param>
    /// <returns></returns>
    public static string GetRegisterCode(string key, string machineCode, int expireDays)
    {
        var now = GetNetDateTime();
        var expireTime = now.AddDays(expireDays).ToTimeStamp();
        var registerTime = now.ToTimeStamp();
        var registerCode = $"{machineCode}&{expireTime}&{registerTime}";
        return registerCode.ToAESEncrypt(key);
    }

    /// <summary>
    /// 验证注册码是否有效
    /// </summary>
    /// <param name="key">32位秘钥</param>
    /// <param name="registerCode">注册码</param>
    /// <returns></returns>
    public static bool VerifyRegisterCode(string key, string registerCode)
    {
        var now = GetNetDateTime();
        var machineCode = GetMachineCode();
        var code = registerCode.ToAESDecrypt(key);
        var codeArray = code.Split('&');
        if (codeArray.Length != 3) return false;
        var codeMachineCode = codeArray[0];
        var codeExpireTime = codeArray[1];
        var codeRegisterTime = codeArray[2];
        if (machineCode != codeMachineCode) return false;
        var expireTime = long.Parse(codeExpireTime);
        var registerTime = long.Parse(codeRegisterTime);
        if (now.ToTimeStamp() > expireTime) return false;
        if (registerTime > now.ToTimeStamp()) return false;
        return true;
    }

    /// <summary>
    /// 获取随机端口号
    /// </summary>
    /// <param name="maxRetryTimes">最大重试次数</param>
    /// <returns></returns>
    public static int GetRandomPort(int maxRetryTimes = 1000)
    {
        var used = GetUsedPort();
        var rand = new Random();
        int times = 0;
        while (true)
        {
            var port = rand.Next(1024, 65535);
            if (!used.Contains(port)) return port;
            times++;
            if (times > maxRetryTimes) return -1;
        }
    }

    /// <summary>
    /// 检查端口号是否被占用
    /// </summary>
    /// <param name="port">端口号</param>
    /// <returns></returns>
    public static bool IsPortInUse(int port)
    {
        return GetUsedPort().Contains(port);
    }

    /// <summary>
    /// 获取当前机器的MAC地址
    /// </summary>
    /// <returns></returns>
    static string GetMacAddress()
    {
        var mac = "";
        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                mac += nic.GetPhysicalAddress().ToString();
                break;
            }
        }
        return mac;
    }

    /// <summary>
    /// 获取当前机器的硬盘ID
    /// </summary>
    /// <returns></returns>
    static string GetDiskId()
    {
        var diskId = "";
        var mc = new ManagementClass("Win32_DiskDrive");
        var moc = mc.GetInstances();
        foreach (var o in moc)
        {
            if (o is ManagementObject)
            {
                var mo = o as ManagementObject;
                diskId = (string)mo.Properties["Model"].Value;
            }
        }
        return diskId;
    }

    /// <summary>
    /// 获取当前机器的CPU序列号
    /// </summary>
    /// <returns></returns>
    static string GetCpuId()
    {
        var cpuId = "";
        var mc = new ManagementClass("Win32_Processor");
        var moc = mc.GetInstances();
        foreach (var o in moc)
        {
            if (o is ManagementObject)
            {
                var mo = o as ManagementObject;
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
            }
        }
        return cpuId;
    }

    /// <summary>
    /// 获取本机所有占用端口
    /// </summary>
    /// <returns></returns>
    static IList GetUsedPort()
    {
        //获取本地计算机的网络连接和通信统计数据的信息
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

        //返回本地计算机上的所有Tcp监听程序
        IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

        //返回本地计算机上的所有UDP监听程序
        IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

        //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        // 显示本地计算机上的所有端口
        IList allPorts = new ArrayList();

        // 添加TCP端口
        foreach (IPEndPoint ep in ipsTCP)
        {
            allPorts.Add(ep.Port);
        }

        // 添加UDP端口
        foreach (IPEndPoint ep in ipsUDP)
        {
            allPorts.Add(ep.Port);
        }

        // 添加TCP连接端口
        foreach (TcpConnectionInformation conn in tcpConnInfoArray)
        {
            allPorts.Add(conn.LocalEndPoint.Port);
        }

        return allPorts;
    }

    /// <summary>
    /// 获取网络时间
    /// </summary>
    /// <returns></returns>
    public static DateTime GetNetDateTime()
    {
        try
        {
            var url = "http://www.baidu.com";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 3000;
            var response = (HttpWebResponse)request.GetResponse();
            var date = response.Headers["date"];
            return DateTime.Parse(date);
        }
        catch
        {
            throw new Exception("网络错误，请检查网络");
        }
    }
}
