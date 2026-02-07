using System;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;

namespace Pathfinding.RVO
{
	// Token: 0x020002A3 RID: 675
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobDestinationReached<MovementPlaneWrapper> : IJob where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x00062938 File Offset: 0x00060B38
		public void Execute()
		{
			for (int i = 0; i < this.numAgents; i++)
			{
				this.output.effectivelyReachedDestination[i] = ReachedEndOfPath.NotReached;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(this.agentData.position.Length * 7, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(this.agentData.position.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeCircularBuffer<int> nativeCircularBuffer = new NativeCircularBuffer<int>(16, Allocator.Temp);
			NativeArray<bool> nativeArray3 = new NativeArray<bool>(this.numAgents, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<JobDestinationReached<MovementPlaneWrapper>.TempAgentData> nativeArray4 = new NativeArray<JobDestinationReached<MovementPlaneWrapper>.TempAgentData>(this.numAgents, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < this.numAgents; j++)
			{
				if (this.agentData.version[j].Valid)
				{
					for (int k = 0; k < 7; k++)
					{
						int num = this.output.blockedByAgents[j * 7 + k];
						if (num == -1)
						{
							break;
						}
						int num2 = nativeArray2[num];
						if (num2 < 7)
						{
							nativeArray[num * 7 + num2] = j;
							nativeArray2[num] = num2 + 1;
						}
					}
				}
			}
			for (int l = 0; l < this.numAgents; l++)
			{
				if (this.agentData.version[l].Valid)
				{
					float3 @float = this.agentData.position[l];
					MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
					movementPlaneWrapper.Set(this.agentData.movementPlane[l]);
					float num3 = this.output.speed[l];
					float3 float2 = this.agentData.endOfPath[l];
					if (math.isfinite(float2.x))
					{
						float num5;
						float num4 = math.lengthsq(movementPlaneWrapper.ToPlane(float2 - @float, out num5));
						float num6 = this.agentData.height[l];
						bool flag = false;
						bool flag2 = false;
						float num7 = this.agentData.radius[l];
						float num8 = this.output.forwardClearance[l];
						if (num4 < num7 * num7 * 0.25f && num5 < num6 && num5 > -num6 * 0.5f)
						{
							flag = true;
						}
						bool flag3 = num8 < num7 * 0.5f;
						bool flag4 = num3 * num3 < math.max(0.0001f, math.lengthsq(this.temporaryAgentData.desiredVelocity[l]) * 0.25f);
						bool flag5 = flag3 && flag4;
						nativeArray4[l] = new JobDestinationReached<MovementPlaneWrapper>.TempAgentData
						{
							blockedAndSlow = flag5,
							distToEndSq = num4
						};
						for (int m = 0; m < 7; m++)
						{
							int num9 = this.output.blockedByAgents[l * 7 + m];
							if (num9 == -1)
							{
								break;
							}
							float3 rhs = this.agentData.position[num9];
							float num10 = (math.sqrt(math.lengthsq(movementPlaneWrapper.ToPlane(@float - rhs))) + num7 + this.agentData.radius[num9]) * 0.5f;
							if (math.lengthsq(movementPlaneWrapper.ToPlane(float2 - 0.5f * (@float + rhs))) < num10 * num10)
							{
								bool flag6 = false;
								for (int n = 0; n < 7; n++)
								{
									int num11 = nativeArray[l * 7 + n];
									if (num11 == -1)
									{
										break;
									}
									if (num11 == num9)
									{
										flag6 = true;
										break;
									}
								}
								if (flag6)
								{
									flag2 = true;
									if (flag5)
									{
										flag = true;
									}
								}
							}
						}
						ReachedEndOfPath reachedEndOfPath = flag ? ReachedEndOfPath.Reached : (flag2 ? ReachedEndOfPath.ReachedSoon : ReachedEndOfPath.NotReached);
						if (reachedEndOfPath != this.output.effectivelyReachedDestination[l])
						{
							this.output.effectivelyReachedDestination[l] = reachedEndOfPath;
							if (reachedEndOfPath == ReachedEndOfPath.Reached)
							{
								nativeArray3[l] = true;
								int num12 = nativeArray2[l];
								for (int num13 = 0; num13 < num12; num13++)
								{
									int num14 = nativeArray[l * 7 + num13];
									if (!nativeArray3[num14])
									{
										nativeCircularBuffer.PushEnd(num14);
									}
								}
							}
						}
					}
				}
			}
			int num15 = 0;
			while (nativeCircularBuffer.Length > 0)
			{
				int num16 = nativeCircularBuffer.PopStart();
				num15++;
				if (this.output.effectivelyReachedDestination[num16] != ReachedEndOfPath.Reached)
				{
					nativeArray3[num16] = false;
					float x = this.output.speed[num16];
					float3 float3 = this.agentData.endOfPath[num16];
					if (math.isfinite(float3.x))
					{
						float3 float4 = this.agentData.position[num16];
						bool blockedAndSlow = nativeArray4[num16].blockedAndSlow;
						float distToEndSq = nativeArray4[num16].distToEndSq;
						float num17 = this.agentData.radius[num16];
						bool flag7 = false;
						bool flag8 = false;
						for (int num18 = 0; num18 < 7; num18++)
						{
							int num19 = this.output.blockedByAgents[num16 * 7 + num18];
							if (num19 == -1)
							{
								break;
							}
							float3 lhs = this.agentData.endOfPath[num19];
							float num20 = this.agentData.radius[num19];
							bool flag9 = math.lengthsq(lhs - float3) <= distToEndSq * 0.25f;
							if (this.output.effectivelyReachedDestination[num19] == ReachedEndOfPath.Reached && (flag9 || math.lengthsq(float3 - this.agentData.position[num19]) < math.lengthsq(num17 + num20)))
							{
								float y = this.output.speed[num19];
								flag8 |= (math.min(x, y) < 0.01f);
								flag7 = (flag7 || blockedAndSlow);
							}
						}
						ReachedEndOfPath reachedEndOfPath2 = flag7 ? ReachedEndOfPath.Reached : (flag8 ? ReachedEndOfPath.ReachedSoon : ReachedEndOfPath.NotReached);
						reachedEndOfPath2 = (ReachedEndOfPath)math.max((int)reachedEndOfPath2, (int)this.output.effectivelyReachedDestination[num16]);
						if (reachedEndOfPath2 != this.output.effectivelyReachedDestination[num16])
						{
							this.output.effectivelyReachedDestination[num16] = reachedEndOfPath2;
							if (reachedEndOfPath2 == ReachedEndOfPath.Reached)
							{
								nativeArray3[num16] = true;
								int num21 = nativeArray2[num16];
								for (int num22 = 0; num22 < num21; num22++)
								{
									int num23 = nativeArray[num16 * 7 + num22];
									if (!nativeArray3[num23])
									{
										nativeCircularBuffer.PushEnd(num23);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000BE7 RID: 3047
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BE8 RID: 3048
		[ReadOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000BE9 RID: 3049
		public SimulatorBurst.AgentOutputData output;

		// Token: 0x04000BEA RID: 3050
		public int numAgents;

		// Token: 0x04000BEB RID: 3051
		public CommandBuilder draw;

		// Token: 0x04000BEC RID: 3052
		private static readonly ProfilerMarker MarkerInvert = new ProfilerMarker("InvertArrows");

		// Token: 0x04000BED RID: 3053
		private static readonly ProfilerMarker MarkerAlloc = new ProfilerMarker("Alloc");

		// Token: 0x04000BEE RID: 3054
		private static readonly ProfilerMarker MarkerFirstPass = new ProfilerMarker("FirstPass");

		// Token: 0x020002A4 RID: 676
		private struct TempAgentData
		{
			// Token: 0x04000BEF RID: 3055
			public bool blockedAndSlow;

			// Token: 0x04000BF0 RID: 3056
			public float distToEndSq;
		}
	}
}
