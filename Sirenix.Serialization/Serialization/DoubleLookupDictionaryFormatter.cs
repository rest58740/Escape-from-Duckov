using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200002D RID: 45
	internal sealed class DoubleLookupDictionaryFormatter<TPrimary, TSecondary, TValue> : BaseFormatter<DoubleLookupDictionary<TPrimary, TSecondary, TValue>>
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00011524 File Offset: 0x0000F724
		static DoubleLookupDictionaryFormatter()
		{
			new DoubleLookupDictionaryFormatter<int, int, string>();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override DoubleLookupDictionary<TPrimary, TSecondary, TValue> GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00011548 File Offset: 0x0000F748
		protected override void SerializeImplementation(ref DoubleLookupDictionary<TPrimary, TSecondary, TValue> value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				bool flag = true;
				foreach (KeyValuePair<TPrimary, Dictionary<TSecondary, TValue>> keyValuePair in value)
				{
					try
					{
						writer.BeginStructNode(null, null);
						DoubleLookupDictionaryFormatter<TPrimary, TSecondary, TValue>.PrimaryReaderWriter.WriteValue("$k", keyValuePair.Key, writer);
						DoubleLookupDictionaryFormatter<TPrimary, TSecondary, TValue>.InnerReaderWriter.WriteValue("$v", keyValuePair.Value, writer);
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

		// Token: 0x06000266 RID: 614 RVA: 0x0001163C File Offset: 0x0000F83C
		protected override void DeserializeImplementation(ref DoubleLookupDictionary<TPrimary, TSecondary, TValue> value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = new DoubleLookupDictionary<TPrimary, TSecondary, TValue>();
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
							TPrimary tprimary = DoubleLookupDictionaryFormatter<TPrimary, TSecondary, TValue>.PrimaryReaderWriter.ReadValue(reader);
							Dictionary<TSecondary, TValue> dictionary = DoubleLookupDictionaryFormatter<TPrimary, TSecondary, TValue>.InnerReaderWriter.ReadValue(reader);
							value.Add(tprimary, dictionary);
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
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						num2++;
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

		// Token: 0x040000B9 RID: 185
		private static readonly Serializer<TPrimary> PrimaryReaderWriter = Serializer.Get<TPrimary>();

		// Token: 0x040000BA RID: 186
		private static readonly Serializer<Dictionary<TSecondary, TValue>> InnerReaderWriter = Serializer.Get<Dictionary<TSecondary, TValue>>();
	}
}
