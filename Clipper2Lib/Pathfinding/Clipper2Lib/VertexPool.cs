using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	internal struct VertexPool
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000E363 File Offset: 0x0000C563
		public VertexPool(int capacity)
		{
			this.stack = new Stack<Vertex>(capacity);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000E371 File Offset: 0x0000C571
		public Vertex GetNew(Point64 pt, VertexFlags flags, [Nullable(2)] Vertex prev)
		{
			if (this.stack.Count > 0)
			{
				Vertex vertex = this.stack.Pop();
				vertex.pt = pt;
				vertex.next = null;
				vertex.prev = prev;
				vertex.flags = flags;
				return vertex;
			}
			return new Vertex(pt, flags, prev);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000E3B1 File Offset: 0x0000C5B1
		public void Pool(Vertex v)
		{
			v.prev = null;
			v.next = null;
			this.stack.Push(v);
		}

		// Token: 0x040000BD RID: 189
		private Stack<Vertex> stack;
	}
}
