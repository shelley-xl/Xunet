namespace Xunet.ObjectMapper.CodeGenerators.Emitters;

using System;

internal interface IEmitterType : IEmitter
{
    Type ObjectType { get; }
}
