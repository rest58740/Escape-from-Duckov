using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200004D RID: 77
	public sealed class SelfFormatterFormatter<T> : BaseFormatter<T> where T : ISelfFormatter
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x00015B43 File Offset: 0x00013D43
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			value.Deserialize(reader);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00015B52 File Offset: 0x00013D52
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			value.Serialize(writer);
		}
	}
}
