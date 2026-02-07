using System;
using Pathfinding.Drawing;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x0200029F RID: 671
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobHorizonAvoidancePhase1<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0001797A File Offset: 0x00015B7A
		public bool allowBoundsChecks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00061DF0 File Offset: 0x0005FFF0
		private static void Sort<T>(NativeSlice<T> arr, NativeSlice<float> keys) where T : struct
		{
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < arr.Length - 1; i++)
				{
					if (keys[i] > keys[i + 1])
					{
						float value = keys[i];
						T value2 = arr[i];
						keys[i] = keys[i + 1];
						keys[i + 1] = value;
						arr[i] = arr[i + 1];
						arr[i + 1] = value2;
						flag = true;
					}
				}
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00061E7C File Offset: 0x0006007C
		public static float DeltaAngle(float current, float target)
		{
			float num = Mathf.Repeat(target - current, 6.2831855f);
			if (num > 3.1415927f)
			{
				num -= 6.2831855f;
			}
			return num;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00061EA8 File Offset: 0x000600A8
		public void Execute(int startIndex, int count)
		{
			NativeArray<float> thisArray = new NativeArray<float>(100, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> thisArray2 = new NativeArray<int>(100, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.locked[i] || this.agentData.manuallyControlled[i])
					{
						this.horizonAgentData.horizonSide[i] = 0;
						this.horizonAgentData.horizonMinAngle[i] = 0f;
						this.horizonAgentData.horizonMaxAngle[i] = 0f;
					}
					else
					{
						float num = math.atan2(this.desiredTargetPointInVelocitySpace[i].y, this.desiredTargetPointInVelocitySpace[i].x);
						int num2 = 0;
						int num3 = 0;
						float num4 = this.agentData.radius[i];
						float3 rhs = this.agentData.position[i];
						MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
						movementPlaneWrapper.Set(this.agentData.movementPlane[i]);
						float num5 = math.all(math.isfinite(this.agentData.endOfPath[i])) ? math.lengthsq(this.agentData.endOfPath[i] - rhs) : float.PositiveInfinity;
						NativeSlice<int> nativeSlice = this.neighbours.Slice(i * 50, 50);
						int num6 = 0;
						while (num6 < nativeSlice.Length && nativeSlice[num6] != -1)
						{
							int index = nativeSlice[num6];
							if (this.agentData.locked[index] || this.agentData.manuallyControlled[index])
							{
								float2 @float = movementPlaneWrapper.ToPlane(this.agentData.position[index] - rhs);
								float num7 = math.length(@float);
								float num8 = this.agentData.radius[index];
								float num9 = num7 - (num4 + num8);
								if (num9 * num9 <= num5)
								{
									float num10 = math.atan2(@float.y, @float.x) - num;
									float num11;
									if (num9 <= 0f)
									{
										num11 = 1.5393804f;
									}
									else
									{
										num11 = math.asin((num4 + num8) / num7) + 0.017453292f;
									}
									float num12 = JobHorizonAvoidancePhase1<MovementPlaneWrapper>.DeltaAngle(0f, num10 - num11);
									float num13 = num12 + JobHorizonAvoidancePhase1<MovementPlaneWrapper>.DeltaAngle(num12, num10 + num11);
									if (num12 < 0f && num13 > 0f)
									{
										num3++;
									}
									thisArray[num2] = num12;
									thisArray2[num2] = 1;
									num2++;
									thisArray[num2] = num13;
									thisArray2[num2] = -1;
									num2++;
								}
							}
							num6++;
						}
						if (num3 == 0)
						{
							this.horizonAgentData.horizonSide[i] = 0;
							this.horizonAgentData.horizonMinAngle[i] = 0f;
							this.horizonAgentData.horizonMaxAngle[i] = 0f;
						}
						else
						{
							JobHorizonAvoidancePhase1<MovementPlaneWrapper>.Sort<int>(thisArray2.Slice(0, num2), thisArray.Slice(0, num2));
							int num14 = 0;
							while (num14 < num2 && thisArray[num14] <= 0f)
							{
								num14++;
							}
							int num15 = num3;
							int j;
							for (j = num14; j < num2; j++)
							{
								num15 += thisArray2[j];
								if (num15 == 0)
								{
									break;
								}
							}
							float num16 = (j == num2) ? 3.1415927f : thisArray[j];
							num15 = num3;
							for (j = num14 - 1; j >= 0; j--)
							{
								num15 -= thisArray2[j];
								if (num15 == 0)
								{
									break;
								}
							}
							float num17 = (j == -1) ? -3.1415927f : thisArray[j];
							if (this.horizonAgentData.horizonSide[i] == 0)
							{
								this.horizonAgentData.horizonSide[i] = 2;
							}
							this.horizonAgentData.horizonMinAngle[i] = num17 + num;
							this.horizonAgentData.horizonMaxAngle[i] = num16 + num;
						}
					}
				}
			}
		}

		// Token: 0x04000BD2 RID: 3026
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BD3 RID: 3027
		[ReadOnly]
		public NativeArray<float2> desiredTargetPointInVelocitySpace;

		// Token: 0x04000BD4 RID: 3028
		[ReadOnly]
		public NativeArray<int> neighbours;

		// Token: 0x04000BD5 RID: 3029
		public SimulatorBurst.HorizonAgentData horizonAgentData;

		// Token: 0x04000BD6 RID: 3030
		public CommandBuilder draw;
	}
}
