using System;

namespace MeshCombineStudio
{
	// Token: 0x02000023 RID: 35
	public abstract class ParentFastHashListIndex<T> : FastIndex
	{
		// Token: 0x04000110 RID: 272
		[NonSerialized]
		public T parent;
	}
}
