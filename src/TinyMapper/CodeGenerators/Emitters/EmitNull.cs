namespace Xunet.ObjectMapper.CodeGenerators.Emitters;

using System;
using System.Reflection.Emit;

internal sealed class EmitNull : IEmitterType
{
    private EmitNull()
    {
        ObjectType = typeof(object);
    }

    public Type ObjectType { get; }

    public void Emit(CodeGenerator generator)
    {
        generator.Emit(OpCodes.Ldnull);
    }

    public static IEmitterType Load()
    {
        return new EmitNull();
    }
}
