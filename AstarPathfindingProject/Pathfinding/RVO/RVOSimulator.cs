using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002CA RID: 714
	[ExecuteInEditMode]
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvosimulator.html")]
	public class RVOSimulator : VersionedMonoBehaviour
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0006AB12 File Offset: 0x00068D12
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x0006AB19 File Offset: 0x00068D19
		public static RVOSimulator active { get; private set; }

		// Token: 0x06001102 RID: 4354 RVA: 0x0006AB21 File Offset: 0x00068D21
		public SimulatorBurst GetSimulator()
		{
			if (this.simulatorBurst == null && Application.isPlaying)
			{
				this.simulatorBurst = new SimulatorBurst(this.movementPlane);
			}
			return this.simulatorBurst;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0006AB4C File Offset: 0x00068D4C
		private void OnEnable()
		{
			if (RVOSimulator.active != null)
			{
				if (RVOSimulator.active != this && Application.isPlaying)
				{
					if (base.enabled)
					{
						Debug.LogWarning("Another RVOSimulator component is already in the scene. More than one RVOSimulator component cannot be active at the same time. Disabling this one.", this);
					}
					base.enabled = false;
				}
				return;
			}
			RVOSimulator.active = this;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0006AB9C File Offset: 0x00068D9C
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.desiredSimulationFPS < 1)
			{
				this.desiredSimulationFPS = 1;
			}
			SimulatorBurst simulator = this.GetSimulator();
			simulator.DesiredDeltaTime = 1f / (float)this.desiredSimulationFPS;
			simulator.SymmetryBreakingBias = this.symmetryBreakingBias;
			simulator.HardCollisions = this.hardCollisions;
			simulator.drawQuadtree = this.drawQuadtree;
			simulator.UseNavmeshAsObstacle = this.useNavmeshAsObstacle;
			simulator.Update(default(JobHandle), Time.deltaTime, true, Allocator.TempJob).Complete();
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0006AC27 File Offset: 0x00068E27
		private void OnDisable()
		{
			if (RVOSimulator.active == this)
			{
				RVOSimulator.active = null;
			}
			if (this.simulatorBurst != null)
			{
				this.simulatorBurst.OnDestroy();
				this.simulatorBurst = null;
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000035CE File Offset: 0x000017CE
		public override void DrawGizmos()
		{
		}

		// Token: 0x04000CCF RID: 3279
		[Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
		public int desiredSimulationFPS = 20;

		// Token: 0x04000CD0 RID: 3280
		[Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
		[Obsolete("The number of worker threads is now set by the unity job system", true)]
		public ThreadCount workerThreads = ThreadCount.Two;

		// Token: 0x04000CD1 RID: 3281
		[Tooltip("Calculate local avoidance in between frames.\nThis can increase jitter in the agents' movement so use it only if you really need the performance boost. It will also reduce the responsiveness of the agents to the commands you send to them.")]
		[Obsolete("Double buffering has been removed")]
		public bool doubleBuffering;

		// Token: 0x04000CD2 RID: 3282
		public bool hardCollisions = true;

		// Token: 0x04000CD3 RID: 3283
		[Tooltip("Bias agents to pass each other on the right side.\nIf the desired velocity of an agent puts it on a collision course with another agent or an obstacle its desired velocity will be rotated this number of radians (1 radian is approximately 57°) to the right. This helps to break up symmetries and makes it possible to resolve some situations much faster.\n\nWhen many agents have the same goal this can however have the side effect that the group clustered around the target point may as a whole start to spin around the target point.")]
		[Range(0f, 0.2f)]
		public float symmetryBreakingBias = 0.1f;

		// Token: 0x04000CD4 RID: 3284
		[Tooltip("Determines if the XY (2D) or XZ (3D) plane is used for movement")]
		public MovementPlane movementPlane;

		// Token: 0x04000CD5 RID: 3285
		public bool useNavmeshAsObstacle;

		// Token: 0x04000CD6 RID: 3286
		public bool drawQuadtree;

		// Token: 0x04000CD7 RID: 3287
		private SimulatorBurst simulatorBurst;
	}
}
