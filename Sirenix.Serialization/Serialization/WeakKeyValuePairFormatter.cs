using System;
using System.Reflection;

namespace Sirenix.Serialization
{
	// Token: 0x0200003B RID: 59
	public sealed class WeakKeyValuePairFormatter : WeakBaseFormatter
	{
		// Token: 0x06000298 RID: 664 RVA: 0x000135FC File Offset: 0x000117FC
		public WeakKeyValuePairFormatter(Type serializedType) : base(serializedType)
		{
			Type[] genericArguments = serializedType.GetGenericArguments();
			this.KeySerializer = Serializer.Get(genericArguments[0]);
			this.ValueSerializer = Serializer.Get(genericArguments[1]);
			this.KeyProperty = serializedType.GetProperty("Key");
			this.ValueProperty = serializedType.GetProperty("Value");
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00013655 File Offset: 0x00011855
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			this.KeySerializer.WriteValueWeak(this.KeyProperty.GetValue(value, null), writer);
			this.ValueSerializer.WriteValueWeak(this.ValueProperty.GetValue(value, null), writer);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0001368B File Offset: 0x0001188B
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			value = Activator.CreateInstance(this.SerializedType, new object[]
			{
				this.KeySerializer.ReadValueWeak(reader),
				this.ValueSerializer.ReadValueWeak(reader)
			});
		}

		// Token: 0x040000CF RID: 207
		private readonly Serializer KeySerializer;

		// Token: 0x040000D0 RID: 208
		private readonly Serializer ValueSerializer;

		// Token: 0x040000D1 RID: 209
		private readonly PropertyInfo KeyProperty;

		// Token: 0x040000D2 RID: 210
		private readonly PropertyInfo ValueProperty;
	}
}
