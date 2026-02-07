using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000032 RID: 50
	public class DisabledMeshRenderer : MonoBehaviour
	{
		// Token: 0x04000138 RID: 312
		[HideInInspector]
		public MeshCombiner meshCombiner;

		// Token: 0x04000139 RID: 313
		public CachedGameObject cachedGO;
	}
}
