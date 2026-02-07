using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.ECS.RVO;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002B5 RID: 693
	public class SimulatorBurst
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x00065A9B File Offset: 0x00063C9B
		public SimulatorBurst.AgentNeighbourLookup GetAgentNeighbourLookup()
		{
			return new SimulatorBurst.AgentNeighbourLookup(this.temporaryAgentData.neighbours);
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x00065AAD File Offset: 0x00063CAD
		// (set) Token: 0x06001065 RID: 4197 RVA: 0x00065AB5 File Offset: 0x00063CB5
		public float DesiredDeltaTime
		{
			get
			{
				return this.desiredDeltaTime;
			}
			set
			{
				this.desiredDeltaTime = Math.Max(value, 0f);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x00065AC8 File Offset: 0x00063CC8
		// (set) Token: 0x06001067 RID: 4199 RVA: 0x00065AD0 File Offset: 0x00063CD0
		public float SymmetryBreakingBias { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00065AD9 File Offset: 0x00063CD9
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x00065AE1 File Offset: 0x00063CE1
		public bool HardCollisions { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x00065AEA File Offset: 0x00063CEA
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x00065AF2 File Offset: 0x00063CF2
		public bool UseNavmeshAsObstacle { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00065AFC File Offset: 0x00063CFC
		public Rect AgentBounds
		{
			get
			{
				this.rwLock.ReadSync().Unlock();
				return this.quadtree.bounds;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00065B27 File Offset: 0x00063D27
		public int AgentCount
		{
			get
			{
				return this.numAgents;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00065B2F File Offset: 0x00063D2F
		public MovementPlane MovementPlane
		{
			get
			{
				return this.movementPlane;
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00065B38 File Offset: 0x00063D38
		public void BlockUntilSimulationStepDone()
		{
			this.rwLock.WriteSync().Unlock();
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00065B58 File Offset: 0x00063D58
		public SimulatorBurst(MovementPlane movementPlane)
		{
			this.DesiredDeltaTime = 1f;
			this.movementPlane = movementPlane;
			this.AllocateAgentSpace();
			this.quadtree.BuildJob(this.simulationData.position, this.simulationData.version, this.simulationData.desiredSpeed, this.simulationData.radius, 0, movementPlane).Run<RVOQuadtreeBurst.JobBuild>();
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00065BFC File Offset: 0x00063DFC
		public void ClearAgents()
		{
			this.BlockUntilSimulationStepDone();
			for (int i = 0; i < this.agentDestroyCallbacks.Length; i++)
			{
				Action action = this.agentDestroyCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
			this.numAgents = 0;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00065C3C File Offset: 0x00063E3C
		public void OnDestroy()
		{
			this.debugDrawingScope.Dispose();
			this.BlockUntilSimulationStepDone();
			this.ClearAgents();
			this.simulationData.Dispose();
			this.temporaryAgentData.Dispose();
			this.outputData.Dispose();
			this.quadtree.Dispose();
			this.horizonAgentData.Dispose();
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00065C98 File Offset: 0x00063E98
		private void AllocateAgentSpace()
		{
			if (this.numAgents > this.agentPreCalculationCallbacks.Length || this.agentPreCalculationCallbacks.Length == 0)
			{
				int length = this.simulationData.version.Length;
				int num = Mathf.Max(64, Mathf.Max(this.numAgents, this.agentPreCalculationCallbacks.Length * 2));
				this.simulationData.Realloc(num, Allocator.Persistent);
				this.temporaryAgentData.Realloc(num, Allocator.Persistent);
				this.outputData.Realloc(num, Allocator.Persistent);
				this.horizonAgentData.Realloc(num, Allocator.Persistent);
				Memory.Realloc<Action>(ref this.agentPreCalculationCallbacks, num);
				Memory.Realloc<Action>(ref this.agentDestroyCallbacks, num);
				for (int i = length; i < num; i++)
				{
					this.simulationData.version[i] = new AgentIndex(0, i);
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x00065D5D File Offset: 0x00063F5D
		public bool anyAgentsInSimulation
		{
			get
			{
				return this.numAgents > this.freeAgentIndices.Count;
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00065D74 File Offset: 0x00063F74
		public IAgent AddAgent(Vector3 position)
		{
			AgentIndex agentIndex = this.AddAgentBurst(position);
			return new SimulatorBurst.Agent
			{
				simulator = this,
				agentIndex = agentIndex
			};
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00065DAC File Offset: 0x00063FAC
		public AgentIndex AddAgentBurst(float3 position)
		{
			this.BlockUntilSimulationStepDone();
			int num;
			if (this.freeAgentIndices.Count > 0)
			{
				num = this.freeAgentIndices.Pop();
			}
			else
			{
				int num2 = this.numAgents;
				this.numAgents = num2 + 1;
				num = num2;
				this.AllocateAgentSpace();
			}
			AgentIndex agentIndex = this.simulationData.version[num].WithIncrementedVersion();
			this.simulationData.version[num] = agentIndex;
			this.simulationData.radius[num] = 5f;
			this.simulationData.height[num] = 5f;
			this.simulationData.desiredSpeed[num] = 0f;
			this.simulationData.maxSpeed[num] = 1f;
			this.simulationData.agentTimeHorizon[num] = 2f;
			this.simulationData.obstacleTimeHorizon[num] = 2f;
			this.simulationData.locked[num] = false;
			this.simulationData.maxNeighbours[num] = 10;
			this.simulationData.layer[num] = RVOLayer.DefaultAgent;
			this.simulationData.collidesWith[num] = (RVOLayer)(-1);
			this.simulationData.flowFollowingStrength[num] = 0f;
			this.simulationData.position[num] = position;
			this.simulationData.collisionNormal[num] = float3.zero;
			this.simulationData.manuallyControlled[num] = false;
			this.simulationData.priority[num] = 0.5f;
			this.simulationData.debugFlags[num] = AgentDebugFlags.Nothing;
			this.simulationData.targetPoint[num] = position;
			this.simulationData.movementPlane[num] = new NativeMovementPlane(((this.movementPlane == MovementPlane.XY) ? SimpleMovementPlane.XYPlane : SimpleMovementPlane.XZPlane).rotation);
			this.simulationData.allowedVelocityDeviationAngles[num] = float2.zero;
			this.simulationData.endOfPath[num] = new float3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			this.simulationData.agentObstacleMapping[num] = -1;
			this.simulationData.hierarchicalNodeIndex[num] = -1;
			this.outputData.speed[num] = 0f;
			this.outputData.numNeighbours[num] = 0;
			this.outputData.targetPoint[num] = position;
			this.outputData.blockedByAgents[num * 7] = -1;
			this.outputData.effectivelyReachedDestination[num] = ReachedEndOfPath.NotReached;
			this.temporaryAgentData.neighbours[num * 50] = -1;
			this.horizonAgentData.horizonSide[num] = 0;
			this.agentPreCalculationCallbacks[num] = null;
			this.agentDestroyCallbacks[num] = null;
			return agentIndex;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000035D8 File Offset: 0x000017D8
		[Obsolete("Use AddAgent(Vector3) instead", true)]
		public IAgent AddAgent(IAgent agent)
		{
			return null;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x000660A4 File Offset: 0x000642A4
		public void RemoveAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("agent");
			}
			SimulatorBurst.Agent agent2 = (SimulatorBurst.Agent)agent;
			this.RemoveAgent(agent2.agentIndex);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000660D4 File Offset: 0x000642D4
		public void RemoveAgent(AgentIndex agent)
		{
			this.BlockUntilSimulationStepDone();
			int num;
			if (!agent.TryGetIndex(ref this.simulationData, out num))
			{
				throw new InvalidOperationException("Trying to remove agent which does not exist");
			}
			this.simulationData.version[num] = this.simulationData.version[num].WithIncrementedVersion().WithDeleted();
			this.agentPreCalculationCallbacks[num] = null;
			try
			{
				if (this.agentDestroyCallbacks[num] != null)
				{
					this.agentDestroyCallbacks[num]();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.agentDestroyCallbacks[num] = null;
			this.freeAgentIndices.Push(num);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00066184 File Offset: 0x00064384
		private void PreCalculation(JobHandle dependency)
		{
			bool flag = false;
			for (int i = 0; i < this.numAgents; i++)
			{
				Action action = this.agentPreCalculationCallbacks[i];
				if (action != null)
				{
					if (!flag)
					{
						dependency.Complete();
						this.rwLock.ReadSync().Unlock();
						flag = true;
					}
					action();
				}
			}
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000661D8 File Offset: 0x000643D8
		public JobHandle Update(JobHandle dependency, float dt, bool drawGizmos, Allocator allocator)
		{
			if (false)
			{
				default(JobRVO<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVO<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVO<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOPreprocess<XYMovementPlane>).Schedule(default(JobHandle));
				default(JobRVOPreprocess<XZMovementPlane>).Schedule(default(JobHandle));
				default(JobRVOPreprocess<ArbitraryMovementPlane>).Schedule(default(JobHandle));
				default(JobHorizonAvoidancePhase1<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase1<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase1<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHorizonAvoidancePhase2<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobRVOCalculateNeighbours<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<XYMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<XZMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobHardCollisions<ArbitraryMovementPlane>).ScheduleBatch(0, 0, default(JobHandle));
				default(JobDestinationReached<XYMovementPlane>).Schedule(default(JobHandle));
				default(JobDestinationReached<XZMovementPlane>).Schedule(default(JobHandle));
				default(JobDestinationReached<ArbitraryMovementPlane>).Schedule(default(JobHandle));
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				return this.UpdateInternal<XYMovementPlane>(dependency, dt, drawGizmos, allocator);
			}
			if (this.movementPlane == MovementPlane.XZ)
			{
				return this.UpdateInternal<XZMovementPlane>(dependency, dt, drawGizmos, allocator);
			}
			return this.UpdateInternal<ArbitraryMovementPlane>(dependency, dt, drawGizmos, allocator);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00066447 File Offset: 0x00064647
		public RWLock.ReadLockAsync LockSimulationDataReadOnly()
		{
			return this.rwLock.Read();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00066454 File Offset: 0x00064654
		public RWLock.WriteLockAsync LockSimulationDataReadWrite()
		{
			return this.rwLock.Write();
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00066464 File Offset: 0x00064664
		private JobHandle UpdateInternal<T>(JobHandle dependency, float deltaTime, bool drawGizmos, Allocator allocator) where T : struct, IMovementPlaneWrapper
		{
			if (!this.anyAgentsInSimulation)
			{
				return dependency;
			}
			deltaTime = math.max(deltaTime, 0.0005f);
			this.PreCalculation(dependency);
			RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
			dependency = JobHandle.CombineDependencies(dependency, writeLockAsync.dependency);
			JobHandle jobHandle = this.quadtree.BuildJob(this.simulationData.position, this.simulationData.version, this.outputData.speed, this.simulationData.radius, this.numAgents, this.movementPlane).Schedule(dependency);
			JobHandle job = new JobRVOPreprocess<T>
			{
				agentData = this.simulationData,
				previousOutput = this.outputData,
				temporaryAgentData = this.temporaryAgentData,
				startIndex = 0,
				endIndex = this.numAgents
			}.Schedule(dependency);
			int minIndicesPerJobCount = math.max(this.numAgents / 64, 8);
			JobHandle job2 = new JobRVOCalculateNeighbours<T>
			{
				agentData = this.simulationData,
				quadtree = this.quadtree,
				outNeighbours = this.temporaryAgentData.neighbours,
				output = this.outputData
			}.ScheduleBatch(this.numAgents, minIndicesPerJobCount, JobHandle.CombineDependencies(job, jobHandle));
			JobHandle.ScheduleBatchedJobs();
			JobHandle dependsOn = JobHandle.CombineDependencies(job, job2);
			this.debugDrawingScope.Rewind();
			CommandBuilder builder = DrawingManager.GetBuilder(this.debugDrawingScope, false);
			JobHandle dependsOn2 = new JobHorizonAvoidancePhase1<T>
			{
				agentData = this.simulationData,
				neighbours = this.temporaryAgentData.neighbours,
				desiredTargetPointInVelocitySpace = this.temporaryAgentData.desiredTargetPointInVelocitySpace,
				horizonAgentData = this.horizonAgentData,
				draw = builder
			}.ScheduleBatch(this.numAgents, minIndicesPerJobCount, dependsOn);
			JobHandle job3 = new JobHorizonAvoidancePhase2<T>
			{
				neighbours = this.temporaryAgentData.neighbours,
				versions = this.simulationData.version,
				desiredVelocity = this.temporaryAgentData.desiredVelocity,
				desiredTargetPointInVelocitySpace = this.temporaryAgentData.desiredTargetPointInVelocitySpace,
				horizonAgentData = this.horizonAgentData,
				movementPlane = this.simulationData.movementPlane
			}.ScheduleBatch(this.numAgents, minIndicesPerJobCount, dependsOn2);
			JobHandle job4 = new JobHardCollisions<T>
			{
				agentData = this.simulationData,
				neighbours = this.temporaryAgentData.neighbours,
				collisionVelocityOffsets = this.temporaryAgentData.collisionVelocityOffsets,
				deltaTime = deltaTime,
				enabled = this.HardCollisions
			}.ScheduleBatch(this.numAgents, minIndicesPerJobCount, dependsOn);
			bool flag = AstarPath.active != null;
			RWLock.CombinedReadLockAsync combinedReadLockAsync;
			NavmeshEdges.NavmeshBorderData navmeshEdgeData;
			if (flag)
			{
				navmeshEdgeData = AstarPath.active.GetNavmeshBorderData(out combinedReadLockAsync);
			}
			else
			{
				navmeshEdgeData = NavmeshEdges.NavmeshBorderData.CreateEmpty(allocator);
				combinedReadLockAsync = default(RWLock.CombinedReadLockAsync);
			}
			JobRVO<T> jobData = new JobRVO<T>
			{
				agentData = this.simulationData,
				temporaryAgentData = this.temporaryAgentData,
				navmeshEdgeData = navmeshEdgeData,
				output = this.outputData,
				deltaTime = deltaTime,
				symmetryBreakingBias = Mathf.Max(0f, this.SymmetryBreakingBias),
				draw = builder,
				useNavmeshAsObstacle = this.UseNavmeshAsObstacle,
				priorityMultiplier = 1f
			};
			dependsOn = JobHandle.CombineDependencies(job3, job4, combinedReadLockAsync.dependency);
			JobHandle jobHandle2 = jobData.ScheduleBatch(this.numAgents, minIndicesPerJobCount, dependsOn);
			if (flag)
			{
				combinedReadLockAsync.UnlockAfter(jobHandle2);
			}
			else
			{
				navmeshEdgeData.DisposeEmpty(jobHandle2);
			}
			JobHandle jobHandle3 = new JobDestinationReached<T>
			{
				agentData = this.simulationData,
				temporaryAgentData = this.temporaryAgentData,
				output = this.outputData,
				draw = builder,
				numAgents = this.numAgents
			}.Schedule(jobHandle2);
			JobHandle job5 = this.simulationData.collisionNormal.MemSet(float3.zero).Schedule(jobHandle3);
			JobHandle job6 = this.simulationData.manuallyControlled.MemSet(false).Schedule(jobHandle3);
			JobHandle job7 = this.simulationData.hierarchicalNodeIndex.MemSet(-1).Schedule(jobHandle3);
			dependency = JobHandle.CombineDependencies(jobHandle3, job5, job6);
			dependency = JobHandle.CombineDependencies(dependency, job7);
			if (this.drawQuadtree && drawGizmos)
			{
				dependency = JobHandle.CombineDependencies(dependency, new RVOQuadtreeBurst.DebugDrawJob
				{
					draw = builder,
					quadtree = this.quadtree
				}.Schedule(jobHandle));
			}
			builder.DisposeAfter(dependency, AllowedDelay.EndOfFrame);
			writeLockAsync.UnlockAfter(dependency);
			return dependency;
		}

		// Token: 0x04000C3D RID: 3133
		private float desiredDeltaTime = 0.05f;

		// Token: 0x04000C3E RID: 3134
		private int numAgents;

		// Token: 0x04000C3F RID: 3135
		private RedrawScope debugDrawingScope;

		// Token: 0x04000C40 RID: 3136
		public RVOQuadtreeBurst quadtree;

		// Token: 0x04000C41 RID: 3137
		public bool drawQuadtree;

		// Token: 0x04000C42 RID: 3138
		private Action[] agentPreCalculationCallbacks = new Action[0];

		// Token: 0x04000C43 RID: 3139
		private Action[] agentDestroyCallbacks = new Action[0];

		// Token: 0x04000C44 RID: 3140
		private Stack<int> freeAgentIndices = new Stack<int>();

		// Token: 0x04000C45 RID: 3141
		private SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000C46 RID: 3142
		private SimulatorBurst.HorizonAgentData horizonAgentData;

		// Token: 0x04000C47 RID: 3143
		public SimulatorBurst.AgentData simulationData;

		// Token: 0x04000C48 RID: 3144
		public SimulatorBurst.AgentOutputData outputData;

		// Token: 0x04000C49 RID: 3145
		public const int MaxNeighbourCount = 50;

		// Token: 0x04000C4A RID: 3146
		public const int MaxBlockingAgentCount = 7;

		// Token: 0x04000C4B RID: 3147
		public const int MaxObstacleVertices = 256;

		// Token: 0x04000C4F RID: 3151
		public readonly MovementPlane movementPlane;

		// Token: 0x04000C50 RID: 3152
		private RWLock rwLock = new RWLock();

		// Token: 0x020002B6 RID: 694
		public struct AgentNeighbourLookup
		{
			// Token: 0x0600107F RID: 4223 RVA: 0x000668F7 File Offset: 0x00064AF7
			public AgentNeighbourLookup(NativeArray<int> neighbours)
			{
				this.neighbours = neighbours;
			}

			// Token: 0x06001080 RID: 4224 RVA: 0x00066900 File Offset: 0x00064B00
			public UnsafeSpan<int> GetNeighbours(int agentIndex)
			{
				int num = agentIndex * 50;
				int num2 = num;
				while (this.neighbours[num2] != -1)
				{
					num2++;
				}
				return this.neighbours.AsUnsafeReadOnlySpan<int>().Slice(num, num2 - num);
			}

			// Token: 0x04000C51 RID: 3153
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			private NativeArray<int> neighbours;
		}

		// Token: 0x020002B7 RID: 695
		private struct Agent : IAgent
		{
			// Token: 0x17000253 RID: 595
			// (get) Token: 0x06001081 RID: 4225 RVA: 0x00066940 File Offset: 0x00064B40
			public int AgentIndex
			{
				get
				{
					return this.agentIndex.Index;
				}
			}

			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06001082 RID: 4226 RVA: 0x0006694D File Offset: 0x00064B4D
			// (set) Token: 0x06001083 RID: 4227 RVA: 0x0006696F File Offset: 0x00064B6F
			public Vector3 Position
			{
				get
				{
					return this.simulator.simulationData.position[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.position[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06001084 RID: 4228 RVA: 0x00066992 File Offset: 0x00064B92
			// (set) Token: 0x06001085 RID: 4229 RVA: 0x000669AF File Offset: 0x00064BAF
			public bool Locked
			{
				get
				{
					return this.simulator.simulationData.locked[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.locked[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06001086 RID: 4230 RVA: 0x000669CD File Offset: 0x00064BCD
			// (set) Token: 0x06001087 RID: 4231 RVA: 0x000669EA File Offset: 0x00064BEA
			public float Radius
			{
				get
				{
					return this.simulator.simulationData.radius[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.radius[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x06001088 RID: 4232 RVA: 0x00066A08 File Offset: 0x00064C08
			// (set) Token: 0x06001089 RID: 4233 RVA: 0x00066A25 File Offset: 0x00064C25
			public float Height
			{
				get
				{
					return this.simulator.simulationData.height[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.height[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000258 RID: 600
			// (get) Token: 0x0600108A RID: 4234 RVA: 0x00066A43 File Offset: 0x00064C43
			// (set) Token: 0x0600108B RID: 4235 RVA: 0x00066A60 File Offset: 0x00064C60
			public float AgentTimeHorizon
			{
				get
				{
					return this.simulator.simulationData.agentTimeHorizon[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.agentTimeHorizon[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x0600108C RID: 4236 RVA: 0x00066A7E File Offset: 0x00064C7E
			// (set) Token: 0x0600108D RID: 4237 RVA: 0x00066A9B File Offset: 0x00064C9B
			public float ObstacleTimeHorizon
			{
				get
				{
					return this.simulator.simulationData.obstacleTimeHorizon[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.obstacleTimeHorizon[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x0600108E RID: 4238 RVA: 0x00066AB9 File Offset: 0x00064CB9
			// (set) Token: 0x0600108F RID: 4239 RVA: 0x00066AD6 File Offset: 0x00064CD6
			public int MaxNeighbours
			{
				get
				{
					return this.simulator.simulationData.maxNeighbours[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.maxNeighbours[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025B RID: 603
			// (get) Token: 0x06001090 RID: 4240 RVA: 0x00066AF4 File Offset: 0x00064CF4
			// (set) Token: 0x06001091 RID: 4241 RVA: 0x00066B11 File Offset: 0x00064D11
			public RVOLayer Layer
			{
				get
				{
					return this.simulator.simulationData.layer[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.layer[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025C RID: 604
			// (get) Token: 0x06001092 RID: 4242 RVA: 0x00066B2F File Offset: 0x00064D2F
			// (set) Token: 0x06001093 RID: 4243 RVA: 0x00066B4C File Offset: 0x00064D4C
			public RVOLayer CollidesWith
			{
				get
				{
					return this.simulator.simulationData.collidesWith[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.collidesWith[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025D RID: 605
			// (get) Token: 0x06001094 RID: 4244 RVA: 0x00066B6A File Offset: 0x00064D6A
			// (set) Token: 0x06001095 RID: 4245 RVA: 0x00066B87 File Offset: 0x00064D87
			public float FlowFollowingStrength
			{
				get
				{
					return this.simulator.simulationData.flowFollowingStrength[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.flowFollowingStrength[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025E RID: 606
			// (get) Token: 0x06001096 RID: 4246 RVA: 0x00066BA5 File Offset: 0x00064DA5
			// (set) Token: 0x06001097 RID: 4247 RVA: 0x00066BC2 File Offset: 0x00064DC2
			public AgentDebugFlags DebugFlags
			{
				get
				{
					return this.simulator.simulationData.debugFlags[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.debugFlags[this.AgentIndex] = value;
				}
			}

			// Token: 0x1700025F RID: 607
			// (get) Token: 0x06001098 RID: 4248 RVA: 0x00066BE0 File Offset: 0x00064DE0
			// (set) Token: 0x06001099 RID: 4249 RVA: 0x00066BFD File Offset: 0x00064DFD
			public float Priority
			{
				get
				{
					return this.simulator.simulationData.priority[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.priority[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000260 RID: 608
			// (get) Token: 0x0600109A RID: 4250 RVA: 0x00066C1B File Offset: 0x00064E1B
			// (set) Token: 0x0600109B RID: 4251 RVA: 0x00066C38 File Offset: 0x00064E38
			public int HierarchicalNodeIndex
			{
				get
				{
					return this.simulator.simulationData.hierarchicalNodeIndex[this.AgentIndex];
				}
				set
				{
					this.simulator.simulationData.hierarchicalNodeIndex[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x0600109C RID: 4252 RVA: 0x00066C56 File Offset: 0x00064E56
			// (set) Token: 0x0600109D RID: 4253 RVA: 0x00066C82 File Offset: 0x00064E82
			public SimpleMovementPlane MovementPlane
			{
				get
				{
					return new SimpleMovementPlane(this.simulator.simulationData.movementPlane[this.AgentIndex].rotation);
				}
				set
				{
					this.simulator.simulationData.movementPlane[this.AgentIndex] = new NativeMovementPlane(value);
				}
			}

			// Token: 0x17000262 RID: 610
			// (set) Token: 0x0600109E RID: 4254 RVA: 0x00066CA5 File Offset: 0x00064EA5
			public Action PreCalculationCallback
			{
				set
				{
					this.simulator.agentPreCalculationCallbacks[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000263 RID: 611
			// (set) Token: 0x0600109F RID: 4255 RVA: 0x00066CBA File Offset: 0x00064EBA
			public Action DestroyedCallback
			{
				set
				{
					this.simulator.agentDestroyCallbacks[this.AgentIndex] = value;
				}
			}

			// Token: 0x17000264 RID: 612
			// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00066CCF File Offset: 0x00064ECF
			public Vector3 CalculatedTargetPoint
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.targetPoint[this.AgentIndex];
				}
			}

			// Token: 0x17000265 RID: 613
			// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00066CFC File Offset: 0x00064EFC
			public float CalculatedSpeed
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.speed[this.AgentIndex];
				}
			}

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00066D24 File Offset: 0x00064F24
			public ReachedEndOfPath CalculatedEffectivelyReachedDestination
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.effectivelyReachedDestination[this.AgentIndex];
				}
			}

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00066D4C File Offset: 0x00064F4C
			public int NeighbourCount
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.numNeighbours[this.AgentIndex];
				}
			}

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00066D74 File Offset: 0x00064F74
			public bool AvoidingAnyAgents
			{
				get
				{
					this.simulator.BlockUntilSimulationStepDone();
					return this.simulator.outputData.blockedByAgents[this.AgentIndex * 7] != -1;
				}
			}

			// Token: 0x060010A5 RID: 4261 RVA: 0x00066DA4 File Offset: 0x00064FA4
			public void SetObstacleQuery(GraphNode sourceNode)
			{
				this.HierarchicalNodeIndex = ((sourceNode != null && !sourceNode.Destroyed && sourceNode.Walkable) ? sourceNode.HierarchicalNodeIndex : -1);
			}

			// Token: 0x060010A6 RID: 4262 RVA: 0x00066DC8 File Offset: 0x00064FC8
			public void SetTarget(Vector3 targetPoint, float desiredSpeed, float maxSpeed, Vector3 endOfPath)
			{
				this.simulator.simulationData.SetTarget(this.AgentIndex, targetPoint, desiredSpeed, maxSpeed, endOfPath);
			}

			// Token: 0x060010A7 RID: 4263 RVA: 0x00066DEF File Offset: 0x00064FEF
			public void SetCollisionNormal(Vector3 normal)
			{
				this.simulator.simulationData.collisionNormal[this.AgentIndex] = normal;
			}

			// Token: 0x060010A8 RID: 4264 RVA: 0x00066E14 File Offset: 0x00065014
			public void ForceSetVelocity(Vector3 velocity)
			{
				this.simulator.simulationData.targetPoint[this.AgentIndex] = this.simulator.simulationData.position[this.AgentIndex] + velocity * 1000f;
				this.simulator.simulationData.desiredSpeed[this.AgentIndex] = velocity.magnitude;
				this.simulator.simulationData.allowedVelocityDeviationAngles[this.AgentIndex] = float2.zero;
				this.simulator.simulationData.manuallyControlled[this.AgentIndex] = true;
			}

			// Token: 0x04000C52 RID: 3154
			public SimulatorBurst simulator;

			// Token: 0x04000C53 RID: 3155
			public AgentIndex agentIndex;
		}

		// Token: 0x020002B8 RID: 696
		public struct ObstacleData
		{
			// Token: 0x060010A9 RID: 4265 RVA: 0x00066ECC File Offset: 0x000650CC
			public void Init(Allocator allocator)
			{
				if (!this.obstacles.IsCreated)
				{
					this.obstacles = new NativeList<UnmanagedObstacle>(0, allocator);
				}
				if (!this.obstacleVertexGroups.IsCreated)
				{
					this.obstacleVertexGroups = new SlabAllocator<ObstacleVertexGroup>(4, allocator);
				}
				if (!this.obstacleVertices.IsCreated)
				{
					this.obstacleVertices = new SlabAllocator<float3>(16, allocator);
				}
			}

			// Token: 0x060010AA RID: 4266 RVA: 0x00066F37 File Offset: 0x00065137
			public void Dispose()
			{
				if (this.obstacleVertexGroups.IsCreated)
				{
					this.obstacleVertexGroups.Dispose();
					this.obstacleVertices.Dispose();
					this.obstacles.Dispose();
				}
			}

			// Token: 0x04000C54 RID: 3156
			public SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups;

			// Token: 0x04000C55 RID: 3157
			public SlabAllocator<float3> obstacleVertices;

			// Token: 0x04000C56 RID: 3158
			public NativeList<UnmanagedObstacle> obstacles;
		}

		// Token: 0x020002B9 RID: 697
		public struct AgentData
		{
			// Token: 0x060010AB RID: 4267 RVA: 0x00066F68 File Offset: 0x00065168
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<AgentIndex>(ref this.version, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.radius, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.height, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.desiredSpeed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.maxSpeed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.agentTimeHorizon, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.obstacleTimeHorizon, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<bool>(ref this.locked, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.maxNeighbours, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<RVOLayer>(ref this.layer, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<RVOLayer>(ref this.collidesWith, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.flowFollowingStrength, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.position, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.collisionNormal, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<bool>(ref this.manuallyControlled, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.priority, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<AgentDebugFlags>(ref this.debugFlags, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.targetPoint, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<NativeMovementPlane>(ref this.movementPlane, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float2>(ref this.allowedVelocityDeviationAngles, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.endOfPath, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.agentObstacleMapping, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.hierarchicalNodeIndex, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060010AC RID: 4268 RVA: 0x000670B8 File Offset: 0x000652B8
			public void SetTarget(int agentIndex, float3 targetPoint, float desiredSpeed, float maxSpeed, float3 endOfPath)
			{
				maxSpeed = math.max(maxSpeed, 0f);
				desiredSpeed = math.clamp(desiredSpeed, 0f, maxSpeed);
				this.targetPoint[agentIndex] = targetPoint;
				this.desiredSpeed[agentIndex] = desiredSpeed;
				this.maxSpeed[agentIndex] = maxSpeed;
				this.endOfPath[agentIndex] = endOfPath;
			}

			// Token: 0x060010AD RID: 4269 RVA: 0x00067118 File Offset: 0x00065318
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool HasDebugFlag(int agentIndex, AgentDebugFlags flag)
			{
				return Hint.Unlikely((this.debugFlags[agentIndex] & flag) > AgentDebugFlags.Nothing);
			}

			// Token: 0x060010AE RID: 4270 RVA: 0x00067130 File Offset: 0x00065330
			public void Dispose()
			{
				this.version.Dispose();
				this.radius.Dispose();
				this.height.Dispose();
				this.desiredSpeed.Dispose();
				this.maxSpeed.Dispose();
				this.agentTimeHorizon.Dispose();
				this.obstacleTimeHorizon.Dispose();
				this.locked.Dispose();
				this.maxNeighbours.Dispose();
				this.layer.Dispose();
				this.collidesWith.Dispose();
				this.flowFollowingStrength.Dispose();
				this.position.Dispose();
				this.collisionNormal.Dispose();
				this.manuallyControlled.Dispose();
				this.priority.Dispose();
				this.debugFlags.Dispose();
				this.targetPoint.Dispose();
				this.movementPlane.Dispose();
				this.allowedVelocityDeviationAngles.Dispose();
				this.endOfPath.Dispose();
				this.agentObstacleMapping.Dispose();
				this.hierarchicalNodeIndex.Dispose();
			}

			// Token: 0x04000C57 RID: 3159
			public NativeArray<AgentIndex> version;

			// Token: 0x04000C58 RID: 3160
			public NativeArray<float> radius;

			// Token: 0x04000C59 RID: 3161
			public NativeArray<float> height;

			// Token: 0x04000C5A RID: 3162
			public NativeArray<float> desiredSpeed;

			// Token: 0x04000C5B RID: 3163
			public NativeArray<float> maxSpeed;

			// Token: 0x04000C5C RID: 3164
			public NativeArray<float> agentTimeHorizon;

			// Token: 0x04000C5D RID: 3165
			public NativeArray<float> obstacleTimeHorizon;

			// Token: 0x04000C5E RID: 3166
			public NativeArray<bool> locked;

			// Token: 0x04000C5F RID: 3167
			public NativeArray<int> maxNeighbours;

			// Token: 0x04000C60 RID: 3168
			public NativeArray<RVOLayer> layer;

			// Token: 0x04000C61 RID: 3169
			public NativeArray<RVOLayer> collidesWith;

			// Token: 0x04000C62 RID: 3170
			public NativeArray<float> flowFollowingStrength;

			// Token: 0x04000C63 RID: 3171
			public NativeArray<float3> position;

			// Token: 0x04000C64 RID: 3172
			public NativeArray<float3> collisionNormal;

			// Token: 0x04000C65 RID: 3173
			public NativeArray<bool> manuallyControlled;

			// Token: 0x04000C66 RID: 3174
			public NativeArray<float> priority;

			// Token: 0x04000C67 RID: 3175
			public NativeArray<AgentDebugFlags> debugFlags;

			// Token: 0x04000C68 RID: 3176
			public NativeArray<float3> targetPoint;

			// Token: 0x04000C69 RID: 3177
			public NativeArray<float2> allowedVelocityDeviationAngles;

			// Token: 0x04000C6A RID: 3178
			public NativeArray<NativeMovementPlane> movementPlane;

			// Token: 0x04000C6B RID: 3179
			public NativeArray<float3> endOfPath;

			// Token: 0x04000C6C RID: 3180
			public NativeArray<int> agentObstacleMapping;

			// Token: 0x04000C6D RID: 3181
			public NativeArray<int> hierarchicalNodeIndex;
		}

		// Token: 0x020002BA RID: 698
		public struct AgentOutputData
		{
			// Token: 0x060010AF RID: 4271 RVA: 0x0006723C File Offset: 0x0006543C
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<float3>(ref this.targetPoint, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.speed, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.numNeighbours, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.blockedByAgents, size * 7, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<ReachedEndOfPath>(ref this.effectivelyReachedDestination, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.forwardClearance, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060010B0 RID: 4272 RVA: 0x000672A0 File Offset: 0x000654A0
			public void Move(int fromIndex, int toIndex)
			{
				this.targetPoint[toIndex] = this.targetPoint[fromIndex];
				this.speed[toIndex] = this.speed[fromIndex];
				this.numNeighbours[toIndex] = this.numNeighbours[fromIndex];
				this.effectivelyReachedDestination[toIndex] = this.effectivelyReachedDestination[fromIndex];
				for (int i = 0; i < 7; i++)
				{
					this.blockedByAgents[toIndex * 7 + i] = this.blockedByAgents[fromIndex * 7 + i];
				}
				this.forwardClearance[toIndex] = this.forwardClearance[fromIndex];
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x00067354 File Offset: 0x00065554
			public void Dispose()
			{
				this.targetPoint.Dispose();
				this.speed.Dispose();
				this.numNeighbours.Dispose();
				this.blockedByAgents.Dispose();
				this.effectivelyReachedDestination.Dispose();
				this.forwardClearance.Dispose();
			}

			// Token: 0x04000C6E RID: 3182
			public NativeArray<float3> targetPoint;

			// Token: 0x04000C6F RID: 3183
			public NativeArray<float> speed;

			// Token: 0x04000C70 RID: 3184
			public NativeArray<int> numNeighbours;

			// Token: 0x04000C71 RID: 3185
			[NativeDisableParallelForRestriction]
			public NativeArray<int> blockedByAgents;

			// Token: 0x04000C72 RID: 3186
			public NativeArray<ReachedEndOfPath> effectivelyReachedDestination;

			// Token: 0x04000C73 RID: 3187
			public NativeArray<float> forwardClearance;
		}

		// Token: 0x020002BB RID: 699
		public struct HorizonAgentData
		{
			// Token: 0x060010B2 RID: 4274 RVA: 0x000673A3 File Offset: 0x000655A3
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<int>(ref this.horizonSide, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.horizonMinAngle, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float>(ref this.horizonMaxAngle, size, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060010B3 RID: 4275 RVA: 0x000673CF File Offset: 0x000655CF
			public void Move(int fromIndex, int toIndex)
			{
				this.horizonSide[toIndex] = this.horizonSide[fromIndex];
			}

			// Token: 0x060010B4 RID: 4276 RVA: 0x000673E9 File Offset: 0x000655E9
			public void Dispose()
			{
				this.horizonSide.Dispose();
				this.horizonMinAngle.Dispose();
				this.horizonMaxAngle.Dispose();
			}

			// Token: 0x04000C74 RID: 3188
			public NativeArray<int> horizonSide;

			// Token: 0x04000C75 RID: 3189
			public NativeArray<float> horizonMinAngle;

			// Token: 0x04000C76 RID: 3190
			public NativeArray<float> horizonMaxAngle;
		}

		// Token: 0x020002BC RID: 700
		public struct TemporaryAgentData
		{
			// Token: 0x060010B5 RID: 4277 RVA: 0x0006740C File Offset: 0x0006560C
			public void Realloc(int size, Allocator allocator)
			{
				Memory.Realloc<float2>(ref this.desiredTargetPointInVelocitySpace, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.desiredVelocity, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float3>(ref this.currentVelocity, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<float2>(ref this.collisionVelocityOffsets, size, allocator, NativeArrayOptions.UninitializedMemory);
				Memory.Realloc<int>(ref this.neighbours, size * 50, allocator, NativeArrayOptions.UninitializedMemory);
			}

			// Token: 0x060010B6 RID: 4278 RVA: 0x00067462 File Offset: 0x00065662
			public void Dispose()
			{
				this.desiredTargetPointInVelocitySpace.Dispose();
				this.desiredVelocity.Dispose();
				this.currentVelocity.Dispose();
				this.neighbours.Dispose();
				this.collisionVelocityOffsets.Dispose();
			}

			// Token: 0x04000C77 RID: 3191
			public NativeArray<float2> desiredTargetPointInVelocitySpace;

			// Token: 0x04000C78 RID: 3192
			public NativeArray<float3> desiredVelocity;

			// Token: 0x04000C79 RID: 3193
			public NativeArray<float3> currentVelocity;

			// Token: 0x04000C7A RID: 3194
			public NativeArray<float2> collisionVelocityOffsets;

			// Token: 0x04000C7B RID: 3195
			public NativeArray<int> neighbours;
		}
	}
}
