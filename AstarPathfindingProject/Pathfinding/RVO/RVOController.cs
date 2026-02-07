using System;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding.RVO
{
	// Token: 0x020002C5 RID: 709
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	[UniqueComponent(tag = "rvo")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvocontroller.html")]
	public class RVOController : VersionedMonoBehaviour
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0006A14D File Offset: 0x0006834D
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0006A169 File Offset: 0x00068369
		public float radius
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.radius;
				}
				return this.radiusBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.radius = value;
				}
				this.radiusBackingField = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0006A186 File Offset: 0x00068386
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x0006A1A2 File Offset: 0x000683A2
		public float height
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height;
				}
				return this.heightBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.height = value;
				}
				this.heightBackingField = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0006A1BF File Offset: 0x000683BF
		// (set) Token: 0x060010DC RID: 4316 RVA: 0x0006A1E1 File Offset: 0x000683E1
		public float center
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height / 2f;
				}
				return this.centerBackingField;
			}
			set
			{
				this.centerBackingField = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0006A1EA File Offset: 0x000683EA
		public MovementPlane movementPlaneMode
		{
			get
			{
				SimulatorBurst simulator = this.simulator;
				if (simulator != null)
				{
					return simulator.MovementPlane;
				}
				RVOSimulator active = RVOSimulator.active;
				if (active == null)
				{
					return MovementPlane.XZ;
				}
				return active.movementPlane;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0006A20C File Offset: 0x0006840C
		// (set) Token: 0x060010DF RID: 4319 RVA: 0x0006A280 File Offset: 0x00068480
		public SimpleMovementPlane movementPlane
		{
			get
			{
				SimulatorBurst simulator = this.simulator;
				MovementPlane? movementPlane;
				if (simulator == null)
				{
					RVOSimulator active = RVOSimulator.active;
					movementPlane = ((active != null) ? new MovementPlane?(active.movementPlane) : null);
				}
				else
				{
					movementPlane = new MovementPlane?(simulator.MovementPlane);
				}
				MovementPlane? movementPlane2 = movementPlane;
				if (movementPlane2 != null)
				{
					if (movementPlane2.Value == MovementPlane.Arbitrary)
					{
						return this.movementPlaneBackingField;
					}
					if (movementPlane2.Value == MovementPlane.XY)
					{
						return SimpleMovementPlane.XYPlane;
					}
				}
				return SimpleMovementPlane.XZPlane;
			}
			set
			{
				SimulatorBurst simulator = this.simulator;
				MovementPlane? movementPlane;
				if (simulator == null)
				{
					RVOSimulator active = RVOSimulator.active;
					movementPlane = ((active != null) ? new MovementPlane?(active.movementPlane) : null);
				}
				else
				{
					movementPlane = new MovementPlane?(simulator.MovementPlane);
				}
				MovementPlane? movementPlane2 = movementPlane;
				if (movementPlane2 != null && movementPlane2.Value != MovementPlane.Arbitrary)
				{
					throw new InvalidOperationException("Cannot set the movement plane unless the RVOSimulator's movement plane setting is set to Arbitrary.");
				}
				this.movementPlaneBackingField = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0006A2E7 File Offset: 0x000684E7
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0006A2EF File Offset: 0x000684EF
		public IAgent rvoAgent { get; private set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0006A2F8 File Offset: 0x000684F8
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0006A300 File Offset: 0x00068500
		private SimulatorBurst simulator { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0006A309 File Offset: 0x00068509
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0006A32B File Offset: 0x0006852B
		protected IAstarAI ai
		{
			get
			{
				if (this.aiBackingField as MonoBehaviour == null)
				{
					this.aiBackingField = null;
				}
				return this.aiBackingField;
			}
			set
			{
				this.aiBackingField = value;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x0006A334 File Offset: 0x00068534
		public Vector3 position
		{
			get
			{
				this.simulator.BlockUntilSimulationStepDone();
				return this.rvoAgent.Position;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x0006A34C File Offset: 0x0006854C
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x0006A37F File Offset: 0x0006857F
		public Vector3 velocity
		{
			get
			{
				float num = (Time.deltaTime > 0.0001f) ? Time.deltaTime : 0.02f;
				return this.CalculateMovementDelta(num) / num;
			}
			set
			{
				this.simulator.BlockUntilSimulationStepDone();
				this.rvoAgent.ForceSetVelocity(value);
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0006A398 File Offset: 0x00068598
		public Vector3 CalculateMovementDelta(float deltaTime)
		{
			return this.CalculateMovementDelta((this.ai != null) ? this.ai.position : this.tr.position, deltaTime);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0006A3C4 File Offset: 0x000685C4
		public Vector3 CalculateMovementDelta(Vector3 position, float deltaTime)
		{
			if (this.rvoAgent == null)
			{
				return Vector3.zero;
			}
			Vector2 vector = this.movementPlane.ToPlane(this.rvoAgent.CalculatedTargetPoint - position);
			return this.movementPlane.ToWorld(Vector2.ClampMagnitude(vector, this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0006A425 File Offset: 0x00068625
		public bool AvoidingAnyAgents
		{
			get
			{
				return this.rvoAgent != null && this.rvoAgent.AvoidingAnyAgents;
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0006A43C File Offset: 0x0006863C
		public void SetCollisionNormal(Vector3 normal)
		{
			this.simulator.BlockUntilSimulationStepDone();
			this.rvoAgent.SetCollisionNormal(normal);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0006A455 File Offset: 0x00068655
		public void SetObstacleQuery(GraphNode sourceNode)
		{
			this.obstacleQuery = sourceNode;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0006A460 File Offset: 0x00068660
		public Vector2 To2D(Vector3 p)
		{
			return this.movementPlane.ToPlane(p);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0006A47C File Offset: 0x0006867C
		public Vector2 To2D(Vector3 p, out float elevation)
		{
			return this.movementPlane.ToPlane(p, out elevation);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0006A49C File Offset: 0x0006869C
		public Vector3 To3D(Vector2 p, float elevationCoordinate)
		{
			return this.movementPlane.ToWorld(p, elevationCoordinate);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0006A4B9 File Offset: 0x000686B9
		private void OnDisable()
		{
			if (this.simulator == null)
			{
				return;
			}
			this.simulator.RemoveAgent(this.rvoAgent);
			this.simulator = null;
			this.rvoAgent = null;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0006A4E4 File Offset: 0x000686E4
		private void OnEnable()
		{
			this.tr = base.transform;
			this.ai = base.GetComponent<IAstarAI>();
			AIBase aibase = this.ai as AIBase;
			if (aibase != null)
			{
				aibase.FindComponents();
			}
			if (RVOSimulator.active == null)
			{
				Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
				base.enabled = false;
				return;
			}
			this.simulator = RVOSimulator.active.GetSimulator();
			this.rvoAgent = this.simulator.AddAgent(Vector3.zero);
			this.rvoAgent.PreCalculationCallback = new Action(this.UpdateAgentProperties);
			this.rvoAgent.DestroyedCallback = new Action(this.OnAgentDestroyed);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0006A591 File Offset: 0x00068791
		private void OnAgentDestroyed()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.simulator = null;
				this.rvoAgent = null;
				base.enabled = false;
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0006A5B8 File Offset: 0x000687B8
		protected void UpdateAgentProperties()
		{
			Vector3 localScale = this.tr.localScale;
			this.rvoAgent.Radius = Mathf.Max(0.001f, this.radius * Mathf.Abs(localScale.x));
			this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
			this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
			this.rvoAgent.Locked = this.locked;
			this.rvoAgent.MaxNeighbours = this.maxNeighbours;
			this.rvoAgent.DebugFlags = this.debug;
			this.rvoAgent.Layer = this.layer;
			this.rvoAgent.CollidesWith = this.collidesWith;
			SimpleMovementPlane movementPlane = this.movementPlane;
			this.rvoAgent.MovementPlane = movementPlane;
			float num;
			Vector2 point = movementPlane.ToPlane((this.ai != null) ? this.ai.position : this.tr.position, out num);
			if (this.movementPlaneMode == MovementPlane.XY)
			{
				this.rvoAgent.Height = 1f;
				this.rvoAgent.Position = movementPlane.ToWorld(point, 0f);
			}
			else
			{
				this.rvoAgent.Height = this.height * localScale.y;
				this.rvoAgent.Position = movementPlane.ToWorld(point, num + (this.center - 0.5f * this.height) * localScale.y);
			}
			ReachedEndOfPath calculatedEffectivelyReachedDestination = this.rvoAgent.CalculatedEffectivelyReachedDestination;
			float num2 = this.priority * this.priorityMultiplier;
			float num3 = this.flowFollowingStrength;
			if (calculatedEffectivelyReachedDestination == ReachedEndOfPath.Reached)
			{
				num3 = 1f;
				num2 *= 0.3f;
			}
			else if (calculatedEffectivelyReachedDestination == ReachedEndOfPath.ReachedSoon)
			{
				num3 = 1f;
				num2 *= 0.45f;
			}
			this.rvoAgent.Priority = num2;
			this.rvoAgent.FlowFollowingStrength = num3;
			this.rvoAgent.SetObstacleQuery(this.obstacleQuery);
			this.obstacleQuery = null;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0006A7AA File Offset: 0x000689AA
		public void SetTarget(Vector3 pos, float speed, float maxSpeed, Vector3 endOfPath)
		{
			if (this.rvoAgent == null)
			{
				return;
			}
			this.simulator.BlockUntilSimulationStepDone();
			this.rvoAgent.SetTarget(pos, speed, maxSpeed, endOfPath);
			if (this.lockWhenNotMoving)
			{
				this.locked = (speed < 0.001f);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0006A7E8 File Offset: 0x000689E8
		public void Move(Vector3 velocity)
		{
			if (this.rvoAgent == null)
			{
				return;
			}
			this.simulator.BlockUntilSimulationStepDone();
			float magnitude = this.movementPlane.ToPlane(velocity).magnitude;
			this.rvoAgent.SetTarget(((this.ai != null) ? this.ai.position : this.tr.position) + velocity, magnitude, magnitude, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
			if (this.lockWhenNotMoving)
			{
				this.locked = (magnitude < 0.001f);
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0006A880 File Offset: 0x00068A80
		public override void DrawGizmos()
		{
			this.tr = base.transform;
			if (this.ai == null)
			{
				Color color = AIBase.ShapeGizmoColor * (this.locked ? 0.5f : 1f);
				Vector3 position = base.transform.position;
				Vector3 localScale = this.tr.localScale;
				if (this.movementPlaneMode == MovementPlane.XY)
				{
					Draw.WireCylinder(position, Vector3.forward, 0f, this.radius * localScale.x, color);
					return;
				}
				Draw.WireCylinder(position + this.To3D(Vector2.zero, this.center - this.height * 0.5f) * localScale.y, this.To3D(Vector2.zero, 1f), this.height * localScale.y, this.radius * localScale.x, color);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0006A978 File Offset: 0x00068B78
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num > 1)
			{
				migrations.MarkMigrationFinished(0);
			}
			if (migrations.AddAndMaybeRunMigration(0, unityThread))
			{
				if (base.transform.localScale.y != 0f)
				{
					this.centerBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.y != 0f)
				{
					this.heightBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.x != 0f)
				{
					this.radiusBackingField /= Mathf.Abs(base.transform.localScale.x);
				}
			}
		}

		// Token: 0x04000CB0 RID: 3248
		[SerializeField]
		[FormerlySerializedAs("radius")]
		internal float radiusBackingField = 0.5f;

		// Token: 0x04000CB1 RID: 3249
		[SerializeField]
		[FormerlySerializedAs("height")]
		private float heightBackingField = 2f;

		// Token: 0x04000CB2 RID: 3250
		[SerializeField]
		[FormerlySerializedAs("center")]
		private float centerBackingField = 1f;

		// Token: 0x04000CB3 RID: 3251
		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		// Token: 0x04000CB4 RID: 3252
		[Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving;

		// Token: 0x04000CB5 RID: 3253
		[Tooltip("How far into the future to look for collisions with other agents (in seconds)")]
		public float agentTimeHorizon = 2f;

		// Token: 0x04000CB6 RID: 3254
		[Tooltip("How far into the future to look for collisions with obstacles (in seconds)")]
		public float obstacleTimeHorizon = 0.5f;

		// Token: 0x04000CB7 RID: 3255
		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		// Token: 0x04000CB8 RID: 3256
		public RVOLayer layer = RVOLayer.DefaultAgent;

		// Token: 0x04000CB9 RID: 3257
		[EnumFlag]
		public RVOLayer collidesWith = (RVOLayer)(-1);

		// Token: 0x04000CBA RID: 3258
		[HideInInspector]
		[Obsolete]
		public float wallAvoidForce = 1f;

		// Token: 0x04000CBB RID: 3259
		[HideInInspector]
		[Obsolete]
		public float wallAvoidFalloff = 1f;

		// Token: 0x04000CBC RID: 3260
		[Tooltip("How strongly other agents will avoid this agent")]
		[Range(0f, 1f)]
		public float priority = 0.5f;

		// Token: 0x04000CBD RID: 3261
		[NonSerialized]
		public float priorityMultiplier = 1f;

		// Token: 0x04000CBE RID: 3262
		[NonSerialized]
		public float flowFollowingStrength;

		// Token: 0x04000CBF RID: 3263
		private GraphNode obstacleQuery;

		// Token: 0x04000CC2 RID: 3266
		protected Transform tr;

		// Token: 0x04000CC3 RID: 3267
		[SerializeField]
		[FormerlySerializedAs("ai")]
		private IAstarAI aiBackingField;

		// Token: 0x04000CC4 RID: 3268
		internal SimpleMovementPlane movementPlaneBackingField = GraphTransform.xzPlane.ToSimpleMovementPlane();

		// Token: 0x04000CC5 RID: 3269
		public AgentDebugFlags debug;

		// Token: 0x020002C6 RID: 710
		[Flags]
		private enum RVOControllerMigrations
		{
			// Token: 0x04000CC7 RID: 3271
			MigrateScale = 0
		}
	}
}
