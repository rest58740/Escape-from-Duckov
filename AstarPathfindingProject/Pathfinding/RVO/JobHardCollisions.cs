using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002A1 RID: 673
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHardCollisions<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x000185BF File Offset: 0x000167BF
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x000624D8 File Offset: 0x000606D8
		public void Execute(int startIndex, int count)
		{
			if (!this.enabled)
			{
				for (int i = startIndex; i < startIndex + count; i++)
				{
					this.collisionVelocityOffsets[i] = float2.zero;
				}
				return;
			}
			for (int j = startIndex; j < startIndex + count; j++)
			{
				if (!this.agentData.version[j].Valid || this.agentData.locked[j])
				{
					this.collisionVelocityOffsets[j] = float2.zero;
				}
				else
				{
					NativeSlice<int> nativeSlice = this.neighbours.Slice(j * 50, 50);
					float num = this.agentData.radius[j];
					float2 lhs = float2.zero;
					float num2 = 0f;
					float3 lhs2 = this.agentData.position[j];
					MovementPlaneWrapper movementPlaneWrapper = Activator.CreateInstance<MovementPlaneWrapper>();
					movementPlaneWrapper.Set(this.agentData.movementPlane[j]);
					int num3 = 0;
					while (num3 < nativeSlice.Length && nativeSlice[num3] != -1)
					{
						int index = nativeSlice[num3];
						float2 @float = movementPlaneWrapper.ToPlane(lhs2 - this.agentData.position[index]);
						float num4 = math.lengthsq(@float);
						float num5 = this.agentData.radius[index] + num;
						if (num4 < num5 * num5 && num4 > 1E-08f)
						{
							float num6 = math.sqrt(num4);
							float2 lhs3 = @float * (1f / num6);
							float num7 = num5 - num6;
							float2 rhs = lhs3 * num7 * num7;
							lhs += rhs;
							num2 += num7;
						}
						num3++;
					}
					float2 float2 = lhs * (1f / (0.0001f + num2));
					float2 *= 0.4f / this.deltaTime;
					this.collisionVelocityOffsets[j] = float2;
				}
			}
		}

		// Token: 0x04000BDD RID: 3037
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BDE RID: 3038
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000BDF RID: 3039
		[WriteOnly]
		public NativeArray<float2> collisionVelocityOffsets;

		// Token: 0x04000BE0 RID: 3040
		public float deltaTime;

		// Token: 0x04000BE1 RID: 3041
		public bool enabled;

		// Token: 0x04000BE2 RID: 3042
		private const float CollisionStrength = 0.8f;
	}
}
