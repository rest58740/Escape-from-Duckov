using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200002F RID: 47
	[NullableContext(2)]
	[Nullable(0)]
	public class OutPt2
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		public OutPt2(Point64 pt)
		{
			this.pt = pt;
		}

		// Token: 0x040000AD RID: 173
		public OutPt2 next;

		// Token: 0x040000AE RID: 174
		public OutPt2 prev;

		// Token: 0x040000AF RID: 175
		public Point64 pt;

		// Token: 0x040000B0 RID: 176
		public int ownerIdx;

		// Token: 0x040000B1 RID: 177
		public List<OutPt2> edge;
	}
}
