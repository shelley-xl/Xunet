namespace Xunet.ObjectMapper.Reflection;

using System;
using System.Reflection.Emit;

internal interface IDynamicAssembly
{
    TypeBuilder DefineType(string typeName, Type parentType);
    void Save();
}
