namespace Xunet.ObjectMapper;

using System;
using Xunet.ObjectMapper.Mappers;

internal sealed class TinyMapperConfig : ITinyMapperConfig
{
    private readonly TargetMapperBuilder _targetMapperBuilder;

    public TinyMapperConfig(TargetMapperBuilder targetMapperBuilder)
    {
        _targetMapperBuilder = targetMapperBuilder ?? throw new ArgumentNullException();
    }

    public void NameMatching(Func<string, string, bool> nameMatching)
    {
        if (nameMatching == null)
        {
            throw new ArgumentNullException();
        }
        _targetMapperBuilder.SetNameMatching(nameMatching);
    }

    public void Reset()
    {
        _targetMapperBuilder.SetNameMatching(TargetMapperBuilder.DefaultNameMatching);
    }
}
