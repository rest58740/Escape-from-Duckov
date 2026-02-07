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
	// Token: 0x02000204 RID: 516
	[Preserve]
	public class RuleAnglePenalty : GridGraphRule
	{
		// Token: 0x06000CE1 RID: 3297 RVA: 0x000506D8 File Offset: 0x0004E8D8
		public override void Register(GridGraphRules rules)
		{
			if (!this.angleToPenalty.IsCreated)
			{
				this.angleToPenalty = new NativeArray<float>(32, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			for (int i = 0; i < this.angleToPenalty.Length; i++)
			{
				this.angleToPenalty[i] = Mathf.Max(0f, this.curve.Evaluate(90f * (float)i / (float)(this.angleToPenalty.Length - 1)) * this.penaltyScale);
			}
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				new RuleAnglePenalty.JobPenaltyAngle
				{
					angleToPenalty = this.angleToPenalty,
					up = context.data.up,
					nodeNormals = context.data.nodes.normals,
					penalty = context.data.nodes.penalties
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00050769 File Offset: 0x0004E969
		public override void DisposeUnmanagedData()
		{
			if (this.angleToPenalty.IsCreated)
			{
				this.angleToPenalty.Dispose();
			}
		}

		// Token: 0x0400096E RID: 2414
		public float penaltyScale = 10000f;

		// Token: 0x0400096F RID: 2415
		public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 90f, 1f);

		// Token: 0x04000970 RID: 2416
		private NativeArray<float> angleToPenalty;

		// Token: 0x02000205 RID: 517
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobPenaltyAngle : IJob
		{
			// Token: 0x06000CE5 RID: 3301 RVA: 0x00050828 File Offset: 0x0004EA28
			public void Execute()
			{
				float4 y = new float4(this.up.x, this.up.y, this.up.z, 0f);
				for (int i = 0; i < this.penalty.Length; i++)
				{
					float4 x = this.nodeNormals[i];
					if (math.any(x))
					{
						float num = math.acos(math.dot(x, y)) * (float)(this.angleToPenalty.Length - 1) / 3.1415927f;
						int num2 = (int)num;
						float start = this.angleToPenalty[math.max(num2, 0)];
						float end = this.angleToPenalty[math.min(num2 + 1, this.angleToPenalty.Length - 1)];
						ref NativeArray<uint> ptr = ref this.penalty;
						int index = i;
						ptr[index] += (uint)math.lerp(start, end, num - (float)num2);
					}
				}
			}

			// Token: 0x04000971 RID: 2417
			public Vector3 up;

			// Token: 0x04000972 RID: 2418
			[ReadOnly]
			public NativeArray<float> angleToPenalty;

			// Token: 0x04000973 RID: 2419
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x04000974 RID: 2420
			public NativeArray<uint> penalty;
		}
	}
}
