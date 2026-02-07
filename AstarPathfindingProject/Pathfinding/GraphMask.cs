using System;

namespace Pathfinding
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public struct GraphMask
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A896 File Offset: 0x00008A96
		public static GraphMask everything
		{
			get
			{
				return new GraphMask(-1);
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A89E File Offset: 0x00008A9E
		public GraphMask(int value)
		{
			this.value = value;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		public static implicit operator int(GraphMask mask)
		{
			return mask.value;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A8AF File Offset: 0x00008AAF
		public static implicit operator GraphMask(int mask)
		{
			return new GraphMask(mask);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A8B7 File Offset: 0x00008AB7
		public static GraphMask operator &(GraphMask lhs, GraphMask rhs)
		{
			return new GraphMask(lhs.value & rhs.value);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A8CB File Offset: 0x00008ACB
		public static GraphMask operator |(GraphMask lhs, GraphMask rhs)
		{
			return new GraphMask(lhs.value | rhs.value);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A8DF File Offset: 0x00008ADF
		public static GraphMask operator ~(GraphMask lhs)
		{
			return new GraphMask(~lhs.value);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A8ED File Offset: 0x00008AED
		public bool Contains(int graphIndex)
		{
			return (this.value >> graphIndex & 1) != 0;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A8FF File Offset: 0x00008AFF
		public static GraphMask FromGraph(NavGraph graph)
		{
			return 1 << (int)graph.graphIndex;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A911 File Offset: 0x00008B11
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A91E File Offset: 0x00008B1E
		public static GraphMask FromGraphIndex(uint graphIndex)
		{
			return new GraphMask(1 << (int)graphIndex);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A92C File Offset: 0x00008B2C
		public static GraphMask FromGraphName(string graphName)
		{
			NavGraph navGraph = AstarPath.active.data.FindGraph((NavGraph g) => g.name == graphName);
			if (navGraph == null)
			{
				throw new ArgumentException("Could not find any graph with the name '" + graphName + "'");
			}
			return GraphMask.FromGraph(navGraph);
		}

		// Token: 0x04000176 RID: 374
		public int value;
	}
}
