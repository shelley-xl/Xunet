namespace Xunet.ObjectMapper.Mappers.Classes.Members;

using System;
using System.Reflection;
using Xunet.ObjectMapper.Core.DataStructures;

internal sealed class MappingMember
{
    public MappingMember(MemberInfo source, MemberInfo target, TypePair typePair)
    {
        Source = source;
        Target = target;
        TypePair = typePair;
    }

    public MemberInfo Source { get; }
    public MemberInfo Target { get; }
    public TypePair TypePair { get; }
}
