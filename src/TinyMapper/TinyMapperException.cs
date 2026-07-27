namespace Xunet.ObjectMapper;

using System;
#if !NET6_0_OR_GREATER
using System.Runtime.Serialization;
#endif

/// <summary>
///     Exception during mapping or binding
/// </summary>
#if !NET6_0_OR_GREATER
[Serializable]
#endif
public class TinyMapperException : Exception
{
    public TinyMapperException()
    {
    }

    public TinyMapperException(string message) : base(message)
    {
    }

    public TinyMapperException(string message, Exception innerException) : base(message, innerException)
    {
    }
#if !NET6_0_OR_GREATER
    protected TinyMapperException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
#endif
}
