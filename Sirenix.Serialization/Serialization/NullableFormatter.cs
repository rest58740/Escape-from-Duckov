using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000044 RID: 68
	public sealed class NullableFormatter<T> : BaseFormatter<T?> where T : struct
	{
		// Token: 0x060002CE RID: 718 RVA: 0x00014BDF File Offset: 0x00012DDF
		static NullableFormatter()
		{
			new NullableFormatter<int>();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00014BFC File Offset: 0x00012DFC
		protected override void DeserializeImplementation(ref T? value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Null)
			{
				value = default(T?);
				reader.ReadNull();
				return;
			}
			value = new T?(NullableFormatter<T>.TSerializer.ReadValue(reader));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00014C3B File Offset: 0x00012E3B
		protected override void SerializeImplementation(ref T? value, IDataWriter writer)
		{
			if (value != null)
			{
				NullableFormatter<T>.TSerializer.WriteValue(value.Value, writer);
				return;
			}
			writer.WriteNull(null);
		}

		// Token: 0x040000E4 RID: 228
		private static readonly Serializer<T> TSerializer = Serializer.Get<T>();
	}
}
