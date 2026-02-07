using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000051 RID: 81
	public class StackFormatter<TStack, TValue> : BaseFormatter<TStack> where TStack : Stack<TValue>, new()
	{
		// Token: 0x06000307 RID: 775 RVA: 0x000163C8 File Offset: 0x000145C8
		static StackFormatter()
		{
			new StackFormatter<Stack<int>, int>();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00016400 File Offset: 0x00014600
		protected override TStack GetUninitializedObject()
		{
			return default(TStack);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00016418 File Offset: 0x00014618
		protected override void DeserializeImplementation(ref TStack value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					if (StackFormatter<TStack, TValue>.IsPlainStack)
					{
						value = (TStack)((object)new Stack<TValue>((int)num));
					}
					else
					{
						value = Activator.CreateInstance<TStack>();
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
						TStack tstack = value;
						tstack.Push(StackFormatter<TStack, TValue>.TSerializer.ReadValue(reader));
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

		// Token: 0x0600030B RID: 779 RVA: 0x00016550 File Offset: 0x00014750
		protected override void SerializeImplementation(ref TStack value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				using (Cache<List<TValue>> cache = Cache<List<TValue>>.Claim())
				{
					List<TValue> value2 = cache.Value;
					value2.Clear();
					foreach (TValue tvalue in value)
					{
						value2.Add(tvalue);
					}
					for (int i = value2.Count - 1; i >= 0; i--)
					{
						try
						{
							StackFormatter<TStack, TValue>.TSerializer.WriteValue(value2[i], writer);
						}
						catch (Exception exception)
						{
							writer.Context.Config.DebugContext.LogException(exception);
						}
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000F3 RID: 243
		private static readonly Serializer<TValue> TSerializer = Serializer.Get<TValue>();

		// Token: 0x040000F4 RID: 244
		private static readonly bool IsPlainStack = typeof(TStack) == typeof(Stack<TValue>);
	}
}
