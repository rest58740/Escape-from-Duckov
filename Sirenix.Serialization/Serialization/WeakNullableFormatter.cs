using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000045 RID: 69
	public sealed class WeakNullableFormatter : WeakBaseFormatter
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x00014C60 File Offset: 0x00012E60
		public WeakNullableFormatter(Type nullableType) : base(nullableType)
		{
			Type[] genericArguments = nullableType.GetGenericArguments();
			this.ValueSerializer = Serializer.Get(genericArguments[0]);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00014C8C File Offset: 0x00012E8C
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Null)
			{
				value = null;
				reader.ReadNull();
				return;
			}
			value = this.ValueSerializer.ReadValueWeak(reader);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00014CBF File Offset: 0x00012EBF
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			if (value != null)
			{
				this.ValueSerializer.WriteValueWeak(value, writer);
				return;
			}
			writer.WriteNull(null);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x040000E5 RID: 229
		private readonly Serializer ValueSerializer;
	}
}
