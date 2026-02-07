using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000655 RID: 1621
	[Serializable]
	public abstract class SerializationBinder
	{
		// Token: 0x06003CA2 RID: 15522 RVA: 0x000D1B0B File Offset: 0x000CFD0B
		public virtual void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = null;
		}

		// Token: 0x06003CA3 RID: 15523
		public abstract Type BindToType(string assemblyName, string typeName);
	}
}
