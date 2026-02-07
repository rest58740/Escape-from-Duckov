using System;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000133 RID: 307
	public class FloodPathTracer : ABPath
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000185BF File Offset: 0x000167BF
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00033819 File Offset: 0x00031A19
		public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
		{
			FloodPathTracer path = PathPool.GetPath<FloodPathTracer>();
			path.Setup(start, flood, callback);
			return path;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00033829 File Offset: 0x00031A29
		protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
		{
			this.flood = flood;
			if (flood == null || flood.PipelineState < PathState.Returning)
			{
				throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
			}
			base.Setup(start, flood.originalStartPoint, callback);
			this.nnConstraint = new FloodPathConstraint(flood);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00033863 File Offset: 0x00031A63
		protected override void Reset()
		{
			base.Reset();
			this.flood = null;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00033874 File Offset: 0x00031A74
		protected override void Prepare()
		{
			if (!this.flood.IsValid(this.pathHandler.nodeStorage))
			{
				base.FailWithError("The flood path is invalid because nodes have been destroyed since it was calculated. Please recalculate the flood path.");
				return;
			}
			base.Prepare();
			if (base.CompleteState == PathCompleteState.NotCalculated)
			{
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
					ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
					if (temporaryNode.type == TemporaryNodeType.Start)
					{
						GraphNode node = this.pathHandler.GetNode(temporaryNode.associatedNode);
						bool flag = false;
						uint num2 = 0U;
						while ((ulong)num2 < (ulong)((long)node.PathNodeVariants))
						{
							if (this.flood.GetParent(node.NodeIndex + num2) != 0U)
							{
								flag = true;
								base.CompleteState = PathCompleteState.Complete;
								this.Trace(node.NodeIndex + num2);
								break;
							}
							num2 += 1U;
						}
						if (!flag)
						{
							base.FailWithError("The flood path did not contain any information about the end node. Have you modified the path's nnConstraint to an instance which does not subclass FloodPathConstraint?");
						}
						return;
					}
					num += 1U;
				}
				base.FailWithError("Could not find a valid start node");
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0003396F File Offset: 0x00031B6F
		protected override void CalculateStep(long targetTick)
		{
			if (base.CompleteState != PathCompleteState.Complete)
			{
				throw new Exception("Something went wrong. At this point the path should be completed");
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00033988 File Offset: 0x00031B88
		protected override void Trace(uint fromPathNodeIndex)
		{
			uint num = fromPathNodeIndex;
			int num2 = 0;
			GraphNode graphNode = null;
			while (num != 0U)
			{
				if ((num & 2147483648U) != 0U)
				{
					num = this.flood.GetParent(num & 2147483647U);
				}
				else
				{
					GraphNode node = this.pathHandler.GetNode(num);
					if (node == null)
					{
						base.FailWithError("A node in the path has been destroyed. The FloodPath needs to be recalculated before you can use a FloodPathTracer.");
						return;
					}
					if (node != graphNode)
					{
						if (!base.CanTraverse(node))
						{
							base.FailWithError("A node in the path is no longer walkable. The FloodPath needs to be recalculated before you can use a FloodPathTracer.");
							return;
						}
						this.path.Add(node);
						graphNode = node;
						this.vectorPath.Add((Vector3)node.position);
					}
					uint parent = this.flood.GetParent(num);
					if (parent == num)
					{
						break;
					}
					num = parent;
				}
				num2++;
				if (num2 > 10000)
				{
					Debug.LogWarning("Infinite loop? >10000 node path. Remove this message if you really have that long paths");
					return;
				}
			}
		}

		// Token: 0x04000672 RID: 1650
		protected FloodPath flood;
	}
}
