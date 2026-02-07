using System;

namespace Pathfinding
{
	// Token: 0x0200012F RID: 303
	public class EndingConditionDistance : PathEndingCondition
	{
		// Token: 0x06000952 RID: 2386 RVA: 0x000334CA File Offset: 0x000316CA
		public EndingConditionDistance(Path p, int maxGScore) : base(p)
		{
			this.maxGScore = maxGScore;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000334E2 File Offset: 0x000316E2
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return G >= (uint)this.maxGScore;
		}

		// Token: 0x04000669 RID: 1641
		public int maxGScore = 100;
	}
}
