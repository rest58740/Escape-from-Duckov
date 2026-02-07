using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000037 RID: 55
	public struct Sphere3
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0000B353 File Offset: 0x00009553
		public Sphere3(Vector3 center, float radius)
		{
			this.center = center;
			this.radius = radius;
		}

		// Token: 0x0400014F RID: 335
		public Vector3 center;

		// Token: 0x04000150 RID: 336
		public float radius;
	}
}
