using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000CA RID: 202
	[JsonOptIn]
	[Preserve]
	public class LinkGraph : NavGraph
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001797A File Offset: 0x00015B7A
		public override bool isScanned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool persistent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool showInInspector
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000223D7 File Offset: 0x000205D7
		public override int CountNodes()
		{
			return this.nodeCount;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000223DF File Offset: 0x000205DF
		protected override void DestroyAllNodes()
		{
			base.DestroyAllNodes();
			this.nodes = new LinkNode[0];
			this.nodeCount = 0;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000223FC File Offset: 0x000205FC
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodeCount; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022434 File Offset: 0x00020634
		internal LinkNode AddNode()
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodeCount >= this.nodes.Length)
			{
				Memory.Realloc<LinkNode>(ref this.nodes, Mathf.Max(16, this.nodeCount * 2));
			}
			this.nodeCount++;
			LinkNode[] array = this.nodes;
			int num = this.nodeCount - 1;
			LinkNode linkNode = new LinkNode(this.active);
			linkNode.nodeInGraphIndex = this.nodeCount - 1;
			linkNode.GraphIndex = this.graphIndex;
			linkNode.Walkable = true;
			LinkNode result = linkNode;
			array[num] = linkNode;
			return result;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000224C0 File Offset: 0x000206C0
		internal void RemoveNode(LinkNode node)
		{
			if (this.nodes[node.nodeInGraphIndex] != node)
			{
				throw new ArgumentException("Node is not in this graph");
			}
			this.nodeCount--;
			this.nodes[node.nodeInGraphIndex] = this.nodes[this.nodeCount];
			this.nodes[node.nodeInGraphIndex].nodeInGraphIndex = node.nodeInGraphIndex;
			this.nodes[this.nodeCount] = null;
			node.Destroy();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002253C File Offset: 0x0002073C
		public override float NearestNodeDistanceSqrLowerBound(Vector3 position, NNConstraint constraint = null)
		{
			return float.PositiveInfinity;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00022544 File Offset: 0x00020744
		public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, float maxDistanceSqr)
		{
			return default(NNInfo);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002255A File Offset: 0x0002075A
		public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope)
		{
			base.OnDrawGizmos(gizmos, drawNodes, redrawScope);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00022565 File Offset: 0x00020765
		protected override IGraphUpdatePromise ScanInternal()
		{
			return new LinkGraph.LinkGraphUpdatePromise
			{
				graph = this
			};
		}

		// Token: 0x04000465 RID: 1125
		private LinkNode[] nodes = new LinkNode[0];

		// Token: 0x04000466 RID: 1126
		private int nodeCount;

		// Token: 0x020000CB RID: 203
		private class LinkGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x06000670 RID: 1648 RVA: 0x00022587 File Offset: 0x00020787
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x000035D8 File Offset: 0x000017D8
			public IEnumerator<JobHandle> Prepare()
			{
				return null;
			}

			// Token: 0x04000467 RID: 1127
			public LinkGraph graph;
		}
	}
}
