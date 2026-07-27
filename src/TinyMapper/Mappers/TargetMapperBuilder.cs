namespace Xunet.ObjectMapper.Mappers;

using System;
using System.Collections.Generic;
using Xunet.ObjectMapper.Bindings;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Core.Extensions;
using Xunet.ObjectMapper.Mappers.Caches;
using Xunet.ObjectMapper.Mappers.Classes;
using Xunet.ObjectMapper.Mappers.Classes.Members;
using Xunet.ObjectMapper.Mappers.Collections;
using Xunet.ObjectMapper.Mappers.Types.Convertible;
using Xunet.ObjectMapper.Mappers.Types.Custom;
using Xunet.ObjectMapper.Reflection;

internal sealed class TargetMapperBuilder : IMapperBuilderConfig
{
    public static readonly Func<string, string, bool> DefaultNameMatching = (source, target) => string.Equals(source, target, StringComparison.Ordinal);

    private readonly Dictionary<TypePair, BindingConfig> _bindingConfigs = new Dictionary<TypePair, BindingConfig>();
    private readonly ClassMapperBuilder _classMapperBuilder;
    private readonly CollectionMapperBuilder _collectionMapperBuilder;
    private readonly ConvertibleTypeMapperBuilder _convertibleTypeMapperBuilder;
    private readonly CustomTypeMapperBuilder _customTypeMapperBuilder;

    public TargetMapperBuilder(IDynamicAssembly assembly)
    {
        Assembly = assembly;

        var mapperCache = new MapperCache();
        _classMapperBuilder = new ClassMapperBuilder(mapperCache, this);
        _collectionMapperBuilder = new CollectionMapperBuilder(mapperCache, this);
        _convertibleTypeMapperBuilder = new ConvertibleTypeMapperBuilder(this);
        _customTypeMapperBuilder = new CustomTypeMapperBuilder(this);

        NameMatching = DefaultNameMatching;
    }

    public Func<string, string, bool> NameMatching { get; private set; }

    public IDynamicAssembly Assembly { get; }

    public Option<BindingConfig> GetBindingConfig(TypePair typePair)
    {
        Option<BindingConfig> result = _bindingConfigs.GetValue(typePair);
        return result;
    }

    public MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
    {
        if (_customTypeMapperBuilder.IsSupported(parentTypePair, mappingMember))
        {
            return _customTypeMapperBuilder;
        }
        return GetTypeMapperBuilder(mappingMember.TypePair);
    }

    public MapperBuilder GetMapperBuilder(TypePair typePair)
    {
        return GetTypeMapperBuilder(typePair);
    }

    public void SetNameMatching(Func<string, string, bool> nameMatching)
    {
        NameMatching = nameMatching;
    }

    public Mapper Build(TypePair typePair, BindingConfig bindingConfig)
    {
        _bindingConfigs[typePair] = bindingConfig;
        return Build(typePair);
    }

    public Mapper Build(TypePair typePair)
    {
        MapperBuilder mapperBuilder = GetTypeMapperBuilder(typePair);
        Mapper mapper = mapperBuilder.Build(typePair);
        return mapper;
    }

    private MapperBuilder GetTypeMapperBuilder(TypePair typePair)
    {
        if (_convertibleTypeMapperBuilder.IsSupported(typePair))
        {
            return _convertibleTypeMapperBuilder;
        }
        else if (_collectionMapperBuilder.IsSupported(typePair))
        {
            return _collectionMapperBuilder;
        }
        return _classMapperBuilder;
    }
}
