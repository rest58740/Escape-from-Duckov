using System;

namespace Pathfinding
{
	// Token: 0x020000A7 RID: 167
	public interface ITraversalProvider
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001797A File Offset: 0x00015B7A
		bool filterDiagonalGridConnections
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00019DCC File Offset: 0x00017FCC
		bool CanTraverse(Path path, GraphNode node)
		{
			return DefaultITraversalProvider.CanTraverse(path, node);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00019DD5 File Offset: 0x00017FD5
		bool CanTraverse(Path path, GraphNode from, GraphNode to)
		{
			return this.CanTraverse(path, to);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019DDF File Offset: 0x00017FDF
		uint GetTraversalCost(Path path, GraphNode node)
		{
			return DefaultITraversalProvider.GetTraversalCost(path, node);
		}
	}
}
