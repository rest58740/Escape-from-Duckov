using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000035 RID: 53
	public struct AABB3
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000B14E File Offset: 0x0000934E
		public AABB3(Vector3 min, Vector3 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0400013F RID: 319
		public Vector3 min;

		// Token: 0x04000140 RID: 320
		public Vector3 max;
	}
}
