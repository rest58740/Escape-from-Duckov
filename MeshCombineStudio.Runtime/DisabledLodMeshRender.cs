using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000031 RID: 49
	public class DisabledLodMeshRender : MonoBehaviour
	{
		// Token: 0x04000136 RID: 310
		[HideInInspector]
		public MeshCombiner meshCombiner;

		// Token: 0x04000137 RID: 311
		public CachedLodGameObject cachedLodGO;
	}
}
