using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x0200029E RID: 670
	[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
	public struct JobRVOPreprocess<MovementPlaneWrapper> : IJob where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x00061BD8 File Offset: 0x0005FDD8
		public void Execute()
		{
			for (int i = this.startIndex; i < this.endIndex; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.locked[i] & !this.agentData.manuallyControlled[i])
					{
						this.temporaryAgentData.desiredTargetPointInVelocitySpace[i] = float2.zero;
						this.temporaryAgentData.desiredVelocity[i] = float3.zero;
						this.temporaryAgentData.currentVelocity[i] = float3.zero;
					}
					else
					{
						MovementPlaneWrapper movementPlaneWrapper = default(MovementPlaneWrapper);
						movementPlaneWrapper.Set(this.agentData.movementPlane[i]);
						float2 @float = movementPlaneWrapper.ToPlane(this.agentData.targetPoint[i] - this.agentData.position[i]);
						this.temporaryAgentData.desiredTargetPointInVelocitySpace[i] = @float;
						float3 float2 = math.normalizesafe(this.previousOutput.targetPoint[i] - this.agentData.position[i], default(float3)) * this.previousOutput.speed[i];
						this.temporaryAgentData.desiredVelocity[i] = movementPlaneWrapper.ToWorld(math.normalizesafe(@float, default(float2)) * this.agentData.desiredSpeed[i], 0f);
						float3 float3 = math.normalizesafe(this.agentData.collisionNormal[i], default(float3));
						float y = math.dot(float2, float3);
						float2 -= math.min(0f, y) * float3;
						this.temporaryAgentData.currentVelocity[i] = float2;
					}
				}
			}
		}

		// Token: 0x04000BCD RID: 3021
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BCE RID: 3022
		[ReadOnly]
		public SimulatorBurst.AgentOutputData previousOutput;

		// Token: 0x04000BCF RID: 3023
		[WriteOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000BD0 RID: 3024
		public int startIndex;

		// Token: 0x04000BD1 RID: 3025
		public int endIndex;
	}
}
