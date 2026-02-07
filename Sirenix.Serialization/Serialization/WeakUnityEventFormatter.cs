using System;

namespace Sirenix.Serialization
{
	// Token: 0x020000A9 RID: 169
	public class WeakUnityEventFormatter : WeakReflectionFormatter
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x00021130 File Offset: 0x0001F330
		public WeakUnityEventFormatter(Type serializedType) : base(serializedType)
		{
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00012F28 File Offset: 0x00011128
		protected override object GetUninitializedObject()
		{
			return Activator.CreateInstance(this.SerializedType);
		}
	}
}
