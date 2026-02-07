using System;
using System.Runtime.CompilerServices;
using System.Text;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Profiling;

namespace Pathfinding
{
	// Token: 0x020000B3 RID: 179
	public class PathHandler
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001B257 File Offset: 0x00019457
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x0001B25F File Offset: 0x0001945F
		public int numTemporaryNodes { [IgnoredByDeepProfiler] get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001B268 File Offset: 0x00019468
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0001B270 File Offset: 0x00019470
		public uint temporaryNodeStartIndex { [IgnoredByDeepProfiler] get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001B279 File Offset: 0x00019479
		public ushort PathID
		{
			get
			{
				return this.pathID;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001B284 File Offset: 0x00019484
		internal PathHandler(GlobalNodeStorage nodeStorage, int threadID, int totalThreadCount)
		{
			this.threadID = threadID;
			this.totalThreadCount = totalThreadCount;
			this.nodeStorage = nodeStorage;
			this.temporaryNodes = default(UnsafeSpan<TemporaryNode>);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001B2E0 File Offset: 0x000194E0
		public void InitializeForPath(Path p)
		{
			ushort num = this.pathID;
			this.pathID = p.pathID;
			this.numTemporaryNodes = 0;
			this.pathNodes = this.nodeStorage.pathfindingThreadData[this.threadID].pathNodes;
			this.temporaryNodeStartIndex = this.nodeStorage.reservedPathNodeData;
			int num2 = this.pathNodes.Length - (int)this.temporaryNodeStartIndex;
			if (num2 > this.temporaryNodes.Length)
			{
				this.temporaryNodes = this.temporaryNodes.Reallocate(Allocator.Persistent, num2);
			}
			if (this.pathID < num)
			{
				this.ClearPathIDs();
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001B37C File Offset: 0x0001957C
		public unsafe PathNode GetPathNode(GraphNode node, uint variant = 0U)
		{
			return *this.pathNodes[node.NodeIndex + variant];
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B396 File Offset: 0x00019596
		public bool IsTemporaryNode(uint pathNodeIndex)
		{
			return pathNodeIndex >= this.temporaryNodeStartIndex;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B3A4 File Offset: 0x000195A4
		public unsafe uint AddTemporaryNode(TemporaryNode node)
		{
			if (this.numTemporaryNodes >= this.temporaryNodes.Length)
			{
				this.nodeStorage.GrowTemporaryNodeStorage(this.threadID);
				this.pathNodes = this.nodeStorage.pathfindingThreadData[this.threadID].pathNodes;
				this.temporaryNodes = this.temporaryNodes.Reallocate(Allocator.Persistent, this.pathNodes.Length - (int)this.temporaryNodeStartIndex);
			}
			uint num = this.temporaryNodeStartIndex + (uint)this.numTemporaryNodes;
			*this.temporaryNodes[this.numTemporaryNodes] = node;
			*this.pathNodes[num] = PathNode.Default;
			int numTemporaryNodes = this.numTemporaryNodes;
			this.numTemporaryNodes = numTemporaryNodes + 1;
			return num;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B466 File Offset: 0x00019666
		public GraphNode GetNode(uint nodeIndex)
		{
			return this.nodeStorage.GetNode(nodeIndex);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B474 File Offset: 0x00019674
		public ref TemporaryNode GetTemporaryNode(uint nodeIndex)
		{
			if (nodeIndex < this.temporaryNodeStartIndex || (ulong)nodeIndex >= (ulong)this.temporaryNodeStartIndex + (ulong)((long)this.numTemporaryNodes))
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.temporaryNodes[(int)(nodeIndex - this.temporaryNodeStartIndex)];
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000035CE File Offset: 0x000017CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LogVisitedNode(uint pathNodeIndex, uint h, uint g)
		{
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B4AC File Offset: 0x000196AC
		public void ClearPathIDs()
		{
			for (int i = 0; i < this.pathNodes.Length; i++)
			{
				this.pathNodes[i].pathID = 0;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B4E1 File Offset: 0x000196E1
		public void Dispose()
		{
			this.heap.Dispose();
			this.temporaryNodes.Free(Allocator.Persistent);
			this.pathNodes = default(UnsafeSpan<PathNode>);
		}

		// Token: 0x040003BB RID: 955
		private ushort pathID;

		// Token: 0x040003BC RID: 956
		public readonly int threadID;

		// Token: 0x040003BD RID: 957
		public readonly int totalThreadCount;

		// Token: 0x040003BE RID: 958
		public readonly NNConstraintWithTraversalProvider constraintWrapper = new NNConstraintWithTraversalProvider();

		// Token: 0x040003BF RID: 959
		internal readonly GlobalNodeStorage nodeStorage;

		// Token: 0x040003C2 RID: 962
		private UnsafeSpan<TemporaryNode> temporaryNodes;

		// Token: 0x040003C3 RID: 963
		public UnsafeSpan<PathNode> pathNodes;

		// Token: 0x040003C4 RID: 964
		public BinaryHeap heap = new BinaryHeap(128);

		// Token: 0x040003C5 RID: 965
		public readonly StringBuilder DebugStringBuilder = new StringBuilder();
	}
}
