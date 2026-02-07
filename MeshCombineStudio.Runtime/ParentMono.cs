using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000024 RID: 36
	public abstract class ParentMono<T> : MonoBehaviour
	{
		// Token: 0x04000111 RID: 273
		[NonSerialized]
		public T parent;
	}
}
