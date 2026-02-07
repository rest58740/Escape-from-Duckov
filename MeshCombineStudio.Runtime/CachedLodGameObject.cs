using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public class CachedLodGameObject : CachedGameObject
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000E900 File Offset: 0x0000CB00
		public CachedLodGameObject(CachedGameObject cachedGO, int lodCount, int lodLevel) : base(cachedGO.searchParentT, cachedGO.go, cachedGO.t, cachedGO.mr, cachedGO.mf, cachedGO.mesh)
		{
			this.lodCount = lodCount;
			this.lodLevel = lodLevel;
		}

		// Token: 0x0400020A RID: 522
		public Vector3 center;

		// Token: 0x0400020B RID: 523
		public int lodCount;

		// Token: 0x0400020C RID: 524
		public int lodLevel;
	}
}
