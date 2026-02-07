using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000009 RID: 9
	public struct FindGroundResult
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00004ED9 File Offset: 0x000030D9
		public bool isWalkableGround
		{
			get
			{
				return this.hitGround && this.isWalkable;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00004EEB File Offset: 0x000030EB
		public Vector3 point
		{
			get
			{
				return this.hitResult.point;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004EF8 File Offset: 0x000030F8
		public Vector3 normal
		{
			get
			{
				return this.hitResult.normal;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004F05 File Offset: 0x00003105
		public Rigidbody rigidbody
		{
			get
			{
				if (!this.collider)
				{
					return null;
				}
				return this.collider.attachedRigidbody;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00004F24 File Offset: 0x00003124
		public Transform transform
		{
			get
			{
				if (this.collider == null)
				{
					return null;
				}
				Rigidbody attachedRigidbody = this.collider.attachedRigidbody;
				if (!attachedRigidbody)
				{
					return this.collider.transform;
				}
				return attachedRigidbody.transform;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00004F67 File Offset: 0x00003167
		public float GetDistanceToGround()
		{
			if (!this.isRaycastResult)
			{
				return this.groundDistance;
			}
			return this.raycastDistance;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00004F80 File Offset: 0x00003180
		public void SetFromSweepResult(bool hitGround, bool isWalkable, Vector3 position, float sweepDistance, ref RaycastHit inHit, Vector3 surfaceNormal)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			this.collider = inHit.collider;
			this.groundDistance = sweepDistance;
			this.isRaycastResult = false;
			this.raycastDistance = 0f;
			this.hitResult = inHit;
			this.surfaceNormal = surfaceNormal;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00004FE0 File Offset: 0x000031E0
		public void SetFromSweepResult(bool hitGround, bool isWalkable, Vector3 position, Vector3 point, Vector3 normal, Vector3 surfaceNormal, Collider collider, float sweepDistance)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			this.collider = collider;
			this.groundDistance = sweepDistance;
			this.isRaycastResult = false;
			this.raycastDistance = 0f;
			this.hitResult = new RaycastHit
			{
				point = point,
				normal = normal,
				distance = sweepDistance
			};
			this.surfaceNormal = surfaceNormal;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005058 File Offset: 0x00003258
		public void SetFromRaycastResult(bool hitGround, bool isWalkable, Vector3 position, float sweepDistance, float castDistance, ref RaycastHit inHit)
		{
			this.hitGround = hitGround;
			this.isWalkable = isWalkable;
			this.position = position;
			this.collider = inHit.collider;
			this.groundDistance = sweepDistance;
			this.isRaycastResult = true;
			this.raycastDistance = castDistance;
			float distance = this.hitResult.distance;
			this.hitResult = inHit;
			this.hitResult.distance = distance;
			this.surfaceNormal = this.hitResult.normal;
		}

		// Token: 0x0400006B RID: 107
		public bool hitGround;

		// Token: 0x0400006C RID: 108
		public bool isWalkable;

		// Token: 0x0400006D RID: 109
		public Vector3 position;

		// Token: 0x0400006E RID: 110
		public Vector3 surfaceNormal;

		// Token: 0x0400006F RID: 111
		public Collider collider;

		// Token: 0x04000070 RID: 112
		public float groundDistance;

		// Token: 0x04000071 RID: 113
		public bool isRaycastResult;

		// Token: 0x04000072 RID: 114
		public float raycastDistance;

		// Token: 0x04000073 RID: 115
		public RaycastHit hitResult;
	}
}
