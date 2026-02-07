using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002F RID: 47
	public class GraphUpdateObject
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000A040 File Offset: 0x00008240
		public GraphUpdateStage stage
		{
			get
			{
				switch (this.internalStage)
				{
				case -3:
					return GraphUpdateStage.Aborted;
				case -1:
					return GraphUpdateStage.Created;
				case 0:
					return GraphUpdateStage.Applied;
				}
				return GraphUpdateStage.Pending;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void WillUpdateNode(GraphNode node)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000035CE File Offset: 0x000017CE
		[Obsolete("Use AstarPath.Snapshot instead", true)]
		public virtual void RevertFromBackup()
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A078 File Offset: 0x00008278
		public virtual void Apply(GraphNode node)
		{
			if (this.shape == null || this.shape.Contains(node))
			{
				node.Penalty = (uint)((ulong)node.Penalty + (ulong)((long)this.addPenalty));
				if (this.modifyWalkability)
				{
					node.Walkable = this.setWalkability;
				}
				if (this.modifyTag)
				{
					node.Tag = this.setTag;
				}
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public virtual void ApplyJob(GraphUpdateObject.GraphUpdateData data, JobDependencyTracker dependencyTracker)
		{
			if (this.addPenalty == 0 && !this.modifyWalkability && !this.modifyTag)
			{
				return;
			}
			new GraphUpdateObject.JobGraphUpdate
			{
				shape = ((this.shape != null) ? new GraphUpdateShape.BurstShape(this.shape, Allocator.Persistent) : GraphUpdateShape.BurstShape.Everything),
				data = data,
				bounds = this.bounds,
				penaltyDelta = this.addPenalty,
				modifyWalkability = this.modifyWalkability,
				walkabilityValue = this.setWalkability,
				modifyTag = this.modifyTag,
				tagValue = (int)this.setTag.value
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A193 File Offset: 0x00008393
		public GraphUpdateObject()
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A1C2 File Offset: 0x000083C2
		public GraphUpdateObject(Bounds b)
		{
			this.bounds = b;
		}

		// Token: 0x04000153 RID: 339
		public Bounds bounds;

		// Token: 0x04000154 RID: 340
		public bool updatePhysics = true;

		// Token: 0x04000155 RID: 341
		public bool resetPenaltyOnPhysics = true;

		// Token: 0x04000156 RID: 342
		public bool updateErosion = true;

		// Token: 0x04000157 RID: 343
		public NNConstraint nnConstraint = NNConstraint.None;

		// Token: 0x04000158 RID: 344
		public int addPenalty;

		// Token: 0x04000159 RID: 345
		public bool modifyWalkability;

		// Token: 0x0400015A RID: 346
		public bool setWalkability;

		// Token: 0x0400015B RID: 347
		public bool modifyTag;

		// Token: 0x0400015C RID: 348
		public PathfindingTag setTag;

		// Token: 0x0400015D RID: 349
		[Obsolete("This field does not do anything anymore. Use AstarPath.Snapshot instead.")]
		public bool trackChangedNodes;

		// Token: 0x0400015E RID: 350
		public GraphUpdateShape shape;

		// Token: 0x0400015F RID: 351
		internal int internalStage = -1;

		// Token: 0x04000160 RID: 352
		internal const int STAGE_CREATED = -1;

		// Token: 0x04000161 RID: 353
		internal const int STAGE_PENDING = -2;

		// Token: 0x04000162 RID: 354
		internal const int STAGE_ABORTED = -3;

		// Token: 0x04000163 RID: 355
		internal const int STAGE_APPLIED = 0;

		// Token: 0x02000030 RID: 48
		public struct GraphUpdateData
		{
			// Token: 0x04000164 RID: 356
			public NativeArray<Vector3> nodePositions;

			// Token: 0x04000165 RID: 357
			public NativeArray<uint> nodePenalties;

			// Token: 0x04000166 RID: 358
			public NativeArray<bool> nodeWalkable;

			// Token: 0x04000167 RID: 359
			public NativeArray<int> nodeTags;

			// Token: 0x04000168 RID: 360
			public NativeArray<float4> nodeNormals;

			// Token: 0x04000169 RID: 361
			public NativeArray<int> nodeIndices;
		}

		// Token: 0x02000031 RID: 49
		[BurstCompile]
		public struct JobGraphUpdate : IJob
		{
			// Token: 0x06000202 RID: 514 RVA: 0x0000A1F8 File Offset: 0x000083F8
			public void Execute()
			{
				for (int i = 0; i < this.data.nodeIndices.Length; i++)
				{
					int num = this.data.nodeIndices[i];
					if (math.any(this.data.nodeNormals[num]) && this.bounds.Contains(this.data.nodePositions[num]) && this.shape.Contains(this.data.nodePositions[num]))
					{
						ref NativeArray<uint> ptr = ref this.data.nodePenalties;
						int index = num;
						ptr[index] += (uint)this.penaltyDelta;
						if (this.modifyWalkability)
						{
							this.data.nodeWalkable[num] = this.walkabilityValue;
						}
						if (this.modifyTag)
						{
							this.data.nodeTags[num] = this.tagValue;
						}
					}
				}
			}

			// Token: 0x0400016A RID: 362
			public GraphUpdateShape.BurstShape shape;

			// Token: 0x0400016B RID: 363
			public GraphUpdateObject.GraphUpdateData data;

			// Token: 0x0400016C RID: 364
			public Bounds bounds;

			// Token: 0x0400016D RID: 365
			public int penaltyDelta;

			// Token: 0x0400016E RID: 366
			public bool modifyWalkability;

			// Token: 0x0400016F RID: 367
			public bool walkabilityValue;

			// Token: 0x04000170 RID: 368
			public bool modifyTag;

			// Token: 0x04000171 RID: 369
			public int tagValue;
		}
	}
}
