namespace Xunet.ObjectMapper.Mappers.Types.Custom;

using System;
using Xunet.ObjectMapper.Bindings;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Mappers.Classes.Members;

internal sealed class CustomTypeMapperBuilder : MapperBuilder
{
    public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
    {
    }

    protected override string ScopeName => "CustomTypeMapper";

    public bool IsSupported(TypePair parentTypePair, MappingMember mappingMember)
    {
        Option<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
        if (bindingConfig.HasNoValue)
        {
            return false;
        }
        return bindingConfig.Value.HasCustomTypeConverter(mappingMember.Target.Name);
    }

    protected override Mapper BuildCore(TypePair typePair)
    {
        throw new NotSupportedException();
    }

    protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
    {
        Option<BindingConfig> bindingConfig = _config.GetBindingConfig(parentTypePair);
        Func<object, object> converter = bindingConfig.Value.GetCustomTypeConverter(mappingMember.Target.Name).Value;
        return new CustomTypeMapper(converter);
    }

    protected override bool IsSupportedCore(TypePair typePair)
    {
        throw new NotSupportedException();
    }
}
