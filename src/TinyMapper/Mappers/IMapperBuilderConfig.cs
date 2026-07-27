namespace Xunet.ObjectMapper.Mappers;

using System;
using Xunet.ObjectMapper.Bindings;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Mappers.Classes.Members;
using Xunet.ObjectMapper.Reflection;

internal interface IMapperBuilderConfig
{
    IDynamicAssembly Assembly { get; }
    Func<string, string, bool> NameMatching { get; }
    Option<BindingConfig> GetBindingConfig(TypePair typePair);
    MapperBuilder GetMapperBuilder(TypePair typePair);
    MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
}
