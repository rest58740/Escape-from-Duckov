using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x0200003A RID: 58
	public sealed class KeyValuePairFormatter<TKey, TValue> : BaseFormatter<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06000294 RID: 660 RVA: 0x00013594 File Offset: 0x00011794
		protected override void SerializeImplementation(ref KeyValuePair<TKey, TValue> value, IDataWriter writer)
		{
			KeyValuePairFormatter<TKey, TValue>.KeySerializer.WriteValue(value.Key, writer);
			KeyValuePairFormatter<TKey, TValue>.ValueSerializer.WriteValue(value.Value, writer);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000135B8 File Offset: 0x000117B8
		protected override void DeserializeImplementation(ref KeyValuePair<TKey, TValue> value, IDataReader reader)
		{
			value = new KeyValuePair<TKey, TValue>(KeyValuePairFormatter<TKey, TValue>.KeySerializer.ReadValue(reader), KeyValuePairFormatter<TKey, TValue>.ValueSerializer.ReadValue(reader));
		}

		// Token: 0x040000CD RID: 205
		private static readonly Serializer<TKey> KeySerializer = Serializer.Get<TKey>();

		// Token: 0x040000CE RID: 206
		private static readonly Serializer<TValue> ValueSerializer = Serializer.Get<TValue>();
	}
}
