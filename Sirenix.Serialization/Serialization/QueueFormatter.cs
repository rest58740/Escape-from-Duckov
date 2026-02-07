using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x02000048 RID: 72
	public class QueueFormatter<TQueue, TValue> : BaseFormatter<TQueue> where TQueue : Queue<TValue>, new()
	{
		// Token: 0x060002DF RID: 735 RVA: 0x000150E5 File Offset: 0x000132E5
		static QueueFormatter()
		{
			new QueueFormatter<Queue<int>, int>();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00015120 File Offset: 0x00013320
		protected override TQueue GetUninitializedObject()
		{
			return default(TQueue);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00015138 File Offset: 0x00013338
		protected override void DeserializeImplementation(ref TQueue value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					if (QueueFormatter<TQueue, TValue>.IsPlainQueue)
					{
						value = (TQueue)((object)new Queue<TValue>((int)num));
					}
					else
					{
						value = Activator.CreateInstance<TQueue>();
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
						TQueue tqueue = value;
						tqueue.Enqueue(QueueFormatter<TQueue, TValue>.TSerializer.ReadValue(reader));
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

		// Token: 0x060002E3 RID: 739 RVA: 0x00015270 File Offset: 0x00013470
		protected override void SerializeImplementation(ref TQueue value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				foreach (TValue value2 in value)
				{
					try
					{
						QueueFormatter<TQueue, TValue>.TSerializer.WriteValue(value2, writer);
					}
					catch (Exception exception)
					{
						writer.Context.Config.DebugContext.LogException(exception);
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000E9 RID: 233
		private static readonly Serializer<TValue> TSerializer = Serializer.Get<TValue>();

		// Token: 0x040000EA RID: 234
		private static readonly bool IsPlainQueue = typeof(TQueue) == typeof(Queue<TValue>);
	}
}
