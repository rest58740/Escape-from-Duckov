using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000132 RID: 306
	public class FloodPathConstraint : NNConstraint
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x000337DC File Offset: 0x000319DC
		public FloodPathConstraint(FloodPath path)
		{
			if (path == null)
			{
				Debug.LogWarning("FloodPathConstraint should not be used with a NULL path");
			}
			this.path = path;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000337F8 File Offset: 0x000319F8
		public override bool Suitable(GraphNode node)
		{
			return base.Suitable(node) && this.path.HasPathTo(node);
		}

		// Token: 0x04000671 RID: 1649
		private readonly FloodPath path;
	}
}
