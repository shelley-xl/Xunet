#if NET6_0_OR_GREATER

using System;
using System.Net;

namespace Xunet.IP2Region;

public static class Util
{
    public static uint IpAddressToUInt32(string ipAddress)
    {
        var address = IPAddress.Parse(ipAddress);
        return IpAddressToUInt32(address);
    }
    
    public static uint IpAddressToUInt32(IPAddress ipAddress)
    {
        byte[] bytes = ipAddress.GetAddressBytes();
        Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public static uint GetMidIp(uint x, uint y)
        => (x & y) + ((x ^ y) >> 1);

    public static int GetMidIp(int x, int y)
        => (x & y) + ((x ^ y) >> 1);
}

#endif