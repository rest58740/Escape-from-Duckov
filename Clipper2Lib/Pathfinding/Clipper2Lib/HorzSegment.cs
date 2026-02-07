using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200001D RID: 29
	[NullableContext(2)]
	[Nullable(0)]
	internal class HorzSegment
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000589B File Offset: 0x00003A9B
		[NullableContext(1)]
		public HorzSegment(OutPt op)
		{
			this.leftOp = op;
			this.rightOp = null;
			this.leftToRight = true;
		}

		// Token: 0x04000058 RID: 88
		public OutPt leftOp;

		// Token: 0x04000059 RID: 89
		public OutPt rightOp;

		// Token: 0x0400005A RID: 90
		public bool leftToRight;
	}
}
