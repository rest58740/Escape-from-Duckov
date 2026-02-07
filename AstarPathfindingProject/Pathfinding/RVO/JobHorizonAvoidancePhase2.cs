using System;
using Pathfinding.ECS.RVO;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002A0 RID: 672
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHorizonAvoidancePhase2<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x000185BF File Offset: 0x000167BF
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000622EC File Offset: 0x000604EC
		public void Execute(int startIndex, int count)
		{
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.versions[i].Valid && this.horizonAgentData.horizonSide[i] != 0)
				{
					if (this.horizonAgentData.horizonSide[i] == 2)
					{
						float num = 0f;
						NativeSlice<int> nativeSlice = this.neighbours.Slice(i * 50, 50);
						int num2 = 0;
						while (num2 < nativeSlice.Length && nativeSlice[num2] != -1)
						{
							int index = nativeSlice[num2];
							float num3 = -(this.horizonAgentData.horizonMinAngle[index] + this.horizonAgentData.horizonMaxAngle[index]);
							num += num3;
							num2++;
						}
						float num4 = -(this.horizonAgentData.horizonMinAngle[i] + this.horizonAgentData.horizonMaxAngle[i]);
						num += num4;
						this.horizonAgentData.horizonSide[i] = ((num < 0f) ? -1 : 1);
					}
					float2 rhs;
					math.sincos((this.horizonAgentData.horizonSide[i] < 0) ? this.horizonAgentData.horizonMinAngle[i] : this.horizonAgentData.horizonMaxAngle[i], out rhs.y, out rhs.x);
					MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
					movementPlaneWrapper.Set(this.movementPlane[i]);
					this.desiredVelocity[i] = movementPlaneWrapper.ToWorld(math.length(this.desiredVelocity[i]) * rhs, 0f);
					this.desiredTargetPointInVelocitySpace[i] = math.length(this.desiredTargetPointInVelocitySpace[i]) * rhs;
				}
			}
		}

		// Token: 0x04000BD7 RID: 3031
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000BD8 RID: 3032
		[ReadOnly]
		public NativeArray<AgentIndex> versions;

		// Token: 0x04000BD9 RID: 3033
		public NativeArray<float3> desiredVelocity;

		// Token: 0x04000BDA RID: 3034
		public NativeArray<float2> desiredTargetPointInVelocitySpace;

		// Token: 0x04000BDB RID: 3035
		[ReadOnly]
		public NativeArray<NativeMovementPlane> movementPlane;

		// Token: 0x04000BDC RID: 3036
		public SimulatorBurst.HorizonAgentData horizonAgentData;
	}
}
