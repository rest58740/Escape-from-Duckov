using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000139 RID: 313
	public class EndingConditionProximity : ABPathEndingCondition
	{
		// Token: 0x06000994 RID: 2452 RVA: 0x00034874 File Offset: 0x00032A74
		public EndingConditionProximity(ABPath p, float maxDistance) : base(p)
		{
			this.maxDistance = maxDistance;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00034890 File Offset: 0x00032A90
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return ((Vector3)node.position - this.abPath.originalEndPoint).sqrMagnitude <= this.maxDistance * this.maxDistance;
		}

		// Token: 0x0400068B RID: 1675
		public float maxDistance = 10f;
	}
}
