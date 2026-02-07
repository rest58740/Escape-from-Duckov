using System;
using System.Diagnostics;

namespace Sirenix.Serialization
{
	// Token: 0x0200008A RID: 138
	public abstract class Serializer<T> : Serializer
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0001E9A7 File Offset: 0x0001CBA7
		public override object ReadValueWeak(IDataReader reader)
		{
			return this.ReadValue(reader);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001E9B5 File Offset: 0x0001CBB5
		public override void WriteValueWeak(string name, object value, IDataWriter writer)
		{
			this.WriteValue(name, (T)((object)value), writer);
		}

		// Token: 0x06000449 RID: 1097
		public abstract T ReadValue(IDataReader reader);

		// Token: 0x0600044A RID: 1098 RVA: 0x0001E9C5 File Offset: 0x0001CBC5
		public void WriteValue(T value, IDataWriter writer)
		{
			this.WriteValue(null, value, writer);
		}

		// Token: 0x0600044B RID: 1099
		public abstract void WriteValue(string name, T value, IDataWriter writer);

		// Token: 0x0600044C RID: 1100 RVA: 0x000021B8 File Offset: 0x000003B8
		[Conditional("UNITY_EDITOR")]
		protected static void FireOnSerializedType()
		{
		}
	}
}
