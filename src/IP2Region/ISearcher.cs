#if NET6_0_OR_GREATER

using System.Net;

namespace Xunet.IP2Region;

public interface ISearcher
{
    string? Search(string ipStr);

    string? Search(IPAddress ipAddress);

    string? Search(uint ipAddress);

    int IoCount { get; }
}

#endif