using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	internal class OutPt
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000585D File Offset: 0x00003A5D
		public OutPt(Point64 pt, OutRec outrec)
		{
			this.pt = pt;
			this.outrec = outrec;
			this.next = this;
			this.prev = this;
			this.horz = null;
		}

		// Token: 0x04000040 RID: 64
		public Point64 pt;

		// Token: 0x04000041 RID: 65
		[Nullable(2)]
		public OutPt next;

		// Token: 0x04000042 RID: 66
		public OutPt prev;

		// Token: 0x04000043 RID: 67
		public OutRec outrec;

		// Token: 0x04000044 RID: 68
		[Nullable(2)]
		public HorzSegment horz;
	}
}
