using System;

namespace Sirenix.Serialization
{
	// Token: 0x020000B0 RID: 176
	public interface IOverridesSerializationPolicy
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060004DE RID: 1246
		ISerializationPolicy SerializationPolicy { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060004DF RID: 1247
		bool OdinSerializesUnityFields { get; }
	}
}
