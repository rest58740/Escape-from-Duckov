using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000034 RID: 52
	public struct PathOptions : IPlugOptions
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000D83C File Offset: 0x0000BA3C
		public void Reset()
		{
			this.mode = PathMode.Ignore;
			this.orientType = OrientType.None;
			this.lockPositionAxis = (this.lockRotationAxis = AxisConstraint.None);
			this.isClosedPath = false;
			this.lookAtPosition = Vector3.zero;
			this.lookAtTransform = null;
			this.lookAhead = 0f;
			this.hasCustomForwardDirection = false;
			this.forward = Quaternion.identity;
			this.useLocalPosition = false;
			this.parent = null;
			this.isRigidbody = (this.isRigidbody2D = false);
			this.stableZRotation = false;
			this.startupRot = Quaternion.identity;
			this.startupZRot = 0f;
			this.addedExtraStartWp = (this.addedExtraEndWp = false);
		}

		// Token: 0x040000EB RID: 235
		public PathMode mode;

		// Token: 0x040000EC RID: 236
		public OrientType orientType;

		// Token: 0x040000ED RID: 237
		public AxisConstraint lockPositionAxis;

		// Token: 0x040000EE RID: 238
		public AxisConstraint lockRotationAxis;

		// Token: 0x040000EF RID: 239
		public bool isClosedPath;

		// Token: 0x040000F0 RID: 240
		public Vector3 lookAtPosition;

		// Token: 0x040000F1 RID: 241
		public Transform lookAtTransform;

		// Token: 0x040000F2 RID: 242
		public float lookAhead;

		// Token: 0x040000F3 RID: 243
		public bool hasCustomForwardDirection;

		// Token: 0x040000F4 RID: 244
		public Quaternion forward;

		// Token: 0x040000F5 RID: 245
		public bool useLocalPosition;

		// Token: 0x040000F6 RID: 246
		public Transform parent;

		// Token: 0x040000F7 RID: 247
		public bool isRigidbody;

		// Token: 0x040000F8 RID: 248
		public bool isRigidbody2D;

		// Token: 0x040000F9 RID: 249
		public bool stableZRotation;

		// Token: 0x040000FA RID: 250
		internal Quaternion startupRot;

		// Token: 0x040000FB RID: 251
		internal float startupZRot;

		// Token: 0x040000FC RID: 252
		internal bool addedExtraStartWp;

		// Token: 0x040000FD RID: 253
		internal bool addedExtraEndWp;
	}
}
