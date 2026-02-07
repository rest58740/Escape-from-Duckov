using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000021 RID: 33
	public sealed class ArrayFormatter<T> : BaseFormatter<T[]>
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override T[] GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000EE70 File Offset: 0x0000D070
		protected override void DeserializeImplementation(ref T[] value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				long num;
				reader.EnterArray(out num);
				value = new T[num];
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
					value[num2] = ArrayFormatter<T>.valueReaderWriter.ReadValue(reader);
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

		// Token: 0x06000221 RID: 545 RVA: 0x0000EF40 File Offset: 0x0000D140
		protected override void SerializeImplementation(ref T[] value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					ArrayFormatter<T>.valueReaderWriter.WriteValue(value[i], writer);
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x0400008C RID: 140
		private static Serializer<T> valueReaderWriter = Serializer.Get<T>();
	}
}
