using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200001F RID: 31
	[NullableContext(2)]
	[Nullable(0)]
	internal class Active : IDisposable
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x000058D0 File Offset: 0x00003AD0
		public void Dispose()
		{
			this.bot = default(Point64);
			this.top = default(Point64);
			this.curX = 0L;
			this.dx = 0.0;
			this.windDx = 0;
			this.windCount = 0;
			this.windCount2 = 0;
			this.outrec = null;
			this.prevInAEL = null;
			this.nextInAEL = null;
			this.prevInSEL = null;
			this.nextInSEL = null;
			this.jump = null;
			this.vertexTop = null;
			this.localMin = default(LocalMinima);
			this.isLeftBound = false;
			this.joinWith = JoinWith.None;
		}

		// Token: 0x0400005D RID: 93
		public Point64 bot;

		// Token: 0x0400005E RID: 94
		public Point64 top;

		// Token: 0x0400005F RID: 95
		public long curX;

		// Token: 0x04000060 RID: 96
		public double dx;

		// Token: 0x04000061 RID: 97
		public int windDx;

		// Token: 0x04000062 RID: 98
		public int windCount;

		// Token: 0x04000063 RID: 99
		public int windCount2;

		// Token: 0x04000064 RID: 100
		public OutRec outrec;

		// Token: 0x04000065 RID: 101
		public Active prevInAEL;

		// Token: 0x04000066 RID: 102
		public Active nextInAEL;

		// Token: 0x04000067 RID: 103
		public Active prevInSEL;

		// Token: 0x04000068 RID: 104
		public Active nextInSEL;

		// Token: 0x04000069 RID: 105
		public Active jump;

		// Token: 0x0400006A RID: 106
		public Vertex vertexTop;

		// Token: 0x0400006B RID: 107
		public LocalMinima localMin;

		// Token: 0x0400006C RID: 108
		internal bool isLeftBound;

		// Token: 0x0400006D RID: 109
		internal JoinWith joinWith;
	}
}
