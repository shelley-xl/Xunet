namespace Xunet.ObjectMapper.Reflection;

using System;
using System.Reflection;
using System.Reflection.Emit;

internal class DynamicAssemblyBuilder
{
    internal const string AssemblyName = "DynamicTinyMapper";
#if !NET6_0_OR_GREATER
    //        private const string AssemblyNameFileName = AssemblyName + ".dll";
    //        private static AssemblyBuilder _assemblyBuilder;
#endif
    private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();

    public static IDynamicAssembly Get()
    {
        return _dynamicAssembly;
    }


    private sealed class DynamicAssembly : IDynamicAssembly
    {
        private readonly ModuleBuilder _moduleBuilder;

        public DynamicAssembly()
        {
            var assemblyName = new AssemblyName(AssemblyName);

#if NET6_0_OR_GREATER
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

#else
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//                        _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
//                        _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, AssemblyNameFileName, true);
#endif

        }

        public TypeBuilder DefineType(string typeName, Type parentType)
        {
            return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType, null);
        }

        public void Save()
        {
#if !NET6_0_OR_GREATER
//                _assemblyBuilder.Save(AssemblyNameFileName);
#endif
        }
    }
}
