using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class MeshObject
	{
		// Token: 0x06000194 RID: 404 RVA: 0x0000E764 File Offset: 0x0000C964
		public MeshObject(CachedGameObject cachedGO, int subMeshIndex)
		{
			this.cachedGO = cachedGO;
			this.subMeshIndex = subMeshIndex;
			Transform t = cachedGO.t;
			this.position = t.position;
			this.rotation = t.rotation;
			this.scale = t.lossyScale;
			this.lightmapScaleOffset = cachedGO.mr.lightmapScaleOffset;
		}

		// Token: 0x040001F1 RID: 497
		public CachedGameObject cachedGO;

		// Token: 0x040001F2 RID: 498
		public MeshCache meshCache;

		// Token: 0x040001F3 RID: 499
		public int subMeshIndex;

		// Token: 0x040001F4 RID: 500
		public Vector3 position;

		// Token: 0x040001F5 RID: 501
		public Vector3 scale;

		// Token: 0x040001F6 RID: 502
		public Quaternion rotation;

		// Token: 0x040001F7 RID: 503
		public Vector4 lightmapScaleOffset;

		// Token: 0x040001F8 RID: 504
		public bool intersectsSurface;

		// Token: 0x040001F9 RID: 505
		public int startNewTriangleIndex;

		// Token: 0x040001FA RID: 506
		public int newTriangleCount;

		// Token: 0x040001FB RID: 507
		public bool skip;
	}
}
