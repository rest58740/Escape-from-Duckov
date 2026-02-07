using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct IntersectNode
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x0000581F File Offset: 0x00003A1F
		public IntersectNode(Point64 pt, Active edge1, Active edge2)
		{
			this.pt = pt;
			this.edge1 = edge1;
			this.edge2 = edge2;
		}

		// Token: 0x0400003D RID: 61
		public readonly Point64 pt;

		// Token: 0x0400003E RID: 62
		public readonly Active edge1;

		// Token: 0x0400003F RID: 63
		public readonly Active edge2;
	}
}
