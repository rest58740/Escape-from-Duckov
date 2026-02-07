using System;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000206 RID: 518
	[Preserve]
	public class RuleElevationPenalty : GridGraphRule
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x00050924 File Offset: 0x0004EB24
		public override void Register(GridGraphRules rules)
		{
			if (!this.elevationToPenalty.IsCreated)
			{
				this.elevationToPenalty = new NativeArray<float>(64, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			for (int i = 0; i < this.elevationToPenalty.Length; i++)
			{
				this.elevationToPenalty[i] = Mathf.Max(0f, this.penaltyScale * this.curve.Evaluate((float)i * 1f / (float)(this.elevationToPenalty.Length - 1)));
			}
			Vector2 clampedElevationRange = new Vector2(math.max(0f, this.elevationRange.x), math.max(1f, this.elevationRange.y));
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				Matrix4x4 lhs = Matrix4x4.Scale(new Vector3(1f, 1f / (clampedElevationRange.y - clampedElevationRange.x), 1f)) * Matrix4x4.Translate(new Vector3(0f, -clampedElevationRange.x, 0f));
				new RuleElevationPenalty.JobElevationPenalty
				{
					elevationToPenalty = this.elevationToPenalty,
					nodePositions = context.data.nodes.positions,
					worldToGraph = lhs * context.data.transform.matrix.inverse,
					penalty = context.data.nodes.penalties
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000509F7 File Offset: 0x0004EBF7
		public override void DisposeUnmanagedData()
		{
			if (this.elevationToPenalty.IsCreated)
			{
				this.elevationToPenalty.Dispose();
			}
		}

		// Token: 0x04000975 RID: 2421
		public float penaltyScale = 10000f;

		// Token: 0x04000976 RID: 2422
		public Vector2 elevationRange = new Vector2(0f, 100f);

		// Token: 0x04000977 RID: 2423
		public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000978 RID: 2424
		private NativeArray<float> elevationToPenalty;

		// Token: 0x02000207 RID: 519
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobElevationPenalty : IJob
		{
			// Token: 0x06000CE9 RID: 3305 RVA: 0x00050A68 File Offset: 0x0004EC68
			public void Execute()
			{
				for (int i = 0; i < this.penalty.Length; i++)
				{
					float num = math.clamp(this.worldToGraph.MultiplyPoint3x4(this.nodePositions[i]).y, 0f, 1f) * (float)(this.elevationToPenalty.Length - 1);
					int num2 = (int)num;
					float start = this.elevationToPenalty[num2];
					float end = this.elevationToPenalty[math.min(num2 + 1, this.elevationToPenalty.Length - 1)];
					ref NativeArray<uint> ptr = ref this.penalty;
					int index = i;
					ptr[index] += (uint)math.lerp(start, end, num - (float)num2);
				}
			}

			// Token: 0x04000979 RID: 2425
			[ReadOnly]
			public NativeArray<float> elevationToPenalty;

			// Token: 0x0400097A RID: 2426
			[ReadOnly]
			public NativeArray<Vector3> nodePositions;

			// Token: 0x0400097B RID: 2427
			public Matrix4x4 worldToGraph;

			// Token: 0x0400097C RID: 2428
			public NativeArray<uint> penalty;
		}
	}
}
