using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000026 RID: 38
	public struct GraphHitInfo
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00009BB4 File Offset: 0x00007DB4
		public readonly float distance
		{
			get
			{
				return (this.point - this.origin).magnitude;
			}
		}

		// Token: 0x0400012F RID: 303
		public Vector3 origin;

		// Token: 0x04000130 RID: 304
		public Vector3 point;

		// Token: 0x04000131 RID: 305
		public GraphNode node;

		// Token: 0x04000132 RID: 306
		public Vector3 tangentOrigin;

		// Token: 0x04000133 RID: 307
		public Vector3 tangent;
	}
}
