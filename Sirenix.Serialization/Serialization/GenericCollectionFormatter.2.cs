using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x02000034 RID: 52
	public sealed class GenericCollectionFormatter<TCollection, TElement> : BaseFormatter<TCollection> where TCollection : ICollection<TElement>, new()
	{
		// Token: 0x0600027D RID: 637 RVA: 0x00012B38 File Offset: 0x00010D38
		static GenericCollectionFormatter()
		{
			Type type;
			if (!GenericCollectionFormatter.CanFormat(typeof(TCollection), out type))
			{
				throw new ArgumentException("Cannot treat the type " + typeof(TCollection).Name + " as a generic collection.");
			}
			if (type != typeof(TElement))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Type ",
					typeof(TElement).Name,
					" is not the element type of the generic collection type ",
					typeof(TCollection).Name,
					"."
				}));
			}
			new GenericCollectionFormatter<List<int>, int>();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00012BF3 File Offset: 0x00010DF3
		protected override TCollection GetUninitializedObject()
		{
			return Activator.CreateInstance<TCollection>();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00012BFC File Offset: 0x00010DFC
		protected override void DeserializeImplementation(ref TCollection value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
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
						try
						{
							ref TCollection ptr = ref value;
							if (default(TCollection) == null)
							{
								TCollection tcollection = value;
								ptr = ref tcollection;
							}
							ptr.Add(GenericCollectionFormatter<TCollection, TElement>.valueReaderWriter.ReadValue(reader));
						}
						catch (Exception exception)
						{
							reader.Context.Config.DebugContext.LogException(exception);
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
				catch (Exception exception2)
				{
					reader.Context.Config.DebugContext.LogException(exception2);
					return;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			reader.SkipEntry();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00012D88 File Offset: 0x00010F88
		protected override void SerializeImplementation(ref TCollection value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				foreach (TElement value2 in value)
				{
					GenericCollectionFormatter<TCollection, TElement>.valueReaderWriter.WriteValue(value2, writer);
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000C4 RID: 196
		private static Serializer<TElement> valueReaderWriter = Serializer.Get<TElement>();
	}
}
