using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x0200002B RID: 43
	public sealed class DictionaryFormatter<TKey, TValue> : BaseFormatter<Dictionary<TKey, TValue>>
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		static DictionaryFormatter()
		{
			new DictionaryFormatter<int, string>();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override Dictionary<TKey, TValue> GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00010D28 File Offset: 0x0000EF28
		protected override void DeserializeImplementation(ref Dictionary<TKey, TValue> value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			IEqualityComparer<TKey> equalityComparer = null;
			if (text == "comparer" || entryType != EntryType.StartOfArray)
			{
				equalityComparer = DictionaryFormatter<TKey, TValue>.EqualityComparerSerializer.ReadValue(reader);
				entryType = reader.PeekEntry(out text);
			}
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = ((equalityComparer == null) ? new Dictionary<TKey, TValue>((int)num) : new Dictionary<TKey, TValue>((int)num, equalityComparer));
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
							TKey tkey = DictionaryFormatter<TKey, TValue>.KeyReaderWriter.ReadValue(reader);
							TValue tvalue = DictionaryFormatter<TKey, TValue>.ValueReaderWriter.ReadValue(reader);
							if (!DictionaryFormatter<TKey, TValue>.KeyIsValueType && tkey == null)
							{
								reader.Context.Config.DebugContext.LogWarning("Dictionary key of type '" + typeof(TKey).FullName + "' was null upon deserialization. A key has gone missing.");
								goto IL_19D;
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
						goto IL_16E;
						IL_19D:
						num2++;
						continue;
						IL_16E:
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						goto IL_19D;
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

		// Token: 0x0600025D RID: 605 RVA: 0x00010F58 File Offset: 0x0000F158
		protected override void SerializeImplementation(ref Dictionary<TKey, TValue> value, IDataWriter writer)
		{
			try
			{
				if (value.Comparer != null)
				{
					DictionaryFormatter<TKey, TValue>.EqualityComparerSerializer.WriteValue("comparer", value.Comparer, writer);
				}
				writer.BeginArrayNode((long)value.Count);
				foreach (KeyValuePair<TKey, TValue> keyValuePair in value)
				{
					bool flag = true;
					try
					{
						writer.BeginStructNode(null, null);
						DictionaryFormatter<TKey, TValue>.KeyReaderWriter.WriteValue("$k", keyValuePair.Key, writer);
						DictionaryFormatter<TKey, TValue>.ValueReaderWriter.WriteValue("$v", keyValuePair.Value, writer);
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

		// Token: 0x040000AC RID: 172
		private static readonly bool KeyIsValueType = typeof(TKey).IsValueType;

		// Token: 0x040000AD RID: 173
		private static readonly Serializer<IEqualityComparer<TKey>> EqualityComparerSerializer = Serializer.Get<IEqualityComparer<TKey>>();

		// Token: 0x040000AE RID: 174
		private static readonly Serializer<TKey> KeyReaderWriter = Serializer.Get<TKey>();

		// Token: 0x040000AF RID: 175
		private static readonly Serializer<TValue> ValueReaderWriter = Serializer.Get<TValue>();
	}
}
