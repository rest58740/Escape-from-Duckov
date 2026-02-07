using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200007A RID: 122
	public abstract class TwoWaySerializationBinder
	{
		// Token: 0x06000407 RID: 1031
		public abstract string BindToName(Type type, DebugContext debugContext = null);

		// Token: 0x06000408 RID: 1032
		public abstract Type BindToType(string typeName, DebugContext debugContext = null);

		// Token: 0x06000409 RID: 1033
		public abstract bool ContainsType(string typeName);

		// Token: 0x04000164 RID: 356
		public static readonly TwoWaySerializationBinder Default = new DefaultSerializationBinder();
	}
}
