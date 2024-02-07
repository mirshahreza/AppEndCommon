﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Caching.Memory;

public static class ExtensionsForMemory
{

	private static readonly Lazy<Func<MemoryCache, object>> GetCoherentState =
		new Lazy<Func<MemoryCache, object>>(() =>
			CreateGetter<MemoryCache, object>(typeof(MemoryCache)
				.GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)));

	private static readonly Lazy<Func<object, IDictionary>> GetEntriesLocal =
		new Lazy<Func<object, IDictionary>>(() =>
			CreateGetter<object, IDictionary>(typeof(MemoryCache)
				.GetNestedType("CoherentState", BindingFlags.NonPublic)
				.GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance)));

	private static Func<TParam, TReturn> CreateGetter<TParam, TReturn>(FieldInfo field)
	{
		var methodName = $"{field.ReflectedType?.FullName}.get_{field.Name}";
		var method = new DynamicMethod(methodName, typeof(TReturn), [typeof(TParam)], typeof(TParam), true);
		var ilGen = method.GetILGenerator();
		ilGen.Emit(OpCodes.Ldarg_0);
		ilGen.Emit(OpCodes.Ldfld, field);
		ilGen.Emit(OpCodes.Ret);
		return (Func<TParam, TReturn>)method.CreateDelegate(typeof(Func<TParam, TReturn>));
	}

	private static readonly Func<MemoryCache, IDictionary> GetEntries = cache => GetEntriesLocal.Value(GetCoherentState.Value(cache));

	public static ICollection GetKeys(this IMemoryCache memoryCache) => GetEntries((MemoryCache)memoryCache).Keys;
	public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) => memoryCache.GetKeys().OfType<T>();
}