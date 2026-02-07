using System;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001EF RID: 495
	[Serializable]
	public class GraphCollision
	{
		// Token: 0x06000C86 RID: 3206 RVA: 0x0004E114 File Offset: 0x0004C314
		public void Initialize(GraphTransform transform, float scale)
		{
			this.up = (transform.Transform(Vector3.up) - transform.Transform(Vector3.zero)).normalized;
			this.upheight = this.up * this.height;
			this.finalRadius = this.diameter * scale * 0.5f;
			this.finalRaycastRadius = this.thickRaycastDiameter * scale * 0.5f;
			this.contactFilter = new ContactFilter2D
			{
				layerMask = this.mask,
				useDepth = false,
				useLayerMask = true,
				useNormalAngle = false,
				useTriggers = false
			};
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0004E1C8 File Offset: 0x0004C3C8
		public bool Check(Vector3 position)
		{
			if (!this.collisionCheck)
			{
				return true;
			}
			if (this.use2D)
			{
				ColliderType colliderType = this.type;
				if (colliderType <= ColliderType.Capsule)
				{
					return Physics2D.OverlapCircle(position, this.finalRadius, this.contactFilter, GraphCollision.dummyArray) == 0;
				}
				return Physics2D.OverlapPoint(position, this.contactFilter, GraphCollision.dummyArray) == 0;
			}
			else
			{
				position += this.up * this.collisionOffset;
				ColliderType colliderType = this.type;
				if (colliderType == ColliderType.Sphere)
				{
					return !Physics.CheckSphere(position, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				if (colliderType == ColliderType.Capsule)
				{
					return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, this.mask, QueryTriggerInteraction.Ignore);
				}
				return !Physics.Raycast(position, this.up, this.height, this.mask, QueryTriggerInteraction.Ignore) && !Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask, QueryTriggerInteraction.Ignore);
			}
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0004E2EC File Offset: 0x0004C4EC
		public Vector3 CheckHeight(Vector3 position)
		{
			RaycastHit raycastHit;
			bool flag;
			return this.CheckHeight(position, out raycastHit, out flag);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0004E304 File Offset: 0x0004C504
		public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!this.heightCheck || this.use2D)
			{
				hit = default(RaycastHit);
				return position;
			}
			if (this.thickRaycast)
			{
				Ray ray = new Ray(position + this.up * this.fromHeight, -this.up);
				if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			else
			{
				if (Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore))
				{
					return hit.point;
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			return position;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0004E418 File Offset: 0x0004C618
		public RaycastHit[] CheckHeightAll(Vector3 position, out int numHits)
		{
			if (!this.heightCheck || this.use2D)
			{
				this.hitBuffer[0] = new RaycastHit
				{
					point = position,
					distance = 0f
				};
				numHits = 1;
				return this.hitBuffer;
			}
			numHits = Physics.RaycastNonAlloc(position + this.up * this.fromHeight, -this.up, this.hitBuffer, this.fromHeight + 0.005f, this.heightMask, QueryTriggerInteraction.Ignore);
			if (numHits == this.hitBuffer.Length)
			{
				this.hitBuffer = new RaycastHit[this.hitBuffer.Length * 2];
				return this.CheckHeightAll(position, out numHits);
			}
			return this.hitBuffer;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0004E4E0 File Offset: 0x0004C6E0
		public void JobCollisionRay(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<RaycastCommand> nativeArray = dependencyTracker.NewNativeArray<RaycastCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastCommand> nativeArray2 = dependencyTracker.NewNativeArray<RaycastCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastHit> nativeArray3 = dependencyTracker.NewNativeArray<RaycastHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<RaycastHit> nativeArray4 = dependencyTracker.NewNativeArray<RaycastHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareRaycasts
			{
				origins = nodePositions,
				originOffset = up * (this.height + this.collisionOffset),
				direction = -up,
				distance = this.height,
				mask = this.mask,
				physicsScene = Physics.defaultPhysicsScene,
				raycastCommands = nativeArray
			}.Schedule(dependencyTracker);
			new JobPrepareRaycasts
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				direction = up,
				distance = this.height,
				mask = this.mask,
				physicsScene = Physics.defaultPhysicsScene,
				raycastCommands = nativeArray2
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(nativeArray, nativeArray3, 2048);
			dependencyTracker.ScheduleBatch(nativeArray2, nativeArray4, 2048);
			new JobMergeRaycastCollisionHits
			{
				hit1 = nativeArray3,
				hit2 = nativeArray4,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0004E650 File Offset: 0x0004C850
		public void JobCollisionCapsule(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<OverlapCapsuleCommand> commands = dependencyTracker.NewNativeArray<OverlapCapsuleCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<ColliderHit> nativeArray = dependencyTracker.NewNativeArray<ColliderHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareCapsuleCommands
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				direction = up * this.height,
				radius = this.finalRadius,
				mask = this.mask,
				commands = commands,
				physicsScene = Physics.defaultPhysicsScene
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(commands, nativeArray, 2048);
			new JobColliderHitsToBooleans
			{
				hits = nativeArray,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0004E720 File Offset: 0x0004C920
		public void JobCollisionSphere(NativeArray<Vector3> nodePositions, NativeArray<bool> collisionCheckResult, Vector3 up, Allocator allocationMethod, JobDependencyTracker dependencyTracker)
		{
			NativeArray<OverlapSphereCommand> commands = dependencyTracker.NewNativeArray<OverlapSphereCommand>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			NativeArray<ColliderHit> nativeArray = dependencyTracker.NewNativeArray<ColliderHit>(nodePositions.Length, allocationMethod, NativeArrayOptions.ClearMemory);
			new JobPrepareSphereCommands
			{
				origins = nodePositions,
				originOffset = up * this.collisionOffset,
				radius = this.finalRadius,
				mask = this.mask,
				commands = commands,
				physicsScene = Physics.defaultPhysicsScene
			}.Schedule(dependencyTracker);
			dependencyTracker.ScheduleBatch(commands, nativeArray, 2048);
			new JobColliderHitsToBooleans
			{
				hits = nativeArray,
				result = collisionCheckResult
			}.Schedule(dependencyTracker);
		}

		// Token: 0x04000926 RID: 2342
		public ColliderType type = ColliderType.Capsule;

		// Token: 0x04000927 RID: 2343
		public float diameter = 1f;

		// Token: 0x04000928 RID: 2344
		public float height = 2f;

		// Token: 0x04000929 RID: 2345
		public float collisionOffset;

		// Token: 0x0400092A RID: 2346
		[Obsolete("Only the Both mode is supported now")]
		public RayDirection rayDirection = RayDirection.Both;

		// Token: 0x0400092B RID: 2347
		public LayerMask mask;

		// Token: 0x0400092C RID: 2348
		public LayerMask heightMask = -1;

		// Token: 0x0400092D RID: 2349
		public float fromHeight = 100f;

		// Token: 0x0400092E RID: 2350
		public bool thickRaycast;

		// Token: 0x0400092F RID: 2351
		public float thickRaycastDiameter = 1f;

		// Token: 0x04000930 RID: 2352
		public bool unwalkableWhenNoGround = true;

		// Token: 0x04000931 RID: 2353
		public bool use2D;

		// Token: 0x04000932 RID: 2354
		public bool collisionCheck = true;

		// Token: 0x04000933 RID: 2355
		public bool heightCheck = true;

		// Token: 0x04000934 RID: 2356
		public Vector3 up;

		// Token: 0x04000935 RID: 2357
		private Vector3 upheight;

		// Token: 0x04000936 RID: 2358
		private ContactFilter2D contactFilter;

		// Token: 0x04000937 RID: 2359
		private static Collider2D[] dummyArray = new Collider2D[1];

		// Token: 0x04000938 RID: 2360
		private float finalRadius;

		// Token: 0x04000939 RID: 2361
		private float finalRaycastRadius;

		// Token: 0x0400093A RID: 2362
		public const float RaycastErrorMargin = 0.005f;

		// Token: 0x0400093B RID: 2363
		private RaycastHit[] hitBuffer = new RaycastHit[8];
	}
}
