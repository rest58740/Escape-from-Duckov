using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x0200029B RID: 667
	[Serializable]
	public struct RVODestinationCrowdedBehavior
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x000614D0 File Offset: 0x0005F6D0
		public void ReadJobResult(ref RVODestinationCrowdedBehavior.JobDensityCheck jobResult, int index)
		{
			bool flag = jobResult.outThresholdResult[index];
			this.progressAverage = jobResult.progressAverage[index];
			this.lastJobDensityResult = flag;
			this.shouldStopDelayTimer = Mathf.Lerp(this.shouldStopDelayTimer, (float)(flag ? 1 : 0), Time.deltaTime);
			flag = (flag && this.shouldStopDelayTimer > 0.1f);
			this.lastShouldStopResult = flag;
			this.lastShouldStopDestination = jobResult.data[index].agentDestination;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00061558 File Offset: 0x0005F758
		public RVODestinationCrowdedBehavior(bool enabled, float densityFraction, bool returnAfterBeingPushedAway)
		{
			this.wasEnabled = enabled;
			this.enabled = enabled;
			this.densityThreshold = densityFraction;
			this.returnAfterBeingPushedAway = returnAfterBeingPushedAway;
			this.lastJobDensityResult = false;
			this.progressAverage = 0f;
			this.wasStopped = false;
			this.lastShouldStopDestination = new Vector3(float.NaN, float.NaN, float.NaN);
			this.reachedDestinationPoint = new Vector3(float.NaN, float.NaN, float.NaN);
			this.timer1 = 0f;
			this.shouldStopDelayTimer = 0f;
			this.reachedDestination = false;
			this.lastShouldStopResult = false;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x000615F4 File Offset: 0x0005F7F4
		public void ClearDestinationReached()
		{
			this.wasStopped = false;
			this.progressAverage = 1f;
			this.reachedDestination = false;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0006160F File Offset: 0x0005F80F
		public void OnDestinationChanged(Vector3 newDestination, bool reachedDestination)
		{
			this.timer1 = float.PositiveInfinity;
			this.reachedDestination = reachedDestination;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00061623 File Offset: 0x0005F823
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0006162B File Offset: 0x0005F82B
		public bool reachedDestination { readonly get; private set; }

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00061634 File Offset: 0x0005F834
		public void Update(bool rvoControllerEnabled, bool reachedDestination, ref bool isStopped, ref float rvoPriorityMultiplier, ref float rvoFlowFollowingStrength, Vector3 agentPosition)
		{
			if (!this.enabled || !rvoControllerEnabled)
			{
				if (this.wasEnabled)
				{
					this.wasEnabled = false;
					rvoPriorityMultiplier = 1f;
					rvoFlowFollowingStrength = 0f;
					this.timer1 = float.PositiveInfinity;
					this.progressAverage = 1f;
				}
				return;
			}
			this.wasEnabled = true;
			if (reachedDestination)
			{
				float sqrMagnitude = (agentPosition - this.reachedDestinationPoint).sqrMagnitude;
				if ((this.lastShouldStopDestination - this.reachedDestinationPoint).sqrMagnitude > sqrMagnitude)
				{
					this.reachedDestination = false;
				}
			}
			if (reachedDestination || this.lastShouldStopResult)
			{
				this.timer1 = 0f;
				this.reachedDestination = true;
				this.reachedDestinationPoint = this.lastShouldStopDestination;
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
				this.wasStopped |= (math.abs(this.progressAverage) < 0.1f);
				isStopped |= this.wasStopped;
				return;
			}
			if (isStopped)
			{
				this.timer1 = 0f;
				this.reachedDestination = false;
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
				this.wasStopped |= (math.abs(this.progressAverage) < 0.1f);
				return;
			}
			if (!this.reachedDestination)
			{
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 1f, Time.deltaTime * 4f);
				rvoFlowFollowingStrength = 0f;
				isStopped = false;
				this.wasStopped = false;
				return;
			}
			this.timer1 += Time.deltaTime;
			if (this.timer1 > 3f && this.returnAfterBeingPushedAway)
			{
				rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.5f, Time.deltaTime * 2f);
				rvoFlowFollowingStrength = 0f;
				isStopped = false;
				this.wasStopped = false;
				return;
			}
			rvoPriorityMultiplier = Mathf.Lerp(rvoPriorityMultiplier, 0.1f, Time.deltaTime * 2f);
			rvoFlowFollowingStrength = Mathf.Lerp(rvoFlowFollowingStrength, 1f, Time.deltaTime * 4f);
			this.wasStopped |= (math.abs(this.progressAverage) < 0.1f);
			isStopped = this.wasStopped;
		}

		// Token: 0x04000BAE RID: 2990
		public bool enabled;

		// Token: 0x04000BAF RID: 2991
		[Range(0f, 1f)]
		public float densityThreshold;

		// Token: 0x04000BB0 RID: 2992
		public bool returnAfterBeingPushedAway;

		// Token: 0x04000BB1 RID: 2993
		public float progressAverage;

		// Token: 0x04000BB2 RID: 2994
		private bool wasEnabled;

		// Token: 0x04000BB3 RID: 2995
		private float timer1;

		// Token: 0x04000BB4 RID: 2996
		private float shouldStopDelayTimer;

		// Token: 0x04000BB5 RID: 2997
		private bool lastShouldStopResult;

		// Token: 0x04000BB6 RID: 2998
		private Vector3 lastShouldStopDestination;

		// Token: 0x04000BB7 RID: 2999
		private Vector3 reachedDestinationPoint;

		// Token: 0x04000BB8 RID: 3000
		public bool lastJobDensityResult;

		// Token: 0x04000BB9 RID: 3001
		private const float MaximumCirclePackingDensity = 0.9069f;

		// Token: 0x04000BBB RID: 3003
		private bool wasStopped;

		// Token: 0x04000BBC RID: 3004
		private const float DefaultPriority = 1f;

		// Token: 0x04000BBD RID: 3005
		private const float StoppedPriority = 0.1f;

		// Token: 0x04000BBE RID: 3006
		private const float MoveBackPriority = 0.5f;

		// Token: 0x0200029C RID: 668
		[BurstCompile(CompileSynchronously = false, FloatMode = FloatMode.Fast)]
		public struct JobDensityCheck : IJobParallelForBatched
		{
			// Token: 0x1700022B RID: 555
			// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000185BF File Offset: 0x000167BF
			public bool allowBoundsChecks
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000FF3 RID: 4083 RVA: 0x000618A8 File Offset: 0x0005FAA8
			public JobDensityCheck(int size, float deltaTime, SimulatorBurst simulator)
			{
				this.agentPosition = simulator.simulationData.position;
				this.agentTargetPoint = simulator.simulationData.targetPoint;
				this.agentRadius = simulator.simulationData.radius;
				this.agentDesiredSpeed = simulator.simulationData.desiredSpeed;
				this.agentOutputTargetPoint = simulator.outputData.targetPoint;
				this.agentOutputSpeed = simulator.outputData.speed;
				this.quadtree = simulator.quadtree;
				this.data = new NativeArray<RVODestinationCrowdedBehavior.JobDensityCheck.QueryData>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.outThresholdResult = new NativeArray<bool>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.progressAverage = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
				this.deltaTime = deltaTime;
			}

			// Token: 0x06000FF4 RID: 4084 RVA: 0x00061958 File Offset: 0x0005FB58
			public void Dispose()
			{
				this.data.Dispose();
				this.outThresholdResult.Dispose();
				this.progressAverage.Dispose();
			}

			// Token: 0x06000FF5 RID: 4085 RVA: 0x0006197C File Offset: 0x0005FB7C
			public void Set(int index, int rvoAgentIndex, float3 destination, float densityThreshold, float progressAverage)
			{
				this.data[index] = new RVODestinationCrowdedBehavior.JobDensityCheck.QueryData
				{
					agentDestination = destination,
					densityThreshold = densityThreshold,
					agentIndex = rvoAgentIndex
				};
				this.progressAverage[index] = progressAverage;
			}

			// Token: 0x06000FF6 RID: 4086 RVA: 0x000619C8 File Offset: 0x0005FBC8
			void IJobParallelForBatched.Execute(int start, int count)
			{
				for (int i = start; i < start + count; i++)
				{
					this.Execute(i);
				}
			}

			// Token: 0x06000FF7 RID: 4087 RVA: 0x000619EA File Offset: 0x0005FBEA
			private float AgentDensityInCircle(float3 position, float radius)
			{
				return this.quadtree.QueryArea(position, radius) / (radius * radius * 3.1415927f);
			}

			// Token: 0x06000FF8 RID: 4088 RVA: 0x00061A04 File Offset: 0x0005FC04
			private void Execute(int i)
			{
				RVODestinationCrowdedBehavior.JobDensityCheck.QueryData queryData = this.data[i];
				float3 rhs = this.agentPosition[queryData.agentIndex];
				float num = this.agentRadius[queryData.agentIndex];
				float3 x = math.normalizesafe(this.agentTargetPoint[queryData.agentIndex] - rhs, default(float3));
				float num2;
				if (this.agentDesiredSpeed[queryData.agentIndex] > 0.01f)
				{
					num2 = math.dot(x, math.normalizesafe(this.agentOutputTargetPoint[queryData.agentIndex] - rhs, default(float3)) * this.agentOutputSpeed[queryData.agentIndex]) / math.max(0.001f, math.min(this.agentDesiredSpeed[queryData.agentIndex], this.agentRadius[queryData.agentIndex]));
					num2 = math.clamp(num2, -1f, 1f);
				}
				else
				{
					num2 = 1f;
				}
				this.progressAverage[i] = math.lerp(this.progressAverage[i], num2, 2f * this.deltaTime);
				if (math.any(math.isinf(queryData.agentDestination)))
				{
					this.outThresholdResult[i] = true;
					return;
				}
				float num3 = math.length(queryData.agentDestination - rhs);
				float num4 = num * 5f;
				if (num3 > num4 && this.AgentDensityInCircle(queryData.agentDestination, num4) < 0.9069f * queryData.densityThreshold)
				{
					this.outThresholdResult[i] = false;
					return;
				}
				this.outThresholdResult[i] = (this.AgentDensityInCircle(queryData.agentDestination, num3) > 0.9069f * queryData.densityThreshold);
			}

			// Token: 0x04000BBF RID: 3007
			[ReadOnly]
			private RVOQuadtreeBurst quadtree;

			// Token: 0x04000BC0 RID: 3008
			[ReadOnly]
			public NativeArray<RVODestinationCrowdedBehavior.JobDensityCheck.QueryData> data;

			// Token: 0x04000BC1 RID: 3009
			[ReadOnly]
			public NativeArray<float3> agentPosition;

			// Token: 0x04000BC2 RID: 3010
			[ReadOnly]
			private NativeArray<float3> agentTargetPoint;

			// Token: 0x04000BC3 RID: 3011
			[ReadOnly]
			private NativeArray<float> agentRadius;

			// Token: 0x04000BC4 RID: 3012
			[ReadOnly]
			private NativeArray<float> agentDesiredSpeed;

			// Token: 0x04000BC5 RID: 3013
			[ReadOnly]
			private NativeArray<float3> agentOutputTargetPoint;

			// Token: 0x04000BC6 RID: 3014
			[ReadOnly]
			private NativeArray<float> agentOutputSpeed;

			// Token: 0x04000BC7 RID: 3015
			[WriteOnly]
			public NativeArray<bool> outThresholdResult;

			// Token: 0x04000BC8 RID: 3016
			public NativeArray<float> progressAverage;

			// Token: 0x04000BC9 RID: 3017
			public float deltaTime;

			// Token: 0x0200029D RID: 669
			public struct QueryData
			{
				// Token: 0x04000BCA RID: 3018
				public float3 agentDestination;

				// Token: 0x04000BCB RID: 3019
				public int agentIndex;

				// Token: 0x04000BCC RID: 3020
				public float densityThreshold;
			}
		}
	}
}
