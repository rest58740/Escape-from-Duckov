using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000022 RID: 34
	public sealed class WeakArrayFormatter : WeakBaseFormatter
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		public WeakArrayFormatter(Type arrayType, Type elementType) : base(arrayType)
		{
			this.ValueReaderWriter = Serializer.Get(elementType);
			this.ElementType = elementType;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				long num;
				reader.EnterArray(out num);
				Array array = Array.CreateInstance(this.ElementType, new long[]
				{
					num
				});
				value = array;
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
					array.SetValue(this.ValueReaderWriter.ReadValueWeak(reader), num2);
					if (reader.PeekEntry(out text) == EntryType.EndOfStream)
					{
						break;
					}
					num2++;
				}
				reader.ExitArray();
				return;
			}
			reader.SkipEntry();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			Array array = (Array)value;
			try
			{
				int length = array.Length;
				writer.BeginArrayNode((long)length);
				for (int i = 0; i < length; i++)
				{
					this.ValueReaderWriter.WriteValueWeak(array.GetValue(i), writer);
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x0400008D RID: 141
		private readonly Serializer ValueReaderWriter;

		// Token: 0x0400008E RID: 142
		private readonly Type ElementType;
	}
}
