using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002A2 RID: 674
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobRVOCalculateNeighbours<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x000185BF File Offset: 0x000167BF
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x000626D8 File Offset: 0x000608D8
		public void Execute(int startIndex, int count)
		{
			NativeArray<float> neighbourDistances = new NativeArray<float>(50, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					this.CalculateNeighbours(i, this.outNeighbours, neighbourDistances);
				}
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00062728 File Offset: 0x00060928
		private void CalculateNeighbours(int agentIndex, NativeArray<int> neighbours, NativeArray<float> neighbourDistances)
		{
			int maxCount = math.min(50, this.agentData.maxNeighbours[agentIndex]);
			int num = agentIndex * 50;
			int num2 = this.quadtree.QueryKNearest(new RVOQuadtreeBurst.QuadtreeQuery
			{
				position = this.agentData.position[agentIndex],
				speed = this.agentData.maxSpeed[agentIndex],
				agentRadius = this.agentData.radius[agentIndex],
				timeHorizon = this.agentData.agentTimeHorizon[agentIndex],
				outputStartIndex = num,
				maxCount = maxCount,
				result = neighbours,
				layerMask = this.agentData.collidesWith[agentIndex],
				layers = this.agentData.layer,
				resultDistances = neighbourDistances
			});
			this.output.numNeighbours[agentIndex] = num2;
			MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
			movementPlaneWrapper.Set(this.agentData.movementPlane[agentIndex]);
			float num3;
			movementPlaneWrapper.ToPlane(this.agentData.position[agentIndex], out num3);
			for (int i = 0; i < num2; i++)
			{
				int num4 = neighbours[num + i];
				if (num4 == -1)
				{
					throw new Exception("Invalid neighbour index");
				}
				float num5;
				movementPlaneWrapper.ToPlane(this.agentData.position[num4], out num5);
				float num6 = math.min(num3 + this.agentData.height[agentIndex], num5 + this.agentData.height[num4]);
				float num7 = math.max(num3, num5);
				if (num6 < num7 | num4 == agentIndex)
				{
					num2--;
					neighbours[num + i] = neighbours[num + num2];
					i--;
				}
			}
			if (num2 < 50)
			{
				neighbours[num + num2] = -1;
			}
		}

		// Token: 0x04000BE3 RID: 3043
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BE4 RID: 3044
		[ReadOnly]
		public RVOQuadtreeBurst quadtree;

		// Token: 0x04000BE5 RID: 3045
		public NativeArray<int> outNeighbours;

		// Token: 0x04000BE6 RID: 3046
		[WriteOnly]
		public SimulatorBurst.AgentOutputData output;
	}
}
