using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000017 RID: 23
	[AddComponentMenu("Pathfinding/AI/AIPath (2D,3D)")]
	[UniqueComponent(tag = "ai")]
	[DisallowMultipleComponent]
	public class AIPath : AIBase, IAstarAI
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00006487 File Offset: 0x00004687
		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			this.reachedEndOfPath = false;
			base.Teleport(newPosition, clearPath);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006498 File Offset: 0x00004698
		public float remainingDistance
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return float.PositiveInfinity;
				}
				return this.interpolator.remainingDistance + this.movementPlane.ToPlane(this.interpolator.position - base.position).magnitude;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000064F0 File Offset: 0x000046F0
		public override bool reachedDestination
		{
			get
			{
				if (!this.reachedEndOfPath)
				{
					return false;
				}
				if (!this.interpolator.valid || this.remainingDistance + this.movementPlane.ToPlane(base.destination - this.interpolator.endPoint).magnitude > this.endReachedDistance)
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000065A6 File Offset: 0x000047A6
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000065AE File Offset: 0x000047AE
		public bool reachedEndOfPath { get; protected set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000065B7 File Offset: 0x000047B7
		public bool hasPath
		{
			get
			{
				return this.interpolator.valid;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000065C4 File Offset: 0x000047C4
		public bool pathPending
		{
			get
			{
				return this.waitingForPathCalculation;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000065CC File Offset: 0x000047CC
		public Vector3 steeringTarget
		{
			get
			{
				if (!this.interpolator.valid)
				{
					return base.position;
				}
				return this.interpolator.position;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000065ED File Offset: 0x000047ED
		public override Vector3 endOfPath
		{
			get
			{
				if (this.interpolator.valid)
				{
					return this.interpolator.endPoint;
				}
				if (float.IsFinite(base.destination.x))
				{
					return base.destination;
				}
				return base.position;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006627 File Offset: 0x00004827
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000662F File Offset: 0x0000482F
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006638 File Offset: 0x00004838
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00006640 File Offset: 0x00004840
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006649 File Offset: 0x00004849
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00006651 File Offset: 0x00004851
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000665A File Offset: 0x0000485A
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00006662 File Offset: 0x00004862
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000666B File Offset: 0x0000486B
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00006673 File Offset: 0x00004873
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000667C File Offset: 0x0000487C
		NativeMovementPlane IAstarAI.movementPlane
		{
			get
			{
				return new NativeMovementPlane(this.movementPlane);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006689 File Offset: 0x00004889
		public void GetRemainingPath(List<Vector3> buffer, out bool stale)
		{
			buffer.Clear();
			buffer.Add(base.position);
			if (!this.interpolator.valid)
			{
				stale = true;
				return;
			}
			stale = false;
			this.interpolator.GetRemainingPath(buffer);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000066C0 File Offset: 0x000048C0
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale)
		{
			this.GetRemainingPath(buffer, out stale);
			if (partsBuffer != null)
			{
				partsBuffer.Clear();
				partsBuffer.Add(new PathPartWithLinkInfo
				{
					startIndex = 0,
					endIndex = buffer.Count - 1
				});
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006704 File Offset: 0x00004904
		protected override void OnDisable()
		{
			base.OnDisable();
			this.rotationFilterState = Vector2.zero;
			this.rotationFilterState2 = Vector2.zero;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnTargetReached()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006724 File Offset: 0x00004924
		protected virtual void UpdateMovementPlane()
		{
			if (this.path.path == null || this.path.path.Count == 0)
			{
				return;
			}
			ITransformedGraph transformedGraph = AstarData.GetGraph(this.path.path[0]) as ITransformedGraph;
			IMovementPlane movementPlane = (transformedGraph != null) ? transformedGraph.transform : ((this.orientation == OrientationMode.YAxisForward) ? new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 270f, 90f), Vector3.one)) : GraphTransform.identityTransform);
			this.movementPlane = movementPlane.ToSimpleMovementPlane();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000067C0 File Offset: 0x000049C0
		protected override void OnPathComplete(Path newPath)
		{
			ABPath abpath = newPath as ABPath;
			if (abpath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			this.waitingForPathCalculation = false;
			abpath.Claim(this);
			if (abpath.error)
			{
				abpath.Release(this, false);
				base.SetPath(null, true);
				return;
			}
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = abpath;
			if (!abpath.endPointKnownBeforeCalculation)
			{
				base.destination = abpath.originalEndPoint;
			}
			if (this.path.vectorPath.Count == 1)
			{
				this.path.vectorPath.Add(this.path.vectorPath[0]);
			}
			this.interpolatorPath.SetPath(this.path.vectorPath);
			this.interpolator = this.interpolatorPath.start;
			this.UpdateMovementPlane();
			this.reachedEndOfPath = false;
			this.interpolator.MoveToLocallyClosestPoint((this.GetFeetPosition() + abpath.originalStartPoint) * 0.5f, true, true);
			this.interpolator.MoveToLocallyClosestPoint(this.GetFeetPosition(), true, true);
			this.interpolator.MoveToCircleIntersection2D<SimpleMovementPlane>(base.position, this.pickNextWaypointDist, this.movementPlane);
			if (this.remainingDistance <= this.endReachedDistance)
			{
				this.reachedEndOfPath = true;
				this.OnTargetReached();
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006914 File Offset: 0x00004B14
		protected override void ClearPath()
		{
			base.CancelCurrentPathRequest();
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = null;
			this.interpolatorPath.SetPath(null);
			this.reachedEndOfPath = false;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000694C File Offset: 0x00004B4C
		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			float num = this.maxAcceleration;
			if (num < 0f)
			{
				num *= -this.maxSpeed;
			}
			if (base.updatePosition)
			{
				this.simulatedPosition = this.tr.position;
			}
			if (base.updateRotation)
			{
				this.simulatedRotation = this.tr.rotation;
			}
			Vector3 simulatedPosition = this.simulatedPosition;
			Vector2 vector = this.movementPlane.ToPlane(this.simulatedRotation * ((this.orientation == OrientationMode.YAxisForward) ? Vector3.up : Vector3.forward));
			bool flag = base.isStopped || (this.reachedDestination && this.whenCloseToDestination == CloseToDestinationMode.Stop);
			if (this.rvoController != null)
			{
				this.rvoDensityBehavior.Update(this.rvoController.enabled, this.reachedDestination, ref flag, ref this.rvoController.priorityMultiplier, ref this.rvoController.flowFollowingStrength, simulatedPosition);
			}
			float num2 = 0f;
			float num3;
			if (this.interpolator.valid)
			{
				this.interpolator.MoveToCircleIntersection2D<SimpleMovementPlane>(simulatedPosition, this.pickNextWaypointDist, this.movementPlane);
				Vector2 deltaPosition = this.movementPlane.ToPlane(this.steeringTarget - simulatedPosition);
				num3 = deltaPosition.magnitude + Mathf.Max(0f, this.interpolator.remainingDistance);
				bool reachedEndOfPath = this.reachedEndOfPath;
				this.reachedEndOfPath = (num3 <= this.endReachedDistance);
				if (!reachedEndOfPath && this.reachedEndOfPath)
				{
					this.OnTargetReached();
				}
				if (!flag)
				{
					num2 = ((num3 < this.slowdownDistance) ? Mathf.Sqrt(num3 / this.slowdownDistance) : 1f);
					this.velocity2D += MovementUtilities.CalculateAccelerationToReachPoint(deltaPosition, deltaPosition.normalized * this.maxSpeed, this.velocity2D, num, this.rotationSpeed, this.maxSpeed, vector) * deltaTime;
				}
			}
			else
			{
				this.reachedEndOfPath = false;
				num3 = float.PositiveInfinity;
			}
			if (!this.interpolator.valid || flag)
			{
				this.velocity2D -= Vector2.ClampMagnitude(this.velocity2D, num * deltaTime);
				num2 = 1f;
			}
			this.velocity2D = MovementUtilities.ClampVelocity(this.velocity2D, this.maxSpeed, num2, this.slowWhenNotFacingTarget && this.enableRotation, this.preventMovingBackwards, vector);
			this.ApplyGravity(deltaTime);
			bool avoidingOtherAgents = false;
			if (this.rvoController != null && this.rvoController.enabled)
			{
				Vector3 pos = simulatedPosition + this.movementPlane.ToWorld(Vector2.ClampMagnitude(this.velocity2D, num3), 0f);
				this.rvoController.SetTarget(pos, this.velocity2D.magnitude, this.maxSpeed, this.endOfPath);
				avoidingOtherAgents = this.rvoController.AvoidingAnyAgents;
			}
			Vector2 point = this.lastDeltaPosition = base.CalculateDeltaToMoveThisFrame(simulatedPosition, num3, deltaTime);
			nextPosition = simulatedPosition + this.movementPlane.ToWorld(point, this.verticalVelocity * deltaTime);
			this.CalculateNextRotation(num2, avoidingOtherAgents, out nextRotation);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006C6C File Offset: 0x00004E6C
		protected virtual void CalculateNextRotation(float slowdown, bool avoidingOtherAgents, out Quaternion nextRotation)
		{
			if (this.lastDeltaTime > 1E-05f && this.enableRotation)
			{
				float threshold = this.radius * this.tr.localScale.x * 0.2f;
				float num = MovementUtilities.FilterRotationDirection(ref this.rotationFilterState, ref this.rotationFilterState2, this.lastDeltaPosition, threshold, this.lastDeltaTime, avoidingOtherAgents);
				nextRotation = base.SimulateRotationTowards(this.rotationFilterState, this.rotationSpeed * this.lastDeltaTime * num, this.rotationSpeed * this.lastDeltaTime);
				return;
			}
			nextRotation = this.rotation;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006D08 File Offset: 0x00004F08
		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (this.constrainInsideGraph)
			{
				AIPath.cachedNNConstraint.tags = this.seeker.traversableTags;
				AIPath.cachedNNConstraint.graphMask = this.seeker.graphMask;
				AIPath.cachedNNConstraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft();
				NNInfo nearest = AstarPath.active.GetNearest(position, AIPath.cachedNNConstraint);
				if (nearest.node == null)
				{
					positionChanged = false;
					return position;
				}
				Vector3 position2 = nearest.position;
				if (this.rvoController != null && this.rvoController.enabled)
				{
					this.rvoController.SetObstacleQuery(nearest.node);
				}
				Vector2 vector = this.movementPlane.ToPlane(position2 - position);
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude > 1.0000001E-06f)
				{
					this.velocity2D -= vector * Vector2.Dot(vector, this.velocity2D) / sqrMagnitude;
					positionChanged = true;
					return position + this.movementPlane.ToWorld(vector, 0f);
				}
			}
			positionChanged = false;
			return position;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006E15 File Offset: 0x00005015
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			if (migrations.IsLegacyFormat && migrations.LegacyVersion < 1)
			{
				this.rotationSpeed *= 90f;
			}
			base.OnUpgradeSerializedData(ref migrations, unityThread);
		}

		// Token: 0x040000C5 RID: 197
		public float maxAcceleration = -2.5f;

		// Token: 0x040000C6 RID: 198
		[FormerlySerializedAs("turningSpeed")]
		public float rotationSpeed = 360f;

		// Token: 0x040000C7 RID: 199
		public float slowdownDistance = 0.6f;

		// Token: 0x040000C8 RID: 200
		public float pickNextWaypointDist = 2f;

		// Token: 0x040000C9 RID: 201
		public bool alwaysDrawGizmos;

		// Token: 0x040000CA RID: 202
		public bool slowWhenNotFacingTarget = true;

		// Token: 0x040000CB RID: 203
		public bool preventMovingBackwards;

		// Token: 0x040000CC RID: 204
		public bool constrainInsideGraph;

		// Token: 0x040000CD RID: 205
		protected Path path;

		// Token: 0x040000CE RID: 206
		protected PathInterpolator.Cursor interpolator;

		// Token: 0x040000CF RID: 207
		protected PathInterpolator interpolatorPath = new PathInterpolator();

		// Token: 0x040000D1 RID: 209
		private Vector2 rotationFilterState;

		// Token: 0x040000D2 RID: 210
		private Vector2 rotationFilterState2;

		// Token: 0x040000D3 RID: 211
		private static NNConstraint cachedNNConstraint = NNConstraint.Walkable;
	}
}
