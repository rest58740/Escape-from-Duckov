using System;

namespace Pathfinding
{
	// Token: 0x02000138 RID: 312
	public class ABPathEndingCondition : PathEndingCondition
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x00034840 File Offset: 0x00032A40
		public ABPathEndingCondition(ABPath p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.abPath = p;
			this.path = p;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00034864 File Offset: 0x00032A64
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return node == this.abPath.endNode;
		}

		// Token: 0x0400068A RID: 1674
		protected ABPath abPath;
	}
}
