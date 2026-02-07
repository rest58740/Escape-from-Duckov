using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200001E RID: 30
	[NullableContext(2)]
	[Nullable(0)]
	internal class HorzJoin
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000058B8 File Offset: 0x00003AB8
		[NullableContext(1)]
		public HorzJoin(OutPt ltor, OutPt rtol)
		{
			this.op1 = ltor;
			this.op2 = rtol;
		}

		// Token: 0x0400005B RID: 91
		public OutPt op1;

		// Token: 0x0400005C RID: 92
		public OutPt op2;
	}
}
