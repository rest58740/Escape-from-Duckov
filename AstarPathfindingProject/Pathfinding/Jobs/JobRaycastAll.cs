using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x0200016B RID: 363
	public struct JobRaycastAll
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0003C950 File Offset: 0x0003AB50
		public JobRaycastAll(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, PhysicsScene physicsScene, int maxHits, Allocator allocator, JobDependencyTracker dependencyTracker, float minStep = 0.0001f)
		{
			if (maxHits <= 0)
			{
				throw new ArgumentException("maxHits should be greater than zero");
			}
			if (results.Length < commands.Length * maxHits)
			{
				throw new ArgumentException("Results array length does not match maxHits count");
			}
			if (minStep < 0f)
			{
				throw new ArgumentException("minStep should be more or equal to zero");
			}
			this.results = results;
			this.maxHits = maxHits;
			this.minStep = minStep;
			this.commands = commands;
			this.physicsScene = physicsScene;
			this.semiResults = dependencyTracker.NewNativeArray<RaycastHit>(maxHits * commands.Length, allocator, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0003C9E0 File Offset: 0x0003ABE0
		public JobHandle Schedule(JobHandle dependency)
		{
			for (int i = 0; i < this.maxHits; i++)
			{
				NativeArray<RaycastHit> subArray = this.semiResults.GetSubArray(i * this.commands.Length, this.commands.Length);
				dependency = RaycastCommand.ScheduleBatch(this.commands, subArray, 128, dependency);
				if (i < this.maxHits - 1)
				{
					dependency = new JobRaycastAll.JobCreateCommands
					{
						commands = this.commands,
						raycastHits = subArray,
						minStep = this.minStep,
						physicsScene = this.physicsScene
					}.Schedule(this.commands.Length, 256, dependency);
				}
			}
			return new JobRaycastAll.JobCombineResults
			{
				semiResults = this.semiResults,
				maxHits = this.maxHits,
				results = this.results
			}.Schedule(dependency);
		}

		// Token: 0x04000723 RID: 1827
		private int maxHits;

		// Token: 0x04000724 RID: 1828
		public readonly float minStep;

		// Token: 0x04000725 RID: 1829
		private NativeArray<RaycastHit> results;

		// Token: 0x04000726 RID: 1830
		private NativeArray<RaycastHit> semiResults;

		// Token: 0x04000727 RID: 1831
		private NativeArray<RaycastCommand> commands;

		// Token: 0x04000728 RID: 1832
		public PhysicsScene physicsScene;

		// Token: 0x0200016C RID: 364
		[BurstCompile]
		private struct JobCreateCommands : IJobParallelFor
		{
			// Token: 0x06000AB0 RID: 2736 RVA: 0x0003CAD0 File Offset: 0x0003ACD0
			public void Execute(int index)
			{
				RaycastHit raycastHit = this.raycastHits[index];
				if (raycastHit.normal != default(Vector3))
				{
					RaycastCommand raycastCommand = this.commands[index];
					Vector3 vector = raycastHit.point + raycastCommand.direction.normalized * this.minStep;
					float distance = raycastCommand.distance - (vector - raycastCommand.from).magnitude;
					QueryParameters queryParameters = new QueryParameters(raycastCommand.queryParameters.layerMask, false, QueryTriggerInteraction.Ignore, false);
					this.commands[index] = new RaycastCommand(this.physicsScene, vector, raycastCommand.direction, queryParameters, distance);
					return;
				}
				this.commands[index] = new RaycastCommand(this.physicsScene, Vector3.zero, Vector3.up, new QueryParameters(0, false, QueryTriggerInteraction.Ignore, false), 1f);
			}

			// Token: 0x04000729 RID: 1833
			public NativeArray<RaycastCommand> commands;

			// Token: 0x0400072A RID: 1834
			[ReadOnly]
			public NativeArray<RaycastHit> raycastHits;

			// Token: 0x0400072B RID: 1835
			public float minStep;

			// Token: 0x0400072C RID: 1836
			public PhysicsScene physicsScene;
		}

		// Token: 0x0200016D RID: 365
		[BurstCompile]
		private struct JobCombineResults : IJob
		{
			// Token: 0x06000AB1 RID: 2737 RVA: 0x0003CBC0 File Offset: 0x0003ADC0
			public void Execute()
			{
				int num = this.semiResults.Length / this.maxHits;
				for (int i = 0; i < num; i++)
				{
					int num2 = 0;
					for (int j = this.maxHits - 1; j >= 0; j--)
					{
						if (math.any(this.semiResults[i + j * num].normal))
						{
							this.results[i + num2] = this.semiResults[i + j * num];
							num2 += num;
						}
					}
				}
			}

			// Token: 0x0400072D RID: 1837
			public int maxHits;

			// Token: 0x0400072E RID: 1838
			[ReadOnly]
			public NativeArray<RaycastHit> semiResults;

			// Token: 0x0400072F RID: 1839
			public NativeArray<RaycastHit> results;
		}
	}
}
