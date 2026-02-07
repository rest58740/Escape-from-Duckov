using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001A RID: 26
	[AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
	[UniqueComponent(tag = "ai")]
	[DisallowMultipleComponent]
	public class RichAI : AIBase, IAstarAI
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00006EB5 File Offset: 0x000050B5
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00006EBD File Offset: 0x000050BD
		public bool traversingOffMeshLink { get; protected set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006EC6 File Offset: 0x000050C6
		public float remainingDistance
		{
			get
			{
				return this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, this.richPath.Endpoint);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006EE5 File Offset: 0x000050E5
		public bool reachedEndOfPath
		{
			get
			{
				return this.approachingPathEndpoint && this.distanceToSteeringTarget < this.endReachedDistance;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006F00 File Offset: 0x00005100
		public override bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath)
				{
					return false;
				}
				if (this.distanceToSteeringTarget + this.movementPlane.ToPlane(this.steeringTarget - this.richPath.Endpoint).magnitude + this.movementPlane.ToPlane(base.destination - this.richPath.Endpoint).magnitude > this.endReachedDistance)
				{
					return false;
				}
				if (this.orientation != OrientationMode.YAxisForward)
				{
					float num;
					this.movementPlane.ToPlane(base.destination - base.position, out num);
					float num2 = this.tr.localScale.y * this.height;
					if (num > num2 || (double)num < (double)(-(double)num2) * 0.5)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00006FD3 File Offset: 0x000051D3
		public bool hasPath
		{
			get
			{
				return this.richPath.GetCurrentPart() != null;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006FE3 File Offset: 0x000051E3
		public bool pathPending
		{
			get
			{
				return this.waitingForPathCalculation || this.delayUpdatePath;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00006FF5 File Offset: 0x000051F5
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00006FFD File Offset: 0x000051FD
		public Vector3 steeringTarget { get; protected set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00006627 File Offset: 0x00004827
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000662F File Offset: 0x0000482F
		float IAstarAI.radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00006638 File Offset: 0x00004838
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00006640 File Offset: 0x00004840
		float IAstarAI.height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006649 File Offset: 0x00004849
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00006651 File Offset: 0x00004851
		float IAstarAI.maxSpeed
		{
			get
			{
				return this.maxSpeed;
			}
			set
			{
				this.maxSpeed = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000665A File Offset: 0x0000485A
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00006662 File Offset: 0x00004862
		bool IAstarAI.canSearch
		{
			get
			{
				return base.canSearch;
			}
			set
			{
				base.canSearch = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000666B File Offset: 0x0000486B
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00006673 File Offset: 0x00004873
		bool IAstarAI.canMove
		{
			get
			{
				return this.canMove;
			}
			set
			{
				this.canMove = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000667C File Offset: 0x0000487C
		NativeMovementPlane IAstarAI.movementPlane
		{
			get
			{
				return new NativeMovementPlane(this.movementPlane);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00007006 File Offset: 0x00005206
		public bool approachingPartEndpoint
		{
			get
			{
				return this.lastCorner && this.nextCorners.Count == 1;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007020 File Offset: 0x00005220
		public bool approachingPathEndpoint
		{
			get
			{
				return this.approachingPartEndpoint && this.richPath.IsLastPart;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00007037 File Offset: 0x00005237
		public override Vector3 endOfPath
		{
			get
			{
				if (this.hasPath)
				{
					return this.richPath.Endpoint;
				}
				if (float.IsFinite(base.destination.x))
				{
					return base.destination;
				}
				return base.position;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000706C File Offset: 0x0000526C
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00007074 File Offset: 0x00005274
		public override Quaternion rotation
		{
			get
			{
				return base.rotation;
			}
			set
			{
				base.rotation = value;
				this.rotationFilterState = Vector2.zero;
				this.rotationFilterState2 = Vector2.zero;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007093 File Offset: 0x00005293
		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			base.Teleport(this.ClampPositionToGraph(newPosition), clearPath);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000070A4 File Offset: 0x000052A4
		protected virtual Vector3 ClampPositionToGraph(Vector3 newPosition)
		{
			NNInfo nninfo = (AstarPath.active != null) ? AstarPath.active.GetNearest(newPosition) : default(NNInfo);
			float elevation;
			this.movementPlane.ToPlane(newPosition, out elevation);
			return this.movementPlane.ToWorld(this.movementPlane.ToPlane((nninfo.node != null) ? nninfo.position : newPosition), elevation);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000710C File Offset: 0x0000530C
		protected override void OnDisable()
		{
			base.OnDisable();
			this.traversingOffMeshLink = false;
			base.StopAllCoroutines();
			this.rotationFilterState = Vector2.zero;
			this.rotationFilterState2 = Vector2.zero;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007137 File Offset: 0x00005337
		protected override bool shouldRecalculatePath
		{
			get
			{
				return base.shouldRecalculatePath && !this.traversingOffMeshLink;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000714C File Offset: 0x0000534C
		public override void SearchPath()
		{
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
				return;
			}
			base.SearchPath();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007164 File Offset: 0x00005364
		protected override void OnPathComplete(Path p)
		{
			this.waitingForPathCalculation = false;
			p.Claim(this);
			if (p.error)
			{
				p.Release(this, false);
				return;
			}
			if (this.traversingOffMeshLink)
			{
				this.delayUpdatePath = true;
			}
			else
			{
				ABPath abpath = p as ABPath;
				if (abpath != null && !abpath.endPointKnownBeforeCalculation)
				{
					base.destination = abpath.originalEndPoint;
				}
				this.richPath.Initialize(this.seeker, p, true, this.funnelSimplification);
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					if (base.updatePosition)
					{
						this.simulatedPosition = this.tr.position;
					}
					Vector2 b = this.movementPlane.ToPlane(this.UpdateTarget(richFunnel));
					this.steeringTarget = this.nextCorners[0];
					Vector2 a = this.movementPlane.ToPlane(this.steeringTarget);
					this.distanceToSteeringTarget = (a - b).magnitude;
					if (this.lastCorner && this.nextCorners.Count == 1 && this.distanceToSteeringTarget <= this.endReachedDistance)
					{
						this.NextPart();
					}
				}
			}
			p.Release(this, false);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000728A File Offset: 0x0000548A
		protected override void ClearPath()
		{
			base.CancelCurrentPathRequest();
			this.richPath.Clear();
			this.lastCorner = false;
			this.delayUpdatePath = false;
			this.distanceToSteeringTarget = float.PositiveInfinity;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000072B8 File Offset: 0x000054B8
		protected void NextPart()
		{
			if (!this.richPath.CompletedAllParts)
			{
				if (!this.richPath.IsLastPart)
				{
					this.lastCorner = false;
				}
				this.richPath.NextPart();
				if (this.richPath.CompletedAllParts)
				{
					this.OnTargetReached();
				}
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007304 File Offset: 0x00005504
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			this.richPath.GetRemainingPath(buffer, null, this.simulatedPosition, out stale);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000731A File Offset: 0x0000551A
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			this.richPath.GetRemainingPath(buffer, partsBuffer, this.simulatedPosition, out stale);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000035CE File Offset: 0x000017CE
		protected virtual void OnTargetReached()
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007330 File Offset: 0x00005530
		protected virtual Vector3 UpdateTarget(RichFunnel fn)
		{
			this.nextCorners.Clear();
			bool flag;
			Vector3 result = fn.Update(this.simulatedPosition, this.nextCorners, 2, out this.lastCorner, out flag);
			if (flag && !this.waitingForPathCalculation && base.canSearch)
			{
				this.SearchPath();
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000737C File Offset: 0x0000557C
		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (base.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (base.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			RichPathPart currentPart = this.richPath.GetCurrentPart();
			if (currentPart is RichSpecial)
			{
				if (!this.traversingOffMeshLink && !this.richPath.CompletedAllParts)
				{
					base.StartCoroutine(this.TraverseSpecial(currentPart as RichSpecial));
				}
				nextPosition = (this.steeringTarget = this.simulatedPosition);
				nextRotation = this.rotation;
				return;
			}
			RichFunnel richFunnel = currentPart as RichFunnel;
			bool flag = base.isStopped || (this.reachedDestination && this.whenCloseToDestination == CloseToDestinationMode.Stop);
			if (this.rvoController != null)
			{
				this.rvoDensityBehavior.Update(this.rvoController.enabled, this.reachedDestination, ref flag, ref this.rvoController.priorityMultiplier, ref this.rvoController.flowFollowingStrength, this.simulatedPosition);
			}
			if (richFunnel != null && !flag)
			{
				this.TraverseFunnel(richFunnel, deltaTime, out nextPosition, out nextRotation);
				return;
			}
			this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, this.acceleration * deltaTime);
			this.FinalMovement(this.simulatedPosition, deltaTime, float.PositiveInfinity, 1f, out nextPosition, out nextRotation);
			if (richFunnel == null || base.isStopped)
			{
				this.steeringTarget = this.simulatedPosition;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000074F0 File Offset: 0x000056F0
		private void TraverseFunnel(RichFunnel fn, float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector3 vector = this.UpdateTarget(fn);
			float elevation;
			Vector2 vector2 = this.movementPlane.ToPlane(vector, out elevation);
			if (Time.frameCount % 5 == 0 && this.wallForce > 0f && this.wallDist > 0f)
			{
				this.wallBuffer.Clear();
				fn.FindWalls(this.wallBuffer, this.wallDist);
			}
			this.steeringTarget = this.nextCorners[0];
			Vector2 vector3 = this.movementPlane.ToPlane(this.steeringTarget);
			Vector2 vector4 = vector3 - vector2;
			Vector2 vector5 = VectorMath.Normalize(vector4, out this.distanceToSteeringTarget);
			Vector2 a = this.CalculateWallForce(vector2, elevation, vector5);
			Vector2 targetVelocity;
			if (this.approachingPartEndpoint)
			{
				targetVelocity = ((this.slowdownTime > 0f) ? Vector2.zero : (vector5 * this.maxSpeed));
				a *= Math.Min(this.distanceToSteeringTarget / 0.5f, 1f);
				if (this.distanceToSteeringTarget <= this.endReachedDistance)
				{
					this.NextPart();
				}
			}
			else
			{
				targetVelocity = (((this.nextCorners.Count > 1) ? this.movementPlane.ToPlane(this.nextCorners[1]) : (vector2 + 2f * vector4)) - vector3).normalized * this.maxSpeed;
			}
			Vector2 forwardsVector = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			Vector2 a2 = MovementUtilities.CalculateAccelerationToReachPoint(vector3 - vector2, targetVelocity, this.velocity2D, this.acceleration, this.rotationSpeed, this.maxSpeed, forwardsVector);
			this.velocity2D += (a2 + a * this.wallForce) * deltaTime;
			float num = this.distanceToSteeringTarget + Vector3.Distance(this.steeringTarget, fn.exactEnd);
			float speedLimitFactor = (num < this.maxSpeed * this.slowdownTime) ? Mathf.Sqrt(num / (this.maxSpeed * this.slowdownTime)) : 1f;
			this.FinalMovement(vector, deltaTime, num, speedLimitFactor, out nextPosition, out nextRotation);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000772C File Offset: 0x0000592C
		private void FinalMovement(Vector3 position3D, float deltaTime, float distanceToEndOfPath, float speedLimitFactor, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector2 forward = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			this.ApplyGravity(deltaTime);
			this.velocity2D = MovementUtilities.ClampVelocity(this.velocity2D, this.maxSpeed, speedLimitFactor, this.slowWhenNotFacingTarget && this.enableRotation, this.preventMovingBackwards, forward);
			bool avoidingOtherAgents = false;
			if (this.rvoController != null && this.rvoController.enabled)
			{
				Vector3 pos = position3D + this.movementPlane.ToWorld(Vector2.ClampMagnitude(this.velocity2D, distanceToEndOfPath), 0f);
				this.rvoController.SetTarget(pos, this.velocity2D.magnitude, this.maxSpeed, this.endOfPath);
				avoidingOtherAgents = this.rvoController.AvoidingAnyAgents;
			}
			Vector2 vector = this.lastDeltaPosition = base.CalculateDeltaToMoveThisFrame(position3D, distanceToEndOfPath, deltaTime);
			if (this.enableRotation)
			{
				float threshold = this.radius * this.tr.localScale.x * 0.2f;
				float num = MovementUtilities.FilterRotationDirection(ref this.rotationFilterState, ref this.rotationFilterState2, vector, threshold, deltaTime, avoidingOtherAgents);
				nextRotation = base.SimulateRotationTowards(this.rotationFilterState, this.rotationSpeed * deltaTime * num, this.rotationSpeed * deltaTime);
			}
			else
			{
				nextRotation = this.simulatedRotation;
			}
			nextPosition = position3D + this.movementPlane.ToWorld(vector, this.verticalVelocity * deltaTime);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000078B8 File Offset: 0x00005AB8
		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (this.richPath != null)
			{
				RichFunnel richFunnel = this.richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					Vector3 a = richFunnel.ClampToNavmesh(position);
					if (this.rvoController != null && this.rvoController.enabled)
					{
						this.rvoController.SetObstacleQuery(richFunnel.CurrentNode);
					}
					Vector2 vector = this.movementPlane.ToPlane(a - position);
					float sqrMagnitude = vector.sqrMagnitude;
					if (sqrMagnitude > 1.0000001E-06f)
					{
						this.velocity2D -= vector * Vector2.Dot(vector, this.velocity2D) / sqrMagnitude;
						positionChanged = true;
						return position + this.movementPlane.ToWorld(vector, 0f);
					}
				}
			}
			positionChanged = false;
			return position;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007988 File Offset: 0x00005B88
		private Vector2 CalculateWallForce(Vector2 position, float elevation, Vector2 directionToTarget)
		{
			if (this.wallForce <= 0f || this.wallDist <= 0f)
			{
				return Vector2.zero;
			}
			float num = 0f;
			float num2 = 0f;
			Vector3 vector = this.movementPlane.ToWorld(position, elevation);
			for (int i = 0; i < this.wallBuffer.Count; i += 2)
			{
				float sqrMagnitude = (VectorMath.ClosestPointOnSegment(this.wallBuffer[i], this.wallBuffer[i + 1], vector) - vector).sqrMagnitude;
				if (sqrMagnitude <= this.wallDist * this.wallDist)
				{
					Vector2 normalized = this.movementPlane.ToPlane(this.wallBuffer[i + 1] - this.wallBuffer[i]).normalized;
					float num3 = Vector2.Dot(directionToTarget, normalized);
					float num4 = 1f - Math.Max(0f, 2f * (sqrMagnitude / (this.wallDist * this.wallDist)) - 1f);
					if (num3 > 0f)
					{
						num2 = Math.Max(num2, num3 * num4);
					}
					else
					{
						num = Math.Max(num, -num3 * num4);
					}
				}
			}
			return new Vector2(directionToTarget.y, -directionToTarget.x) * (num2 - num);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007ADB File Offset: 0x00005CDB
		protected virtual IEnumerator TraverseSpecial(RichSpecial link)
		{
			this.traversingOffMeshLink = true;
			this.velocity2D = Vector3.zero;
			IEnumerator routine = (this.onTraverseOffMeshLink != null) ? this.onTraverseOffMeshLink(link) : this.TraverseOffMeshLinkFallback(link);
			yield return base.StartCoroutine(routine);
			this.traversingOffMeshLink = false;
			this.NextPart();
			if (this.delayUpdatePath)
			{
				this.delayUpdatePath = false;
				if (base.canSearch)
				{
					this.SearchPath();
				}
			}
			yield break;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007AF1 File Offset: 0x00005CF1
		protected IEnumerator TraverseOffMeshLinkFallback(RichSpecial link)
		{
			float duration = (this.maxSpeed > 0f) ? (Vector3.Distance(link.second.position, link.first.position) / this.maxSpeed) : 1f;
			float startTime = Time.time;
			for (;;)
			{
				Vector3 vector = Vector3.Lerp(link.first.position, link.second.position, Mathf.InverseLerp(startTime, startTime + duration, Time.time));
				if (base.updatePosition)
				{
					this.tr.position = vector;
				}
				else
				{
					this.simulatedPosition = vector;
				}
				if (Time.time >= startTime + duration)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007B08 File Offset: 0x00005D08
		public override void DrawGizmos()
		{
			base.DrawGizmos();
			if (this.tr != null)
			{
				Vector3 a = base.position;
				for (int i = 0; i < this.nextCorners.Count; i++)
				{
					Draw.Line(a, this.nextCorners[i], RichAI.GizmoColorPath);
					a = this.nextCorners[i];
				}
			}
		}

		// Token: 0x040000D4 RID: 212
		public float acceleration = 5f;

		// Token: 0x040000D5 RID: 213
		public float rotationSpeed = 360f;

		// Token: 0x040000D6 RID: 214
		public float slowdownTime = 0.5f;

		// Token: 0x040000D7 RID: 215
		public float wallForce = 3f;

		// Token: 0x040000D8 RID: 216
		public float wallDist = 1f;

		// Token: 0x040000D9 RID: 217
		public bool funnelSimplification;

		// Token: 0x040000DA RID: 218
		public bool slowWhenNotFacingTarget = true;

		// Token: 0x040000DB RID: 219
		public bool preventMovingBackwards;

		// Token: 0x040000DC RID: 220
		public Func<RichSpecial, IEnumerator> onTraverseOffMeshLink;

		// Token: 0x040000DD RID: 221
		protected readonly RichPath richPath = new RichPath();

		// Token: 0x040000DE RID: 222
		protected bool delayUpdatePath;

		// Token: 0x040000DF RID: 223
		protected bool lastCorner;

		// Token: 0x040000E0 RID: 224
		private Vector2 rotationFilterState;

		// Token: 0x040000E1 RID: 225
		private Vector2 rotationFilterState2;

		// Token: 0x040000E2 RID: 226
		protected float distanceToSteeringTarget = float.PositiveInfinity;

		// Token: 0x040000E3 RID: 227
		protected readonly List<Vector3> nextCorners = new List<Vector3>();

		// Token: 0x040000E4 RID: 228
		protected readonly List<Vector3> wallBuffer = new List<Vector3>();

		// Token: 0x040000E7 RID: 231
		protected static readonly Color GizmoColorPath = new Color(0.03137255f, 0.30588236f, 0.7607843f);
	}
}
