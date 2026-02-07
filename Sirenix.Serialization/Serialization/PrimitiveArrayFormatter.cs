using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000046 RID: 70
	public sealed class PrimitiveArrayFormatter<T> : MinimalBaseFormatter<T[]> where T : struct
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override T[] GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00014CDC File Offset: 0x00012EDC
		protected override void Read(ref T[] value, IDataReader reader)
		{
			string text;
			if (reader.PeekEntry(out text) == EntryType.PrimitiveArray)
			{
				reader.ReadPrimitiveArray<T>(out value);
				base.RegisterReferenceID(value, reader);
				return;
			}
			reader.SkipEntry();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00014D0D File Offset: 0x00012F0D
		protected override void Write(ref T[] value, IDataWriter writer)
		{
			writer.WritePrimitiveArray<T>(value);
		}
	}
}
