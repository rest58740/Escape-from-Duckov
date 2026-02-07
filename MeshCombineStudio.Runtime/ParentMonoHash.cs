using System;

namespace MeshCombineStudio
{
	// Token: 0x02000025 RID: 37
	public abstract class ParentMonoHash<T> : MonoBehaviourFastIndex
	{
		// Token: 0x04000112 RID: 274
		[NonSerialized]
		public T parent;
	}
}
