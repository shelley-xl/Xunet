namespace Xunet.ObjectMapper.Mappers.Classes.Members;

using System;
using Xunet.ObjectMapper.CodeGenerators.Emitters;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Core.Extensions;
using Xunet.ObjectMapper.Mappers.Caches;

internal sealed class MemberEmitterDescription
{
    public MemberEmitterDescription(IEmitter emitter, MapperCache mappers)
    {
        Emitter = emitter;
        MapperCache = new Option<MapperCache>(mappers, mappers.IsEmpty);
    }

    public IEmitter Emitter { get; }
    public Option<MapperCache> MapperCache { get; private set; }

    public void AddMapper(MapperCache value)
    {
        MapperCache = value.ToOption();
    }
}
