using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000010 RID: 16
	[RequireComponent(typeof(Animator))]
	public class RootMotionController : MonoBehaviour
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000A14F File Offset: 0x0000834F
		public virtual void FlushAccumulatedDeltas()
		{
			this._rootMotionDeltaPosition = Vector3.zero;
			this._rootMotionDeltaRotation = Quaternion.identity;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000A167 File Offset: 0x00008367
		public virtual Quaternion ConsumeRootMotionRotation()
		{
			Quaternion rootMotionDeltaRotation = this._rootMotionDeltaRotation;
			this._rootMotionDeltaRotation = Quaternion.identity;
			return rootMotionDeltaRotation;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000A17A File Offset: 0x0000837A
		public virtual Vector3 GetRootMotionVelocity(float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return Vector3.zero;
			}
			return this._rootMotionDeltaPosition / deltaTime;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000A196 File Offset: 0x00008396
		public virtual Vector3 ConsumeRootMotionVelocity(float deltaTime)
		{
			Vector3 rootMotionVelocity = this.GetRootMotionVelocity(deltaTime);
			this._rootMotionDeltaPosition = Vector3.zero;
			return rootMotionVelocity;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000A1AC File Offset: 0x000083AC
		public virtual void Awake()
		{
			this._animator = base.GetComponent<Animator>();
			if (this._animator == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"RootMotionController: There is no 'Animator' attached to the '",
					base.name,
					"' game object.\nPlease attach a 'Animator' to the '",
					base.name,
					"' game object"
				}));
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000A20D File Offset: 0x0000840D
		public virtual void Start()
		{
			this._rootMotionDeltaPosition = Vector3.zero;
			this._rootMotionDeltaRotation = Quaternion.identity;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000A225 File Offset: 0x00008425
		public virtual void OnAnimatorMove()
		{
			this._rootMotionDeltaPosition += this._animator.deltaPosition;
			this._rootMotionDeltaRotation = this._animator.deltaRotation * this._rootMotionDeltaRotation;
		}

		// Token: 0x040000DD RID: 221
		protected Animator _animator;

		// Token: 0x040000DE RID: 222
		protected Vector3 _rootMotionDeltaPosition;

		// Token: 0x040000DF RID: 223
		protected Quaternion _rootMotionDeltaRotation;
	}
}
