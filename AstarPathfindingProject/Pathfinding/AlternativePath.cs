using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000111 RID: 273
	[AddComponentMenu("Pathfinding/Modifiers/Alternative Path Modifier")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/alternativepath.html")]
	[Serializable]
	public class AlternativePath : MonoModifier
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0002E5AD File Offset: 0x0002C7AD
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002E5B1 File Offset: 0x0002C7B1
		public override void Apply(Path p)
		{
			if (this == null)
			{
				return;
			}
			this.ApplyNow(p.path);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002E5C9 File Offset: 0x0002C7C9
		protected void OnDestroy()
		{
			this.destroyed = true;
			this.ClearOnDestroy();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002E5D8 File Offset: 0x0002C7D8
		private void ClearOnDestroy()
		{
			this.InversePrevious();
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002E5E0 File Offset: 0x0002C7E0
		private void InversePrevious()
		{
			if (this.prevNodes != null)
			{
				bool flag = false;
				for (int i = 0; i < this.prevNodes.Count; i++)
				{
					if ((ulong)this.prevNodes[i].Penalty < (ulong)((long)this.prevPenalty))
					{
						flag = true;
						this.prevNodes[i].Penalty = 0U;
					}
					else
					{
						this.prevNodes[i].Penalty = (uint)((ulong)this.prevNodes[i].Penalty - (ulong)((long)this.prevPenalty));
					}
				}
				if (flag)
				{
					Debug.LogWarning("Penalty for some nodes has been reset while the AlternativePath modifier was active (possibly because of a graph update). Some penalties might be incorrect (they may be lower than expected for the affected nodes)");
				}
			}
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0002E67C File Offset: 0x0002C87C
		private void ApplyNow(List<GraphNode> nodes)
		{
			this.InversePrevious();
			this.prevNodes.Clear();
			if (this.destroyed)
			{
				return;
			}
			if (nodes != null)
			{
				for (int i = this.rnd.Next(this.randomStep); i < nodes.Count; i += this.rnd.Next(1, this.randomStep))
				{
					nodes[i].Penalty = (uint)((ulong)nodes[i].Penalty + (ulong)((long)this.penalty));
					this.prevNodes.Add(nodes[i]);
				}
			}
			this.prevPenalty = this.penalty;
		}

		// Token: 0x040005C0 RID: 1472
		public int penalty = 1000;

		// Token: 0x040005C1 RID: 1473
		public int randomStep = 10;

		// Token: 0x040005C2 RID: 1474
		private List<GraphNode> prevNodes = new List<GraphNode>();

		// Token: 0x040005C3 RID: 1475
		private int prevPenalty;

		// Token: 0x040005C4 RID: 1476
		private readonly System.Random rnd = new System.Random();

		// Token: 0x040005C5 RID: 1477
		private bool destroyed;
	}
}
