namespace Xunet.ObjectMapper.CodeGenerators.Emitters;

using System;
using System.Reflection;
using Xunet.ObjectMapper.Core.Extensions;

internal sealed class EmitNewObj : IEmitterType
{
    private EmitNewObj(Type objectType)
    {
        ObjectType = objectType;
    }

    public Type ObjectType { get; }

    public void Emit(CodeGenerator generator)
    {
        ConstructorInfo ctor = ObjectType.GetDefaultCtor();
        generator.EmitNewObject(ctor);
    }

    public static IEmitterType NewObj(Type objectType)
    {
        return new EmitNewObj(objectType);
    }
}
