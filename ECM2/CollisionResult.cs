using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x0200000A RID: 10
	public struct CollisionResult
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000050D4 File Offset: 0x000032D4
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000050F0 File Offset: 0x000032F0
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

		// Token: 0x04000074 RID: 116
		public bool startPenetrating;

		// Token: 0x04000075 RID: 117
		public HitLocation hitLocation;

		// Token: 0x04000076 RID: 118
		public bool isWalkable;

		// Token: 0x04000077 RID: 119
		public Vector3 position;

		// Token: 0x04000078 RID: 120
		public Vector3 velocity;

		// Token: 0x04000079 RID: 121
		public Vector3 otherVelocity;

		// Token: 0x0400007A RID: 122
		public Vector3 point;

		// Token: 0x0400007B RID: 123
		public Vector3 normal;

		// Token: 0x0400007C RID: 124
		public Vector3 surfaceNormal;

		// Token: 0x0400007D RID: 125
		public Vector3 displacementToHit;

		// Token: 0x0400007E RID: 126
		public Vector3 remainingDisplacement;

		// Token: 0x0400007F RID: 127
		public Collider collider;

		// Token: 0x04000080 RID: 128
		public RaycastHit hitResult;
	}
}
