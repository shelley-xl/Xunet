namespace Xunet.ObjectMapper.CodeGenerators.Emitters;

using System;

internal static class EmitThis
{
    public static IEmitterType Load(Type thisType)
    {
        return EmitArgument.Load(thisType, 0);
    }
}
