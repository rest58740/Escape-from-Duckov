using System;

namespace Pathfinding
{
	// Token: 0x02000137 RID: 311
	public abstract class PathEndingCondition
	{
		// Token: 0x0600098F RID: 2447 RVA: 0x000035D0 File Offset: 0x000017D0
		protected PathEndingCondition()
		{
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00034823 File Offset: 0x00032A23
		public PathEndingCondition(Path p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.path = p;
		}

		// Token: 0x06000991 RID: 2449
		public abstract bool TargetFound(GraphNode node, uint H, uint G);

		// Token: 0x04000689 RID: 1673
		protected Path path;
	}
}
