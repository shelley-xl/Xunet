namespace Xunet.ObjectMapper.CodeGenerators.Emitters;

using System;
using System.Reflection.Emit;
using Xunet.ObjectMapper.Core;
using Xunet.ObjectMapper.Core.DataStructures;
using Xunet.ObjectMapper.Core.Extensions;

internal sealed class EmitLocalVariable : IEmitterType
{
    private readonly Option<LocalBuilder> _localBuilder;

    private EmitLocalVariable(LocalBuilder localBuilder)
    {
        _localBuilder = localBuilder.ToOption();
        ObjectType = localBuilder.LocalType;
    }

    public Type ObjectType { get; }

    public void Emit(CodeGenerator generator)
    {
        _localBuilder.Where(x => Helpers.IsValueType(x.LocalType))
                     .Do(x => generator.Emit(OpCodes.Ldloca, x.LocalIndex))
                     .Do(x => generator.Emit(OpCodes.Initobj, x.LocalType));
    }

    public static IEmitterType Declare(LocalBuilder localBuilder)
    {
        return new EmitLocalVariable(localBuilder);
    }
}
