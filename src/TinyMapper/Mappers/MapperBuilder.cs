namespace Xunet.ObjectMapper.Mappers;

using System;
using System.Reflection;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Mappers.Classes.Members;
using Xunet.ObjectMapper.Reflection;

internal abstract class MapperBuilder
{
    protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
    private const string AssemblyName = "DynamicTinyMapper";
    protected readonly IDynamicAssembly _assembly;
    protected readonly IMapperBuilderConfig _config;

    protected MapperBuilder(IMapperBuilderConfig config)
    {
        _config = config;
        _assembly = config.Assembly;
    }

    protected abstract string ScopeName { get; }

    public Mapper Build(TypePair typePair)
    {
        return BuildCore(typePair);
    }

    public Mapper Build(TypePair parentTypePair, MappingMember mappingMember)
    {
        return BuildCore(parentTypePair, mappingMember);
    }

    public bool IsSupported(TypePair typePair)
    {
        return IsSupportedCore(typePair);
    }

    protected abstract Mapper BuildCore(TypePair typePair);
    protected abstract Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember);

    protected MapperBuilder GetMapperBuilder(TypePair typePair)
    {
        return _config.GetMapperBuilder(typePair);
    }

    protected string GetMapperFullName()
    {
        string random = Guid.NewGuid().ToString("N");
        return $"{AssemblyName}.{ScopeName}.Mapper{random}";
    }

    protected abstract bool IsSupportedCore(TypePair typePair);
}
