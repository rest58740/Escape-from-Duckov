using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000015 RID: 21
	[NullableContext(2)]
	[Nullable(0)]
	internal class Vertex
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00005791 File Offset: 0x00003991
		public Vertex(Point64 pt, VertexFlags flags, Vertex prev)
		{
			this.pt = pt;
			this.flags = flags;
			this.next = null;
			this.prev = prev;
		}

		// Token: 0x04000036 RID: 54
		public Point64 pt;

		// Token: 0x04000037 RID: 55
		public Vertex next;

		// Token: 0x04000038 RID: 56
		public Vertex prev;

		// Token: 0x04000039 RID: 57
		public VertexFlags flags;
	}
}
