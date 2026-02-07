using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000033 RID: 51
	public interface IRaycastableGraph
	{
		// Token: 0x06000204 RID: 516
		bool Linecast(Vector3 start, Vector3 end);

		// Token: 0x06000205 RID: 517
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint);

		// Token: 0x06000206 RID: 518
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit);

		// Token: 0x06000207 RID: 519
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter = null);

		// Token: 0x06000208 RID: 520
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace = null, Func<GraphNode, bool> filter = null);
	}
}
