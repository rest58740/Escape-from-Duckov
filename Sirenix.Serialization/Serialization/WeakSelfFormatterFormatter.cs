using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200004E RID: 78
	public sealed class WeakSelfFormatterFormatter : WeakBaseFormatter
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x00015848 File Offset: 0x00013A48
		public WeakSelfFormatterFormatter(Type serializedType) : base(serializedType)
		{
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00015B69 File Offset: 0x00013D69
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			((ISelfFormatter)value).Deserialize(reader);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00015B78 File Offset: 0x00013D78
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			((ISelfFormatter)value).Serialize(writer);
		}
	}
}
