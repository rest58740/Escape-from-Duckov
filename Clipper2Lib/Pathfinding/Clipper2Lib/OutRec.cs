using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200001C RID: 28
	[NullableContext(2)]
	[Nullable(0)]
	internal class OutRec
	{
		// Token: 0x0400004D RID: 77
		public int idx;

		// Token: 0x0400004E RID: 78
		public OutRec owner;

		// Token: 0x0400004F RID: 79
		public Active frontEdge;

		// Token: 0x04000050 RID: 80
		public Active backEdge;

		// Token: 0x04000051 RID: 81
		public OutPt pts;

		// Token: 0x04000052 RID: 82
		public PolyPathBase polypath;

		// Token: 0x04000053 RID: 83
		public Rect64 bounds;

		// Token: 0x04000054 RID: 84
		[Nullable(1)]
		public List<Point64> path = new List<Point64>();

		// Token: 0x04000055 RID: 85
		public bool isOpen;

		// Token: 0x04000056 RID: 86
		public List<int> splits;

		// Token: 0x04000057 RID: 87
		public OutRec recursiveSplit;
	}
}
