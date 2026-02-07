using System;
using UnityEngine;
using UnityEngine.AI;

namespace ECM2
{
	// Token: 0x0200000C RID: 12
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class NavMeshCharacter : MonoBehaviour
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00009ABA File Offset: 0x00007CBA
		public NavMeshAgent agent
		{
			get
			{
				return this._agent;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009AC2 File Offset: 0x00007CC2
		public Character character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00009ACA File Offset: 0x00007CCA
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00009AD2 File Offset: 0x00007CD2
		public bool autoBraking
		{
			get
			{
				return this._autoBraking;
			}
			set
			{
				this._autoBraking = value;
				this.agent.autoBraking = this._autoBraking;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00009AEC File Offset: 0x00007CEC
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00009AF4 File Offset: 0x00007CF4
		public float brakingDistance
		{
			get
			{
				return this._brakingDistance;
			}
			set
			{
				this._brakingDistance = Mathf.Max(0.0001f, value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00009B07 File Offset: 0x00007D07
		public float brakingRatio
		{
			get
			{
				if (!this.autoBraking)
				{
					return 1f;
				}
				if (!this.agent.hasPath)
				{
					return 1f;
				}
				return Mathf.InverseLerp(0f, this.brakingDistance, this.agent.remainingDistance);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00009B45 File Offset: 0x00007D45
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00009B4D File Offset: 0x00007D4D
		public float stoppingDistance
		{
			get
			{
				return this._stoppingDistance;
			}
			set
			{
				this._stoppingDistance = Mathf.Max(0f, value);
				this.agent.stoppingDistance = this._stoppingDistance;
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600022B RID: 555 RVA: 0x00009B74 File Offset: 0x00007D74
		// (remove) Token: 0x0600022C RID: 556 RVA: 0x00009BAC File Offset: 0x00007DAC
		public event NavMeshCharacter.DestinationReachedEventHandler DestinationReached;

		// Token: 0x0600022D RID: 557 RVA: 0x00009BE1 File Offset: 0x00007DE1
		public virtual void OnDestinationReached()
		{
			NavMeshCharacter.DestinationReachedEventHandler destinationReached = this.DestinationReached;
			if (destinationReached == null)
			{
				return;
			}
			destinationReached();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009BF3 File Offset: 0x00007DF3
		protected virtual void CacheComponents()
		{
			this._agent = base.GetComponent<NavMeshAgent>();
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00009C0D File Offset: 0x00007E0D
		public virtual bool HasPath()
		{
			return this.agent.hasPath;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009C1A File Offset: 0x00007E1A
		public virtual bool IsPathFollowing()
		{
			return this.agent.hasPath && !this.agent.isStopped;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009C39 File Offset: 0x00007E39
		public virtual Vector3 GetDestination()
		{
			return this.agent.destination;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009C48 File Offset: 0x00007E48
		public virtual void MoveToDestination(Vector3 destination)
		{
			Vector3 planeNormal = -this.character.GetGravityDirection();
			if (Vector3.ProjectOnPlane(destination - this.character.position, planeNormal).sqrMagnitude >= MathLib.Square(this.stoppingDistance))
			{
				this.agent.SetDestination(destination);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009C9F File Offset: 0x00007E9F
		public virtual void PauseMovement(bool pause)
		{
			this.agent.isStopped = pause;
			this.character.SetMovementDirection(Vector3.zero);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009CBD File Offset: 0x00007EBD
		public virtual void StopMovement()
		{
			this.agent.ResetPath();
			this.character.SetMovementDirection(Vector3.zero);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009CDC File Offset: 0x00007EDC
		protected virtual float ComputeAnalogInputModifier(Vector3 desiredVelocity)
		{
			float maxSpeed = this._character.GetMaxSpeed();
			if (desiredVelocity.sqrMagnitude > 0f && maxSpeed > 1E-08f)
			{
				return Mathf.Clamp01(desiredVelocity.magnitude / maxSpeed);
			}
			return 0f;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009D20 File Offset: 0x00007F20
		protected virtual Vector3 CalcMovementDirection(Vector3 desiredVelocity)
		{
			Vector3 planeNormal = -this.character.GetGravityDirection();
			Vector3 vector = Vector3.ProjectOnPlane(desiredVelocity, planeNormal) * this.brakingRatio;
			float minAnalogSpeed = this._character.GetMinAnalogSpeed();
			if (vector.sqrMagnitude < MathLib.Square(minAnalogSpeed))
			{
				vector = vector.normalized * minAnalogSpeed;
			}
			return Vector3.ClampMagnitude(vector, this.ComputeAnalogInputModifier(vector));
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009D88 File Offset: 0x00007F88
		protected virtual void DoPathFollowing()
		{
			if (!this.IsPathFollowing())
			{
				return;
			}
			if (this.agent.remainingDistance <= this.stoppingDistance)
			{
				this.StopMovement();
				this.OnDestinationReached();
				return;
			}
			Vector3 movementDirection = this.CalcMovementDirection(this.agent.desiredVelocity);
			this.character.SetMovementDirection(movementDirection);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009DDC File Offset: 0x00007FDC
		protected virtual void SyncNavMeshAgent()
		{
			this.agent.angularSpeed = this._character.rotationRate;
			this.agent.speed = this._character.GetMaxSpeed();
			this.agent.acceleration = this._character.GetMaxAcceleration();
			this.agent.velocity = this._character.GetVelocity();
			this.agent.nextPosition = this._character.GetPosition();
			this.agent.radius = this._character.radius;
			this.agent.height = this._character.height;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009E83 File Offset: 0x00008083
		protected virtual void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMovementMode)
		{
			if (!this.character.IsWalking() || !this.character.IsFalling())
			{
				this.StopMovement();
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009EA5 File Offset: 0x000080A5
		protected virtual void OnBeforeSimulationUpdated(float deltaTime)
		{
			this.DoPathFollowing();
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009EAD File Offset: 0x000080AD
		private void Reset()
		{
			this._autoBraking = true;
			this._brakingDistance = 2f;
			this._stoppingDistance = 1f;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009ECC File Offset: 0x000080CC
		private void OnValidate()
		{
			if (this._agent == null)
			{
				this._agent = base.GetComponent<NavMeshAgent>();
			}
			this.brakingDistance = this._brakingDistance;
			this.stoppingDistance = this._stoppingDistance;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009F00 File Offset: 0x00008100
		protected virtual void Awake()
		{
			this.CacheComponents();
			this.agent.autoBraking = this.autoBraking;
			this.agent.stoppingDistance = this.stoppingDistance;
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
			this.agent.updateUpAxis = false;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009F59 File Offset: 0x00008159
		protected virtual void OnEnable()
		{
			this.character.MovementModeChanged += this.OnMovementModeChanged;
			this.character.BeforeSimulationUpdated += this.OnBeforeSimulationUpdated;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009F8B File Offset: 0x0000818B
		protected virtual void OnDisable()
		{
			this.character.MovementModeChanged -= this.OnMovementModeChanged;
			this.character.BeforeSimulationUpdated -= this.OnBeforeSimulationUpdated;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009FBD File Offset: 0x000081BD
		protected virtual void LateUpdate()
		{
			this.SyncNavMeshAgent();
		}

		// Token: 0x040000CA RID: 202
		[Space(15f)]
		[Tooltip("Should the agent brake automatically to avoid overshooting the destination point? \nIf true, the agent will brake automatically as it nears the destination.")]
		[SerializeField]
		private bool _autoBraking;

		// Token: 0x040000CB RID: 203
		[Tooltip("Distance from target position to start braking.")]
		[SerializeField]
		private float _brakingDistance;

		// Token: 0x040000CC RID: 204
		[Tooltip("Stop within this distance from the target position.")]
		[SerializeField]
		private float _stoppingDistance;

		// Token: 0x040000CD RID: 205
		private NavMeshAgent _agent;

		// Token: 0x040000CE RID: 206
		private Character _character;

		// Token: 0x0200002F RID: 47
		// (Invoke) Token: 0x060002C1 RID: 705
		public delegate void DestinationReachedEventHandler();
	}
}
