using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sirenix.Serialization
{
	// Token: 0x0200002A RID: 42
	internal sealed class DerivedDictionaryFormatter<TDictionary, TKey, TValue> : BaseFormatter<TDictionary> where TDictionary : Dictionary<TKey, TValue>, new()
	{
		// Token: 0x06000254 RID: 596 RVA: 0x000108D4 File Offset: 0x0000EAD4
		static DerivedDictionaryFormatter()
		{
			new DerivedDictionaryFormatter<Dictionary<int, string>, int, string>();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00010948 File Offset: 0x0000EB48
		protected override TDictionary GetUninitializedObject()
		{
			return default(TDictionary);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00010960 File Offset: 0x0000EB60
		protected override void DeserializeImplementation(ref TDictionary value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			IEqualityComparer<TKey> equalityComparer = null;
			if (text == "comparer" || entryType == EntryType.StartOfNode)
			{
				equalityComparer = DerivedDictionaryFormatter<TDictionary, TKey, TValue>.EqualityComparerSerializer.ReadValue(reader);
				entryType = reader.PeekEntry(out text);
			}
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					if (equalityComparer != null && DerivedDictionaryFormatter<TDictionary, TKey, TValue>.ComparerConstructor != null)
					{
						value = (TDictionary)((object)DerivedDictionaryFormatter<TDictionary, TKey, TValue>.ComparerConstructor.Invoke(new object[]
						{
							equalityComparer
						}));
					}
					else
					{
						value = Activator.CreateInstance<TDictionary>();
					}
					base.RegisterReferenceID(value, reader);
					int num2 = 0;
					while ((long)num2 < num)
					{
						if (reader.PeekEntry(out text) == EntryType.EndOfArray)
						{
							reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
							{
								"Reached end of array after ",
								num2.ToString(),
								" elements, when ",
								num.ToString(),
								" elements were expected."
							}));
							break;
						}
						bool flag = true;
						try
						{
							Type type;
							reader.EnterNode(out type);
							TKey tkey = DerivedDictionaryFormatter<TDictionary, TKey, TValue>.KeyReaderWriter.ReadValue(reader);
							TValue tvalue = DerivedDictionaryFormatter<TDictionary, TKey, TValue>.ValueReaderWriter.ReadValue(reader);
							if (!DerivedDictionaryFormatter<TDictionary, TKey, TValue>.KeyIsValueType && tkey == null)
							{
								reader.Context.Config.DebugContext.LogWarning("Dictionary key of type '" + typeof(TKey).FullName + "' was null upon deserialization. A key has gone missing.");
								goto IL_1CB;
							}
							value[tkey] = tvalue;
						}
						catch (SerializationAbortException ex)
						{
							flag = false;
							throw ex;
						}
						catch (Exception exception)
						{
							reader.Context.Config.DebugContext.LogException(exception);
						}
						finally
						{
							if (flag)
							{
								reader.ExitNode();
							}
						}
						goto IL_19C;
						IL_1CB:
						num2++;
						continue;
						IL_19C:
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						goto IL_1CB;
					}
					return;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			reader.SkipEntry();
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00010BBC File Offset: 0x0000EDBC
		protected override void SerializeImplementation(ref TDictionary value, IDataWriter writer)
		{
			try
			{
				if (value.Comparer != null)
				{
					DerivedDictionaryFormatter<TDictionary, TKey, TValue>.EqualityComparerSerializer.WriteValue("comparer", value.Comparer, writer);
				}
				writer.BeginArrayNode((long)value.Count);
				foreach (KeyValuePair<TKey, TValue> keyValuePair in value)
				{
					bool flag = true;
					try
					{
						writer.BeginStructNode(null, null);
						DerivedDictionaryFormatter<TDictionary, TKey, TValue>.KeyReaderWriter.WriteValue("$k", keyValuePair.Key, writer);
						DerivedDictionaryFormatter<TDictionary, TKey, TValue>.ValueReaderWriter.WriteValue("$v", keyValuePair.Value, writer);
					}
					catch (SerializationAbortException ex)
					{
						flag = false;
						throw ex;
					}
					catch (Exception exception)
					{
						writer.Context.Config.DebugContext.LogException(exception);
					}
					finally
					{
						if (flag)
						{
							writer.EndNode(null);
						}
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000A7 RID: 167
		private static readonly bool KeyIsValueType = typeof(TKey).IsValueType;

		// Token: 0x040000A8 RID: 168
		private static readonly Serializer<IEqualityComparer<TKey>> EqualityComparerSerializer = Serializer.Get<IEqualityComparer<TKey>>();

		// Token: 0x040000A9 RID: 169
		private static readonly Serializer<TKey> KeyReaderWriter = Serializer.Get<TKey>();

		// Token: 0x040000AA RID: 170
		private static readonly Serializer<TValue> ValueReaderWriter = Serializer.Get<TValue>();

		// Token: 0x040000AB RID: 171
		private static readonly ConstructorInfo ComparerConstructor = typeof(TDictionary).GetConstructor(new Type[]
		{
			typeof(IEqualityComparer<TKey>)
		});
	}
}
