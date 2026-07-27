namespace Xunet.ObjectMapper.Core.Extensions;

using System;
using System.Collections.Generic;
using Xunet.ObjectMapper.Core.DataStructures;

internal static class DictionaryExtensions
{
    public static Option<TValue> GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
    {
        TValue result;
        bool exists = value.TryGetValue(key, out result);
        return new Option<TValue>(result, exists);
    }
}
